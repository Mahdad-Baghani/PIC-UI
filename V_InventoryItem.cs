using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

public class V_InventoryItem : V_UIElement, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    // fileds
    V_Inventory_UI Inventory;

    public ItemTypes itemType;
    public ItemClass itemClass;
    public Sprite icon;
    public GameObject itemPrfb;

    // buttons
    public Button donateBtn;
    public Button deleteBtn;
    public Button customizeBtn;
    public Text itemNameTxt;
    public Image level;
    public Text timeTxt;


    // methods
    public new void Awake()
    {
        base.Awake();
        Inventory = FindObjectOfType<V_Inventory_UI>();

        switch (itemClass)
        {
            case ItemClass.WEAPON:
                icon = itemPrfb.GetComponent<V_Weapon>().icon;
                itemNameTxt.text = itemPrfb.GetComponent<V_Weapon>().name;
                timeTxt.text = itemPrfb.GetComponent<V_Weapon>().lifeTime.ToString();

                break;

            case ItemClass.GEAR:
                icon = itemPrfb.GetComponent<V_Gear>().icon;
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
        else
        {
            Inventory.selectedItem = null;
        }

    }
    public virtual void OnPointerDown(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            EventSystem.current.SetSelectedGameObject(gameObject, data);
        }
        if (Inventory.compareeItem == this)
        {
            Inventory.compareeItem = null;
        }
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