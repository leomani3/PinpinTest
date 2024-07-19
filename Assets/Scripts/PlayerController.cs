using MyBox;
using System.Collections.Generic;
using UnityEngine;

namespace Pinpin
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerStatCollection playerStatCollection;
        [SerializeField] private Rigidbody m_rigidBody;
        [SerializeField] private float m_rotationSpeed = 10.0f;
        [SerializeField] private Vector3Data playerPositionData;
        [SerializeField] private LayerMask ressourceLayer;
        [SerializeField] private float ressourceDetectionRadius;

        [Separator("Ears animation")]
        [SerializeField] private DynamicBone leftDynamicBone;
        [SerializeField] private DynamicBone rightDynamicBone;

        [Separator("Tools")]
        [SerializeField] private GameObject axe;
        [SerializeField] private GameObject pickaxe;

        [Separator("Movement refinement")]
        [SerializeField] private float distanceFromPlayer;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float maxSlopeAngle;

        [Separator("Pet")]
        [SerializeField] private Transform playerPetTarget;
        [SerializeField] private Vector3Data playerPetPosition;
        [SerializeField] private int nbFrameDelay;

        private bool m_hasInput = false;
        private Vector3 m_inputDir = Vector3.zero;
        private Camera m_mainCamera;
        private Animator _animator;
        private bool _invalidSlopeDetected;
        private bool _voidDetected;

        private List<Vector3> _positionsByFrame = new List<Vector3>();

        //detect ressources
        private Collider[] _detectedRessourceColliders;
        private List<Ressource> _detectedRessources = new List<Ressource>();

        //Raycasting
        private RaycastHit _movementRaycastHit;

        public PlayerStatCollection PlayerStatCollection => playerStatCollection;

        private void Reset()
        {
            print("reset");
            m_rigidBody = GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            m_mainCamera = Camera.main;
            _animator = GetComponentInChildren<Animator>();
            playerStatCollection.Init();
        }

        private void MovementRaycast()
        {
            if (m_hasInput)
            {
                Ray ray = new Ray(transform.position + (m_inputDir * distanceFromPlayer) + new Vector3(0, 4, 0), Vector3.down);
                if (Physics.Raycast(ray, out _movementRaycastHit, 10, groundLayer))
                {
                    _voidDetected = false;
                    if (Vector3.Angle(_movementRaycastHit.normal, Vector3.up) <= maxSlopeAngle)
                    {
                        _invalidSlopeDetected = false;
                    }
                    else
                    {
                        _invalidSlopeDetected = true;
                    }
                }
                else
                {
                    _voidDetected = true;
                }
            }
            else
            {
                _voidDetected = false;
                _invalidSlopeDetected = false;
            }
        }


        private void GetInput()
        {
            m_hasInput = TouchJoystick.hasInput;
            if (!m_hasInput)
            {
                m_inputDir = Vector3.zero;
                return;
            }

            //Align the input to the camera's forward and right vectors
            Vector2 input = TouchJoystick.input;
            Vector3 forward = m_mainCamera.transform.forward;
            Vector3 right = m_mainCamera.transform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            m_inputDir = forward * input.y + right * input.x;
        }

        private void RecordPosition()
        {
            if (_positionsByFrame.Count >= nbFrameDelay)
            {
                _positionsByFrame.RemoveAt(0);
            }

            _positionsByFrame.Add(playerPetTarget.position);
            playerPetPosition.data = _positionsByFrame[0];
        }

        private void OnDrawGizmos()
        {
            //Gizmos.DrawRay(new Ray(transform.position + (m_inputDir * distanceFromPlayer) + new Vector3(0, 4, 0), Vector3.down));
        }

        private void DetectNearbyRessource()
        {
            _detectedRessourceColliders = Physics.OverlapSphere(transform.position + (transform.forward * 0.5f), ressourceDetectionRadius, ressourceLayer);

            _detectedRessources.Clear();
            foreach (Collider collider in _detectedRessourceColliders)
            {
                Ressource detectedRessource = collider.GetComponent<Ressource>();
                if (detectedRessource != null && detectedRessource.Alive)
                {
                    _detectedRessources.Add(detectedRessource);
                }
            }

            if (_detectedRessources.Count > 0)
            {
                _animator.SetBool("Harvesting", true);
                _animator.SetLayerWeight(1, 1);

                HideTools();
                switch (_detectedRessources[0].RessourceType)
                {
                    case RessourceType.Wood:
                        _animator.SetInteger("HarvestingAnimationID", 0);
                        _animator.speed = playerStatCollection.GetStat(PlayerStatType.ChoppingSpeed);
                        axe.SetActive(true); //Todo : remplacer par un tween
                        break;
                    case RessourceType.Stone:
                        _animator.SetInteger("HarvestingAnimationID", 1);
                        _animator.speed = playerStatCollection.GetStat(PlayerStatType.MiningSpeed);
                        pickaxe.SetActive(true); //Todo : remplacer par un tween
                        break;
                    default:
                        break;
                }
            }
            else
            {
                _animator.SetLayerWeight(1, 0);
                _animator.SetBool("Harvesting", false);
                _animator.speed = 1;
                HideTools();
            }
        }

        public void HarvestNearbyRessources()
        {
            foreach (Ressource ressource in _detectedRessources)
            {
                ressource.ReceiveHit(1);
            }
        }

        private void HideTools()
        {
            axe.SetActive(false);  //Todo : remplacer par un tween
            pickaxe.SetActive(false);  //Todo : remplacer par un tween
        }

        private void Update()
        {
            GetInput();
            MovementRaycast();
            playerPositionData.data = transform.position;

            DetectNearbyRessource();
            RecordPosition();
        }

        private void FixedUpdate()
        {
            if (!_invalidSlopeDetected && !_voidDetected && m_hasInput)
            {
                Vector3 vel = m_inputDir * playerStatCollection.GetStat(PlayerStatType.MoveSpeed);
                vel.y = m_rigidBody.velocity.y;
                //vel.y = 0;
                m_rigidBody.velocity = vel;

                // Rotate character towards movement direction
                if (m_inputDir != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(m_inputDir);
                    m_rigidBody.rotation = Quaternion.Slerp(m_rigidBody.rotation, targetRotation, Time.fixedDeltaTime * m_rotationSpeed);

                    leftDynamicBone.m_Damping = 0.45f;
                    rightDynamicBone.m_Damping = 0.45f;
                    leftDynamicBone.UpdateParameters();
                    rightDynamicBone.UpdateParameters();
                }

                if (_movementRaycastHit.collider != null)
                {
                    transform.position = transform.position.SetY(_movementRaycastHit.point.y);
                }
            }
            else
            {
                m_rigidBody.velocity = Vector3.zero;

                leftDynamicBone.m_Damping = 0.01f;
                rightDynamicBone.m_Damping = 0.01f;
                leftDynamicBone.UpdateParameters();
                rightDynamicBone.UpdateParameters();
            }

            _animator.SetFloat("Velocity", m_rigidBody.velocity.magnitude);
        }
    }
}
