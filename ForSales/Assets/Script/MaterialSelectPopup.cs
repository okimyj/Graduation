using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSelectPopup : UIBase
{
	public UIScrollRect ScrollView_mtrl;
	public UIScalingButton bt_addMtrl;
	CallbackObject selectedCallback;
	public void SetSelectedCallback(CallbackObject callback)
	{
		selectedCallback = callback;
	}
	void Refresh()
	{
		Dictionary<string, MaterialData> map = DataManager.Instance.GetMaterialMap();
		List<object> cellDatas = new List<object>();
		foreach(KeyValuePair<string, MaterialData> kv in map)
		{
			cellDatas.Add(kv.Value);
		}
		ScrollView_mtrl.AddCells(cellDatas);
		ScrollView_mtrl.SetCellTouchedCallback(TouchCellInScrollView);
	}
	public void TouchCellInScrollView(UIScalingButton button)
	{
		if(null != selectedCallback)
		{
			UIScrollCellBase cell = (UIScrollCellBase )button.param;
			selectedCallback(cell.GetCellData());
		}
		BackPressed();
	}
	public void TouchAddMaterial(UIScalingButton button)
	{
		MaterialRegisterPopup ui = MainScene.Instance.ShowMaterialRegisterPopup() as MaterialRegisterPopup;
		ui.SetMaterialData(null, MaterialRegisterCallback);
	}
	public void MaterialRegisterCallback(object mtrlData)
	{
		if(null != mtrlData)
		{
			DataManager.Instance.UpdateMaterialData((MaterialData)mtrlData);
			Refresh();
		}
	}
	protected override void Awake()
	{
		base.Awake();
		bt_addMtrl.SetTouchedCallback(TouchAddMaterial);
	}
	
	private void OnEnable()
	{
		Refresh();
	}

}
