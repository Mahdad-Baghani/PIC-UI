using UnityEngine;
using UnityEngine.UI;
using System.Collections;
// using System.Collections.Generic;
public class V_Shop : V_UIElement 
{

	// fields
	[SpaceAttribute(5f)]
	[HeaderAttribute("Shop Items and categories")]
	[SpaceAttribute(10f)]

	// to use while comparing the items
	[SerializeField] private V_ShopItem _selectedItem, itemTobuy;
	[SerializeField] private V_ShopItem _selectedItemByMouseHover;
	[SerializeField] private V_ShopItem _compareeItem;

	public V_ShopItem selectedItem 
	{ 
		get{return _selectedItem;} 
		set 
		{
			_selectedItem = value; 
			if (_selectedItemByMouseHover == value) _selectedItemByMouseHover = null;
			if (value != null) 
			{
				CompareItems(_selectedItem, _compareeItem);
				ShowWeaponDescription(_selectedItem.description);
				itemTobuy = _selectedItem;
			} 
			else
			{
				StopComparing();
			}
		}
	}

  
    public V_ShopItem selectedItemByMouseHover
	{
		get { return _selectedItemByMouseHover;}
		set 
		{
			_selectedItemByMouseHover = value;
			if(value != null)
				CompareItems(_selectedItemByMouseHover);
		}
	}
    public V_ShopItem compareeItem 
	{ 
		get{return _compareeItem;} 
		set
		{
			_compareeItem = value; 
			if (_compareeItem != _selectedItem)
			{
				CompareItems(_selectedItem, _compareeItem); 
			}
		}
	}

	[HeaderAttribute("Shop Buttons")]
	[SpaceAttribute(10f)]
	public Button weaponBtn;
	public Button gearsBtn, characterBtn, specialItemsBtn;

	[SpaceAttribute(5f)]
	public Button buyCreditBtn;
	public Button buyItemBtn, donateItemBtn;


	[HeaderAttribute("Shop subButtons")]
	[SpaceAttribute(10f)]
	// weaponButtons
	public Button pistolBtn;
	public Button assaultBtn;
	
	[SpaceAttribute(5f)]
	// gearButtons
	public Button upperBodyBtn;
	public Button loweBodyBtn, headBtn;
	
	[SpaceAttribute(5f)]
	// SpecialItems Buttons
	public Button specialWeaponsBtn;
	public Button specialCharactersBtn;


	[HeaderAttribute("Shop Panels")]
	[SpaceAttribute(10f)]
	public GameObject weaponsPanel;
	public GameObject gearsPanel, charactersPanel, specialItemsPanel;


	[HeaderAttribute("Shop subPanels")]
	[SpaceAttribute(10f)]
	// Weapons
	public GameObject pistolPanel;
	public GameObject assaultPanel;

	[SpaceAttribute(5f)]
	// Gears
	public GameObject upperBodyPanel;
	public GameObject lowerBodyPanel, headPanel;

	[SpaceAttribute(5f)]
	// SpecialItems
	public GameObject specialWeaponsPanel;
	public GameObject specialCharacterPanel;

	[HeaderAttribute("References and prefabs")]
	[SpaceAttribute(10f)]

	public GameObject BuyItemModal;

	public V_PlayerTemplate playerModel;
	public GameObject weaponTo3DPlaceholder;
	public GameObject weaponTo3D;
	public Text WeaponDescriptionTxt;
	public Slider rotateWeaponSlider;

	// character's score and credit
	public Text score;
	public Text credit;

	[SpaceAttribute(5f)]
	// comparison between items
	public GameObject weaponComp;
	public GameObject gearComp, characterComp;
	private V_WeaponComparison weaponComparer; //!!
	private V_GearComparison gearComparer;
	private V_CharacterComparison characterComparer;
	private V_Weapon tmpWeapon1, tmpWeapon2;
	private V_Gear tmpGear1, tmpGear2;

	[HeaderAttribute("extras")]
	[SpaceAttribute(10f)]
	public float shopRefreshRate;
	private bool isRefreshingPlayerStat = false;



