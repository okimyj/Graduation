using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITweener
{
	public static LTDescr TweenPosition(GameObject target, Vector3 start, Vector3 end, float duration, float delay, int loopCount = 1, LeanTweenType loopType = LeanTweenType.linear, System.Action callback = null, LeanTweenType tweenType = LeanTweenType.linear, bool useUnScaledTime = false)
	{
		target.transform.localPosition = start;
		return LeanTween.moveLocal(target, end, duration).SetBaseTweenData(delay, loopType, callback, loopCount).setEase(tweenType).setIgnoreTimeScale(useUnScaledTime);
	}
	public static LTDescr TweenWorldPosition(GameObject target, Vector3 start, Vector3 end, float duration, float delay, int loopCount = 1, LeanTweenType loopType = LeanTweenType.linear, System.Action callback = null, LeanTweenType tweenType = LeanTweenType.linear, bool useUnScaledTime = false)
	{
		target.transform.position = start;
		return LeanTween.move(target, end, duration).SetBaseTweenData(delay, loopType, callback, loopCount).setEase(tweenType).setIgnoreTimeScale(useUnScaledTime);
	}

	public static LTDescr TweenRotation(GameObject target, Vector3 start, Vector3 end, float duration, float delay, int loopCount = 1, LeanTweenType loopType = LeanTweenType.linear, System.Action callback = null, LeanTweenType tweenType = LeanTweenType.linear, bool useUnScaledTime = false)
	{
		target.transform.eulerAngles = start;
		return LeanTween.rotateLocal(target, end, duration).SetBaseTweenData(delay, loopType, callback, loopCount).setEase(tweenType).setIgnoreTimeScale(useUnScaledTime);
	}

	public static LTDescr TweenScale(GameObject target, Vector3 start, Vector3 end, float duration, float delay, int loopCount = 1, LeanTweenType loopType = LeanTweenType.linear, System.Action callback = null, LeanTweenType tweenType = LeanTweenType.linear, bool useUnScaledTime = false)
	{
		target.transform.localScale = start;
		return LeanTween.scale(target, end, duration).SetBaseTweenData(delay, loopType, callback, loopCount).setEase(tweenType).setIgnoreTimeScale(useUnScaledTime);
	}

	public static LTDescr TweenAlphaForCanvasGroup(CanvasGroup target, float start, float end, float duration, float delay, int loopCount = 1, LeanTweenType loopType = LeanTweenType.linear, System.Action callback = null, LeanTweenType tweenType = LeanTweenType.linear, bool useUnScaledTime = false)
	{
		target.alpha = start;
		return LeanTween.alphaCanvasGroup(target.gameObject, end, duration).setDelay(delay).SetBaseTweenData(delay, loopType, callback, loopCount).setEase(tweenType).setIgnoreTimeScale(useUnScaledTime);
	}

	public static LTDescr TweenAlpha(GameObject target, float start, float end, float duration, float delay, int loopCount = 1, LeanTweenType loopType = LeanTweenType.linear, System.Action callback = null, bool applyChildren = true, LeanTweenType tweenType = LeanTweenType.linear, bool useUnScaledTime = false)
	{
		LTDescr tween = null;
		if (applyChildren)
		{
			CanvasGroup group = target.GetComponent<CanvasGroup>();
			if (null == group)
			{
				group = target.AddComponent<CanvasGroup>();
			}
			group.alpha = start;
			tween = LeanTween.alphaCanvasGroup(target, end, duration).setDelay(delay).SetBaseTweenData(delay, loopType, callback, loopCount).setEase(tweenType).setIgnoreTimeScale(useUnScaledTime);
		}
		else
			tween = LeanTween.alpha(target, end, duration).setDelay(delay).SetBaseTweenData(delay, loopType, callback, loopCount).setEase(tweenType).setIgnoreTimeScale(useUnScaledTime);
		return tween;
	}

	//public static LTDescr TweenAlpha


	public class ToastTween
	{
		public delegate void HandleTweenFinished(ToastTween toastTween);
		public ToastTween(GameObject obj, float tweenTime, float duration, Vector3 showPos, Vector3 hidePos, HandleTweenFinished finishedHandler, float loopDelay = 0, int loopCount = 0)
		{
			this.obj = obj;
			_transform = obj.transform;
			SetTweenData(tweenTime, duration, showPos, hidePos, loopDelay, loopCount);
			this.finishedHandler = finishedHandler;
		}
		public void SetTweenData(float tweenTime, float duration, Vector3 showPos, Vector3 hidePos, float loopDelay, int loopCount)
		{
			this.tweenTime = tweenTime;
			this.duration = duration;
			this.showPos = showPos;
			this.hidePos = hidePos;
			this.loopDelay = loopDelay;
			this.loopCount = loopCount;
			playCount = 0;
		}
		public void Hide()
		{
			if (null != alphaTween && alphaTween.uniqueId == alphaTweenID)
				alphaTween.cancel();
			if (null != posTween && posTween.uniqueId == posTweenID)
				posTween.cancel();
			if (IsShown())
				obj.SetActive(false);
		}
		public void Show()
		{
			if (!IsShown())
				obj.SetActive(true);
			playCount = 0;
			PlayTween(true);
		}
		public void PlayTween(bool isShowTween)
		{
			if (null != alphaTween && alphaTween.uniqueId == alphaTweenID)
				alphaTween.cancel();
			if (null != posTween && posTween.uniqueId == posTweenID)
				posTween.cancel();


			bool isFirstShow = playCount == 0;
			if (isShowTween)
				++playCount;
			float delay = isShowTween ? (isFirstShow ? 0f : loopDelay) : duration;
			alphaTween = UITweener.TweenAlpha(obj, isShowTween ? 0f : 1f, isShowTween ? 1f : 0f, tweenTime, delay, 1, LeanTweenType.linear);
			alphaTweenID = alphaTween.uniqueId;
			posTween = UITweener.TweenPosition(obj, isFirstShow ? hidePos : _transform.localPosition, isShowTween ? showPos : hidePos, tweenTime, delay, 1, LeanTweenType.linear, isShowTween ? (System.Action)ShowTweenFinished : HideTweenFinished);
			posTweenID = posTween.uniqueId;
		}
		void ShowTweenFinished()
		{
			PlayTween(false);
		}
		void HideTweenFinished()
		{
			if (loopCount > playCount)
				PlayTween(true);
			else
			{
				obj.SetActive(false);
				if (null != finishedHandler)
					finishedHandler(this);
			}
		}

		public bool IsShown()
		{
			return obj.activeInHierarchy;
		}
		GameObject obj;
		Transform _transform;
		float tweenTime;
		float duration;
		HandleTweenFinished finishedHandler;
		Vector3 showPos;
		Vector3 hidePos;
		float loopDelay;
		int loopCount;

		int playCount;

		LTDescr alphaTween;
		LTDescr posTween;
		int alphaTweenID;
		int posTweenID;
	}

}
