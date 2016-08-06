using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class V_GachaponItem : MonoBehaviour, IPointerDownHandler, ISelectHandler, IDeselectHandler
{
	V_Gachapon gachapon;
	public GameObject itemPrfb;
	public Sprite sprite;
	void Awake()
	{
		gachapon = FindObjectOfType<V_Gachapon>();
	}
	public void OnPointerDown(PointerEventData data)
	{
		if (data.button == PointerEventData.InputButton.Left && gachapon.selectedItem != this)
		{
			EventSystem.current.SetSelectedGameObject(gameObject, data);
		}
	}

	public void OnSelect(BaseEventData data)
	{
		gachapon.selectedItem = this;
	}

	public void OnDeselect(BaseEventData data)
	{
		gachapon.selectedItem = null;
	}
}
