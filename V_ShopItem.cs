using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ItemPurchaseMode 
{
	SCORE,
	CHARGE, 
	LEVEL
}
public enum ItemTypes
{
	W_PISTOL,
	W_ASSAULT,
	G_UPPERBODY,
	G_LOWERBODY,
	G_HEAD
}

public enum ItemClass
{
	WEAPON,
	GEAR,
	CHARACTER
}
public class V_ShopItem : V_UIElement, IPointerEnterHandler , IPointerDownHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
	// fields

	V_Shop Shop;

	public bool isLocked;
	public bool isPurchased;

	public int requiredLevel;

	public int requiredScore;

	public int requiredCharge;

	public ItemTypes itemType;
	public ItemClass itemClass;
	public GameObject itemPrfb;
	public Image icon;
	

	[HeaderAttribute("UI References")]
	[SpaceAttribute(5f)]
	// item detais
	// the detail panel is what hosts all the buttons, lock panel, etc.
	public GameObject detailPanel;
	[SpaceAttribute(5f)]
	public Button donateBtn;
	public Button buyBtn;
	public Text itemNameTxt;
	public Text timeTxt;
	public Image level;
	public GameObject itemLock;
	public Text requiredLevelTxt;
	public Text requiredScoreTxt;
	public Text requiredChargeTxt;

	// mehtods
	public new void Awake()
	{
		base.Awake();

		Shop = FindObjectOfType<V_Shop>();

		if (itemPrfb == null)
		{
			UIController.ThrowError("V_ShopItem(): Awake(): itemPrfb is null in shopItem: " + gameObject.name, ()=>
			{
				UIController.CloseError();
				return;
			});
		}
		switch (itemClass)
		{
			case(ItemClass.WEAPON):
			// icon.sprite = itemPrfb.GetComponent<V_Weapon>().icon;
			itemNameTxt.text = itemPrfb.GetComponent<V_Weapon>().name;
			timeTxt.text = itemPrfb.GetComponent<V_Weapon>().lifeTime.ToString();
			// #revision
			// level = itemPrfb.GetComponent<V_Weapon>().level
			break;

			case(ItemClass.GEAR):
			// icon.sprite = itemPrfb.GetComponent<V_Gear>().icon;
			itemNameTxt.text = itemPrfb.GetComponent<V_Gear>().name;
			break;

			case(ItemClass.CHARACTER):
			// icon = itemPrfb.GetComponent<V_Character>).icon;

			default:
			UIController.ThrowError("V_ShopItem: Item type is not set properly", ()=>
			{
				UIController.CloseError();
			});
			break;
		}
		requiredLevelTxt.text = requiredLevel.ToString();
		requiredScoreTxt.text = requiredScore.ToString();
		requiredChargeTxt.text= requiredCharge.ToString();
		
		UIController.IfClick_GoTo(donateBtn, ()=> Shop.DonateItem(this));
		UIController.IfClick_GoTo(buyBtn, ()=> Shop.BuyItem(this));
	}
	public new void OnEnable()
	{
		base.OnEnable();
		detailPanel.SetActive(false);

	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		// StartCoroutine(UIController.FadeIn(detailPanel));
		detailPanel.SetActive(true);
		
		if (Shop.selectedItem == null)
		{
			return;
		}
		// to prevent comparing 2 similar weapon or gear!!
		// and to prevent comparing 2 different classes! (weapons and gears)
		if (Shop.selectedItem != this && Shop.selectedItem.itemType == this.itemType)
		{
			// this calls on CompareItems()
			Shop.compareeItem = this;
		}
		else
		{
			// this calls StopComparing() on Shop
			Shop.compareeItem = null;
		}
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			EventSystem.current.SetSelectedGameObject(gameObject, eventData);
		}
		// to prevent comparing 2 similar weapon or gear!!
		if (Shop.compareeItem == this)
		{
			Shop.compareeItem = null;
		}
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		// Shop.compareeItem = null;
		detailPanel.SetActive(false);
		// StartCoroutine(UIController.FadeOut(detailPanel));
	}

	public void OnSelect(BaseEventData eventData)
	{
		try
		{
			// print("selecting item");
			Shop.selectedItem = eventData.selectedObject.GetComponent<V_ShopItem>();
		}
		catch (System.Exception err)
		{
			UIController.ThrowError("V_ShopItem: OnSelect(): " + err.Message, ()=> UIController.CloseError());
		}
	}
	public void OnDeselect(BaseEventData eventData)
	{
		// print("deselecting item");
		Shop.selectedItem = null;
		Shop.compareeItem = null;
	}

	public void UnlockItem()
	{
		itemLock.SetActive(false);
	}


}
