using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CellDataBase
{
	public string ToJson()
	{
		return JsonUtility.ToJson(this);
	}
}
public class UIScrollCellBase : MonoBehaviour
{
	public UIScalingButton buttonComp;
	UIScrollRect scrollRect;
	UIDragEventPasser dragEventPasser;
	object cellData;
	public void SetScrollView(UIScrollRect scrollRect)
	{
		this.scrollRect = scrollRect;
		if (null == dragEventPasser)
			dragEventPasser = GetComponent<UIDragEventPasser>();
		if (null == dragEventPasser)
		{
			dragEventPasser = gameObject.AddComponent<UIDragEventPasser>();
			dragEventPasser.SetScrollRect(scrollRect);
		}
	}
	public virtual void SetCellData(object data)
	{
		cellData = data;
	}
	public virtual object GetCellData()
	{
		return cellData;
	}
	public virtual void Refresh()
	{
		SetCellData(cellData);
	}
	public virtual void Reset()
	{

	}
}
