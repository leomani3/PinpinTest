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

        private bool m_hasInput = false;
        private Vector3 m_inputDir = Vector3.zero;
        private Camera m_mainCamera;
        private Animator _animator;

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

        private void Update()
        {
            GetInput();
            playerPositionData.data = transform.position;
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
