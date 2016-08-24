using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class V_Clan : V_UIElement 
{
	// keep in sync with UI, this part must be in persian
	public const string REG_KHORASAN_RAZAVI = "Khorasan Razavi";
	public Sprite[] defaultLogos;
	// fields:
	public GameObject createClan_ModalPanel;
	public GameObject joinClan_ModalPanel;
	[SerializeField] V_PlayerTemplate currentPlayer;
	[SerializeField] float clanAlertDelay;
	public GameObject initialStateOfClan, clanAlertText;
	public Button CreateClanBtn, JoinClanBtn, SeeOtherClansBtn;

	// createClan_ModalPanel: modal window to create Clan
	public InputField clanNameText, clanGreetingMessageText;
	public Dropdown clanRegionDropDown, clanCurrentPlayersDropDown;
	public Button modalCreateClanBtn, modalCancelBtn;
	
	// joinClan_ModalPanel: modal window to join a clan


	// methods:
	new void Awake()
	{
		base.Awake();

		// setting up buttons
		UIController.IfClick_GoTo(CreateClanBtn, ()=> UIController.Enable_DisableUI(createClan_ModalPanel));
		UIController.IfClick_GoTo(JoinClanBtn, ()=> UIController.Enable_DisableUI(joinClan_ModalPanel));

		// createClan_ModalPanel vars:
		UIController.IfClick_GoTo(modalCancelBtn, ()=> UIController.Disable_EnableUI(createClan_ModalPanel));
		UIController.IfClick_GoTo(modalCreateClanBtn, ()=> OnCreateClan() );

	}
	IEnumerator Start()
	{
		if(currentPlayer.clan == null)
		{
			yield return new WaitForSeconds(2f);
			yield return StartCoroutine(UIController.FadeIn(clanAlertText.gameObject));
			yield return new WaitForSeconds(clanAlertDelay);
			yield return StartCoroutine(UIController.FadeOut(clanAlertText.gameObject));
		}
	}

	private void OnCreateClan()
	{
		try
		{
			if (currentPlayer.clan != null)
			{
				currentPlayer.clan.clanName = clanNameText.text;
				currentPlayer.clan.logo = ReturnClanLogo();
				currentPlayer.clan.region = ReturnClanRegion();
				currentPlayer.clan.purchasedPlayerCapacity = ReturnPurchasedPlayerCapacity();
			}
		}
		catch (System.Exception err)
		{
			UIController.ThrowError("V_Clan: OnCreateClan(): " + err.Message, UIController.CloseError);
			throw;
		}
	}

    private Sprite ReturnClanLogo()
    {
		return null;
    }

    private string ReturnClanRegion()
	{
		switch(clanRegionDropDown.options[clanRegionDropDown.value].text)
		{
			case REG_KHORASAN_RAZAVI:
			return "Razavi Khorasan";

			default:
			UIController.ThrowError("V_Clan: ReturnClanRegion(): Region was not selected", UIController.CloseError);
			return "Error in selecting Region";
		}
	}
	private int ReturnPurchasedPlayerCapacity()
	{
		switch (clanCurrentPlayersDropDown.value)
		{
			case 0:
			return 6;

			case 1:
			return 10;

			default:
			return 6;
		}
	}

}
