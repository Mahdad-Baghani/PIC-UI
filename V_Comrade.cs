using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Random = UnityEngine.Random;

public class V_Comrade : V_UIElement 
{
	[SerializeField] V_PlayerTemplate ourCharacter, hisComrade; // !!!
	[HeaderAttribute("UI refs")]
	public Text nicknameText;
	public Text clanText, badgeText, numberOfTKillsText, numberOfDeathsText, onNotHavingAComradeAlertText;
	[RangeAttribute(0,15f)][SerializeField] float comradeAlertDelay; //  tweak from the inspector

	new void Awake()
	{
		base.Awake();
	}
	new void OnEnable()
	{
		base.OnEnable();
		UIController.currentPanel = this.gameObject;
	}
	IEnumerator Start()
	{
		// ourCharacter = FindObjectOfType<V_PlayerTemplate>();
		if (ourCharacter == null)
		{
				UIController.ThrowError("V_Comrade: Awake: ourCharacter or hisComrade ref is null!!!", ()=> {UIController.CloseError();});
		}
		if (hisComrade == null)
		{
			yield return new WaitForSeconds(2f);
			yield return StartCoroutine(UIController.FadeIn(onNotHavingAComradeAlertText.gameObject));
			yield return new WaitForSeconds(comradeAlertDelay); 
			yield return StartCoroutine(UIController.FadeOut(onNotHavingAComradeAlertText.gameObject));
		}
		else
		{
			nicknameText.text = hisComrade.nickName;
			clanText.text = hisComrade.clan.clanName;
			badgeText.text = hisComrade.badge.badgeName;
			numberOfTKillsText.text = Random.Range(0, 100).ToString();
			numberOfDeathsText.text = Random.Range(0, 100).ToString();

		}
		
	}
}
