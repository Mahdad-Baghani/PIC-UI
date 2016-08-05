using UnityEngine;
using UnityEngine.EventSystems;
public class V_TooltipElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
	V_UIController UIController;
	public string toolTipMsg;

	// methods
	void Awake()
	{
		UIController = FindObjectOfType<V_UIController>();
	}
	public void OnPointerEnter(PointerEventData data)
	{
		UIController.ShowTooltip(toolTipMsg);
	}

	public void OnPointerExit(PointerEventData data)
	{
		// #revision: fade the text
		UIController.ShowTooltip("");
	}
}
