using System.Collections.Generic;
using UnityEngine;
[System.SerializableAttribute]
public class V_RoomTemplate
{
    // fields
    public List<V_PlayerTemplate> players;
    public  bool hasAnActiveGame = false;
    public string roomName;
    public int ID;
    public Maps map;
    public PlayerModes playerMode;
    public GameModes gameMode;
    public int objectives;
    public string password;
    public WeaponFilter weaponFilter;
    public int gameTime;
    public bool autoTeamSelection;

    // methods
    // public void V_PlayerTemplate()
    // {
    //     players = new List<V_PlayerTemplate>();
    // }
    public void Form(ref WWWForm form){
        form.AddField("hasAnActiveGame",hasAnActiveGame.ToString());
        form.AddField("roomName",roomName);
        form.AddField("ID",ID.ToString());
        form.AddField("map",map.ToString());
        form.AddField("playerMode",playerMode.ToString());
        form.AddField("gameMode",gameMode.ToString());
        form.AddField("objectives",objectives.ToString());
        form.AddField("password",password);
        form.AddField("weaponFilter",weaponFilter.ToString());
        form.AddField("gameTime",gameTime.ToString());
        form.AddField("autoTeamSelection",autoTeamSelection.ToString());
    }

}