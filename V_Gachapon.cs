using UnityEngine;
using UnityEngine.UI;
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
			 {RevealItem(value);}
			 else
			 {HideItem();} 
		}
		
	}

	
	public Button buyBtn, chargeBtn;
	public GameObject boxDetailPanel;
	public V_PlayerTemplate playerModel;

	new void Awake()
	{
		base.Awake();
		playerModel = FindObjectOfType<V_PlayerTemplate>();

		UIController.IfClick_GoTo(buyBtn, ()=> BuyItem(_selectedItem));
		UIController.IfClick_GoTo(chargeBtn, BuyCredit);
	}

	new void OnEnable()
	{
		base.OnEnable();
	}

	void BuyCredit()
	{

	}

	void BuyItem(V_GachaponItem item)
	{

		if (item.itemPrfb.GetComponent<V_Weapon>())
		{
			
		}
		
	}

	void RevealItem(V_GachaponItem item)
	{
		boxDetailPanel.GetComponent<Image>().sprite = item.sprite;
		boxDetailPanel.SetActive(true);
	}
	void HideItem()
	{
		boxDetailPanel.SetActive(false);
	}
}
