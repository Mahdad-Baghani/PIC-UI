using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class V_Lobby : V_UIElement 
{
	// fields
	// #revision
	V_CustomLobbyManager LobbyManager;
	
	// panels to go from Lobby
	public GameObject gameModePanel;
	public Transform  fastCreateRoomPanelTransform, CreateRoomPanelTransform;
	
	// Buttons

	public Button enterButton, createRoomButton, fastRoomButton;
	public Button GM_SD_Button, GM_TMD_Button, GM_DM_Button;
	bool firstCreateTheRoom;
	float gameModePreferredHeight;
	
	new void Awake()
	{
		float gameModePreferredHeight = gameModePanel.GetComponent<LayoutElement>().preferredHeight;
		print(gameModePreferredHeight);

		base.Awake();
		// #Revision 
		LobbyManager = FindObjectOfType<V_CustomLobbyManager>();
		try
		{
			UIController.IfClick_GoTo(enterButton, OnEnterTheRoom);
			UIController.IfClick_GoTo(createRoomButton, ()=> OnCreateRoom());
			UIController.IfClick_GoTo(fastRoomButton, ()=> OnCreateFastRoom());	
			UIController.IfClick_GoTo(GM_SD_Button, () => {ChangeGameMode(GameModes.SD);});
			UIController.IfClick_GoTo(GM_TMD_Button, () => {ChangeGameMode(GameModes.TDM);});
			// #add other gameModes buttons
		}
		catch (System.Exception err)
		{
			UIController.ThrowError(err.Message, UIController.CloseError);
			throw;
		}

	}
	new void OnEnable()
	{
		base.OnEnable();
		UIController.currentPanel = this.gameObject;
		gameModePanel.SetActive(true);
	}
	public void OnEnterTheRoom()
	{
		// enters the room
		if (enterButton == null)
		{
			print("V_Lobby: OnEnterTheRoom(): enterButton is not set");
			return;
		}
		
		// Join the Room via V_CustomLobbyManager
		// V_CustomLobbyManager.instance.GetRoom(idFromSelectedRoomInTheList~!);
	}
	void OnCreateRoom()
	{
		// go to Room page
		if (!gameModePanel.activeInHierarchy)
		{
			gameModePanel.SetActive(true);
		}

		firstCreateTheRoom = true;
	}

    private void SetGameModes(bool CreateTheRoomFirst)
    {
		// firstCreateTheRoom = CreateTheRoomFirst;
		// LayoutElement gameModeLayout = gameModePanel.GetComponent<LayoutElement>();
		// if(CreateTheRoomFirst && gameModePanel.transform.parent != CreateRoomPanelTransform)
		// {
		// 	gameModePanel.SetActive(false);
		// 	gameModePanel.transform.SetParent(CreateRoomPanelTransform, false);
		// 	// StartCoroutine(AnimateTheGameModePanel(gameModeLayout));
		// }
		// else if (!CreateTheRoomFirst && gameModePanel.transform.parent != fastCreateRoomPanelTransform)
		// {
		// 	gameModePanel.SetActive(false);
		// 	gameModePanel.transform.SetParent(fastCreateRoomPanelTransform, false);
		// 	// StartCoroutine(AnimateTheGameModePanel(gameModeLayout));
		// }
		// gameModePanel.SetActive(true);
    }
	IEnumerator AnimateTheGameModePanel(LayoutElement layout)
	{
		layout.preferredHeight = 0;
		gameModePanel.SetActive(true);
		while (layout.preferredHeight <= gameModePreferredHeight)
		{
			layout.preferredHeight += Time.time;
			yield return new WaitForSeconds(UIController.waitFactor);
		}
		yield return null;
	}
    void OnCreateFastRoom()
	{
		// fast room creating or finding

		if (!gameModePanel.activeInHierarchy)
		{
			gameModePanel.SetActive(true);
		}
		
		firstCreateTheRoom = false;
	}

	void ChangeGameMode(GameModes mode)
	{
		V_RoomTemplate tmpRoom = new V_RoomTemplate();
		tmpRoom.gameMode = mode;

		if(firstCreateTheRoom)
		{
			// register the room on the server
			// and then on the lobby manager
			LobbyManager.isMasterServer = true;
			LobbyManager.SaveRoom(tmpRoom);
			UIController.Enable_DisableUI(UIController.RoomModalPanel);

		}
		else
		{
			// filter the Available Rooms base on the selected gameMode
			LobbyManager.isMasterServer = false;
			LobbyManager.QuickStartRoom(mode);
			UIController.GoFrom_To(UIController.LobbyPanel, UIController.RoomPanel);
		}
	}
}
