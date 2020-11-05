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
	// Start is called before the first frame update
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
	void Start()
    {
		/*
		for (int i = 0; i < 5; ++i)
			productCellDatas.Add(new ProductCell.CellData(string.Format("productName_{0}", i), i*500, i*100000, 0));
		ScrollView_products.AddCells(productCellDatas);

		List<string> cellStrList = new List<string>();
		for (int i=0; i< productCellDatas.Count; ++i)
		{
			cellStrList.Add(JsonUtility.ToJson((ProductCell.CellData)productCellDatas[i]));
		}
		DataManager.SaveData(DataManager.KEY_PRODUCT_MAP, JsonUtility.ToJson(new DataManager.Serialization<string>(cellStrList)));
		*/
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
