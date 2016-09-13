using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class V_RowCell : MonoBehaviour, IPointerDownHandler
{
	[SerializeField] V_Row parentRow;
	[SerializeField] Text cellText;
	// private string _value;
	// event based indexer for cell.getComponent<Text>().text
	public string value
	{
		get { return cellText.text;}
		set 
		{ 
			// _value = value;
			cellText.text = value;
		}
	}
	
	
	void Awake()
	{
		parentRow = transform.parent.gameObject.GetComponent<V_Row>();
		if(parentRow == null)
		{
			throw new System.Exception("Row cell does not have a row as its parent! something's wrong");
		}
		if(cellText == null)
		{
			throw new System.Exception("Row cell does not have a text component.");
		}

	}
	public void OnPointerDown(PointerEventData data)
	{

	}
	public void Sort(int index, bool sortAscending)
	{
		
	}
}
