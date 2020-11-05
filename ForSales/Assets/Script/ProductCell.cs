using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductCell : UIScrollCellBase
{
	public Text product_name;
	public Text prime_cost;
	public Text sale_price;
	public Text margin;
	public Text sale_num;
	public Text accrue_profit;
	Button bg;
	public UIScalingButton bt_minus;
	public UIScalingButton bt_plus;
	public UIScalingButton bt_this;
	ProductData cellData;
	public override void SetCellData(object data)
	{
		if (null == data)
			return;
		base.SetCellData(data);
		cellData = (ProductData)data;
		product_name.text = cellData.name;
		prime_cost.text = string.Format("단가 : {0}", Definitions.NumberFormatPrice(cellData.GetPrimeCost()));
		sale_price.text = string.Format("판매가 : {0}" , Definitions.NumberFormatPrice(cellData.final_sale_price));
		margin.text = string.Format("마진 : {0}", Definitions.NumberFormatPrice(cellData.GetMargin()));
		sale_num.text = cellData.sale_num.ToString();
		accrue_profit.text = string.Format("누적수익 : {0}", Definitions.NumberFormatPrice(cellData.GetAccureProfit()));
		
	}
	public void TouchMinus(UIScalingButton button)
	{
		--cellData.sale_num;
		cellData.sale_num = Mathf.Max(cellData.sale_num, 0);
		Refresh();
	}
	public void TouchPlus(UIScalingButton button)
	{
		++cellData.sale_num;
		Refresh();
	}
	
	public override void Refresh()
	{
		SetCellData(cellData);
	}
	private void Awake()
	{
		bt_minus.SetTouchedCallback(TouchMinus);
		bt_plus.SetTouchedCallback(TouchPlus);
	}
	
}
