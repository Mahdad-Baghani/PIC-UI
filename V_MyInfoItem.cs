using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class V_MyInfoItem : V_UIElement, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
	V_MyInfo myInfo;

	public GameObject itemPrfb;

	public Image icon;
	public Text itemNameTxt;
	public Image itemLevel;
	// public Button donateBtn;


	public new void Awake()
	{
		base.Awake();
		myInfo = FindObjectOfType<V_MyInfo>();
	}
	new void OnEnable()
	{
		// do not call base.OnEnable on this type of objects, so we keep it seperate from UI panels which need to call base.OnEnable() while hiding it
	}

	public void Initialize(GameObject prfb)
	{
		itemPrfb = prfb;
		V_Weapon someWeapon = itemPrfb.GetComponent<V_Weapon>();
		if (someWeapon)
		{
			// switch (someWeapon.type)
			// {
			// 	case V_Weapon.weaponType.pistol:
			// 	icon.sprite = someWeapon.icon;
			// 	itemNameTxt.text = someWeapon.name;
			// 	// itemLevel.sprite = someWeapon.level;
			// 	break;

			// 	case V_Weapon.weaponType.rifle:
				itemNameTxt.text = someWeapon.name;
				icon.sprite = someWeapon.icon;
				// itemle
				// // itemLevel.sprite = someWeapon.level;
				// break;

				// default:
				// break;
			// }
		}
	}

	public void OnPointerEnter(PointerEventData data)
	{

	}
	public void OnPointerDown(PointerEventData data)
	{
		if(data.button == PointerEventData.InputButton.Left)
		{
			EventSystem.current.SetSelectedGameObject(gameObject, data);
		}
	}

	public void OnPointerExit(PointerEventData data)
	{

	}

	public void OnSelect(BaseEventData data)
	{
		try
		{
			// #revision
			// myInfo.upgradeView.headshots.text = data.selectedObject.GetComponent<V_Weapon>().name;
			myInfo.upgradeView.headshots.text = string.Format(Random.Range(0,10) + "headshots wiht " + this.itemPrfb.GetComponent<V_Weapon>().name);
			// myInfo.upgradeView.level.value = Random.Range(0, 1);
			// myInfo.upgradeView.shootingAccuracy.value = Random.Range(0, 1);
			// if (this.itemPrfb.GetComponent<V_Weapon>().level == V_Weapon.weaponLevel.beginner1)
			// {
			// 	GameObject tmp = Instantiate(myInfo.upgradeView.levelI);
			// 	tmp.transform.SetParent(myInfo.upgradeView.levelPanel.transform, false);
			// }
		}
		catch (System.Exception err)
		{
			UIController.ThrowError(err.Message, UIController.CloseError);
			
			throw;
		}
	}

	public void OnDeselect(BaseEventData data)
	{

	}
}
