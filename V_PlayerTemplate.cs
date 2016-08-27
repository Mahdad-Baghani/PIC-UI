using UnityEngine;
using System.Collections.Generic;

public class V_PlayerTemplate : MonoBehaviour 
{
	public new string name;
	public string nickName;
	public int score;
	public int wins;
	public int kills;
	public int charge;
	public Sprite icon;
	public V_Badge badge;
	public List<V_Achievement> achievements;
	public V_PlayerTemplate comrade;
	public V_ClanTemplate clan;
	public List<V_Weapon> weapons;
	public void Form(ref WWWForm form)
	{
		form.AddField("nickName",nickName);
		form.AddField("name",name);
		form.AddField("nickName",nickName);
		form.AddField("score",score.ToString());
		form.AddField("wins",wins.ToString());
		form.AddField("kills",kills.ToString());
		form.AddField("charge",charge.ToString());
		//form.AddField("icon",icon.ToString());
		form.AddField("badge",badge.ToString());
		//form.AddField("achievements",a);
		//form.AddField("comrade",comrade.ToString());
		//form.AddField("clan",clan.ToString());
	}
	//change
	public override string ToString()
	{
		string tmpStr = "";
		tmpStr += "name=" + name + "$";
		tmpStr += "nickName="+ nickName +"$";
		tmpStr += "score=" + score.ToString() + "$";
		tmpStr += "wins=" + wins.ToString() + "$";
		tmpStr += "kills=" + kills.ToString() + "$";
		tmpStr += "charge=" + charge.ToString() + "$";
		tmpStr += "badge=" + badge.ToString() + "$";

		return tmpStr;
	}		
}
