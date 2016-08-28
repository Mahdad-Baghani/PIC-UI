using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class V_CrosshairItem : MonoBehaviour,  IPointerDownHandler
{
	V_Settings Settings;
	Sprite sprite;

	void Awake()
	{
		Settings = FindObjectOfType<V_Settings>();
		if (Settings == null)
		{
			print("V_CrosshairItem: Awake: Settings is null, dude!!!!");
			return;
		}
		sprite = GetComponent<Image>().sprite;
	}
	public void OnPointerDown(PointerEventData data)
	{
		if (data.button == PointerEventData.InputButton.Left)
		{
			Settings.currentCrosshair.sprite = sprite;
		}	
	}

}
