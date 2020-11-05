using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialRegisterPopup : UIBase
{
	public Text popup_title;
	public InputField InputField_name;
	public InputField InputField_prime_cost;
	public InputField InputField_has_num;
	public UIScalingButton bt_minus;
	public UIScalingButton bt_plus;
	public UIScalingButton bt_ok;
	MaterialData curData;
	CallbackObject callback;
	public void SetMaterialData(MaterialData data, CallbackObject callback)
	{
		this.callback = callback;
		curData = data;
		if(null == curData)
		{
			popup_title.text = "재료 등록";
			InputField_name.text = "";
			InputField_prime_cost.text = "0";
			InputField_has_num.text = "0";
		}
		else
		{
			popup_title.text = "재료 수정";
			InputField_name.text = curData.name;
			InputField_prime_cost.text = curData.prime_cost.ToString();
			InputField_has_num.text = curData.RemainNum.ToString();
		}
	}
	public void TouchOK(UIScalingButton button)
	{
		if(null != callback)
		{
			int prime_cost = 0;
			if (!int.TryParse(InputField_prime_cost.text, out prime_cost))
				prime_cost = 0;
			int has_num = 0;
			if (!int.TryParse(InputField_has_num.text, out has_num))
				has_num = 0;
			if (null == curData)
			{
				curData = new MaterialData(InputField_name.text, prime_cost, has_num, 0);
			}
			else
			{
				curData.name = InputField_name.text;
				curData.hasNum = has_num;
				curData.prime_cost = prime_cost; 
			}
			callback(curData);
		}
		BackPressed();
	}
	public void TouchMinus(UIScalingButton button)
	{
		if (null != curData)
		{
			if(curData.RemainNum > 0)
			{
				--curData.hasNum;
				InputField_has_num.text = curData.RemainNum.ToString();
			}
		}
		else
		{
			int num = int.Parse(InputField_has_num.text);
			if (num > 0)
				InputField_has_num.text = (--num).ToString();
		}
	}
	public void TouchPlus(UIScalingButton button)
	{
		if (null != curData)
		{
			++curData.hasNum;
			InputField_has_num.text = curData.RemainNum.ToString();
		}
		else
		{
			int num = int.Parse(InputField_has_num.text);
			InputField_has_num.text = (++num).ToString();
		}
	}
	protected override void Awake()
	{
		base.Awake();
		bt_ok.SetTouchedCallback(TouchOK);
		bt_minus.SetTouchedCallback(TouchMinus);
		bt_plus.SetTouchedCallback(TouchPlus);
	}
}
