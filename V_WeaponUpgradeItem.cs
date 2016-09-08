using UnityEngine;
using UnityEngine.EventSystems;
public class V_WeaponUpgradeItem : V_UIElement, IPointerDownHandler
{
	public enum UpgradeTypes
	{
		SCOPE,
		SILENCER
	}
	[SerializeField] V_Inventory_UI inventory;
	[SerializeField] private V_UpgradeObject _upgrade;
	public V_UpgradeObject upgrade
	{
		get { return _upgrade;}
		set 
		{ 
			// preventing from setting the upgrade to nulne!!! (null + none)
			if (_upgrade != null && value == null)
			{
				UIController.ThrowError("V_WeaponUpgradeItem: upgrade property: u are trying to set the upgrade to none, which is unacceptable!", UIController.CloseError);
				throw new System.Exception();
			}
			_upgrade = value;

		}
	}
	
	bool isUsed = false;
	public Sprite disabledTransparentTexture;  //used to show that item is now being used and cannot be used more than once!!! or something less fucked-up!!
	new void Awake()
	{
		base.Awake();

		if (disabledTransparentTexture == null)
		{
			throw new System.Exception("V_WeaponUpgradeItem: Awake(): disabledTransparentTexture is null! do something about it");
		}
	}
	public void OnPointerDown(PointerEventData data)
	{
		if(data.button == PointerEventData.InputButton.Left)
		{
			isUsed = !isUsed;
			if(isUsed)
			{

				inventory.AddOrRemoveUpgrade(this.upgrade, add: true);
				// disabledTransparentTexture.
			}
			else
			{
				inventory.AddOrRemoveUpgrade(this.upgrade, add: false);
			}
		}
	}
}
