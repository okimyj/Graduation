using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : AbstractSingleton<DataManager>
{
	public const string KEY_PRODUCT_MAP = "product_map";
	public const string KEY_MATERIAL_MAP = "material_map";
	[SerializeField]
	Dictionary<string, MaterialData> materialMap;
	[SerializeField]
	Dictionary<string, ProductData> productMap;
	bool isInit = false;
	public void Init()
	{
		if (isInit)
			return;
		materialMap = new Dictionary<string, MaterialData>();
		string strMtrlJson = ReadData(KEY_MATERIAL_MAP);
		
		if (!string.IsNullOrEmpty(strMtrlJson))
		{
			Debug.Log("strMtrlJson : " + strMtrlJson);
			SerializationMap<string, MaterialData> serializationMap = JsonUtility.FromJson(strMtrlJson, typeof(SerializationMap<string, MaterialData>)) as SerializationMap<string, MaterialData>;
			materialMap = serializationMap.ToDictionary();
		}
		
		productMap = new Dictionary<string, ProductData>();
		string strProductJson = ReadData(KEY_PRODUCT_MAP);
		if(!string.IsNullOrEmpty(strProductJson))
		{
			SerializationMap<string, ProductData> serializationMap = JsonUtility.FromJson(strProductJson, typeof(SerializationMap<string, ProductData>)) as SerializationMap<string, ProductData>;
			productMap = serializationMap.ToDictionary();
		}
	}
	public Dictionary<string, ProductData> GetProductMap()
	{
		return productMap;
	}
	public Dictionary<string, MaterialData> GetMaterialMap()
	{
		return materialMap;
	}
	public MaterialData GetMaterialByID(string id)
	{
		return materialMap.ContainsKey(id) ? materialMap[id] : null;
	}
	public void UpdateMaterialData(MaterialData data)
	{	
		materialMap[data.id] = data;
		SaveMaterialDataMap();
	}
	public void UpdateProductData(ProductData data)
	{
		productMap[data.id] = data;
		SaveProductDataMap();
	}
	void SaveMaterialDataMap()
	{
		string strJson = JsonUtility.ToJson(new SerializationMap<string, MaterialData>(materialMap));
		SaveData(KEY_MATERIAL_MAP, strJson);
	}
	void SaveProductDataMap()
	{
		string strJson = JsonUtility.ToJson(new SerializationMap<string, ProductData>(productMap));
		SaveData(KEY_PRODUCT_MAP, strJson);
	}
	public static void SaveData(string key, string strJson)
	{
		PlayerPrefs.SetString(key, strJson);
	}
	public static string ReadData(string key)
	{
		return PlayerPrefs.GetString(key, "");
	}
	
}
[System.Serializable]
public class Serialization<T>
{
	[SerializeField]
	List<T> target;
	public List<T> ToList() { return target; }

	public Serialization(List<T> target)
	{
		this.target = target;
	}
}
[System.Serializable]
public class SerializationMap<TKey, TValue> : ISerializationCallbackReceiver
{
	[SerializeField]
	List<TKey> keys;
	[SerializeField]
	List<TValue> values;
	Dictionary<TKey, TValue> target;
	public Dictionary<TKey, TValue> ToDictionary() { return target; }
	public SerializationMap(Dictionary<TKey, TValue> target)
	{
		this.target = target;
	}

	public void OnBeforeSerialize()
	{
		keys = new List<TKey>(target.Keys);
		values = new List<TValue>(target.Values);
	}

