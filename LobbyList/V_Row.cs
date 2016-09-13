using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class V_Row : MonoBehaviour, IPointerDownHandler
{
	public List<V_RowCell> cells;

	void Awake()
	{
		foreach (V_RowCell cell in gameObject.transform.GetComponentsInChildren<V_RowCell>())
		{
			cells.Add(cell);
		}
	}
	public void OnPointerDown(PointerEventData data)
	{

	}
}
