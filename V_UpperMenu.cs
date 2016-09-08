using UnityEngine;
using UnityEngine.UI;
public class V_UpperMenu : V_UIElement
{

	[SerializeField] Button backButton;
	

	new void Awake()
	{
		base.Awake();
		UIController.IfClick_GoTo(backButton, ()=> UIController.GoFrom_To(UIController.currentPanel, UIController.LobbyPanel));
	}

	new void OnEnable()
	{
		
	}
}