using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductMaterialCell : UIScrollCellBase
{
	public class CellData
	{
		public MaterialData mtrlData;
		public int num;
		public CallbackObject callbackRemove;
		public CallbackObject callbackRefresh;
		public CellData(MaterialData mtrlData, int num = 0, CallbackObject callbackRemove = null, CallbackObject callbackRefresh = null)
		{
			this.mtrlData = mtrlData;
			this.num = num;
			this.callbackRemove = callbackRemove;
			this.callbackRefresh = callbackRefresh;
		}
	}
	// Start is called before the first frame update
	public Text mtrl_name;
	public Text prime_cost;
	public UIScalingButton bt_remove;
	public UIScalingButton bt_plus;
	public UIScalingButton bt_minus;
	public Text need_num;
	CellData cellData;
	private void Awake()
	{
		bt_plus.SetTouchedCallback(TouchPlus);
		bt_minus.SetTouchedCallback(TouchMinus);
		bt_remove.SetTouchedCallback(TouchRemove);
	}
	public void TouchPlus(UIScalingButton button)
	{
		++cellData.num;
		Refresh();
	}
	public void TouchMinus(UIScalingButton button)
	{
		--cellData.num;
		cellData.num = Mathf.Min(cellData.num, 0);
		Refresh();
	}
	public void TouchRemove(UIScalingButton button)
	{
		if (null != cellData && null != cellData.callbackRemove)
		{
			cellData.callbackRemove(this);
		}
	}
	public override void Refresh()
	{
		base.Refresh();
		if(null != cellData && null != cellData.callbackRefresh)
		{
			cellData.callbackRefresh(this);
		}
	}
	public override object GetCellData()
	{
		return cellData;
	}
	public override void SetCellData(object data)
	{
		base.SetCellData(data);
		cellData = (CellData)data;
		mtrl_name.text = cellData.mtrlData.name;
		prime_cost.text = string.Format("단가 : {0}", Definitions.NumberFormatPrice(cellData.mtrlData.prime_cost));
		need_num.text = cellData.num.ToString();
	}
}
