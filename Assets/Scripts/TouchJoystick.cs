using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pinpin
{
    [RequireComponent(typeof(InputArea))]
    public class TouchJoystick : MonoBehaviour
    {
        [SerializeField]
        private InputArea m_inputArea;
        private Camera m_mainCamera;

        public static Vector2 input { get; private set; }
        public static bool hasInput { get; private set; }

        private void Reset()
        {
            m_inputArea = GetComponent<InputArea>();
        }

        private Vector2 GetInput()
        {
            Vector2 input = Vector2.zero;
            if (m_inputArea.isMoving)
            {
                input = m_inputArea.touchPosition - m_inputArea.startTouchPosition;
                //Adapt to the screen size
                input = input / (Screen.width / 4f);
                if (input.sqrMagnitude > 1f)
                {
                    input.Normalize();
                }
            }
            return input;
        }

        private void Update()
        {
            input = GetInput();
            hasInput = input.sqrMagnitude > 0.01f;
        }
    }
}
