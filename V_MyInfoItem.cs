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
			myInfo.upgradeView.headshots.text = this.itemPrfb.GetComponent<V_Weapon>().name;
			myInfo.upgradeView.level.value = Random.Range(0, 1);
			myInfo.upgradeView.shootingAccuracy.value = Random.Range(0, 1);
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
