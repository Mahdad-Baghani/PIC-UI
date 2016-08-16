using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class V_Room : V_UIElement 
{
	// fields
	V_CustomLobbyManager LobbyManager;
	
	// room var
	[SerializeField]
	InputField roomName;
	
	[SerializeField]
	Dropdown map;
	
	[SerializeField]
	Dropdown gameMode;
	
	[SerializeField]
	Dropdown playerMode;
	
	[SerializeField]
	Dropdown objectives;
	
	[SerializeField]
	Text objectiveText;

	[SerializeField]
	InputField password;
	[SerializeField]
	Dropdown weaponFilter;

	// public ScrollRect blueTeam;
	// public ScrollRect redTeam;

	//  buttons
	public Button start_Join_Ready;
	public Button inviteBtn;
	
	
	// methods
	new void Awake()
	{
		base.Awake();

		LobbyManager = FindObjectOfType<V_CustomLobbyManager>();

		UIController.IfClick_GoTo(start_Join_Ready, StartJoin);
		UIController.IfClick_GoTo(inviteBtn, Invite);

		UIController.ifType_DoThis(roomName, (value)=> LobbyManager.currentRoom.roomName = value);
		UIController.OnDropDownChangesValue(gameMode, (value)=> 
		{
			LobbyManager.currentRoom.gameMode = UIController.ReturnGameMode(gameMode.options[gameMode.value].text);
			ChangeObjective(value);
		});
		UIController.OnDropDownChangesValue(playerMode, (value)=> LobbyManager.currentRoom.playerMode = UIController.ReturnPlayerMode(playerMode.options[playerMode.value].text));
		UIController.OnDropDownChangesValue(map, (value)=> LobbyManager.currentRoom.map = UIController.ReturnMap(map.options[map.value].text));
		UIController.OnDropDownChangesValue(objectives, (value)=> LobbyManager.currentRoom.objectives = (int)value);
		UIController.ifType_DoThis(password, (value)=> LobbyManager.currentRoom.password = value);
		UIController.OnDropDownChangesValue(weaponFilter, (value)=> LobbyManager.currentRoom.weaponFilter = UIController.ReturnWeaponFilter(weaponFilter.options[weaponFilter.value].text));

		
		// UIController.OnDropDownChangesValue(map, (value) => {LobbyManager.currentRoom.map = UIController.ReturnMap(map.options[map.value].text);});
		// #revision
		// do this for other dropdowns
		
	} 
	new void OnEnable()
	{
		base.OnEnable();
		UIController.IfClick_GoTo(UIController.backButton, 
		() => 
		{
			UIController.AskYesNoQ("Are u sure u want to discard the room?", 
			() => // yes answer
			{
				UIController.GoFrom_To(UIController.genericYesNoModal, UIController.LobbyPanel); 
				LobbyManager.RemoveRoom(LobbyManager.currentRoom.ID);
				this.gameObject.SetActive(false);
			}, 
			() => // no answer
			{
				UIController.GoFrom_To(UIController.genericYesNoModal, UIController.RoomPanel);
			});
		});
		
		// #revision
		
		// check if the player has a host component
		if(LobbyManager.isMasterServer)
		{
			start_Join_Ready.GetComponentInChildren<Text>().text = "Start";
		}
		else
		{
			if (LobbyManager.currentRoom != null && LobbyManager.currentRoom.hasAnActiveGame)
			{
				start_Join_Ready.GetComponentInChildren<Text>().text = "Join";
			}
			else
			{
				start_Join_Ready.GetComponentInChildren<Text>().text = "Ready";
			}
		}
		
		
		// #revision

		V_RoomTemplate tmpRoom = LobbyManager.currentRoom;
		
		roomName.text = tmpRoom.roomName;

		UIController.GetItemInDropDown(gameMode,UIController.ReturnGameMode(LobbyManager.currentRoom.gameMode));
		UIController.GetItemInDropDown(playerMode, UIController.ReturnPlayerMode(LobbyManager.currentRoom.playerMode));
		UIController.GetItemInDropDown(map, UIController.ReturnMap(LobbyManager.currentRoom.map));
		UIController.GetItemInDropDown(weaponFilter, UIController.ReturnWeaponFilter(WeaponFilter.NONE));

		password.text = tmpRoom.password;

	}
	void SetObjectives(int min, int max)
	{
		objectives.options.Clear();
		for (int i = min; i <= max ; i++)
		{	
			objectives.options.Add(new Dropdown.OptionData() {text= i.ToString()});
		}
		// List<Dropdown.OptionData> = new List<Dropdown.op
	}
		void SetObjectives(int min, int max, int everyOtherTime)
	{
		objectives.options.Clear();
		for (int i = min; i <= max ; i++)
		{	
			if (i % everyOtherTime != 0)
			{
				continue;
			}
			objectives.options.Add(new Dropdown.OptionData() {text= i.ToString()});
		}
		// List<Dropdown.OptionData> = new List<Dropdown.op
	}
	void ChangeObjective(int index)
	{
		if(gameMode.options[index].text == UIController.ReturnGameMode(GameModes.SD))
		{
			// #revision: create a min and max for number of rounds or kills
			SetObjectives(1, 20);
			// #revision: persian compatibility required!
			objectiveText.text = "Rounds";
		}
		else if(gameMode.options[index].text == UIController.ReturnGameMode(GameModes.TDM) || 
				gameMode.options[index].text == UIController.ReturnGameMode(GameModes.DM))
		{
			SetObjectives(30, 150, 10);
			objectiveText.text = "Kills";
		}		
	}
		

	private void StartJoin()
	{
		// #revision
		if (LobbyManager.currentRoom == null)
		{
			print("V_CustomLobbyManager: currentRoom is not set");
			return;			
		}

		LobbyManager.currentRoom.hasAnActiveGame = true;
	}
	private void Invite()
	{
		// #revision
		UIController.SelectFromListModal("Type the name of the person u wanna Invite:", 
		()=> //if click select
		{

		},
		()=> // if click cancel
		{
			UIController.CloseSelectModal();
		},
		(nameOfThePlayer)=> // if starts typing name of the player
		{
			// do a search in lobby players and show their name
		});
	}
}
