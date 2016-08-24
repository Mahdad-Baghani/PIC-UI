using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
public class V_InventoryItem : V_UIElement, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    // fileds
    V_Inventory_UI Inventory;

    public ItemTypes itemType;
    public ItemClass itemClass;
    public float itemTime;
    public GameObject itemPrfb;


    // buttons and UI refs
    public Image icon;
    public Button donateBtn, deleteBtn, customizeBtn;
    public Text itemNameTxt, timeTxt;
    public Image level;


    // methods
    public new void Awake()
    {
        base.Awake();
        Inventory = FindObjectOfType<V_Inventory_UI>();

        UIController.IfClick_GoTo(donateBtn, ()=> Inventory.DonateItem(this));
        // #revision: Save the deleted item so it doesnt show up anymore
        UIController.IfClick_GoTo(deleteBtn, () =>
        {
            UIController.AskYesNoQ("Do you want to delete this item?",
            () =>
            {
                Destroy(this.gameObject);
                UIController.CloseYesNoQ();
            },
            () =>
            {
                UIController.CloseYesNoQ();
            });
        });

        if (customizeBtn != null)
        {
            // UIController.IfClick_GoTo(customizeBtn, ()=> CustomizeWeapon(this));
        }
    }
    new void OnEnable()
    {
        // do not call base.OnEnable on this type of objects, so we keep it seperate from UI panels which need to call base.OnEnable() while hiding it
    }

    public void Initialize(GameObject prfb)
    {
        // #revision: big revision!!! 
        itemPrfb = prfb;
        V_Weapon someWeapon = itemPrfb.GetComponent<V_Weapon>();
        if (someWeapon)
        {
            if(itemPrfb.GetComponent<V_Weapon>().type == V_Weapon.weaponType.rifle)
            {
                itemClass = ItemClass.WEAPON;
                itemType = ItemTypes.W_ASSAULT;
            }
            if(itemPrfb.GetComponent<V_Weapon>().type == V_Weapon.weaponType.pistol)
            {
                itemClass = ItemClass.WEAPON;
                itemType = ItemTypes.W_PISTOL;
            }


            switch (itemClass)
            {
                case ItemClass.WEAPON:
                    icon.sprite = itemPrfb.GetComponent<V_Weapon>().icon;
                    itemNameTxt.text = itemPrfb.GetComponent<V_Weapon>().name;
                    timeTxt.text = itemPrfb.GetComponent<V_Weapon>().lifeTime.ToString();

                    break;

                case ItemClass.GEAR:
                    icon.sprite = itemPrfb.GetComponent<V_Gear>().icon;
                    itemNameTxt.text = itemPrfb.GetComponent<V_Gear>().name;
                    // #revision: does Gear have lifeTime?
                    // timeTxt.text = itemPrfb.GetComponent<V_Gear>().lifeTime.ToString();

                    break;

                case ItemClass.CHARACTER:
                    // icon = itemPrfb.GetComponent<V_Character>().icon;
                    break;

                default:
                    UIController.ThrowError("V_InventoryItem: Item type is not set properly", UIController.CloseError);
                    break;
            }
        }
    }
    public virtual void OnPointerDown(PointerEventData data)
    {
        StartCoroutine(OnPointerDown_C(data));
    }
    public virtual void OnPointerEnter(PointerEventData data)
    {
        if (Inventory.selectedItem == null)
        {
            return;
        }
        if (Inventory.selectedItem != this && Inventory.selectedItem.itemType == this.itemType)
        {
            Inventory.compareeItem = this;
        }
        // else
        // {
        //     Inventory.selectedItem = null;
        // }
    }
    public  IEnumerator OnPointerDown_C(PointerEventData data)
    {
        // Double click factor
        float dbFactor = 1f;
        if (data.button == PointerEventData.InputButton.Left)
        {
            EventSystem.current.SetSelectedGameObject(gameObject, data);
        }
        if (Inventory.compareeItem == this)
        {
            Inventory.compareeItem = null;
        }
        yield return new WaitForSeconds(dbFactor);
        if (data.button == PointerEventData.InputButton.Left)
        {
            // Inventory.Equip(this);
            print("equipped item " + this.name);
        }
        yield return null;
    }
    public virtual void OnPointerExit(PointerEventData data)
    {
        // Inventory.compareeItem = null;
    }

    public virtual void OnSelect(BaseEventData data)
    {
        try
        {
            Inventory.selectedItem = data.selectedObject.GetComponent<V_InventoryItem>();
        }
        catch (System.Exception err)
        {
            UIController.ThrowError("V_InventoryItem: OnSelect(): " + err.Message, UIController.CloseError);
            throw;
        }
    }

    public virtual void OnDeselect(BaseEventData data)
    {
        Inventory.selectedItem = null;
        Inventory.compareeItem = null;
    }
}