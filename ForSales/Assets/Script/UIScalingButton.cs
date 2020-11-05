using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIScalingButton : Button
{
	public Transform scalingTarget;
	public Vector3 originScale = Vector3.one;
	public Vector3 pressedScale = new Vector3(0.9f, 0.9f, 0.9f);
	public float scalingDuration = 0.05f;
	public float restoreScalingDuration = 0.05f;
	public object param;
	private CallbackButton touchedCallback;
	public UnityEngine.EventSystems.PointerEventData lastPointerEventData;
	public void Touched()
	{
		if (null != touchedCallback)
			touchedCallback(this);

	}
	public void SetTouchedCallback(CallbackButton callback)
	{
		touchedCallback += callback;
	}

	public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
	{
		lastPointerEventData = eventData;
		base.OnPointerDown(eventData);
		
		
		if (interactable && null != scalingTarget)
		{
			if (scalingDuration == 0f)
				scalingTarget.localScale = pressedScale;
			else
				UITweener.TweenScale(scalingTarget.gameObject, scalingTarget.localScale, pressedScale, scalingDuration, 0f, 1, LeanTweenType.linear, null, LeanTweenType.linear, true);
		}
	}
	public override void OnPointerClick(PointerEventData eventData)
	{
		lastPointerEventData = eventData;
		base.OnPointerClick(eventData);
		if(interactable)
		{
			if(null != scalingTarget)
			{
				if (scalingDuration == 0f)
					scalingTarget.localScale = originScale;
				else
					UITweener.TweenScale(scalingTarget.gameObject, scalingTarget.localScale, originScale, restoreScalingDuration, 0f, 1, LeanTweenType.linear, null, LeanTweenType.linear, true);
			}
			Touched();
		}
	}
}