	// methods
	public new void Awake()
	{
		base.Awake();
		try
		{
			weaponComparer = weaponComp.GetComponent<V_WeaponComparison>();
			gearComparer = gearComp.GetComponent<V_GearComparison>();
			characterComparer = characterComp.GetComponent<V_CharacterComparison>();
			// shop main buttons
			UIController.IfClick_GoTo(weaponBtn, () => 
			{
				UIController.Enable_DisableUI(weaponsPanel, gearsPanel, charactersPanel, specialItemsPanel);
				StopComparing();
			});

			UIController.IfClick_GoTo(gearsBtn, () => 
			{
				UIController.Enable_DisableUI(gearsPanel, weaponsPanel, charactersPanel, specialItemsPanel);
				StopComparing();
			});	
			UIController.IfClick_GoTo(characterBtn, () =>
			{
				UIController.Enable_DisableUI(charactersPanel, weaponsPanel, gearsPanel, specialItemsPanel);
				StopComparing();
			}); 
			UIController.IfClick_GoTo(specialItemsBtn, () => 
			{
				UIController.Enable_DisableUI(specialItemsPanel, weaponsPanel, gearsPanel, charactersPanel);
				StopComparing();
			});

			UIController.IfClick_GoTo(buyCreditBtn, BuyCredit);
			UIController.IfClick_GoTo(buyItemBtn, ()=> BuyItem(itemTobuy));

			// shop sub buttons

			UIController.IfClick_GoTo(pistolBtn, () => UIController.Enable_DisableUI(pistolPanel, assaultPanel));
			UIController.IfClick_GoTo(assaultBtn, () => UIController.Enable_DisableUI(assaultPanel, pistolPanel));

			UIController.IfClick_GoTo(upperBodyBtn, () => UIController.Enable_DisableUI(upperBodyPanel, lowerBodyPanel, headPanel));
			UIController.IfClick_GoTo(loweBodyBtn, () => UIController.Enable_DisableUI(lowerBodyPanel, upperBodyPanel, headPanel));
			UIController.IfClick_GoTo(headBtn, () => UIController.Enable_DisableUI(headPanel, upperBodyPanel, lowerBodyPanel));

			UIController.IfClick_GoTo(specialWeaponsBtn, ()=> UIController.Enable_DisableUI(specialWeaponsPanel, specialCharacterPanel));
			UIController.IfClick_GoTo(specialCharactersBtn, ()=> UIController.Enable_DisableUI(specialCharacterPanel, specialWeaponsPanel));

			UIController.OnSliderChangesValue(rotateWeaponSlider, (value)=>RotateWeapon((int)value));
		}
		catch (System.Exception err)
		{
			UIController.ThrowError("V_Shop: Awake(): " + err.Message,()=>
			{
				UIController.CloseError();
				return;
			});
		}

	}
	private void RotateWeapon(int amount)
	{
		Vector3 pos = weaponTo3DPlaceholder.transform.position;
	
		if(weaponTo3D != null && weaponTo3DPlaceholder != weaponTo3D)
		{
			Destroy(weaponTo3DPlaceholder);
			weaponTo3DPlaceholder = Instantiate(weaponTo3D) as GameObject;
		}
		weaponTo3DPlaceholder.transform.position = pos;
		// weaponTo3DPlaceholder.transform.Rotate(new Vector3(0, 0, 0));
		weaponTo3DPlaceholder.transform.Rotate(new Vector3(0, amount, 0));
	}
	
