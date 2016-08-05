using System;
using UnityEngine;
using UnityEngine.UI;

public class V_Inventory_UI : V_UIElement
{
	// fields
	[SerializeField] private V_InventoryItem _selectedItem;
	public V_InventoryItem selectedItem
	{
		get { return _selectedItem;}
		set 
		{
			 _selectedItem = value;
			 if (value != null)
			 {
				 CompareItems(_selectedItem, _compareeItem);
				 ShowUpgrades(_selectedItem);
			 }
			 else
			 {
				 StopComparing();
				 HideUpgrades();
			 }
		}
	}

    private void HideUpgrades()
    {
        throw new NotImplementedException();
    }

    private void ShowUpgrades(V_InventoryItem item)
    {
        if (item.itemClass != ItemClass.WEAPON)
		{
			return;
		}
		weaponUpgradePanel.SetActive(true);
		// foreach (V_UpgardeObject upgrade in item.itemPrfb.GetComponent<V_Weapon>().upgrades)
		// {
		// 	GameObject tmpObj = UIController.emptyObjectWithImage;
		// 	// tmpObj.AddComponent<Image>();
		// 	tmpObj.GetComponent<Image>().sprite = upgrade.icon;
		// 	tmpObj.transform.SetParent(weaponUpgradePanel.transform ,false);		
		// }
		throw new NotImplementedException();
    }

    [SerializeField] private V_InventoryItem _compareeItem;
	public V_InventoryItem compareeItem
	{
		get { return _compareeItem;}
		set 
		{
			_compareeItem = value;
			if (_compareeItem != _selectedItem)
			{
				CompareItems(_selectedItem, _compareeItem);
			}
		}
	}

	[HeaderAttribute("Inventory main Buttons")]
	[SpaceAttribute(10f)]

	public Button WeaponsBtn;
	public Button GearsBtn;
	public Button CharactersBtn;

	public Button eqPistolsBtn;
	public Button eqRifle1Btn;
	public Button eqRifle2Btn;
	public Button eqGerenadeBtn;
	public Button eqMeleeBtn;

	[HeaderAttribute("Inventory sub Buttons")]
	[SpaceAttribute(10f)]
	public Button pistolsBtn;
	public Button assaultsBtn;
	public Button gerenadesBtn;
	public Button meleesBtn;

	[SpaceAttribute(10f)]
	public Button upperBodyBtn;
	public Button lowerBodyBtn;
	public Button headBtn;
	
	
	[HeaderAttribute("Inventory Panels")]
	[SpaceAttribute(10f)]
	public GameObject  weaponPanel;
	public GameObject gearPanel;
	public GameObject charactersPanel;
	public GameObject weaponUpgradePanel;

	[HeaderAttribute("Inventory sub Panels")]
	[SpaceAttribute(10f)]
	// weapons
	public GameObject pistolPanel;
	public GameObject assaultPanel;

	[SpaceAttribute(10f)]
	// gears
	public GameObject upperBodyPanel;
	public GameObject lowerBodyPanel;
	public GameObject headPanel;

	[HeaderAttribute("References")]
	[SpaceAttribute(10f)]

	[SerializeField] protected GameObject weaponComp;
	[SerializeField] public GameObject gearComp;
	[SerializeField] public GameObject characterComp;
	V_WeaponComparison weaponComparer;
	V_GearComparison gearComparer;
	V_CharacterComparison characterComparer;
	private V_Weapon tmpWeapon1, tmpWeapon2;
	private V_Gear tmpGear1, tmpGear2;
	// private V_Character tmpCharacter1, tmpCharacter2;



	// methods
	public new void Awake()
	{
		base.Awake();
		try
		{
			weaponComparer = weaponComp.GetComponent<V_WeaponComparison>();
			gearComparer = gearComp.GetComponent<V_GearComparison>();
			characterComparer = characterComp.GetComponent<V_CharacterComparison>();
		}
		catch (System.Exception err)
		{
			UIController.ThrowError("V_Inventory_UI: Awake(): " + err.Message, ()=>{UIController.CloseError(); return;});			
		}

		// main buttons
		UIController.IfClick_GoTo(WeaponsBtn, () => 
		{
			UIController.Enable_DisableUI(weaponPanel, gearPanel, charactersPanel);
			StopComparing();
		});

		UIController.IfClick_GoTo(GearsBtn, () => 
		{
			UIController.Enable_DisableUI(gearPanel, weaponPanel, charactersPanel);
			StopComparing();
		});

		UIController.IfClick_GoTo(CharactersBtn, () => 
		{
			UIController.Enable_DisableUI(charactersPanel, weaponPanel, gearPanel);
			StopComparing();
		});

		// sub buttons: Weapons

		UIController.IfClick_GoTo(pistolsBtn, () => UIController.Enable_DisableUI(pistolPanel, assaultPanel));
		UIController.IfClick_GoTo(assaultsBtn, () => UIController.Enable_DisableUI(assaultPanel, pistolPanel));
		// UIController.IfClick_GoTo(gerenadesBtn, () => UIController.Enable_DisableUI(gerenadePanel, pistolPanel, assaultPanel, meleePanel));
		// UIController.IfClick_GoTo(meleesBtn, () => UIController.Enable_DisableUI(meleePanel, pistolPanel, assaultPanel, gerenadePanel));
		
		// sub buttons: Gears
		UIController.IfClick_GoTo(upperBodyBtn, () => UIController.Enable_DisableUI(upperBodyPanel, lowerBodyPanel, headPanel));
		UIController.IfClick_GoTo(lowerBodyBtn, () => UIController.Enable_DisableUI(lowerBodyPanel, upperBodyPanel, headPanel));
		UIController.IfClick_GoTo(headBtn, () => UIController.Enable_DisableUI(headPanel, upperBodyPanel, lowerBodyPanel));

	}

