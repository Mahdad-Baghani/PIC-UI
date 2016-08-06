using UnityEngine;
using System.Collections.Generic;

public class V_InventoryItems : MonoBehaviour
{
	public List<V_InventoryItemTemplate> items;
	private V_ObjectPool objectPool;
	public GameObject inventoryItemPrfb;

	void Awake()
	{
		objectPool = FindObjectOfType<V_ObjectPool>();
		// items = new List<V_InventoryItemTemplate>();
	}
	private void RefreshList()
	{
		// get some data from server and initialize the items list

	}

	public GameObject[] GetList()
	{
		RefreshList();
		GameObject[] tmpList = new GameObject[items.Count];
		try
		{
			for (int i = 0; i < items.Count; i++)
			{
				GameObject tmpObj = Instantiate(inventoryItemPrfb);
				// GameObject item =  objectPool.GetItem(items[i].itemName);
				// tmpObj.AddComponent<V_InventoryItem>();
				// tmpObj.GetComponent<V_InventoryItem>().Initialize(item);
				tmpList[i] = tmpObj;
			}
			return tmpList;
		}
		catch (System.Exception)
		{
			
			throw;
		}
	}


}