	new void OnEnable()
	{
		// hiding inherited OnEnable from V_UIElement
		base.OnEnable();
		UIController.currentPanel = this.gameObject;

		// initializing main panels
		weaponsPanel.SetActive(true);
		gearsPanel.SetActive(false);
		charactersPanel.SetActive(false);
		specialItemsPanel.SetActive(false);
		
		// initializing subPanels
		weaponComp.SetActive(false);
		gearComp.SetActive(false);
		characterComp.SetActive(false);

		// debug
		// print ("V_Shop: OnEnable(): hiding inherited OnEnable()");
	}
	private void ShowWeaponDescription(string description)
    {
		if(WeaponDescriptionTxt = null)
		{
			throw new System.Exception("V_Shop: ShowWeaponDescription(): WeaponDescriptionTxt is not set from inspector");
		}
		WeaponDescriptionTxt.text = description;
    }
	void Update()
	{
		if (!isRefreshingPlayerStat)
		{
			StartCoroutine(RefreshPlayer());
		}
	}
	void BuyCredit()
	{

	}
	void BuyCredit(int amount)
	{
		// we can set the purchase to be sufficient to buy the selectedItem
	}
	public void BuyItem(V_ShopItem item)
	{
		if (item == null)
		{
			UIController.ThrowError("V_Shop: BuyItem(): selectedItem is null", () => UIController.GoFrom_To(UIController.genericErrorModal, this.gameObject));
			return;
		}
		// #revision: checking player Charge and credit
		if (playerModel.badge.badgeType < item.requiredBadge)
		{
			UIController.ThrowError("your badge is not equivalent to what is needed to buy this weapon", UIController.CloseError);
			return;
		}
		if (playerModel.charge < item.requiredCharge)
		{
			UIController.AskYesNoQuestion("You dont have enough credit, wanna charge?",
			()=> // if pressed yes
			{
				BuyCredit();
				UIController.CloseYesNoQ();
				return;
			}, 
			()=> //if pressed no
			{
				UIController.CloseYesNoQ();
				return;
			});
		}
		if (playerModel.score < item.requiredScore)
		{
			UIController.ThrowError("U dont have enough game score to buy this item", UIController.CloseError);
			return;
		}

		// we open a modal to ask for purchase details
		UIController.Enable_DisableUI(BuyItemModal);
	}
	public void DonateItem(V_ShopItem item)
	{
		if (item == null)
		{
			UIController.ThrowError("V_Shop: DonateItem(): selectedItem is null", UIController.CloseError);
			return;
		}
		// #revision
		UIController.SelectFromListModal("Do you want to Donate ?",
		// if click select
		() => {},
		// if click cancel
		() => UIController.CloseSelectModal(),
		// if starts typing name
		(someName) => {/*check for someName in players*/});  
	}

	IEnumerator RefreshPlayer()
	{
		isRefreshingPlayerStat = true;
		// read Player Credit and score and show them!
		// credit.text = something;
		// score.text = something;
		yield return new WaitForSeconds(shopRefreshRate);
		isRefreshingPlayerStat = false;
	}
	private void CompareItems(V_ShopItem item1, V_ShopItem item2 = null)
    {
		
		if (item1 == null ) // if so, there has been an error
		{
			print("V_Shop: CompareItems(): Cannot compare Items");
			return;
		}	
		if (item2 != null)
		{
			if(item1.itemType != item2.itemType) // prevent comparing 2 items from the different types
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
			UIController.ThrowError("V_Shop: CompareItems(): error while loading items' types!", ()=>
			{
				UIController.CloseError();
			});
			break;
		}

    }
	private void StopComparing()
	{
		weaponComp.SetActive(false);
		gearComp.SetActive(false);
		characterComp.SetActive(false);
	}
	void CompareWeapon(V_ShopItem item1, V_ShopItem item2)
	{
		tmpWeapon1 = item1.itemPrfb.GetComponent<V_Weapon>();

		if (tmpWeapon1 == null)
		{
			UIController.ThrowError("V_Shop: CompareWeapon(): Cannot Find a ShopItem reference", UIController.CloseError);
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

	protected void CompareGear(V_ShopItem item1, V_ShopItem item2)
	{
		tmpGear1 = item1.itemPrfb.GetComponent<V_Gear>();
		if(tmpGear1 == null)
		{
			UIController.ThrowError("V_Shop: CompareGear: gears are not set properly", UIController.CloseError );
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
			tmpGear2 = item2.itemPrfb.GetComponent<V_Gear>();
			StartCoroutine(UIController.FillSlider(gearComparer.shield_secondItem, tmpGear2.shield));
			gearComparer.shield_secondItem.gameObject.SetActive(true);
		}
	}

	protected void CompareCharacter(V_ShopItem item1, V_ShopItem item2)
	{
		// #revision
	}
}
