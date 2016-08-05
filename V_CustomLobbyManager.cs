using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public enum PlayerModes
{
	ON1,
	ON2,
	ON3,
	ON4,
	ON5,
	ON6,
	ON7,
	ON8
}
public enum WeaponFilter
{
	NONE,
	KNIFE,
	ASSUALT,
	GERENADE
}

public enum GameModes
{
	SD, 
	TDM,
	DM,

}

public enum Maps
{
	// #revision
	DUST,
	RUST
}
public class V_CustomLobbyManager : NetworkLobbyManager 
{
	private Dictionary<int, V_RoomTemplate> Rooms;
	public V_RoomTemplate currentRoom;	
	
	
	public bool isMasterServer = false;

	private bool _auto;
	public bool autoMatchMaking
	{
		get { return _auto;}
		set 
		{
			 _auto = value;
			 if (_auto == _shuffle)
			 {
				 _shuffle = !_shuffle;				 
			 }
		}
	}
	private bool _shuffle;
	public bool shuffleMatchMaking
	{
		get { return _shuffle;}
		set 
		{ 
			_shuffle = value;
			if (_shuffle == _auto)
			{
				_auto = !_auto;
			}
		}
	}
	


	public void SaveRoom(V_RoomTemplate room)
	{
		// check if the Room is Valid
		
		// #revision
		// currentRoomId = something from the server
		int serverRoomId = UnityEngine.Random.Range(1, 100);

		if(!Rooms.ContainsKey(serverRoomId))
		{
			room.ID = serverRoomId;
			Rooms.Add(serverRoomId, room);

			currentRoom = room;
		}
		else
		{
			// UIController.ThrowError("V_CustomLobbyManager: SaveRoom(): Mismatch in Room ID");
		}		
	}
	
	public void SaveRoom(int id, V_RoomTemplate room)
	{
		if (Rooms.ContainsKey(id))
		{
			Rooms[id] = room;
			// #revision if currentRoomId is going to be set from other methods except SaveRoom()
			currentRoom = room;
		}
	}

	public void QuickStartRoom(GameModes mode)
	{
		for (int i = 0; i < Rooms.Count; i++)
		{
			// #revision
			if (Rooms[i].gameMode == mode) 
			{
				currentRoom = Rooms[i];
				break;
			}
		}
		// there was no Room with that GameModes, so
		//  UIController.ThrowError("V_CustomLobbyManager: QuickStartRoom(): No room with your selected GameMode");
	}
	public V_RoomTemplate GetRoom (int id)
	{
		if (Rooms.ContainsKey(id))
		{
			return Rooms[id];
		}
		// UIController.ThrowError("V_CustomLobbyManager: GetRoom(): No room with ID " + id);
		return null;
	}
	
	public void RemoveRoom(int id)
	{
		if (Rooms.ContainsKey(id))
		{
			Rooms.Remove(id);
		}
		currentRoom = null;
		isMasterServer = false;
	}
	void Awake () 
	{
		Rooms = new Dictionary<int, V_RoomTemplate>();
		currentRoom = new V_RoomTemplate();
	}

	void Join()
	{

	}
	

}
