using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace orangesGame
{
	public class CustomEventHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler, IEventSystemHandler
	{
		public Action<PointerEventData> ActionDrag;

		public Action<PointerEventData> ActionPointerClick;

		public Action<PointerEventData> ActionPointerDown;

		public Action<PointerEventData> ActionPointerUp;

		public Action<PointerEventData> ActionPointerEnter;

		public Action<PointerEventData> ActionPointerExit;

		public void OnDrag(PointerEventData eventData)
		{
			if (ActionDrag != null)
			{
				ActionDrag(eventData);
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (ActionPointerClick != null)
			{
				ActionPointerClick(eventData);
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (ActionPointerDown != null)
			{
				ActionPointerDown(eventData);
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (ActionPointerEnter != null)
			{
				ActionPointerEnter(eventData);
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (ActionPointerExit != null)
			{
				ActionPointerExit(eventData);
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (ActionPointerUp != null)
			{
				ActionPointerUp(eventData);
			}
		}
	}
}
