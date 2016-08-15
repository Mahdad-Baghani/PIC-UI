using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class V_OtherSettingItem : MonoBehaviour, IPointerDownHandler
{
	[SerializeField] V_Settings settings;
	[SerializeField] Color itemColor;
	void Awake()
	{
		settings = FindObjectOfType<V_Settings>();
		itemColor = this.GetComponent<Image>().color;
	}
	public void OnPointerDown(PointerEventData data)
	{
		if(data.button == PointerEventData.InputButton.Left)
		{
			settings.currentCrosshair.color = this.GetComponent<Image>().color;
			print(settings.currentCrosshair.material);
		}
	}
}
