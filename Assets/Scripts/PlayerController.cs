using MyBox;
using System.Collections.Generic;
using UnityEngine;

namespace Pinpin
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_rigidBody;
        [SerializeField] private float m_speed = 5.0f;
        [SerializeField] private float m_rotationSpeed = 10.0f;
        [SerializeField] private Vector3Data playerPositionData;
        [SerializeField] private LayerMask ressourceLayer;
        [SerializeField] private float ressourceDetectionRadius;

        [Separator("Tools")]
        [SerializeField] private GameObject axe;
        [SerializeField] private GameObject pickaxe;

        private bool m_hasInput = false;
        private Vector3 m_inputDir = Vector3.zero;
        private Camera m_mainCamera;
        private Animator _animator;

        //detect ressources
        private Collider[] _detectedRessourceColliders;
        private List<Ressource> _detectedRessources = new List<Ressource>();

        private void Reset()
        {
            print("reset");
            m_rigidBody = GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            m_mainCamera = Camera.main;
            _animator = GetComponentInChildren<Animator>();
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

        private void DetectNearbyRessource()
        {
            _detectedRessourceColliders = Physics.OverlapSphere(transform.position, ressourceDetectionRadius, ressourceLayer);

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
                        axe.SetActive(true); //Todo : remplacer par un tween
                        break;
                    case RessourceType.Stone:
                        _animator.SetInteger("HarvestingAnimationID", 1);
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
            playerPositionData.data = transform.position;

            DetectNearbyRessource();
        }

        private void FixedUpdate()
        {
            if (m_hasInput)
            {
                Vector3 vel = m_inputDir * m_speed;
                vel.y = m_rigidBody.velocity.y;
                m_rigidBody.velocity = vel;

                // Rotate character towards movement direction
                if (m_inputDir != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(m_inputDir);
                    m_rigidBody.rotation = Quaternion.Slerp(m_rigidBody.rotation, targetRotation, Time.fixedDeltaTime * m_rotationSpeed);
                }
            }
            else
            {
                m_rigidBody.velocity = Vector3.zero;
            }

            _animator.SetFloat("Velocity", m_rigidBody.velocity.magnitude);

        }
    }
}
