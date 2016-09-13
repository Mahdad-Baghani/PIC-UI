using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(V_InventoryItems))]
public class V_Inventory_UI : V_UIElement
{
// fields
	// consts 
	public const int MAX_WEAPON_SLOT_NUM = 6;
	public const int MAX_WEAPON_UPGRADE_NUM = 8;

	// a dependency which reads the items from server
	public V_InventoryItems inventoryItems;

	// lists to keep track of inGame weapons
	[SerializeField] List<V_InventoryItem> inGamePistols;
	[SerializeField] List<V_InventoryItem> inGameRifles;
	[SerializeField] List<V_InventoryItem> inGameSecRifles;
	
	public GameObject[] inGamePistolSlots, inGameRifleSlots, inGameSecRifleSlots = new GameObject[MAX_WEAPON_SLOT_NUM];

	// to keep track of upgrades
	public GameObject[] weaponUpgrades = new GameObject[MAX_WEAPON_UPGRADE_NUM];
	
	// selectedItem, selectedItemByMouseHover, compareeItem to keep track of different mouse interactions
	[SerializeField] private V_InventoryItem _selectedItemByMouseHover, _selectedItem, _compareeItem;
	public V_InventoryItem selectedItemByMouseHover
	{
		get { return _selectedItemByMouseHover;}
		set 
		{
			_selectedItemByMouseHover = value;
			if(value != null) 
				CompareItems(_selectedItemByMouseHover);
			else 
				StopComparing();
		}
	}
	public V_InventoryItem selectedItem
	{
		get { return _selectedItem;}
		set 
		{
			 _selectedItem = value;
			 if (value != null)
			 {
				 if (_selectedItemByMouseHover == value) _selectedItemByMouseHover = null;
				 CompareItems(_selectedItem, _compareeItem);
			 }
			 else
			 {
				 StopComparing();
			 }
		}
	}
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

	// last equipped Weapon: used when we want to add the upgrades:
	V_Weapon lastEquippedWeapon;

