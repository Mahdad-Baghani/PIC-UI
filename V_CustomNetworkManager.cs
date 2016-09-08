using UnityEngine;
using UnityEngine.Networking;

public class V_CustomNetworkManager : NetworkManager 
{
	public static V_CustomNetworkManager instance;
	V_UIController UIController;

	// [SerializeField]
	// GM gameMode;

	// [SerializeField]
	// GameObject LobbyScene;

	// [SerializeField] 
	// GameObject PlayScene;

	// void Awake()
	// {
	// 	instance = this;
	// }

	// public override void OnStartHost()
	// {
	// 	// set the game mode
	// 	// LobbyScene.SetActive(true);
	// 	// PlayScene.SetActive(false);
	// }

	// public override void OnStopHost()
	// {
	// 	// LobbyScene.SetActive(true);
	// 	// PlayScene.SetActive(false);

	// }

	// public override void OnStartClient(NetworkClient client)
	// {
	// 	// set the game mode

	// 	// LobbyScene.SetActive(false);
	// 	// PlayScene.SetActive(true);

	// }

	// public override void OnStopClient()
	// {
	// 	// LobbyScene.SetActive(true);
	// 	// PlayScene.SetActive(false);

	// }



}
