using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(V_InventoryItems))]
public class V_Inventory_UI : V_UIElement
{
	public const int MAX_NUM_FOR_WEAPON_SLUTS = 6;
	// fields
	[SerializeField] List<V_InventoryItem> inGamePistols;
	[SerializeField] List<V_InventoryItem> inGameRifles;
	[SerializeField] List<V_InventoryItem> inGameRifles_secondary;
	

	public V_InventoryItems inventoryItems;

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

    private void HideUpgrades()
    {
        // throw new NotImplementedException();
    }

    private void ShowUpgrades(V_InventoryItem item)
    {
        if (item.itemClass != ItemClass.WEAPON)
		{
			return;
		}
		// weaponUpgradePanel.SetActive(true);
		// foreach (V_UpgardeObject upgrade in item.itemPrfb.GetComponent<V_Weapon>().upgrades)
		// {
		// 	GameObject tmpObj = UIController.emptyObjectWithImage;
		// 	// tmpObj.AddComponent<Image>();
		// 	tmpObj.GetComponent<Image>().sprite = upgrade.icon;
		// 	tmpObj.transform.SetParent(weaponUpgradePanel.transform ,false);		
		// }
		// throw new NotImplementedException();
    }


	[HeaderAttribute("Inventory main Buttons")]
	[SpaceAttribute(10f)]

	public Button WeaponsBtn;
	public Button GearsBtn, CharactersBtn;
	public Button inGamePistolsBtn, inGameRifleBtn, inGameSecRifleBtn, inGameGerenadeBtn, inGameMeleeBtn;

	[HeaderAttribute("Inventory sub Buttons")]
	[SpaceAttribute(10f)]
	public Button pistolsBtn;
	public Button assaultsBtn, gerenadesBtn, meleesBtn;

	[SpaceAttribute(10f)]
	public Button upperBodyBtn;
	public Button lowerBodyBtn, headBtn;
	
	
	[HeaderAttribute("Inventory Panels")]
	[SpaceAttribute(10f)]
	public GameObject  weaponPanel;
	public GameObject gearPanel, charactersPanel, weaponUpgradePanel;

	[HeaderAttribute("Inventory sub Panels")]
	[SpaceAttribute(10f)]
	// weapons
	public GameObject pistolPanel;
	public GameObject assaultPanel;
	public GameObject inGamePistolPanel, inGameRiflePanel, inGameSecRiflePanel, inGameGerenadePanel, inGameMeleePanel;

	[SpaceAttribute(10f)]
	// gears
	public GameObject upperBodyPanel;
	public GameObject lowerBodyPanel;
	public GameObject headPanel;

	[HeaderAttribute("References")]
	[SpaceAttribute(10f)]

	public GameObject weaponComp;
	public GameObject gearComp;
	public GameObject characterComp;
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
		// inventoryItems = GetComponent<V_InventoryItems>();

		inGamePistols = new List<V_InventoryItem>();
		inGameRifles  = new List<V_InventoryItem>();
		inGameRifles_secondary = new List<V_InventoryItem>();

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

		// sub buttons: Gears
		UIController.IfClick_GoTo(upperBodyBtn, () => UIController.Enable_DisableUI(upperBodyPanel, lowerBodyPanel, headPanel));
		UIController.IfClick_GoTo(lowerBodyBtn, () => UIController.Enable_DisableUI(lowerBodyPanel, upperBodyPanel, headPanel));
		UIController.IfClick_GoTo(headBtn, () => UIController.Enable_DisableUI(headPanel, upperBodyPanel, lowerBodyPanel));