	// equippedPistol, equippedRifle, equippedSecRifle are set to in case we need to refer to different equipped classes of weapons
	[SerializeField] private V_InventoryItem _equippedPistol;
	public V_InventoryItem equippedPistol
	{
		get { return _equippedPistol;}
		set 
		{
			_equippedPistol = value;
			if(value != null)
			{
				ShowUpgrades(_equippedPistol);
				lastEquippedWeapon = _equippedPistol.itemPrfb.GetComponent<V_Weapon>();
			}
		}
	}
	[SerializeField] private V_InventoryItem _equippedRifle;
	public V_InventoryItem equippedRifle
	{
		get { return _equippedRifle;}
		set 
		{
			_equippedRifle = value;
			if(value != null)
			{
				ShowUpgrades(_equippedRifle);
				lastEquippedWeapon = _equippedRifle.itemPrfb.GetComponent<V_Weapon>();
			}
		}
	}
	[SerializeField] private V_InventoryItem _equippedSecRifle;
	public V_InventoryItem equippedSecRifle
	{
		get { return _equippedSecRifle;}
		set 
		{
			_equippedSecRifle = value;
			if(value != null)
			{
				ShowUpgrades(_equippedSecRifle);
				lastEquippedWeapon = _equippedSecRifle.itemPrfb.GetComponent<V_Weapon>();
			}
		}
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
	public GameObject gearPanel, charactersPanel, weaponUpgradePanel, weaponCustomizePanel, weapon_to_customize_placeholder, weaponToCustomize;
	
	public Slider rotateWeaponSlider;

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

	[HeaderAttribute("References and prefabs")]
	[SpaceAttribute(10f)]

	public GameObject weaponComp;
	public GameObject gearComp;
	public GameObject characterComp;
	V_WeaponComparison weaponComparer;
	V_GearComparison gearComparer;
	V_CharacterComparison characterComparer;
	private V_Weapon tmpWeapon1, tmpWeapon2;
	private V_Gear tmpGear1, tmpGear2;
	public GameObject tmpWeaponUpgrade;

	// private V_Character tmpCharacter1, tmpCharacter2;

	// methods
	public new void Awake()
	{
		base.Awake();

		InitializeInGameSlots();
		try
		{
			weaponComparer = weaponComp.GetComponent<V_WeaponComparison>();
			gearComparer = gearComp.GetComponent<V_GearComparison>();
			characterComparer = characterComp.GetComponent<V_CharacterComparison>();

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
			UIController.IfClick_GoTo(inGamePistolsBtn, OpenInGamePistol);
			UIController.IfClick_GoTo(inGameRifleBtn, OpenInGameRifle );
			UIController.IfClick_GoTo(inGameSecRifleBtn, OpenInGameSecRifle);
			UIController.IfClick_GoTo(inGameGerenadeBtn, OpenInGameGerenade);
			UIController.IfClick_GoTo(inGameMeleeBtn, OpenInGameMelee);

			// customizeWeapon
			UIController.OnSliderChangesValue(rotateWeaponSlider, (value)=> RotateWeapon((int)value));
		}
		catch (System.Exception err)
		{
			UIController.ThrowError("V_Inventory_UI: Awake(): " + err.Message, ()=>{UIController.CloseError(); return;});
			throw new System.Exception();			
		}


	}
	private void RotateWeapon(int amount)
	{
		Vector3 pos = weapon_to_customize_placeholder.transform.position;

		if(weaponToCustomize != null && weapon_to_customize_placeholder != weaponToCustomize)
		{
			Destroy(weapon_to_customize_placeholder);
			weapon_to_customize_placeholder = Instantiate(weaponToCustomize) as GameObject;
		}
		weapon_to_customize_placeholder.transform.position = pos;
		// weapon_to_customize_placeholder.transform.Rotate(new Vector3(0, 0, 0));
		weapon_to_customize_placeholder.transform.Rotate(new Vector3(0, amount, 0));
	}
	private void OpenInGamePistol()
	{
		UIController.Enable_DisableUI(inGamePistolPanel, inGameRiflePanel, inGameSecRiflePanel, inGameGerenadePanel,inGameMeleePanel);
	}
	private void  OpenInGameRifle()
	{
		UIController.Enable_DisableUI(inGameRiflePanel, inGamePistolPanel, inGameSecRiflePanel, inGameGerenadePanel, inGameMeleePanel);
	}
	private void OpenInGameSecRifle()
	{
		UIController.Enable_DisableUI(inGameSecRiflePanel, inGamePistolPanel, inGameRiflePanel, inGameGerenadePanel, inGameMeleePanel);
	}
	private void OpenInGameGerenade()
	{
		UIController.Enable_DisableUI(inGameGerenadePanel, inGamePistolPanel, inGameRiflePanel, inGameSecRiflePanel, inGameMeleePanel);
	}
	private void OpenInGameMelee()
	{	
		UIController.Enable_DisableUI(inGameMeleePanel, inGamePistolPanel, inGameRiflePanel, inGameSecRiflePanel, inGameGerenadePanel);
	}
	public void InitializeInGameSlots()
	{
		inGamePistols = new List<V_InventoryItem>();
		inGameRifles  = new List<V_InventoryItem>();
		inGameSecRifles = new List<V_InventoryItem>();
		for (int i = 0; i < MAX_WEAPON_SLOT_NUM; i++)
		{
			inGamePistols.Add(null);
			inGameRifles.Add(null);
			inGameSecRifles.Add(null);
		}
	}
	    private void HidePreviousUpgrades()
    {
		GameObject[] nestedUpgrades = new GameObject[8]; 
		Transform gridTransform = weaponUpgradePanel.GetComponentInChildren<GridLayoutGroup>().transform;
		for (int i = 0; i < gridTransform.childCount; i++)
		{
			nestedUpgrades[i] = gridTransform.GetChild(i).gameObject;
		}

		if(nestedUpgrades == null)
		{
			throw new System.Exception("V_Inventory_UI: HideUpgrades: No upgrade is set for this Weapon!!!!!");
		}

		int tmpI = 0;
		foreach (GameObject item in nestedUpgrades)
		{
			if (item != null)
			Destroy(item);
			weaponUpgrades[tmpI] = null;
			tmpI++;
		}
    }
    public void ShowUpgrades(V_InventoryItem item)
    {
		HidePreviousUpgrades();
        if (item.itemClass != ItemClass.WEAPON)
		{
			return;
		}
		if(tmpWeaponUpgrade == null)
		{
			throw new System.Exception("V_Inventory_UI: ShowUpgrades(): tmpWeaponUpgrade is null");
		}
		weaponUpgradePanel.SetActive(true);
		int tmpI = 0;
		foreach (V_UpgradeObject upgrade in item.itemPrfb.GetComponent<V_Weapon>().upgrades)
		{
			GameObject tmpObj = Instantiate(tmpWeaponUpgrade) as GameObject;
			tmpObj.GetComponent<Image>().sprite = upgrade.icon;
			tmpObj.GetComponent<V_WeaponUpgradeItem>().upgrade = upgrade;
			tmpObj.transform.SetParent(weaponUpgradePanel.GetComponentInChildren<GridLayoutGroup>().transform ,false);		
			weaponUpgrades[tmpI] = tmpObj;
			tmpI++;
		}
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
				if(!inGameRiflePanel.activeInHierarchy && inGameSecRiflePanel.activeInHierarchy)
				{
					AddRifleToInGameRifles(item);
					break;
				}
				if(inGameRiflePanel.activeInHierarchy)
				{
					Debug.LogAssertion("adding rifle " + item.name + "to primary rifles");
					AddRifleToInGameRifles(item);
				}
				else if (inGameSecRiflePanel.activeInHierarchy)
				{
					Debug.LogAssertion("adding rifle " + item.name + "to secondary rifles");
					AddSecRifleToInGameRifles(item);
				}	
				else
				{
					UIController.ThrowError("V_Inventory_UI: AddToInGameInventory: Cannot add rifle to either primary or secondary group!" 
											+ "somethin wrong with item setting", UIController.CloseError);
					throw new System.Exception("V_Inventory_UI: AddToInGameInventory(): somethin wrong with item setting");
				}
				break;

			default:
				break;
		}
	}
	private void AddPistolToInGamePistols(V_InventoryItem item)
	{
		bool successfulOperation = false;
		// checking for empty space in inGamePistols
		for(int i = 0; i < MAX_WEAPON_SLOT_NUM - 1; i++)
		{
			if (inGamePistols[i] == null)
			{
				// inGamePistolSlots[i] = item.gameObject;
				item.isAnInGameItem = true;
				inGamePistols[i] = item;
				successfulOperation = true;
				// #revision
				// item.gameObject.transform.SetParent(inGamePistolSlots[i].transform, false);
				item.gameObject.transform.SetParent(inGamePistolPanel.GetComponent<GridLayoutGroup>().transform, false);
				break;
			}
		}
		if (!successfulOperation)
		{
			//#revision: notify the player that you cannot add this item to inGames;
			Debug.LogError("inGamesPistols does not have an empty slot"); 
		}
	}

