using UnityEngine;
using UnityEngine.UI;
public enum BadgeType
{
	SOLDIER_III = 1,
	 SOLDIER_II = 601,
	  SOLDIER_I,
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
