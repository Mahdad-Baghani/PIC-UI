using UnityEngine;
using UnityEngine.UI;

public class V_CustomizeWeapon : V_UIElement 
{
	public Button applyBtn, cancelBtn;
	new void Awake()
	{
		base.Awake();
		// #revision:: do something about this messy code!!
		UIController.IfClick_GoTo(applyBtn, ()=> UIController.Disable_EnableUI(this.gameObject));
		UIController.IfClick_GoTo(cancelBtn, ()=> UIController.Disable_EnableUI(this.gameObject));
	}
}
