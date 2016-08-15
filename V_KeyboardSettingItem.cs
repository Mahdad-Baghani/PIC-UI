using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class V_KeyboardSettingItem : V_UIElement, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, ISelectHandler
{
	private bool waitingForText = false;
	public string pressedKey;
	public string defaultKey;
	public InputField placeholder;
	new void Awake () 
	{
		base.Awake();
		UIController.ifType_DoThis(placeholder, (value) => {if(value != "") placeholder.text = value.Substring(placeholder.text.Length-1, 1);});
	}
	
	new void OnEnable()
	{
		placeholder.text =  defaultKey;
		// no need to call base.OnEnable(); cause we are not setting it as LastSibling
	}
	public void OnPointerEnter(PointerEventData data)
	{
		// some effects maybe!
	}
	public void OnPointerDown(PointerEventData data)
	{
		if (data.button == PointerEventData.InputButton.Left)
		{
			EventSystem.current.SetSelectedGameObject(this.gameObject, data);
		}
	}
	public void OnPointerExit(PointerEventData data)
	{
		// some effects maybe!
	}
	public void OnSelect(BaseEventData data)
	{
		placeholder.text = "";
		// StartCoroutine()
	}
}
