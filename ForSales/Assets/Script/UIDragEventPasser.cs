using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragEventPasser : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public UIScrollRect targetScrollRect;
	public void SetScrollRect(UIScrollRect targetScrollRect)
	{
		this.targetScrollRect = targetScrollRect;
	}
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (null != targetScrollRect)
		{
			targetScrollRect.SendMessage("OnBeginDrag", eventData, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (null != targetScrollRect)
		{
			targetScrollRect.SendMessage("OnDrag", eventData, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{

		if (null != targetScrollRect)
		{
			targetScrollRect.SendMessage("OnEndDrag", eventData, SendMessageOptions.DontRequireReceiver);
		}
	}
}
