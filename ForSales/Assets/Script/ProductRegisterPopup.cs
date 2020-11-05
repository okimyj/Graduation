using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductRegisterPopup : UIBase
{
	public Text popup_title;
	public UIScrollRect ScrollView_mtrl;
	public Text total_prime_cost;
	public UIScalingButton bt_addMtrl;
	public InputField InputField_name;
	public InputField InputField_costPerHour;
	public InputField InputField_useHour;
	public InputField InputField_addCost;
	public InputField InputField_commission;
	public InputField InputField_transfort_customer;
	public InputField InputField_transfort_me;
	public Text final_recommend_price;
	public InputField InputField_final_price;
	public UIScalingButton bt_ok;
	public List<object> selectedMaterialDatas = new List<object>();

	ProductData curProductData;
	CallbackObject finishedCallback;
	public void SetProductData(ProductData productData, CallbackObject finishedCallback)
	{
		curProductData = productData;
		this.finishedCallback = finishedCallback;
		if(null == curProductData)
		{
			popup_title.text = "상품 등록";
			curProductData = new ProductData("", null, 0, 0, 0, 0, 0, 0, 0);
		}
		else
		{
			popup_title.text = "상품 수정";
		}
		
		InputField_name.text = curProductData.name;
		InputField_costPerHour.text = curProductData.profit_per_hour.ToString();
		InputField_useHour.text = curProductData.need_hour.ToString();
		InputField_addCost.text = curProductData.add_cost.ToString();
		InputField_commission.text = curProductData.commission.ToString();
		InputField_transfort_customer.text = curProductData.transfort_customer.ToString();
		InputField_transfort_me.text = curProductData.transfort_me.ToString();
		InputField_final_price.text = curProductData.final_sale_price.ToString();
		selectedMaterialDatas.Clear();
		if (null != curProductData.needMtrlMap)
		{
			foreach (KeyValuePair<string, int> kv in curProductData.needMtrlMap)
			{
				ProductMaterialCell.CellData cellData = new ProductMaterialCell.CellData(DataManager.Instance.GetMaterialByID(kv.Key), kv.Value);
				selectedMaterialDatas.Add(cellData);
			}
		}
		RefreshMaterialScrollView();
	}
	public void RefreshRecommendPrice()
	{
		final_recommend_price.text = string.Format("총 비용 : {0:#,##0} 원 수수료 비용 : {1:#,##0} 원 희망 수익 : {2:#,##0} 원 \n <size=22>최종 추천 판매가 : {3:#,##0} 원</size>", curProductData.GetTotalCost(), curProductData.GetCommissionCost(), curProductData.GetProfitMargin(), curProductData.GetRecommendPrice());// curProductData.GetRecommendPrice().ToString();
	}
	public void TouchAddMaterial(UIScalingButton button)
	{
		MaterialSelectPopup popup = (MaterialSelectPopup)MainScene.Instance.ShowMaterialSelectPopup();
		popup.SetSelectedCallback(MaterialSelectedCallback);
	}
	
	public void MaterialSelectedCallback(object obj)
	{
		MaterialData data = (MaterialData)obj;
		bool alreadyHas = false;
		for (int i=0; i < selectedMaterialDatas.Count; ++i)
		{
			ProductMaterialCell.CellData hasCellData = (ProductMaterialCell.CellData)selectedMaterialDatas[i];
			if (hasCellData.mtrlData.id == data.id)
			{
				alreadyHas = true;
				hasCellData.num = hasCellData.num + 1;
				break;
			}
		}
		if (!alreadyHas)
		{
			ProductMaterialCell.CellData cellData = new ProductMaterialCell.CellData(data, 1, MaterialCellRefreshCallback, MaterialCellRemoveCallback);
			selectedMaterialDatas.Add(cellData);
		}
		RefreshMaterialScrollView();
	}
	public void TouchMaterialCell(UIScalingButton button)
	{
		ProductMaterialCell cell = (ProductMaterialCell)button.param;
		ProductMaterialCell.CellData cellData = (ProductMaterialCell.CellData)cell.GetCellData();
		if (null != cellData)
		{
			MaterialRegisterPopup ui = MainScene.Instance.ShowMaterialRegisterPopup() as MaterialRegisterPopup;
			ui.SetMaterialData(cellData.mtrlData, MaterialUpdateCallback);
		}
	}
	public void MaterialCellRefreshCallback(object cellUI)
	{
		ProductMaterialCell cell = (ProductMaterialCell)cellUI;
		RefreshMaterialScrollView();
	}
	public void MaterialCellRemoveCallback(object cellUI)
	{
		ProductMaterialCell cell = (ProductMaterialCell)cellUI;
		ProductMaterialCell.CellData targetCellData = (ProductMaterialCell.CellData)cell.GetCellData();
		for (int i=0; i< selectedMaterialDatas.Count; ++i)
		{
			ProductMaterialCell.CellData cellData = (ProductMaterialCell.CellData)selectedMaterialDatas[i];
			if (cellData.mtrlData.id == targetCellData.mtrlData.id)
			{
				selectedMaterialDatas.RemoveAt(i);
				break;
			}
		}
		RefreshMaterialScrollView();
	}
	void RefreshMaterialScrollView()
	{
		ScrollView_mtrl.AddCells(selectedMaterialDatas);
		int sum = 0;
		for (int i=0; i < selectedMaterialDatas.Count; ++i)
		{
			ProductMaterialCell.CellData cellData = (ProductMaterialCell.CellData)selectedMaterialDatas[i];
			sum += cellData.mtrlData.prime_cost * cellData.num;
		}
		total_prime_cost.text = string.Format("재료 원가 : {0}", Definitions.NumberFormatPrice(sum));
		RefreshRecommendPrice();
	}
	public void MaterialUpdateCallback(object data)
	{
		MaterialData mtrlData = null != data? (MaterialData)data : null; 
		if (null != mtrlData)
		{
			DataManager.Instance.UpdateMaterialData((MaterialData)mtrlData);
			for (int i = 0; i < selectedMaterialDatas.Count; ++i)
			{
				ProductMaterialCell.CellData hasCellData = (ProductMaterialCell.CellData)selectedMaterialDatas[i];
				if (hasCellData.mtrlData.id == mtrlData.id)
				{
					hasCellData.mtrlData = mtrlData;
					break;
				}
			}
		}
		RefreshMaterialScrollView();
	}
	public void TouchOK(UIScalingButton button)
	{
		Dictionary<string, int> mtrlMap = new Dictionary<string, int>();
		for(int i=0; i< selectedMaterialDatas.Count; ++i)
		{
			ProductMaterialCell.CellData cellData = (ProductMaterialCell.CellData)selectedMaterialDatas[i];
			mtrlMap[cellData.mtrlData.id] = cellData.num;
		}
		int add_cost = int.Parse(InputField_addCost.text);
		int commission = int.Parse(InputField_commission.text);
		int transfort_customer = int.Parse(InputField_transfort_customer.text);
		int transfort_me = int.Parse(InputField_transfort_me.text);
		int profit_per_hour = int.Parse(InputField_costPerHour.text);
		int need_hour = int.Parse(InputField_useHour.text);
		int final_sale_price = int.Parse(InputField_final_price.text);
		
		curProductData.name = InputField_name.text;
		curProductData.needMtrlMap = mtrlMap;
		curProductData.add_cost = add_cost;
		curProductData.commission = commission;
		curProductData.transfort_customer = transfort_customer;
		curProductData.transfort_me = transfort_me;
		curProductData.profit_per_hour = profit_per_hour;
		curProductData.need_hour = need_hour;
		curProductData.final_sale_price = final_sale_price;
		
		DataManager.Instance.UpdateProductData(curProductData);
		if(null != finishedCallback)
			finishedCallback(curProductData);
		BackPressed();
	}
	public void OnEndEditProfitPerHour(string v)
	{
		if (null == curProductData)
			return;
		int value = 0;
		int.TryParse(v, out value);
		curProductData.profit_per_hour = value;
		RefreshRecommendPrice();
	}
	public void OnEndEditUseHour(string v)
	{
		if (null == curProductData)
			return;
		int value = 0;
		int.TryParse(v, out value);
		curProductData.need_hour = value;
		RefreshRecommendPrice();
	}
	public void OnEndEditAddCost(string v)
	{
		if (null == curProductData)
			return;
		int value = 0;
		int.TryParse(v, out value);
		curProductData.add_cost= value;
		RefreshRecommendPrice();
	}
	public void OnEndEditCommision(string v)
	{
		if (null == curProductData)
			return;
		int value = 0;
		int.TryParse(v, out value);
		curProductData.commission= value;
		RefreshRecommendPrice();
	}
	public void OnEndEditTransfortCustomer(string v)
	{
		if (null == curProductData)
			return;
		int value = 0;
		int.TryParse(v, out value);
		curProductData.transfort_customer= value;
		RefreshRecommendPrice();
	}
	public void OnEndEditTransfortMe(string v)
	{
		if (null == curProductData)
			return;
		int value = 0;
		int.TryParse(v, out value);
		curProductData.transfort_me= value;
		RefreshRecommendPrice();
	}
	protected override void Awake()
	{
		base.Awake();
		bt_addMtrl.SetTouchedCallback(TouchAddMaterial);
		bt_ok.SetTouchedCallback(TouchOK);
		ScrollView_mtrl.SetCellTouchedCallback(TouchMaterialCell);
		InputField_costPerHour.onEndEdit.AddListener(OnEndEditProfitPerHour);
		InputField_useHour.onEndEdit.AddListener(OnEndEditUseHour);
		InputField_addCost.onEndEdit.AddListener(OnEndEditAddCost);
		InputField_commission.onEndEdit.AddListener(OnEndEditCommision);
		InputField_transfort_customer.onEndEdit.AddListener(OnEndEditTransfortCustomer);
		InputField_transfort_me.onEndEdit.AddListener(OnEndEditTransfortMe);
		
	}
}