	public void OnAfterDeserialize()
	{
		var count = System.Math.Min(keys.Count, values.Count);
		target = new Dictionary<TKey, TValue>(count);
		for (var i = 0; i < count; ++i)
		{
			target.Add(keys[i], values[i]);
		}
	}
}
[System.Serializable]
public class MaterialData
{
	public string name;
	public int prime_cost;
	public int hasNum;
	public int useNum;
	public string id;
	public int RemainNum { get { return hasNum - useNum; } }
	
	
	public MaterialData(string name, int prime_cost, int hasNum, int useNum, string id="")
	{
		this.name = name;
		this.prime_cost = prime_cost;
		this.hasNum = hasNum;
		this.useNum = useNum;
		if (string.IsNullOrEmpty(id))
			id = System.DateTime.Now.ToFileTime().ToString();
		this.id = id;
	}

}
[System.Serializable]
public class ProductData : ISerializationCallbackReceiver
{
	public string id;
	public string name;
	public Dictionary<string, int> needMtrlMap = new Dictionary<string, int>();
	[SerializeField]
	List<string> mtrl_keys;
	[SerializeField]
	List<int> mtrl_values;

	public int add_cost;
	public float commission;
	public int transfort;
	public int transfort_customer;
	public int profit_per_hour;
	public float need_hour;
	public int final_sale_price;
	public int sale_num;
	public ProductData(string name, Dictionary<string, int> needMtrlMap, int add_cost, float commission, int transfort_customer, int transfort, int profit_per_hour, float need_hour, int final_sale_price, string id = "")
	{
		this.name = name;
		this.needMtrlMap = needMtrlMap;
		this.add_cost = add_cost;
		this.commission = commission;
		this.transfort_customer = transfort_customer;
		this.transfort = transfort;
		this.profit_per_hour = profit_per_hour;
		this.need_hour = need_hour;
		this.final_sale_price = final_sale_price;
		this.sale_num = 0;
		if (string.IsNullOrEmpty(id))
			id = System.DateTime.Now.ToFileTime().ToString();
		this.id = id;
	}
	public ProductData()
	{

	}
	public void OnBeforeSerialize()
	{
		mtrl_keys = new List<string>(null != needMtrlMap ? needMtrlMap.Keys : null);
		mtrl_values = new List<int>(null != needMtrlMap ? needMtrlMap.Values : null);
	}
	public void OnAfterDeserialize()
	{
		var count = System.Math.Min(mtrl_keys.Count, mtrl_values.Count);
		needMtrlMap = new Dictionary<string, int>(count);
		for (var i = 0; i < count; ++i)
		{
			needMtrlMap.Add(mtrl_keys[i], mtrl_values[i]);
		}
	}
	public int GetPrimeCost()
	{
		if (null == needMtrlMap || needMtrlMap.Count <= 0)
			return 0;
		int prime_cost = 0;
		Dictionary<string, MaterialData> map = DataManager.Instance.GetMaterialMap();
		foreach (KeyValuePair<string, int> kv in needMtrlMap)
		{
			if (map.ContainsKey(kv.Key))
			{
				prime_cost += map[kv.Key].prime_cost * kv.Value;
			}
		}
		return prime_cost;
	}
	public int GetMargin()
	{
		return final_sale_price - Mathf.CeilToInt(GetPrimeCost() + GetTransfortCost ()+ GetCommissionCost());
	}
	public int GetAccureProfit()
	{
		return GetMargin() * sale_num;
	}
	
	public float GetCommissionCost()
	{
		int total_cost = GetTotalCost();
		int profit_margin = GetProfitMargin();
		total_cost += profit_margin;
		float _commission = commission * 0.01f;
		float commision_cost = (total_cost - (total_cost * (1 - _commission))) / (1 - _commission);
		return commision_cost;
	}
	public int GetTotalCost()
	{
		int total_cost = GetPrimeCost() + add_cost;
		total_cost += GetTransfortCost();
		return total_cost;
	}
	public int GetTransfortCost()
	{
		return transfort - transfort_customer;
	}
	public int GetProfitMargin()
	{
		return Mathf.CeilToInt(profit_per_hour * need_hour);
	}
	public int GetRecommendPrice()
	{
		int total_cost = GetTotalCost();
		int profit_margin = GetProfitMargin();
		total_cost += profit_margin;
		float commision_cost = GetCommissionCost();
		float final_cost = commision_cost + total_cost;
		return Mathf.CeilToInt(final_cost);
	}
}