using UnityEngine;
using UnityEngine.UI;

public class V_MyInfo : V_UIElement 
{
	// fields 
	// #revision: I have to read the true reference to the local player somehow...
	public V_PlayerTemplate playerModel;
	private V_ObjectPool pool;
	public V_WeaponUpgradeView upgradeView;
	public GameObject myInfoItemPrfb;
	
	[HeaderAttribute("UI Panels")]
	[SpaceAttribute(10f)]
	public GameObject achievementPanel;
	public GameObject assaultPanel;
	public GameObject pistolPanel;
	public GameObject snipePanel;
	public GameObject shotgunPanel;
	// public GameObject weaponProgressPanel;
	[HeaderAttribute("UI buttons")]
	[SpaceAttribute(10f)]
	public Button pistolBtn;
	public Button assaultBtn;
	public Button snipeBtn;
	public Button shotGunBtn;

	[HeaderAttribute("UI reference")]
	[SpaceAttribute(5f)]
	public Image badgeImage;
	public Text badgeName;
	public Text nickName;
	// public V_ClanTemplate clan;
	public Image clanLogo;
	public Text clanName;
	public Text scoreBadgeTxt;

	public GameObject achievementPrfb;
	public GameObject weaponPrfb;

	public Button playerDetailsBtn;
	// public GameObject weaponDetailsPanel;
	public GameObject progressDiagram;

	// methods
	public new void Awake()
	{
		base.Awake();
		// #revision : do something about the player
		try
		{
			playerModel = FindObjectOfType<V_PlayerTemplate>();
			upgradeView = FindObjectOfType<V_WeaponUpgradeView>();
			pool = FindObjectOfType <V_ObjectPool>();
		}
		catch (System.Exception err)
		{
			print(err.Message);
			throw;
		}

		UIController.IfClick_GoTo(UIController.backButton, ()=> UIController.GoFrom_To(UIController.currentPanel, UIController.LobbyPanel));
		UIController.IfClick_GoTo(playerDetailsBtn, ShowPlayerDetailsOnWebView);
		UIController.IfClick_GoTo(pistolBtn,()=> UIController.Enable_DisableUI(pistolPanel.transform.parent.gameObject, assaultPanel.transform.parent.gameObject, snipePanel.transform.parent.gameObject, shotgunPanel.transform.parent.gameObject));
		UIController.IfClick_GoTo(assaultBtn, ()=> UIController.Enable_DisableUI(assaultPanel.transform.parent.gameObject, pistolPanel.transform.parent.gameObject, snipePanel.transform.parent.gameObject, shotgunPanel.transform.parent.gameObject));
		UIController.IfClick_GoTo(snipeBtn, ()=> UIController.Enable_DisableUI(snipePanel.transform.parent.gameObject, pistolPanel.transform.parent.gameObject, assaultPanel.transform.parent.gameObject, shotgunPanel.transform.parent.gameObject));
		UIController.IfClick_GoTo(shotGunBtn, ()=> UIController.Enable_DisableUI(shotgunPanel.transform.parent.gameObject, assaultPanel.transform.parent.gameObject, pistolPanel.transform.parent.gameObject, snipePanel.transform.parent.gameObject));
	}

	public new void OnEnable()
	{
		base.OnEnable();
		try
		{
			badgeImage.sprite = playerModel.badge.badgeIcon;
			badgeName.text = playerModel.badge.badgeName;
			nickName.text = playerModel.nickName;
			if (playerModel.clan != null)
			{
				clanLogo.transform.parent.gameObject.SetActive(true);
				clanLogo.sprite = playerModel.clan.logo;
				clanName.text = playerModel.clan.clanName;
			}
			else
			{
				clanLogo.transform.parent.gameObject.SetActive(false);
			}

			scoreBadgeTxt.text = string.Format(playerModel.score + "/" + (++playerModel.badge.badgeType));

			foreach (V_Achievement achievement in playerModel.achievements)
			{
				GameObject tmpObj = Instantiate(achievementPrfb);
				V_Achievement someAchievement = tmpObj.GetComponent<V_Achievement>();
				someAchievement = achievement;
				tmpObj.GetComponent<Image>().sprite = achievement.achievementIcon;
				tmpObj.transform.SetParent(achievementPanel.transform, false);
				tmpObj.transform.SetAsLastSibling();
			}
			foreach (V_Weapon weapon in playerModel.weapons)
			{
				GameObject tmpObj = Instantiate(myInfoItemPrfb);
				// print(weapon.name);
				GameObject item = pool.GetItem(weapon.name);
				tmpObj.GetComponent<V_MyInfoItem>().Initialize(item);
				switch (item.GetComponent<V_Weapon>().type)
				{
					case V_Weapon.weaponType.pistol:
					tmpObj.transform.SetParent(pistolPanel.transform, false);
					break;

					case V_Weapon.weaponType.rifle:
					tmpObj.transform.SetParent(assaultPanel.transform, false);
					break;
					
					default:
					print ("V_MyInfo: OnEnable: error in defining item");
					break;
				}
				
			}


		}
		catch (System.Exception err)
		{
			UIController.ThrowError ("V_MyInfo: OnEnable: " + err.Message, UIController.CloseError);
			throw;
		}
	}
	void ShowPlayerDetailsOnWebView()
	{
		UIController.ThrowError ("Showing player details on web view", UIController.CloseError);
	}

}
