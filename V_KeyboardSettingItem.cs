using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
public class V_KeyboardSettingItem : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
	public KeyCode defaultKey;
	private KeyCode previousKey;
	public Button placeholder;
	Text cachedBtnTxt;
	void Awake () 
	{
		cachedBtnTxt = GetComponentInChildren<Text>();
		previousKey = defaultKey;
	}
	IEnumerator WaitForKey()
	{
		cachedBtnTxt.text = "Press some Key";
		while(ReadKey() == KeyCode.None)
		{
			yield return new WaitForFixedUpdate();
			// print("waitingForText");
		}
		previousKey = ReadKey();
		cachedBtnTxt.text = previousKey.ToString(); 
		StopWaitingForIdiotUserToPressADamnKey();
	}
	KeyCode ReadKey()
	{
		int tmpInt = System.Enum.GetNames(typeof(KeyCode)).Length;
		for(int i = 0; i < tmpInt; i++)
		{
			if(Input.GetKey((KeyCode)i))
			{
				return (KeyCode)i;
			}
		}
		return KeyCode.None;
	}
	void OnEnable()
	{
		// #revision:: this might throw some error!!!
		cachedBtnTxt.text = defaultKey.ToString();
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
		StartCoroutine(WaitForKey());
	}
	public void OnDeselect(BaseEventData data)
	{
		StopWaitingForIdiotUserToPressADamnKey();
	}
	private void StopWaitingForIdiotUserToPressADamnKey()
	{
		StopCoroutine(WaitForKey());
		StopAllCoroutines();
		cachedBtnTxt.text = previousKey.ToString();
		
	}
}
