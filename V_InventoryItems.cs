using UnityEngine;
using System.Collections.Generic;

public class V_InventoryItems : MonoBehaviour
{
	public List<V_InventoryItemTemplate> items;
	[SerializeField] V_ObjectPool objectPool;
	public GameObject inventoryItemPrfb;
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
			for (int i = 0; i < tmpList.Length; i++)
			{
				GameObject tmpObj = Instantiate(inventoryItemPrfb) as GameObject;
				GameObject item = Instantiate(objectPool.GetItem(items[i].itemName)) as GameObject;
				// print(item.name);
				// tmpObj.AddComponent<V_InventoryItem>();
				tmpObj.GetComponent<V_InventoryItem>().Initialize(item);
				tmpObj.name = items[i].itemName;
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
