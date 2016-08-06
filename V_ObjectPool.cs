using UnityEngine;
using System.Collections.Generic;

public class V_ObjectPool : MonoBehaviour 
{
	public Dictionary<string, GameObject> pool;
	[SerializeField] GameObject[] objectsToAdd;

	void Awake()
	{
		pool = new Dictionary<string, GameObject>();
	}

	private void FillPool()
	{
		for (int i = 0; i < objectsToAdd.Length; i++)
		{
			// check for duplicate items
			if (!pool.ContainsKey(objectsToAdd[i].name))
			{
				pool.Add(objectsToAdd[i].name, objectsToAdd[i]);
				print(objectsToAdd[i].name);
			}
		}
		print ("V_ObjectPool: FillPool: pool count is " + pool.Count);
	}

	public GameObject GetItem(string itemName)
	{
		if (pool == null || pool.Count == 0)
		{
			FillPool();
		}
		if (pool.ContainsKey(itemName))
		{
			return pool[itemName];
		}
		else
		{
			return null;
		}
	}
}
