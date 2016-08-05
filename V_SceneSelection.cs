using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
public class V_SceneSelection : MonoBehaviour 
{
	//public Image 
	// #revision
	V_UIController UIController;
	V_CustomLobbyManager LobbyController;
	private AsyncOperation async;

	public void Awake()
	{
		UIController = FindObjectOfType<V_UIController>();
		LobbyController = FindObjectOfType<V_CustomLobbyManager>();
	}

	public void Load (GameObject panel)
	{
		if (UIController.currentPanel == UIController.RoomPanel)
		{
			UIController.AskYesNoQ ("Do you want to discard the Room?", 
			() => LobbyController.RemoveRoom(LobbyController.currentRoom.ID),
			() => this.gameObject.SetActive(false));
		}
		if (panel != null)
		{
			UIController.GoFrom_To(UIController.currentPanel, panel);
			UIController.IfClick_GoTo(UIController.backButton, () => UIController.GoFrom_To(UIController.currentPanel, UIController.LobbyPanel));
		}
			
	}
	public void LoadAsync(string scene)
	{
		StartCoroutine(WaitWhileLoading(scene));
	}

	IEnumerator WaitWhileLoading(string scene)
	{
		async = SceneManager.LoadSceneAsync(scene);
		while(!async.isDone)
		{
			// #revision
			// show a loading bar and other stuff
			yield return new WaitForEndOfFrame();
		}
	}

	
}
