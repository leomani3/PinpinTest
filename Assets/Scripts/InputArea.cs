using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pinpin
{
    public class InputArea : Graphic, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform m_rectTransform;

        private int m_touchCount = 0;

        [HideInInspector]
        public Vector2 touchPosition;
        public Vector2 startTouchPosition { get; private set; }
        public Vector2 touchPosRatio { get; private set; }
        public Vector2 startTouchPosRatio { get; private set; }


        protected override void OnEnable()
        {
            m_touchCount = 0;
            base.OnEnable();
        }

        public bool isMoving
        {
            get { return (m_touchCount > 0); }
        }

        public void OnPointerDown(PointerEventData ped)
        {
            m_touchCount++;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, ped.position, ped.pressEventCamera, out touchPosition))
            {
                return;
            }

            touchPosition *= canvas.scaleFactor;
            startTouchPosition = touchPosition;

            touchPosRatio = new Vector2(touchPosition.x / Screen.width * 2, touchPosition.y / Screen.height * 2);
            startTouchPosRatio = new Vector2(startTouchPosition.x / Screen.width * 2, startTouchPosition.y / Screen.height * 2);
        }

        public void OnDrag(PointerEventData ped)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, ped.position, ped.pressEventCamera, out touchPosition))
            {
                return;
            }
            touchPosition *= canvas.scaleFactor;
            touchPosRatio = new Vector2(touchPosition.x / Screen.width * 2, touchPosition.y / Screen.height * 2);
        }

        public void OnPointerUp(PointerEventData ped)
        {
            if (m_touchCount > 0)
                m_touchCount--;
            touchPosRatio = Vector2.zero;
            touchPosition = Vector2.zero;
        }
    }
}
