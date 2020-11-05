using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UITab : Toggle
{
	public GameObject selectedObj;
	public GameObject deselectedObj;
	protected override void Awake()
	{
		base.Awake();
		onValueChanged.AddListener(SetState);
		SetState(isOn);
	}
	private void SetState(bool isOn)
	{
		if (null != selectedObj && selectedObj.activeSelf != isOn)
		{
			selectedObj.SetActive(isOn);
		}
		if (null != deselectedObj && deselectedObj.activeSelf == isOn)
			deselectedObj.SetActive(!isOn);
		
	}
}
