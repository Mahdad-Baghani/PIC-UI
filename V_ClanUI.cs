using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class V_ClanUI : V_UIElement 
{
	// fields:
	public GameObject initialStateOfClan;
	public Button GoToCreateClanModalBtn, JoinClanBtn, SeeOtherClansBtn;
	// modal window to create Clan
	
	public InputField clanNameText, clanGreetingMessageText;
	public Dropdown clanRegionDropDown, clanCurrentPlayersDropDown;
	public Button CreateClanBtn, CancelBtn;


	// methods:
	new void Awake()
	{
		base.Awake();

		// setting up buttons
		UIController.IfClick_GoTo(GoToCreateClanModalBtn, ()=> UIController.Enable_DisableUI(UIController.ClanModalPanel));
	}

}
