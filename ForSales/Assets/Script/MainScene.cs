using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : AbstractSingleton<MainScene>
{
	const string RESOURCE_PATH = "Prefabs/";
	
	public UIBase ShowMainUI()
	{
		return ShowUI("MainUI");
	}
	public UIBase ShowProductRegisterPopup()
	{
		return ShowUI("ProductRegisterPopup");
	}
	public UIBase ShowMaterialRegisterPopup()
	{
		return ShowUI("MaterialRegisterPopup");
	}
	public UIBase ShowMaterialSelectPopup()
	{
		return ShowUI("MaterialSelectPopup");
	}
	UIBase ShowUI(string prefabName)
	{
		GameObject uiPrefab = Resources.Load(RESOURCE_PATH + prefabName, typeof(GameObject)) as GameObject;
		UIBase ui = UINavigationStack.Instance.CreateUI(uiPrefab);
		UINavigationStack.Instance.Push(ui);
		return ui;
	}

	void Start()
    {
		ShowMainUI();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
