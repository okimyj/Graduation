using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScrollRect : ScrollRect
{
	public GameObject cellPrefab;       // scroll cell prefab
	public Vector3 cellStartPos = Vector3.zero;
	public Vector2 cellSize = Vector3.zero;
	public Vector2 cellSpacing = Vector3.zero;
	public bool scrollBlock = false;
	public GameObject EmptyListShowTarget;
	UIScrollCellBase[] shownCells;
	int showCellNum = 0;
	ArrayList cellPool;
	Vector2 scrollViewSize;
	List<object> cellDatas;
	float acceptScrollRange = 25f;
	UnityEngine.EventSystems.PointerEventData lastEventData;
	bool nowDragging;
	List<UIScrollCellBase> createdCells;        // 그냥 생성된 모든 셀.
	CallbackButton cellTouchedCallback;
	bool init;
	int totalCellNum = 0;
	int mStartIndex = -1;
	int mEndIndex = -1;
	protected override void Awake()
	{
		base.Awake();
		Init();
	}
	void Init()
	{
		if (init || !Application.isPlaying)
			return;
		
		RecalcPoolNum();
		
		init = true;
		content.sizeDelta = CalcContentsSize();
		SetTotalCellNum();

	}
	public void SetCellTouchedCallback(CallbackButton callback)
	{
		cellTouchedCallback = callback;
	}
	public void SetTotalCellNum(int totalCell = -1, bool isRefresh = true)
	{
		Init();
		if (isRefresh)
		{
			if (null != shownCells)
			{
				for (int i = 0; i < shownCells.Length; ++i)
				{
					if (null != shownCells[i])
						PushIntoPool(shownCells[i]);
				}
				shownCells = null;
			}
			mStartIndex = -1;
			mEndIndex = -1;
		}


		totalCellNum = Mathf.Max(0, (totalCell == -1 && null != cellDatas) ? cellDatas.Count : totalCell);
		shownCells = new UIScrollCellBase[totalCellNum];
		content.sizeDelta = CalcContentsSize();

		CheckScrollPosition();
		if (null != EmptyListShowTarget)
			EmptyListShowTarget.SetActive(totalCellNum <= 0);

		bool canScroll = vertical ? content.sizeDelta.y >= scrollViewSize.y : content.sizeDelta.x >= scrollViewSize.x;
		SetScrollBlock(!canScroll);
	}
	void PushIntoPool(UIScrollCellBase usedCell)
	{
		if (usedCell != null)
		{
			SetCellActive(usedCell.transform, false);
			usedCell.transform.localPosition = new Vector3(cellStartPos.x, cellStartPos.y, 0);
			//	usedCell.SetActive(false);
			cellPool.Add(usedCell);
			usedCell.Reset();
		}
	}
	UIScrollCellBase PopFromPool()
	{
		//	Debug.Log ("cellPool.Count - 2 : " + cellPool.Count);
		if (null != cellPool && cellPool.Count > 0)
		{
			UIScrollCellBase newCell = cellPool[0] as UIScrollCellBase;
			SetCellActive(newCell.transform, true);
			cellPool.RemoveAt(0);
			return newCell;
		}
		else
		{
			return null;
		}
	}
	private Vector2 CalcContentsSize()
	{
		Vector2 contentsSize = Vector2.zero;
		if (vertical)
		{
			contentsSize.x = scrollViewSize.x;
			contentsSize.y = totalCellNum * GetCellHeight() - cellSpacing.y + cellStartPos.y + cellStartPos.y;
		}
		else
		{
			contentsSize.x = totalCellNum * GetCellWidth() - cellSpacing.x + cellStartPos.x + cellStartPos.x;
			contentsSize.y = scrollViewSize.y;
		}
		
		return contentsSize;
	}
	private UIScrollCellBase CreateOneCell()
	{
		GameObject newCell = Instantiate(cellPrefab.gameObject) as GameObject;
		UIScrollCellBase cellComp = newCell.GetComponent<UIScrollCellBase>();
		newCell.transform.SetParent(content);
		newCell.transform.localScale = new Vector3(1, 1, 1);
		newCell.transform.localPosition = new Vector3(0, 0, 0);
		InitOneCell(cellComp);
		return cellComp;
	}
	void InitOneCell(UIScrollCellBase cellComp)
	{
		if (cellComp.gameObject.activeSelf)
			SetCellActive(cellComp.transform, false);
		if (cellComp.buttonComp)
		{
			cellComp.buttonComp.param = cellComp;
			cellComp.buttonComp.SetTouchedCallback(TouchCell);
		}
		createdCells.Add(cellComp);
	}
	public void TouchCell(UIScalingButton button)
	{
		if (null != cellTouchedCallback)
			cellTouchedCallback(button);
	}
	void SetCellActive(Transform target, bool active)
	{
		Vector3 changeScale = active ? Vector3.one : Vector3.zero;
		if (target.localScale != changeScale)
			target.localScale = changeScale;
	}
	void RecalcPoolNum()
	{
		int poolNum = GetPoolSize();
		if (null != cellPool && cellPool.Count >= poolNum)
		{
			return;
		}
		cellPool = null != cellPool ? cellPool : new ArrayList();
		createdCells = null != createdCells ? createdCells : new List<UIScrollCellBase>();
		poolNum = poolNum - cellPool.Count;
		
		if (cellPrefab != null)
		{
			int total = poolNum - cellPool.Count;
			for (int i = 0; i < total; i++)
			{
				UIScrollCellBase newCell = CreateOneCell();
				cellPool.Add(newCell);
			}
		}
	}
	private float GetCellWidth(int index = 0)
	{
		return cellSize.x + cellSpacing.x;
	}

	private float GetCellHeight(int index = 0)
	{
		return cellSize.y + cellSpacing.y;
	}
	public int GetPoolSize()
	{
		scrollViewSize = GetComponent<RectTransform>().sizeDelta;
		float viewSize = vertical ? scrollViewSize.y - cellStartPos.y : scrollViewSize.x - cellStartPos.x;
		showCellNum = vertical ? Mathf.CeilToInt(viewSize / (GetCellHeight() + cellSpacing.y)) : Mathf.CeilToInt(viewSize / (GetCellWidth() + cellSpacing.x));
		int poolNum = showCellNum + 2;          // 위 아래 1개씩 버퍼.
		Debug.Log("poolNum : " + poolNum);
		return poolNum;
	}
	public int GetDisplayCellNum()
	{
		return showCellNum;
	}
	void ResetDragData()
	{
		lastEventData = null;
	}
	public override void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
	{
		if (scrollBlock)
			return;
		ResetDragData();
		lastEventData = null;
		nowDragging = false;
		eventData.position = eventData.pressPosition;
		base.OnBeginDrag(eventData);
		
	}
	public override void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
	{
		if (scrollBlock)
			return;
		base.OnDrag(eventData);

		float xGap = Mathf.Abs(eventData.pressPosition.x - eventData.position.x);
		float yGap = Mathf.Abs(eventData.pressPosition.y - eventData.position.y);
		bool ok = vertical ? xGap < acceptScrollRange && yGap > acceptScrollRange : xGap > acceptScrollRange && yGap < acceptScrollRange;
		if (ok)
			nowDragging = true;
	}
	public override void OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
	{
		nowDragging = false;
		if (scrollBlock)
			return;
		base.OnEndDrag(eventData);
		ResetDragData();
	}
	void CheckScrollPosition()
	{
		int startIndex = -1;
		int endIndex = -1;
		
		if (vertical)
		{
			float y = content.localPosition.y < 0 ? 0 : content.localPosition.y;
			startIndex = Mathf.FloorToInt(y / GetCellHeight());
		}
		else
		{
			float x = content.localPosition.x > 0 ? 0 : Mathf.Abs(content.localPosition.x);
			startIndex = Mathf.FloorToInt(x / GetCellWidth());
		}
		
		startIndex = Mathf.Max(0, startIndex);
		if (endIndex == -1)
			endIndex = Mathf.Min(startIndex + showCellNum, totalCellNum - 1);
		
		if (mStartIndex == startIndex && mEndIndex == endIndex)
			return;
		else if (mStartIndex > endIndex || mEndIndex < startIndex)
		{
			ClearCellDataRange(mStartIndex, mEndIndex);
			FillCellData(startIndex, endIndex);
		}
		else if (startIndex < mStartIndex || endIndex < mEndIndex)
		{
			ClearCellDataRange(endIndex + 1, mEndIndex);
			FillCellData(startIndex, mStartIndex - 1);
		}
		else if (startIndex > mStartIndex || endIndex > mEndIndex)
		{
			ClearCellDataRange(mStartIndex, startIndex - 1);
			FillCellData(mEndIndex + 1, endIndex);
		}
		mStartIndex = startIndex;
		mEndIndex = endIndex;
	}
	protected override void LateUpdate()
	{
		if (!(scrollBlock))
			base.LateUpdate();
		if (Application.isPlaying)
		{
			CheckScrollPosition();
		}
	}
	void ClearCellDataRange(int startRank, int endRank)
	{
		//	Debug.LogError ("ClearCellDataRange - " + startRank + ", " + endRank);
		if (null == shownCells || startRank < 0 || endRank < 0)
			return;
		for (int i = startRank; i <= endRank; i++)
		{
			if (shownCells.Length > i && null != shownCells[i])
			{
				PushIntoPool(shownCells[i]);
				shownCells[i] = null;
			}
		}
	}

	void FillCellData(int startRank, int endRank)
	{
		//Debug.LogError ("FillCellData - " + startRank + ", " + endRank);
		for (int i = startRank; i <= endRank; i++)
		{
			UIScrollCellBase cell = PopFromPool();
			if (cell != null)
			{
				int rowIndex = 0;
				int colIndex = 0;
				if (vertical)
				{
					rowIndex = i;
					//colIndex = 0;
				}
				else
				{
					//rowIndex = 0;
					colIndex = i;
				}
				
				float offsetY = 0;
				
				cell.transform.localPosition = new Vector3(GetCellWidth() * colIndex + cellStartPos.x, -(GetCellHeight() * rowIndex + cellStartPos.y + offsetY), 0);
				
				cell.transform.localScale = new Vector3(1, 1, 1);

				shownCells[i] = cell;
				SetOneCellData(i, cell);
				//this.SendMessage("SetData", new ITEM_DATA(i, item), SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	public virtual void SetOneCellData(int index, UIScrollCellBase cell)
	{
		if (null != cellDatas && cellDatas.Count > index)
			cell.SetCellData(cellDatas[index]);
		else
			cell.SetCellData(null);
	}
	private void SetScrollBlock(bool block)
	{
		scrollBlock = block;
	}
	public void AddCells(List<object> cells)
	{
		cellDatas = new List<object>(cells);
		SetTotalCellNum();
	}
}
