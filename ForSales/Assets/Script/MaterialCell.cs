using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialCell : UIScrollCellBase
{
	public Text mtrl_name;
	public Text prime_cost;
	public Text accrue_use_num;
	public Text has_num;
	public UIScalingButton bt_minus;
	public UIScalingButton bt_plus;
	MaterialData cellData;

	public override void SetCellData(object data)
	{
		base.SetCellData(data);
		cellData = (MaterialData)data;
		mtrl_name.text = cellData.name;
		prime_cost.text = string.Format("단가  : {0}", Definitions.NumberFormatPrice(cellData.prime_cost));
		accrue_use_num.text = string.Format("총 소모 수량 : {0}개", cellData.GetUseNum());
		has_num.text = cellData.RemainNum.ToString();

	}
	public void TouchMinus(UIScalingButton button)
	{
		if (null == cellData)
			return;
		++cellData.hasNum;
		DataManager.Instance.UpdateMaterialData(cellData);
	}
	public void TouchPlus(UIScalingButton button)
	{
		if (null == cellData)
			return;
		--cellData.hasNum;
		DataManager.Instance.UpdateMaterialData(cellData);
	}

	private void Awake()
	{
		
	}
}
