using UnityEngine;
public enum BadgeType
{
	NONE = 0,
	SOLDIER_III = 1,
	 SOLDIER_II = 601,
	  SOLDIER_I = 1700,
	   LIEUTENANT_III,
	    LIEUTENANT_II,
		 LIEUTENANT_I
}

public class V_Badge : V_UIElement 
{
	public const string soldier_III = "Soldier III";
	public const string soldier_II = "Soldier II";
	public const string soldier_I = "Soldier I";


	public BadgeType badgeType;
	public Sprite badgeIcon;
	public string badgeName;


	new void Awake()
	{

	}
	new void OnEnable()
	{
		// do not call base.OnEnable on this type of objects, so we keep it seperate from UI panels which need to call base.OnEnable() while hiding it
	}
	public void SetBadge (out V_Badge tmpBadge, BadgeType type)
	{
		try
		{
			tmpBadge = null;
			foreach (V_Badge badge in UIController.allBadges)
			{
				if (badge.badgeType == type)
				{
					tmpBadge = badge;
					break;
				}
			}
		}
		catch (System.Exception err)
		{
			print (err.Message);
			throw;
		}
	}

	public override string ToString()
	{
		switch (this.badgeType)
		{
			case BadgeType.SOLDIER_III:
			return soldier_III;

			case BadgeType.SOLDIER_II:
			return soldier_II;

			case BadgeType.SOLDIER_I:
			return soldier_I;

			// #revision: add more cases
			default:
			return null;
		}
	}
}
