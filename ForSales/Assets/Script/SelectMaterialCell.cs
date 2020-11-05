using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMaterialCell : UIScrollCellBase
{
	public Text mtrl_name;
	public Text prime_cost;
	public Text has_num;
	MaterialData cellData;

	public override void SetCellData(object data)
	{
		cellData = (MaterialData)data;
		mtrl_name.text = cellData.name;
		prime_cost.text = Definitions.NumberFormatPrice(cellData.prime_cost);
		has_num.text = string.Format("남은수량 : {0}", cellData.RemainNum);
	}
	public override object GetCellData()
	{
		return cellData;
	}
}
