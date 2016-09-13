using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

//  attach to a gameobject and use as a prefab
[System.SerializableAttribute]
public class V_Header : MonoBehaviour, IPointerDownHandler
{
	[SerializeField] V_LobbyRoomList lobbyRoomList; // tweak from the inspector 
	public TypesOfData  dataType;
	[SerializeField] Image sortAscendingImage;
	bool sortAscending = false;

	void Awake()
	{
		// checking for dependencies
		if (sortAscendingImage == null)
		{
			throw new System.Exception("V_Header: Awake(): sortAscendingImage is null.");
		}
		DisableSortIcon();
	}
	public void OnPointerDown(PointerEventData data)
	{
		if (data.button == PointerEventData.InputButton.Left)
		{
			sortAscendingImage.transform.Rotate(new Vector3(0, 0, 180));
			lobbyRoomList.Sort(transform.GetSiblingIndex(), sortAscending);
			sortAscending = !sortAscending;
			
			print("sort " + sortAscending + " sibling index: " + transform.GetSiblingIndex());

		}
	}
	
	// this func is called everytime Our Parent lobbyRoomList tries to set the column by which it is doing the sort 
	public void DisableSortIcon()
	{
		sortAscendingImage.enabled = false;
	}
}
