using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
	public Canvas[] canvases;
	public UIScalingButton uiCloseButton;
	protected virtual void Awake()
	{
		if (uiCloseButton != null)
			uiCloseButton.SetTouchedCallback(BackLayerTouched);
	}

	protected virtual void BackLayerTouched(UIScalingButton layer)
	{
		BackPressed();
	}
	public virtual bool BackPressed()
	{
	
		UINavigationStack.Instance.Pop(this);
		return false;
	}
}
