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

	public Button enterButton;
	
	public Button createRoomButton;
	public Button fastRoomButton;

	public Button GM_SD_Button;

	public Button GM_TMD_Button;

	private bool firstCreateTheRoom = true; // !!!
	
	new void Awake()
	{
		base.Awake();
		// #Revision 
		LobbyManager = FindObjectOfType<V_CustomLobbyManager>();
		// UIController = pic.controller.UIcontroller;

		UIController.IfClick_GoTo(enterButton, EnterTheRoom);
		UIController.IfClick_GoTo(createRoomButton, CreateRoom);
		UIController.IfClick_GoTo(fastRoomButton, CreateFastRoom);	
		UIController.IfClick_GoTo(GM_SD_Button, () => {ChangeGameMode(GameModes.SD);});
		UIController.IfClick_GoTo(GM_TMD_Button, () => {ChangeGameMode(GameModes.TDM);});

	}
	new void OnEnable()
	{
		base.OnEnable();
		UIController.IfClick_GoTo(UIController.backButton, () =>
		{
			
		});

		gameModePanel.SetActive(false);

	}



	public void EnterTheRoom()
	{
		// enters the room
		if (enterButton == null)
		{
			print("V_Lobby: EnterTheRoom(): enterButton is not set");
			return;
		}
		
		// Join the Room via V_CustomLobbyManager
		// V_CustomLobbyManager.instance.GetRoom(idFromSelectedRoomInTheList~!);
	}
	void CreateRoom()
	{
		// go to Room page
		if (createRoomButton == null)
		{
			print("V_Lobby: CreateRoom(): createRoomButton is not set");
			return;
		}

		if (!gameModePanel.activeInHierarchy)
		{
			gameModePanel.SetActive(true);
		}

		firstCreateTheRoom = true;
		// #revision
		// add network Host to the player if he wants to create a room
		// pic.model.player.addComponent<NetworkHost>;

		// #revision
		// wait for clicking one of the game modes

		// UIController.GoToNextPanel(roomModal);
		
	}
	void CreateFastRoom()
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
		

		// V_CustomLobbyManager.instance.Room.gameMode  =  mode;
		tmpRoom.gameMode = mode;

		if(firstCreateTheRoom)
		{
			// register the room on the server
			// and then on the lobby manager
			LobbyManager.isMasterServer = true;
			LobbyManager.SaveRoom(tmpRoom);
			UIController.GoFrom_To(UIController.LobbyPanel, UIController.RoomModalPanel);

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
