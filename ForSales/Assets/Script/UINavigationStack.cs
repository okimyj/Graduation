using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class UINavigationStack : AbstractSingleton<UINavigationStack>
{
	private Dictionary<string, UIBase> createdUIMap = new Dictionary<string, UIBase>();
	private List<UIBase> shownUIList = new List<UIBase>();
	private Vector3 lastUIPosition = new Vector3(1000, 1000, 0);
	private Vector3 addPosition = new Vector3(100, 0, 0);
	public UIBase CreateUI(GameObject prefab)
	{
		if(createdUIMap.ContainsKey(prefab.name))
		{
			return createdUIMap[prefab.name];
		}
		else
		{
			GameObject ui = Instantiate(prefab) as GameObject;
			ui.transform.localPosition = lastUIPosition;
			lastUIPosition += addPosition;
			UIBase uiBase = ui.GetComponent<UIBase>();
			createdUIMap[prefab.name] = uiBase;
			return uiBase;
		}
		
	}
	public UIBase GetTopUIInStack(UIBase ignoreUI = null)
	{
		UIBase topUI = null;
		if (null != shownUIList && shownUIList.Count > 0)
		{
			int index = shownUIList.Count;
			do
			{
				if (index == 0)
					return null;
				topUI = shownUIList[--index];
			} while (null != ignoreUI && ignoreUI.Equals(topUI));
		}
		return topUI;
		//return (null != shownUIList && shownUIList.Count > 0) ? shownUIList[shownUIList.Count-1] : null;
	}
	public void Push(UIBase ui)
	{
		UIBase topUI = GetTopUIInStack();
		int sortOrderBase = (null != topUI && null != topUI.canvases && topUI.canvases.Length > 0) ? topUI.canvases[topUI.canvases.Length-1].sortingOrder : 0;
		shownUIList.Add(ui);
		if (null != ui.canvases && ui.canvases.Length > 0)
		{
			for (int i=0; i < ui.canvases.Length; ++i)
			{
				sortOrderBase = sortOrderBase + 1;
				ui.canvases[i].sortingOrder = sortOrderBase;
			}
		}
		UIShow(ui, true);
	}
	public void Pop(UIBase ui)
	{
		shownUIList.Remove(ui);
		UIShow(ui, false);
	}
	public void UIShow(UIBase ui, bool show)
	{
		ui.gameObject.SetActive(show);
	}
}