		// equipped Items
		UIController.IfClick_GoTo(inGamePistolsBtn, ()=> UIController.Enable_DisableUI(inGamePistolPanel, inGameRiflePanel, inGameSecRiflePanel, inGameGerenadePanel,inGameMeleePanel));
		UIController.IfClick_GoTo(inGameSecRifleBtn, ()=> UIController.Enable_DisableUI(inGameRiflePanel, inGamePistolPanel, inGameSecRiflePanel, inGameGerenadePanel, inGameMeleePanel));
		UIController.IfClick_GoTo(inGameSecRifleBtn, ()=> UIController.Enable_DisableUI(inGameSecRiflePanel, inGamePistolPanel, inGameRiflePanel, inGameGerenadePanel, inGameMeleePanel));
		UIController.IfClick_GoTo(inGameGerenadeBtn, ()=> UIController.Enable_DisableUI(inGameGerenadePanel, inGamePistolPanel, inGameRiflePanel, inGameSecRiflePanel, inGameMeleePanel));
		UIController.IfClick_GoTo(inGameMeleeBtn, ()=> UIController.Enable_DisableUI(inGameMeleePanel, inGamePistolPanel, inGameRiflePanel, inGameSecRiflePanel, inGameGerenadePanel));
	}

	public void AddToInGameInventory(ref V_InventoryItem item)
	{
		switch (item.itemType)
		{
			case ItemTypes.W_PISTOL:
				AddPistolToInGamePistols(item);
				break;

			case ItemTypes.W_ASSAULT:
				Debug.LogAssertion("item is a rifle " + item.name);
				if(inGameRiflePanel.activeSelf)
				{
					AddRifleToInGameRifles(item);
				}
				else if (inGameSecRiflePanel.activeSelf)
				{
					AddSecRifleToInGameRifles(item);
				}	
				else
				{
					UIController.ThrowError("V_Inventory_UI: AddToInGameInventory: Cannot add rifle to either primary or secondary group!" 
											+ "somethin wrong with item settign", UIController.CloseError);
				}
				break;

			default:
				break;
		}
	}
	private void AddPistolToInGamePistols(V_InventoryItem item)
	{
		bool successfulOperation = false;
		while(inGamePistols.Count < MAX_NUM_FOR_WEAPON_SLUTS)
		{
			inGamePistols.Add(null);
		}
		// checking for empty space in inGamePistols
		for(int i = 0; i < MAX_NUM_FOR_WEAPON_SLUTS - 1; i++)
		{
			if (inGamePistols[i] = null)
			{
				item.isAnInGameItem = true;
				inGamePistols.Add(item);
				successfulOperation = true;
				break;
			}
		}
		if (!successfulOperation)
		{
			//#revision: notify the player that you cannot add this item to inGames; 
		}
	}

	private void AddRifleToInGameRifles(V_InventoryItem item)
	{
		bool successfulOperation = false;
		while(inGameRifles.Count < MAX_NUM_FOR_WEAPON_SLUTS)
		{
			inGameRifles.Add(null);
		}
		// checking for empty space in inGameRifles
		for(int i = 0; i < MAX_NUM_FOR_WEAPON_SLUTS - 1; i++)
		{
			if (inGameRifles[i] = null)
			{
				item.isAnInGameItem = true;
				inGameRifles.Add(item);
				successfulOperation = true;
				break;
			}
		}
		if (!successfulOperation)
		{
			//#revision: notify the player that you cannot add this item to inGames; 
		}
	}
	private void AddSecRifleToInGameRifles(V_InventoryItem item)
	{
		bool successfulOperation = false;
		while(inGameRifles_secondary.Count < MAX_NUM_FOR_WEAPON_SLUTS)
		{
			inGameRifles_secondary.Add(null);
		}
		// checking for empty space in inGameRifles_secondary
		for(int i = 0; i < MAX_NUM_FOR_WEAPON_SLUTS - 1; i++)
		{
			if (inGameRifles_secondary[i] = null)
			{
				item.isAnInGameItem = true;
				inGameRifles_secondary.Add(item);
				successfulOperation = true;
				break;
			}
		}
		if (!successfulOperation)
		{
			//#revision: notify the player that you cannot add this item to inGames; 
		}

	}
	public void EquipItem(ref V_InventoryItem item)
	{
		switch (item.itemType)
		{
			case ItemTypes.W_PISTOL:
				EquipPistol(item, true);
				break;

			case ItemTypes.W_ASSAULT:
				if(inGameRiflePanel.activeInHierarchy)
				{
					EquipAssault(item, true);
				}
				else if (inGameSecRiflePanel.activeInHierarchy)
				{
					EquipAssault_secondary(item, true);
				}	
				else
				{
					UIController.ThrowError("V_Inventory_UI: EquipItem: Cannot set rifle to either primary or secondary group!" 
											+ "somethin wrong with item settign", UIController.CloseError);
				}
				break;

			default:
				break;
		}
	}
	public void UnEquipItem(ref V_InventoryItem item)
	{
		switch (item.itemType)
		{
			case ItemTypes.W_PISTOL:
				EquipPistol(item, false);
				break;

			case ItemTypes.W_ASSAULT:
				if(inGameRiflePanel.activeInHierarchy)
				{
					EquipAssault(item, false);
				}
				else if (inGameSecRiflePanel.activeInHierarchy)
				{
					EquipAssault_secondary(item, false);
				}
				else
				{
					UIController.ThrowError("V_Inventory_UI: UnEquipItem: Cannot set rifle to either primary or secondary group!" 
											+ "somethin wrong with item settign", UIController.CloseError);
				}	
				break;

			default:
				break;
		}
	}
	private void EquipPistol(V_InventoryItem pistol, bool equipped)
	{
		pistol.isEquipped = equipped;
	}
	private void EquipAssault(V_InventoryItem assault, bool equipped)
	{
		assault.isEquipped = equipped;
	}
	private void EquipAssault_secondary(V_InventoryItem assault_sec, bool equipped)
	{
		assault_sec.isEquipped = equipped;
	}
	public new void OnEnable()
	{
		base.OnEnable();
		UIController.currentPanel = this.gameObject;
		
		weaponPanel.SetActive(true);
		gearPanel.SetActive(false);
		charactersPanel.SetActive(false);	

		weaponComp.SetActive(false);
		gearComp.SetActive(false);
		characterComp.SetActive(false);

		ListItems();
	}
	public void ListItems()
	{

		foreach (GameObject item in inventoryItems.GetList())
		{
			switch (item.GetComponent<V_InventoryItem>().itemType)
			{
				case ItemTypes.W_PISTOL:
				item.transform.SetParent(pistolPanel.GetComponentInChildren<GridLayoutGroup>().gameObject.transform, false);
				break;

				case ItemTypes.W_ASSAULT:
				item.transform.SetParent(assaultPanel.GetComponentInChildren<GridLayoutGroup>().gameObject.transform, false);
				break;

				case ItemTypes.G_UPPERBODY:
				item.transform.SetParent(upperBodyPanel.GetComponentInChildren<GridLayoutGroup>().gameObject.transform, false);
				break;

				case ItemTypes.G_LOWERBODY:
				item.transform.SetParent(lowerBodyPanel.GetComponentInChildren<GridLayoutGroup>().gameObject.transform, false);
				break;

				case ItemTypes.G_HEAD:
				item.transform.SetParent(headPanel.GetComponentInChildren<GridLayoutGroup>().gameObject.transform, false);
				break;

				default:
				UIController.ThrowError("UInitialized gameObject: " + item.name, UIController.CloseError);
				break;
			}
		}
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
		// if so, there has been an error
		if (item1 == null)
		{
			print("V_Inventory_UI: CompareItems(): Cannot compare Items");
			return;
		}	
		if (item2 != null)
		{
			// prevent comparing 2 items from the different types
			if (item1.itemType != item2.itemType)
			{
				return;
			}
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
	void CompareWeapon(V_InventoryItem item1, V_InventoryItem item2)
	{
		tmpWeapon1 = item1.itemPrfb.GetComponent<V_Weapon>();

		if (tmpWeapon1 == null)
		{
			UIController.ThrowError("V_Inventory_UI: CompareWeapon(): Cannot Find a inventoryItem reference", UIController.CloseError);
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
			tmpGear2 = item2.GetComponent<V_Gear>();
			StartCoroutine(UIController.FillSlider(gearComparer.shield_secondItem, tmpGear2.shield));
		}
	}

	protected void CompareCharacter(V_InventoryItem item1, V_InventoryItem item2)
	{

	}
	
	
}
