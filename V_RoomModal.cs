using System;
using UnityEngine;
using UnityEngine.UI;

public class V_RoomModal : V_UIElement 
{
	// #revision
	V_CustomLobbyManager LobbyManager;
	
	// buttons
	[HeaderAttribute("RoomModalPanel Buttons")]
	[SpaceAttribute(10f)]
	public Button createButton;
	public Button cancelButton, goForwardInMaps, goBackwardInMaps;

	// room vars
	[HeaderAttribute("RoomModalPanel variables")]
	[SpaceAttribute(10f)]
	public InputField roomName;
	public InputField password;
	public Dropdown playerMode, gameMode;
	public Image map;
	public Text mapName;

	
	new void Awake()
	{
		base.Awake();
		// #revision
		LobbyManager = FindObjectOfType<V_CustomLobbyManager>();

		UIController.IfClick_GoTo(createButton, OnCreateRoom);
		UIController.IfClick_GoTo(cancelButton, OnCancel);
		UIController.IfClick_GoTo(goForwardInMaps, ()=> ChangeMap(goToNextMap: true));
		UIController.IfClick_GoTo(goBackwardInMaps,()=> ChangeMap(goToNextMap: false));

		// #revision
		if (UIController.thumbnailMaps == null)
		{
			UIController.ThrowError("Thumbnails are not set for maps", ()=> 
			{
				UIController.GoFrom_To(UIController.genericErrorModal, this.gameObject);
				return;
			});
		}
		map.sprite = UIController.thumbnailMaps[0];
		mapName.text = UIController.ReturnMap(0);
	}


    new void OnEnable()
	{
		base.OnEnable();
		UIController.GetItemInDropDown(gameMode, UIController.ReturnGameMode(LobbyManager.currentRoom.gameMode));
	}
	void OnCreateRoom()
	{
		if (LobbyManager.currentRoom == null)
		{
			UIController.ThrowError("V_CustomLobbyManager: currentRoom is not set", UIController.CloseError);
			// do something about it!!! and then:
			return;
		}
		if (roomName.text == "")
		{
			UIController.ThrowError("The name of the room cannot be empty.", UIController.CloseError);
			return;
		}
		LobbyManager.currentRoom.roomName = roomName.text;

		LobbyManager.currentRoom.map = UIController.ReturnMap(mapName.text);

		LobbyManager.currentRoom.playerMode = UIController.ReturnPlayerMode(playerMode.options[playerMode.value].text);

		LobbyManager.currentRoom.gameMode = UIController.ReturnGameMode(gameMode.options[gameMode.value].text);

		LobbyManager.currentRoom.password = password.text;
		
		LobbyManager.SaveRoom(LobbyManager.currentRoom.ID, LobbyManager.currentRoom);
		
		UIController.GoFrom_To(UIController.RoomModalPanel, UIController.RoomPanel);
	}
	void OnCancel()
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
			UIController.CloseYesNoQ();
		});
	}
    private void ChangeMap(bool goToNextMap)
    {
		if (UIController.thumbnailMaps == null)
		{
			UIController.ThrowError("V_UIController: thumbnailMaps is null", ()=> UIController.CloseError());
		}

		for(int i = 0; i < UIController.thumbnailMaps.Length; i++)
		{
			if (map.sprite == UIController.thumbnailMaps[i])
			{
				// if we are pressing go to forward map
				if (goToNextMap)
				{
					//  if we are not skipping the last index of the array
					if (i+1 < UIController.thumbnailMaps.Length)
					{
						map.sprite = UIController.thumbnailMaps[i+1];
						mapName.text = UIController.ReturnMap(i+1);
						break;
					}
					//  if we are proceeding, we just go back to first index!
					else
					{
						map.sprite = UIController.thumbnailMaps[0];
						mapName.text = UIController.ReturnMap(0);
						break;	 
					}
				}
				// if we are going backward in maps
				else
				{
					if (i-1 >= 0)
					{
						map.sprite = UIController.thumbnailMaps[i-1];
						mapName.text = UIController.ReturnMap(i-1);
						break;
					}
					else
					{
						map.sprite = UIController.thumbnailMaps[UIController.thumbnailMaps.Length-1];
						mapName.text = UIController.ReturnMap(UIController.thumbnailMaps.Length-1);
						break;
					}
					
				}
			}
		}
    }
}
