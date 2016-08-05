[System.SerializableAttribute]
public class V_RoomTemplate
{
    
    // fields
    public  bool hasAnActiveGame = false;

    public string roomName;
    public int ID;
    public Maps map;
    public PlayerModes playerMode;
    public GameModes gameMode;
    public int objectives;
    public string password;
    public WeaponFilter weaponFilter;
}