	public new void OnEnable()
	{
		base.OnEnable();
		
		weaponPanel.SetActive(true);
		gearPanel.SetActive(false);
		charactersPanel.SetActive(false);	

		weaponComp.SetActive(false);
		gearComp.SetActive(false);
		characterComp.SetActive(false);

	}

	public void DonateItem(V_InventoryItem item)
	{
		if (item == null)
		{
			return;
		}

		UIController.SelectFromListModal("Do you want to donate " + item.name,
		()=> {},
		()=> {UIController.CloseSelectModal();},
		(playerName)=>{/*find a player with playerName*/});

	}

	protected void CompareItems(V_InventoryItem item1, V_InventoryItem item2 = null)
	{
		if (item1 == null)
		{
			print("V_Inventory_UI: CompareItems(): Cannot compare Items");
			return;
		}		
		switch (item1.itemType)
		{
			case ItemTypes.W_PISTOL:
			CompareWeapon(item1, item2);
			break;
			
			case ItemTypes.W_ASSAULT:
			CompareWeapon(item1, item2);
			break;

			case ItemTypes.G_UPPERBODY:
			CompareGear(item1, item2);
			
			break;

			case ItemTypes.G_LOWERBODY:
			CompareGear(item1, item2);
			break;

			case ItemTypes.G_HEAD:
			CompareGear(item1, item2);
			break;

			default:
			UIController.ThrowError("V_Inventory_UI: CompareItems(): error while loading items' types!", ()=>
			{
				UIController.CloseError();
			});
			break;
		}
	}

	protected void StopComparing()
	{
		weaponComp.SetActive(false);
		gearComp.SetActive(false);
		characterComp.SetActive(false);
	}

	protected void CompareWeapon(V_InventoryItem item1, V_InventoryItem item2)
	{
		tmpWeapon1 = item1.itemPrfb.GetComponent<V_Weapon>();

		if (tmpWeapon1 == null)
		{
			UIController.ThrowError("V_Inventory_UI: CompareWeapon(): Cannot Find a ShopItem reference", UIController.CloseError);
			return;
		}

		UIController.Enable_DisableUI(weaponComp, gearComp, characterComp);
		// if we have just the first Item, we wanna show its detail, but not compare it to anaother item
		if (item2 == null)
		{
			// sliders related to the first selected Item
			StartCoroutine(UIController.FillSlider(weaponComparer.accuracy_firstItem,tmpWeapon1.accuracy / 100f));
			StartCoroutine(UIController.FillSlider(weaponComparer.clipSize_firstItem, tmpWeapon1.clipSize  / 100f));
			StartCoroutine(UIController.FillSlider(weaponComparer.damage_firstItem, tmpWeapon1.damage  / 100f));
			StartCoroutine(UIController.FillSlider(weaponComparer.fireRate_firstItem, tmpWeapon1.fireRate / 100f));
			StartCoroutine(UIController.FillSlider(weaponComparer.weight_firstItem, tmpWeapon1.weight / 100f));
			
			// second Item sliders
			weaponComparer.accuracy_secondItem.gameObject.SetActive(false);
			weaponComparer.clipSize_secondItem.gameObject.SetActive(false);
			weaponComparer.damage_secondItem.gameObject.SetActive(false);
			weaponComparer.fireRate_secondItem.gameObject.SetActive(false);
			weaponComparer.weight_secondItem.gameObject.SetActive(false);
		}


		else if (item2 != null)
		{
			weaponComparer.accuracy_secondItem.gameObject.SetActive(true);
			weaponComparer.clipSize_secondItem.gameObject.SetActive(true);
			weaponComparer.damage_secondItem.gameObject.SetActive(true);
			weaponComparer.fireRate_secondItem.gameObject.SetActive(true);
			weaponComparer.weight_secondItem.gameObject.SetActive(true);

			tmpWeapon2 = item2.itemPrfb.GetComponent<V_Weapon>();
			StartCoroutine(UIController.FillSlider(weaponComparer.accuracy_secondItem, tmpWeapon2.accuracy / 100f));
			StartCoroutine(UIController.FillSlider(weaponComparer.clipSize_secondItem, tmpWeapon2.clipSize  / 100f));
			StartCoroutine(UIController.FillSlider(weaponComparer.damage_secondItem, tmpWeapon2.damage / 100f));
			StartCoroutine(UIController.FillSlider(weaponComparer.fireRate_secondItem, tmpWeapon2.fireRate / 100f));
			StartCoroutine(UIController.FillSlider(weaponComparer.weight_secondItem, tmpWeapon2.weight / 100f));
		}

	}
	protected void CompareGear(V_InventoryItem item1, V_InventoryItem item2)
	{
		tmpGear1 = item1.itemPrfb.GetComponent<V_Gear>();
		if(tmpGear1 == null)
		{
			UIController.ThrowError("V_Inventory_UI: CompareGear: gears are not set properly", UIController.CloseError );
			return;
		}

		UIController.Enable_DisableUI(gearComp, weaponComp, characterComp);

		if (item2 == null)
		{
			StartCoroutine(UIController.FillSlider(gearComparer.shield_firstItem, tmpGear1.shield/ 100f));

			gearComparer.shield_secondItem.gameObject.SetActive(false);
		}
		else if (item2 != null)
		{
			gearComparer.shield_secondItem.gameObject.SetActive(true);
			tmpGear2 = item2.itemPrfb.GetComponent<V_Gear>();
			StartCoroutine(UIController.FillSlider(gearComparer.shield_secondItem, tmpGear2.shield));
		}
	}

	protected void CompareCharacter(V_InventoryItem item1, V_InventoryItem item2)
	{

	}
	
	
}