	private void AddRifleToInGameRifles(V_InventoryItem item)
	{
		bool successfulOperation = false;
		// checking for empty space in inGameRifles
		for(int i = 0; i < MAX_WEAPON_SLOT_NUM - 1; i++)
		{
			if (inGameRifles[i] == null)
			{
				item.isAnInGameItem = true;
				inGameRifles[i] = item;
				successfulOperation = true;
				// #revision
				// item.gameObject.transform.SetParent(inGameRifleSlots[i].transform, false);
				item.gameObject.transform.SetParent(inGameRiflePanel.GetComponent<GridLayoutGroup>().transform, false);
				break;
			}
		}
		if (!successfulOperation)
		{
			//#revision: notify the player that you cannot add this item to inGames;			 
 			Debug.LogError("inGameRifles does not have an empty slot"); 
		}
	}
	private void AddSecRifleToInGameRifles(V_InventoryItem item)
	{
		bool successfulOperation = false;
		// checking for empty space in inGameSecRifles
		for(int i = 0; i < MAX_WEAPON_SLOT_NUM - 1; i++)
		{
			if (inGameSecRifles[i] == null)
			{
				item.isAnInGameItem = true;
				inGameSecRifles[i] = item;
				successfulOperation = true;
				// #revision
				// item.gameObject.transform.SetParent(inSecGameRifleSlots[i].transform, false);
				item.gameObject.transform.SetParent(inGameSecRiflePanel.GetComponent<GridLayoutGroup>().transform, false);
				break;
			}
		}
		if (!successfulOperation)
		{
			//#revision: notify the player that you cannot add this item to inGames;
			Debug.LogError("inGameSecRifles does not have an empty slot"); 
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
				item.transform.SetParent(pistolPanel.GetComponentInChildren<GridLayoutGroup>().transform, false);
				item.transform.SetSiblingIndex(item.indexOfItemInList);
				HidePreviousUpgrades();
				break;

			case ItemTypes.W_ASSAULT:
				if(inGameRiflePanel.activeInHierarchy)
				{
					EquipAssault(item, false);
					item.transform.SetParent(assaultPanel.GetComponentInChildren<GridLayoutGroup>().transform, false);
					item.transform.SetSiblingIndex(item.indexOfItemInList);
					HidePreviousUpgrades();
				}
				else if (inGameSecRiflePanel.activeInHierarchy)
				{
					EquipAssault_secondary(item, false);
					item.transform.SetParent(assaultPanel.GetComponentInChildren<GridLayoutGroup>().transform, false);
					item.transform.SetSiblingIndex(item.indexOfItemInList);
					HidePreviousUpgrades();
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
		if (equipped)
		{
			equippedPistol = pistol;
		}
		else
		{
			pistol.isAnInGameItem = false;
			equippedPistol = null;
			// Debug.LogAssertion("inGamePistols is " + inGamePistols);
			int? tmpIndex = UIController.ReturnIndexInList(inGamePistols, pistol);
			// Debug.LogAssertion("tmpIndex is " + tmpIndex);
			if(tmpIndex != null)
			{
				inGamePistols[(int)tmpIndex] = null;
			}
		}
	}
	private void EquipAssault(V_InventoryItem assault, bool equipped)
	{
		assault.isEquipped = equipped;
		if(equipped)
		{
			equippedRifle = assault;
		}
		else
		{
			assault.isAnInGameItem = false;
			equippedRifle = null;
			int? tmpIndex = UIController.ReturnIndexInList(inGameRifles, assault);
			if(tmpIndex != null)
			{
				inGameRifles[(int)tmpIndex] = null;
			}
		}
	}
	private void EquipAssault_secondary(V_InventoryItem assault_sec, bool equipped)
	{
		assault_sec.isEquipped = equipped;
		if(equipped)
		{
			equippedSecRifle = assault_sec;
		}
		else
		{
			assault_sec.isAnInGameItem = false;
			equippedSecRifle = null;
			int? tmpIndex = UIController.ReturnIndexInList(inGameSecRifles, assault_sec);
			if(tmpIndex != null)
			{
				inGameSecRifles[(int)tmpIndex] = null;
			}
		}
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
	public void AddOrRemoveUpgrade(V_UpgradeObject upItem, bool add)
	{
		// int? tmpIndex = UIController.ReturnIndexInList(lastEquippedWeapon.upgrades, upItem);
		int tmpI = lastEquippedWeapon.upgrades.IndexOf(upItem);

		// #revision: may need to change the lines on setting isUsed and model,SetActive to true
		if(tmpI != -1)
		{
			upItem.isUsed = add;
			upItem.model.SetActive(add);
			print(upItem + " was added");
		}

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
				throw new System.Exception();
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
