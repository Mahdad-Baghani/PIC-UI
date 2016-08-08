using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;

public class V_GachaponItem : V_UIElement, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
	V_Gachapon gachapon;
	public string itemName;
	public GameObject itemPrfb;
	public Sprite sprite;

	// UI refs
	public Button buyBtn;
	public GameObject detailPanel;
	new void Awake()
	{
		base.Awake();
		gachapon = FindObjectOfType<V_Gachapon>();
		UIController.IfClick_GoTo (buyBtn,()=> gachapon.BuyItem(this));
	}
	new void OnEnable()
	{
		base.OnEnable();

		detailPanel.SetActive(false);
	}

	public void OnPointerEnter(PointerEventData data)
	{
		detailPanel.SetActive(true);
	}
	public void OnPointerDown(PointerEventData data)
	{
		if (data.button == PointerEventData.InputButton.Left && gachapon.selectedItem != this)
		{
			EventSystem.current.SetSelectedGameObject(gameObject, data);
		}
	}
	public void OnPointerExit(PointerEventData data)
	{
		detailPanel.SetActive(false);
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
