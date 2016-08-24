using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class V_OtherSettingItem : MonoBehaviour, IPointerDownHandler
{
	[SerializeField] V_Settings settings;
	void Awake()
	{
		settings = FindObjectOfType<V_Settings>();
	}
	public void OnPointerDown(PointerEventData data)
	{
		Color itemColor = GetComponent<Image>().color;
		if(data.button == PointerEventData.InputButton.Left)
		{
			settings.currentCrosshair.color = itemColor;
			print(settings.currentCrosshair.material);
		}
	}
}
