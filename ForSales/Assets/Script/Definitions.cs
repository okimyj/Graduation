using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void CallbackGameObject(GameObject obj);
public delegate void CallbackButton(UIScalingButton obj);
public delegate void CallbackObject(object obj);
public class Definitions 
{
	public static string NumberFormatPrice(int price)
	{
		return string.Format("{0:#,##0} 원", price);
		//return price.ToString();
	}
	
}