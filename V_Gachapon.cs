using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class V_Gachapon : V_UIElement 
{
	[SerializeField] private V_GachaponItem _selectedItem;
	public V_GachaponItem selectedItem
	{
		get { return _selectedItem;}
		set 
		{

			_selectedItem = value;
			if(value != null)
			
			{
				RevealItem(_selectedItem);
			}		
			else
			{
				HideItem();
			} 
		}
	}
	public float gachaponRefreshRate;
	bool isRefreshingPlayer;

	[HeaderAttribute("UI references")] [SpaceAttribute(5f)]
	public Text playerCharge;
	
	public Button chargeBtn;
	public GameObject boxDetailPanel;
	public V_PlayerTemplate playerModel;

	new void Awake()
	{
		base.Awake();
		playerModel = FindObjectOfType<V_PlayerTemplate>();

		UIController.IfClick_GoTo(chargeBtn, BuyCredit);
	}

	new void OnEnable()
	{
		base.OnEnable();
		UpdatePlayerCharge();
	}

	void Update()
	{
		if (!isRefreshingPlayer)
		{
			return;
		}
		StartCoroutine(RefreshLastPurchasers());
	}

	public void UpdatePlayerCharge()
	{
		playerCharge.text = playerModel.charge.ToString();
	}
	void BuyCredit()
	{
		playerModel.charge += 100;
		UpdatePlayerCharge();
	}

	public void BuyItem(V_GachaponItem item)
	{
		// #revision: add other types of items
		if (playerModel.charge < 20)
		{
			UIController.AskYesNoQ("Do you wanna charge?", 
			()=> {/* go buy something */},
			UIController.CloseYesNoQ);
		}
		try
		{
			V_Weapon tmpWeapon = item.itemPrfb.GetComponent<V_Weapon>();

			if (tmpWeapon != null)
			{
				// #revision: name should be on the item itself
				UIController.ThrowError("you bought " +item.itemPrfb.name, UIController.CloseError);
			}
		}
		catch (System.Exception err)
		{
			
			throw err;
		}
		
	}

	public void RevealItem(V_GachaponItem item)
	{
		boxDetailPanel.GetComponent<Image>().sprite = item.sprite;
		boxDetailPanel.SetActive(true);
	}
	public void HideItem()
	{
		boxDetailPanel.SetActive(false);
	}
	IEnumerator RefreshLastPurchasers()
	{
		// get a list of lastPurchasers
		print("Refreshing lastPurchasers");
		yield return new WaitForSeconds(gachaponRefreshRate);
	}
}
