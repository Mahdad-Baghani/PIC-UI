using UnityEngine;
using System.Collections.Generic;

public class V_ObjectPool : MonoBehaviour 
{
	public static V_ObjectPool instance;
	public Dictionary<string, GameObject> pool;
	[SerializeField] GameObject[] objectsToAdd;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}
		pool = new Dictionary<string, GameObject>();

		for (int i = 0; i < objectsToAdd.Length; i++)
		{
			// check for duplicate items
			if (!pool.ContainsKey(objectsToAdd[i].name))
			{
				pool.Add(objectsToAdd[i].name, objectsToAdd[i]);
			}
		}
	}

	public GameObject GetItem(string itemName)
	{
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
