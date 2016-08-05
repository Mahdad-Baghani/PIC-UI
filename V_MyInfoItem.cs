using UnityEngine;
using UnityEngine.EventSystems;

public class V_MyInfoItem : V_UIElement, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
	V_MyInfo myInfo;
	public GameObject itemPrfb;

	public new void Awake()
	{
		base.Awake();
		myInfo = FindObjectOfType<V_MyInfo>();
	}

	public void OnPointerEnter(PointerEventData data)
	{

	}
	public void OnPointerDown(PointerEventData data)
	{
		if(data.button == PointerEventData.InputButton.Left)
		{
			EventSystem.current.SetSelectedGameObject(gameObject, data);
		}
	}

	public void OnPointerExit(PointerEventData data)
	{

	}

	public void OnSelect(BaseEventData data)
	{
		try
		{
			// myInfo.upgradeView.headshots.text = data.selectedObject.GetComponent<V_Weapon>().name;
			myInfo.upgradeView.headshots.text = this.GetComponent<V_Weapon>().name;
			print(data.selectedObject.GetComponent<V_Weapon>().name);
			myInfo.upgradeView.level.value = Random.Range(0, 1);
			myInfo.upgradeView.shootingAccuracy.value = Random.Range(0, 1);
		}
		catch (System.Exception err)
		{
			UIController.ThrowError(err.Message, UIController.CloseError);
			
			throw;
		}
	}

	public void OnDeselect(BaseEventData data)
	{

	}
}
