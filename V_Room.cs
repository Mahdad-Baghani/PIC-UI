using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class V_Room : V_UIElement 
{
	// fields
	[SerializeField]V_CustomLobbyManager LobbyManager;

	[SerializeField]float RoomRefreshRate = 1f; // tweak from inspector
	bool isRefreshingPlayers = false;

	public GameObject playerPrefab; // tweak from inspector
	public List<GameObject> blueTeam;
	public List<GameObject> redTeam;
	public bool teamSelectionIsNotFinished = true;
	private int numberOfCurrentPlayers = 0;

	[SerializeField] private List<V_PlayerTemplate> _currentPlayers;
	public List<V_PlayerTemplate> currentPlayers
	{
		get { return _currentPlayers;}
		set
		{
			_currentPlayers = value;
			if(_currentPlayers.Count > numberOfCurrentPlayers)
			{
				numberOfCurrentPlayers = _currentPlayers.Count;
				SetTeams(LobbyManager.currentRoom.autoTeamSelection, _currentPlayers[_currentPlayers.Count-1]);
				// Debug.LogWarning("number of current players is " + _currentPlayers.Count);
			}
		}
	}
	
	// room var
	[SerializeField] InputField roomName;
	[SerializeField] Dropdown map, gameMode, playerMode, objectives, weaponFilter, gameTime;
	[SerializeField] Text objectiveText;
	[SerializeField] InputField password;
	[SerializeField] Toggle autoTeamSelectionToggle;

	//  buttons
	public Button start_Join_Ready, inviteBtn, SwitchTeam;
	
	
	// methods
	new void Awake()
	{
		base.Awake();

		// LobbyManager = FindObjectOfType<V_CustomLobbyManager>();
		blueTeam = new List<GameObject>();
		redTeam = new List<GameObject>();
		_currentPlayers = new List<V_PlayerTemplate>();

		UIController.IfClick_GoTo(start_Join_Ready, StartJoinReady);
		UIController.IfClick_GoTo(inviteBtn, Invite);
		UIController.IfClick_GoTo(SwitchTeam, SwitchTeams);
		

		UIController.OnDropDownChangesValue(gameMode, (value)=> 
		{
			LobbyManager.currentRoom.gameMode = UIController.ReturnGameMode(gameMode.options[value].text);
			ChangeObjective(value);
		});
		UIController.OnDropDownChangesValue(playerMode, (value)=> LobbyManager.currentRoom.playerMode = UIController.ReturnPlayerMode(playerMode.options[playerMode.value].text));
		UIController.OnDropDownChangesValue(map, (value)=> LobbyManager.currentRoom.map = UIController.ReturnMap(map.options[value].text));
		UIController.OnDropDownChangesValue(objectives, (value)=> LobbyManager.currentRoom.objectives = int.Parse(objectives.options[value].text));
		UIController.ifType_DoThis(password, (value)=> LobbyManager.currentRoom.password = value);
		UIController.OnDropDownChangesValue(weaponFilter, (value)=> LobbyManager.currentRoom.weaponFilter = UIController.ReturnWeaponFilter(weaponFilter.options[weaponFilter.value].text));
		UIController.OnDropDownChangesValue(gameTime, (value)=> LobbyManager.currentRoom.gameTime = int.Parse(gameTime.options[value].text));
		// autoTeamSelection is against shuffleTeamSelection, so we just need to save the state of autoTeamSelection toggle to determine if auto or shuffle are selected
		UIController.OnToggleChangeValue(autoTeamSelectionToggle, (value)=> LobbyManager.currentRoom.autoTeamSelection = value);
		
		// UIController.OnDropDownChangesValue(map, (value) => {LobbyManager.currentRoom.map = UIController.ReturnMap(map.options[map.value].text);});
		// #revision
		// do this for other dropdowns
	}

    private void SwitchTeams()
    {
        // #revision
    }

    new void OnEnable()
	{
		base.OnEnable();
		UIController.currentPanel = this.gameObject;

		// UIController.IfClick_GoTo(UIController.backButton, 
		// () => 
		// {
		// 	UIController.AskYesNoQ("Are u sure u want to discard the room?", 
		// 	() => // yes answer
		// 	{
		// 		UIController.GoFrom_To(UIController.genericYesNoModal, UIController.LobbyPanel); 
		// 		UIController.Disable_EnableUI(this.gameObject);
		// 		LobbyManager.RemoveRoom(LobbyManager.currentRoom.ID);
		// 	}, 
		// 	() => // no answer
		// 	{
		// 		UIController.GoFrom_To(UIController.genericYesNoModal, UIController.RoomPanel);
		// 	});
		// });

		// #revision: read all the on-server data to keep UI in sync.
		SetObjectives(1, 20);
		roomName.text = LobbyManager.currentRoom.roomName;
		UIController.GetItemInDropDown(gameMode, UIController.ReturnGameMode(LobbyManager.currentRoom.gameMode));
		UIController.GetItemInDropDown(playerMode, UIController.ReturnPlayerMode(LobbyManager.currentRoom.playerMode));
		UIController.GetItemInDropDown(map, UIController.ReturnMap(LobbyManager.currentRoom.map));
		UIController.GetItemInDropDown(objectives, UIController.ReturnGameObjectives(LobbyManager.currentRoom.gameMode));
		password.text = LobbyManager.currentRoom.password;
		UIController.GetItemInDropDown(weaponFilter, UIController.ReturnWeaponFilter(WeaponFilter.NONE));
		UIController.GetItemInDropDown(gameTime, V_UIController.T_DEFAULT_TIME);

		
		// #revision: roomName is not editable any ways, so:
		roomName.interactable = false;		
		// Check if player is MasterClient
		if(LobbyManager.isMasterServer)
		{
			start_Join_Ready.GetComponentInChildren<Text>().text = "Start";
		}
		// or he is a normal client
		else
		{
			gameMode.interactable = false;
			playerMode.interactable = false;
			map.interactable = false;
			objectives.interactable = false;
			password.interactable = false;
			weaponFilter.interactable = false;
			gameTime.interactable = false;
			
			if (LobbyManager.currentRoom != null && LobbyManager.currentRoom.hasAnActiveGame)
			{
				start_Join_Ready.GetComponentInChildren<Text>().text = "Join";
			}
			else
			{
				start_Join_Ready.GetComponentInChildren<Text>().text = "Ready";
			}
		}
	}
	void Update()
	{
		if(!isRefreshingPlayers)
		{
			StartCoroutine(UpdateRoomPlayers());
		}
	}
	private void SetObjectives(int min, int max)
	{
		objectives.options.Clear();
		for (int i = min; i <= max ; i++)
		{	
			objectives.options.Add(new Dropdown.OptionData() {text= i.ToString()});
		}
		// List<Dropdown.OptionData> = new List<Dropdown.op
	}
	private void SetObjectives(int min, int max, int everyOtherTime)
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
	private void ChangeObjective(int index)
	{
		if(gameMode.options[index].text == UIController.ReturnGameMode(GameModes.SD))
		{
			// #revision: create a min and max for number of rounds or kills
			SetObjectives(1, 20);
			// #revision: persian compatibility required!
			UIController.GetItemInDropDown(objectives, V_UIController.O_DEFAULT_FOR_SD);
			objectiveText.text = "Rounds";
		}
		else if(gameMode.options[index].text == UIController.ReturnGameMode(GameModes.TDM) || 
				gameMode.options[index].text == UIController.ReturnGameMode(GameModes.DM))
		{
			SetObjectives(30, 150, 10);
			UIController.GetItemInDropDown(objectives, V_UIController.O_DEFAULT_FOR_DM);
			objectiveText.text = "Kills";
		}		
	}
		
	void SetTeams(bool autoSelectTeamMembers, V_PlayerTemplate player)
	{
		if (teamSelectionIsNotFinished)
		{
			// adding players by filling the blueTeam first and then the redTeam
			if(autoSelectTeamMembers)
			{
				if(blueTeam.Count < ReturnMaxPlayersforEachTeam(LobbyManager.currentRoom.playerMode))
				{
					blueTeam.Add(ReturnPlayerFromPrefab(player));
				}
				else
				{
					if (redTeam.Count < ReturnMaxPlayersforEachTeam(LobbyManager.currentRoom.playerMode))
					{
						redTeam.Add(ReturnPlayerFromPrefab(player));
					}
					else
					{
						teamSelectionIsNotFinished = false;
					}
				}
			}
			else // if autoSelectTeamMembers is not selected, then we go to shuffle mode!!! 
			{
				int tmpI = Random.Range(0, 1);
				if (tmpI == 0)
				{
					if(blueTeam.Count < ReturnMaxPlayersforEachTeam(LobbyManager.currentRoom.playerMode))
					{
						blueTeam.Add(ReturnPlayerFromPrefab(player));
					}
					else if (redTeam.Count < ReturnMaxPlayersforEachTeam(LobbyManager.currentRoom.playerMode))
					{
						redTeam.Add(ReturnPlayerFromPrefab(player));
					}
					else
					{
						teamSelectionIsNotFinished = false;
					}
				}
				else
				{
					if(redTeam.Count < ReturnMaxPlayersforEachTeam(LobbyManager.currentRoom.playerMode))
					{
						redTeam.Add(ReturnPlayerFromPrefab(player));
					}
					else if(blueTeam.Count < ReturnMaxPlayersforEachTeam(LobbyManager.currentRoom.playerMode))
					{
						blueTeam.Add(ReturnPlayerFromPrefab(player));
					}
					else
					{
						teamSelectionIsNotFinished = false;
					}
				}
			}
		}
	}
	IEnumerator UpdateRoomPlayers()
	{
		isRefreshingPlayers = true;
		currentPlayers = LobbyManager.currentRoom.players;
		yield return new WaitForSeconds(RoomRefreshRate);
		isRefreshingPlayers = false;
		// Debug.LogWarning("Updating room with " + LobbyManager.currentRoom);
	}

	private GameObject ReturnPlayerFromPrefab(V_PlayerTemplate player)
	{
		GameObject tmpObj = Instantiate(playerPrefab);
		V_PlayerTemplate tmpPlayer;
		tmpObj.AddComponent<V_PlayerTemplate>();
		tmpPlayer = tmpObj.GetComponent<V_PlayerTemplate>();
		tmpPlayer = player;

		return tmpObj;
	}

	private int ReturnMaxPlayersforEachTeam(PlayerModes mode)
	{
		switch (mode)
		{
			case PlayerModes.ON1:
			return 1;

			case PlayerModes.ON2:
			return 2;
			
			case PlayerModes.ON3:
			return 3;

			case PlayerModes.ON4:
			return 4;
			
			case PlayerModes.ON5:
			return 5;

			case PlayerModes.ON6:
			return 6;
			
			case PlayerModes.ON7:
			return 7;

			case PlayerModes.ON8:
			return 8;

			default:
			return 1;
		}
	}
	private void StartJoinReady()
	{
		// #revision
		if (LobbyManager.currentRoom == null)
		{
			print("V_CustomLobbyManager: currentRoom is not set");
			return;			
		}
		if (LobbyManager.currentRoom.hasAnActiveGame)

		LobbyManager.currentRoom.hasAnActiveGame = true;
	}
	private void Invite()
	{
		// #revision
		UIController.SelectFromListModal("Type the name of the person u wanna Invite:", 
		()=> //if click select
		{
			UIController.CloseSelectModal();
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
