using UnityEngine;
using UnityEngine.UI;
public class V_UpperMenu : V_UIElement
{

	[SerializeField] Button settingsButton, clanButton, comradeButton, shopButton, inventoryButton, gachaponButton, myInfoButton;
	

	new void Awake()
	{
		base.Awake();
		// UIController.IfClick_GoTo(UIController.backButton, Back);
		UIController.IfClick_GoTo(settingsButton, ()=> UIController.GoFrom_To(UIController.currentPanel, GameObject.FindGameObjectWithTag("SettingPanel")));
		// UIController.IfClick_GoTo(clanButton, ()=> UIController.GoFrom_To(UIController.currentPanel, GameObject.FindGameObjectWithTag("ClanPanel")));
		UIController.IfClick_GoTo(comradeButton, ()=> UIController.GoFrom_To(UIController.currentPanel, GameObject.FindGameObjectWithTag("ComradePanel")));
		UIController.IfClick_GoTo(shopButton, ()=> UIController.GoFrom_To(UIController.currentPanel, GameObject.FindGameObjectWithTag("ShopPanel")));
		UIController.IfClick_GoTo(inventoryButton, ()=> UIController.GoFrom_To(UIController.currentPanel, GameObject.FindGameObjectWithTag("InventoryPanel")));
		// UIController.IfClick_GoTo(gachaponButton, ()=> UIController.GoFrom_To(UIController.currentPanel, GameObject.FindGameObjectWithTag("GachaponPanel")));
		UIController.IfClick_GoTo(myInfoButton, ()=> UIController.GoFrom_To(UIController.currentPanel, GameObject.FindGameObjectWithTag("MyInfoPanel")));
	}

	new void OnEnable()
	{
		
	}
}