using UnityEngine;

public class V_UIElement : MonoBehaviour 
{
	// Mahdad
	// every UI element which needs to be enabled or disabled should have this ON!

	// HOW TO USE: each UI element which needs UIController:
	// 1. inherits from V_UIElement
	// 2. hide OnEnable() and Awake() by declaring: new OnEnable() {base.OnEnable();} and so on...
	protected V_UIController UIController;
	
	protected void Awake()
	{
		UIController = FindObjectOfType<V_UIController>();

	} 
	protected void OnEnable () 
	{
		transform.SetAsLastSibling();
		if (transform.parent != null)
		{
			transform.parent.SetAsLastSibling();
		}
		UIController.currentPanel = this.gameObject;
	}
}
