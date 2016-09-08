using UnityEngine;
using UnityEngine.UI;

public class V_Lobby : V_UIElement 
{
	// fields
	// #revision
	V_CustomLobbyManager LobbyManager;
	
	// panels to go from Lobby
	public GameObject gameModePanel;
	
	// Buttons

	public Button enterButton, createRoomButton, fastRoomButton, GM_SD_Button, GM_TMD_Button;

	private bool firstCreateTheRoom = true; // !!!
	
	new void Awake()
	{
		base.Awake();
		// #Revision 
		LobbyManager = FindObjectOfType<V_CustomLobbyManager>();

		UIController.IfClick_GoTo(enterButton, OnEnterTheRoom);
		UIController.IfClick_GoTo(createRoomButton, OnCreateRoom);
		UIController.IfClick_GoTo(fastRoomButton, OnCreateFastRoom);	
		UIController.IfClick_GoTo(GM_SD_Button, () => {ChangeGameMode(GameModes.SD);});
		UIController.IfClick_GoTo(GM_TMD_Button, () => {ChangeGameMode(GameModes.TDM);});
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
		if (createRoomButton == null)
		{
			print("V_Lobby: OnCreateRoom(): createRoomButton is not set");
			return;
		}

		if (!gameModePanel.activeInHierarchy)
		{
			gameModePanel.SetActive(true);
		}

		firstCreateTheRoom = true;
	}
	void OnCreateFastRoom()
	{
		// fast room creating or finding
		if (fastRoomButton == null)
		{
			print("V_Lobby: CreateFastRoom(): fastRoomButton is not set");
			return;
		}

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
