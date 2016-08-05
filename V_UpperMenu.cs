using UnityEngine;
using UnityEngine.UI;
public class V_UpperMenu : MonoBehaviour
{
	V_UIController UIController;

	[SerializeField]
	Button backButton;

	[SerializeField]
	Button settingsButton;

	[SerializeField]
	Button clanButton;

	[SerializeField]
	Button comradeButton;

	[SerializeField]
	Button shopButton;

	[SerializeField]
	Button inventoryButton;

	[SerializeField]
	Button gachaponButton;

	[SerializeField]
	Button myInfoButton;
	

	void Awake()
	{
		// #revision
		UIController = FindObjectOfType<V_UIController>();
		UIController.IfClick_GoTo(backButton, Back);
		// #revision and so on....
		// UIController.IfClick_GoTo(settingsButton, Settings);
		// UIController.IfClick_GoTo(clanButton, Clan);
		// UIController.IfClick_GoTo(comradeButton, Comrade);
		// UIController.IfClick_GoTo(shopButton, Shop);
		// UIController.IfClick_GoTo(inventoryButton, Inventory);
		// UIController.IfClick_GoTo(gachaponButton, Gachapon);
		// UIController.IfClick_GoTo(myInfoButton, MyInfo);

	}

	void Back()
	{
		// go back somewhere
		// check if the player is in lobby or somewhere else
		// UIController.GoToNextPanel("Lobby");
		if (UIController.currentPanel != null)
		{
			if (UIController.currentPanel == UIController.LobbyPanel)
			{
				UIController.AskYesNoQ("Do you want to quit the lobby?", 
				() => // yes event
				{
					// #revision
					Application.Quit();
				},
				() => // no event
				{
					UIController.GoFrom_To(UIController.genericYesNoModal, UIController.LobbyPanel);
				});
			}
			if (UIController.currentPanel == UIController.RoomPanel)
			{
				
			}
		}
	}
	public void Settings()
	{
		// go to settings
	}
	void Clan()
	{
		
	}

	void Comrade()
	{
		// go see my Comrade
	}
	void Shop()
	{

	}
	void Inventory()
	{
		// go to Inventory
	}
	void Gachapon()
	{
		// go to Gachapon
	}
	void MyInfo()
	{

	}



}