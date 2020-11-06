using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainUI : UIBase
{
	public UIScrollRect ScrollView_products;
	public UIScalingButton bt_addProduct;
	public UIScrollRect ScrollView_mtrl;
	public UIScalingButton bt_addMtrl;
	public UITab toggle_product;
	public UITab toggle_mtrl;
	public Text total_profit;
	List<object> productCellDatas = new List<object>();
	List<object> materialCellDatas = new List<object>();
	public void TouchAddProduct(UIScalingButton button)
	{
		ProductRegisterPopup ui = MainScene.Instance.ShowProductRegisterPopup() as ProductRegisterPopup;
		ui.SetProductData(null, RefreshProductList);
	}
	public void RefreshProductList(object data)
	{
		Dictionary<string, ProductData> map = DataManager.Instance.GetProductMap();
		productCellDatas.Clear();
		foreach(KeyValuePair<string, ProductData> kv in map)
		{
			productCellDatas.Add(kv.Value);
		}
		ScrollView_products.AddCells(productCellDatas);
	}
	public void RecalcTotalProfit()
	{
		int total = 0;
		Dictionary<string, ProductData> map = DataManager.Instance.GetProductMap();
		foreach (KeyValuePair<string, ProductData> kv in map)
			total += kv.Value.GetAccureProfit();
		total_profit.text = string.Format("총 누적 수익 : {0}", Definitions.NumberFormatPrice(total));
	}
	public void TouchProductCellInScrollRect(UIScalingButton button)
	{
		ProductCell cellUI = (ProductCell)button.param;
		ProductData cellData = (ProductData)cellUI.GetCellData();
		ProductRegisterPopup ui = MainScene.Instance.ShowProductRegisterPopup() as ProductRegisterPopup;
		ui.SetProductData(cellData, RefreshProductList);
	}
	public void RefreshMaterialList(object data)
	{
		Dictionary<string, MaterialData> map = DataManager.Instance.GetMaterialMap();
		materialCellDatas.Clear();
		foreach(KeyValuePair<string, MaterialData> kv in map)
		{
			materialCellDatas.Add(kv.Value);
		}
		ScrollView_mtrl.AddCells(materialCellDatas);
	}
	public void TouchMtrlCellInScrollRect(UIScalingButton button)
	{
		MaterialCell cellUI = (MaterialCell)button.param;
		MaterialData cellData = (MaterialData)cellUI.GetCellData();
		MaterialRegisterPopup ui = MainScene.Instance.ShowMaterialRegisterPopup() as MaterialRegisterPopup;
		ui.SetMaterialData(cellData, RefreshMaterialList);
	}
	public void ToggleProduct(bool isOn)
	{
		if (isOn)
			RefreshProductList(null);
	}
	public void ToggleMtrl(bool isOn)
	{
		if(isOn)
			RefreshMaterialList(null);
	}
	protected override void Awake()
	{
		base.Awake();
		DataManager.Instance.Init();
		bt_addProduct.SetTouchedCallback(TouchAddProduct);
		ScrollView_products.SetCellTouchedCallback(TouchProductCellInScrollRect);
		ScrollView_mtrl.SetCellTouchedCallback(TouchMtrlCellInScrollRect);

		toggle_product.onValueChanged.AddListener(ToggleProduct);
		toggle_mtrl.onValueChanged.AddListener(ToggleMtrl);
		ToggleProduct(toggle_product.isOn);
		ToggleMtrl(toggle_product.isOn);
	}
	
}
