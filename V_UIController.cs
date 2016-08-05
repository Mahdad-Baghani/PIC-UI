using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class V_UIController : MonoBehaviour 
{
    // player model reference would come in handy...
    public static V_PlayerTemplate playerModel;
    // consts for room,
    // use this in sync with UI
    public const string GM_SD = "Search and Destroy";
    public const string GM_TDM = "Team DeathMatch";
    public const string GM_DM = "DeathMatch";

    public const string PM_ON1 = "1 On 1";
    public const string PM_ON2 = "2 On 2";
    public const string PM_ON3 = "3 On 3";
    public const string PM_ON4 = "4 On 4";
    public const string PM_ON5 = "5 On 5";
    public const string PM_ON6 = "6 On 6";
    public const string PM_ON7 = "7 On 7";
    public const string PM_ON8 = "8 On 8";

    public const string M_DUST = "Dust";
    public const string M_RUST = "Rust";

    public const string WF_NONE = "None";
    public const string WF_KNIFE = "Knife only";
    public const string WF_ASSAULT = "Assault only";
    public const string WF_GERENADE = "Gerenade only";


    // fields

    [HeaderAttribute("All arraylike Refs across UI")]
    public Sprite[] thumbnailMaps;
    public V_Badge[] allBadges;
    public GameObject emptyObjectWithImage;

    [SpaceAttribute(5f)]
    [HeaderAttribute("UI Panels")]
    [SpaceAttribute(5f)]
    public GameObject LobbyPanel;
    public GameObject RoomModalPanel;
    public GameObject RoomPanel;
    public Text toolTipBar;

    [SpaceAttribute(5f)]
    public GameObject currentPanel;

    [HeaderAttribute("UI Generic Panels")]
    [SpaceAttribute(5f)]
    public GameObject genericYesNoModal;
    public Button genericYesBtn;
    public Button genericNoBtn;

    [SpaceAttribute(5f)]
    public GameObject genericErrorModal;
    public Button genericOkBtn;

    [SpaceAttribute(5f)]
    public GameObject genericSearch_n_Select;
    public Text genericMessage;
    public InputField genericSearchText;
    public Button genericSelectBtn;
    public Button genericCancelBtn;

    [SpaceAttribute(5f)]
    [HeaderAttribute("UI buttons")]
    [SpaceAttribute(5f)]
    public Button backButton;
    
    [HeaderAttribute("UI Effects vars")]
    [RangeAttribute(0, 1)] public float fadeFactor;
    public float waitFactor;
    

    // methods

    void Awake()
    {
        playerModel = FindObjectOfType<V_PlayerTemplate>();
    }
    public void GoFrom_To (GameObject from, GameObject nextPanel)
    {
       if (nextPanel != null && from != null)
       {           
           from.SetActive(false);
           nextPanel.SetActive(true);
           currentPanel = nextPanel;
       }
    }

    public void GoFrom_To(GameObject from, GameObject nextPanel, bool killPreviousPanel)
    {
        if (nextPanel != null && from != null)
        {
            if (killPreviousPanel)
            {
                from.SetActive(false);
            }
            nextPanel.SetActive(true);
            currentPanel = nextPanel;
        }
    }
    public void ExitLobby()
    {
        // show a Yes/No modal window
    }
    public void IfClick_GoTo(Button button, UnityAction someEvent)
    {
        try
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(someEvent);   
        }
        catch (System.Exception err)
        {
            ThrowError(err.Message, CloseError);
            throw;
        }
     
        // debug
        // print(button.name);
    }
    public void Enable_DisableUI(GameObject enableThis, params GameObject[] disableThese)
    {
        try
        {
            enableThis.SetActive(true);
            foreach (GameObject obj in disableThese)
            {
                obj.SetActive(false);
            }
        }
        catch (System.Exception err)
        {
            ThrowError("V_UIController: Enable_DisableUI(): " + err.Message, CloseError);
            throw;
        }
    }

    public void OnDropDownChangesValue(Dropdown dropdown, UnityAction<int> someEvent)
    {
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener(someEvent);
    }

    public void ifType_DoThis(InputField input, UnityAction eve)
    {
        input.onEndEdit.RemoveAllListeners();
        // input.onEndEdit.AddListener(eve<string>);
    }

    public IEnumerator FadeIn(GameObject panel)
    {
        float lerp = 0;
        CanvasRenderer tmpRndr = panel.GetComponent<CanvasRenderer>();
        if (tmpRndr == null)
        {
            print("V_UIController: FadeIn: Cannot get the CanvasRenderer on " + panel.name);
        }
        else
        {
            while(lerp <= 1f)
            {
                tmpRndr.SetAlpha(lerp);
                panel.SetActive(true);
                lerp += fadeFactor;
                yield return new WaitForSeconds(waitFactor);
            }  
        }

    }
    public IEnumerator FadeOut(GameObject panel)
    {
        float lerp = 1;
        CanvasRenderer tmpRndr = panel.GetComponent<CanvasRenderer>();
        if (tmpRndr == null)
        {
            print("V_UIController: FadeOut: cannot get te CanvasRenderer on " + panel.name);
            panel.AddComponent<CanvasRenderer>();
        }
       
        while (lerp >= 0)
        {
            tmpRndr.SetAlpha(lerp);
            lerp -= fadeFactor;
            yield return new WaitForSeconds(fadeFactor);
        }
        panel.SetActive(false);
    }
    public void LoadThumbnailMaps()
    {
        
    }

     public GameModes ReturnGameMode(string mode)
    {
        switch (mode)
        {
            case(GM_SD):
            return GameModes.SD;

            case(GM_DM):
            return GameModes.DM;

            case(GM_TDM):
            return GameModes.TDM;
            
            default:
            return GameModes.SD;
        }
    }
    public PlayerModes ReturnPlayerMode(string mode)
    {
        switch (mode)
        {
            case(PM_ON1):
            return PlayerModes.ON1;
            
            case(PM_ON2):
            return PlayerModes.ON2;

            case(PM_ON3):
            return PlayerModes.ON3;

            case(PM_ON4):
            return PlayerModes.ON4;

            case(PM_ON5):
            return PlayerModes.ON5;

            case(PM_ON6):
            return PlayerModes.ON6;

            case(PM_ON7):
            return PlayerModes.ON7;

            case(PM_ON8):
            return PlayerModes.ON8;
            
            default:
            return PlayerModes.ON1;
        }
    }

    public string ReturnGameMode (GameModes mode)
    {
        switch (mode)
        {
            case(GameModes.SD):
            return GM_SD;
            case(GameModes.TDM):
            return GM_TDM;
            case(GameModes.DM):
            return GM_DM;
            default:
            return "ReturnGameMode: GameMode not Defined";
        }
    }
    

    public string ReturnPlayerMode(PlayerModes mode)
    {
        switch (mode)
        {
            // #revision
            case(PlayerModes.ON1):
            return PM_ON1;
            
            case(PlayerModes.ON2):
            return PM_ON2;
            
            case(PlayerModes.ON3):
            return PM_ON3;

            case(PlayerModes.ON4):
            return PM_ON4;

            case(PlayerModes.ON5):
            return PM_ON5;

            case(PlayerModes.ON6):
            return PM_ON6;
            
            case(PlayerModes.ON7):
            return PM_ON7;

            case(PlayerModes.ON8):
            return PM_ON8;
            
            default:
            return "ReturnPlayerMode: playerMode not Defined";
        }
    }

    public Maps ReturnMap(string map)
    {
        switch (map)
        {
            case(M_DUST):
            return Maps.DUST;

            case(M_RUST):
            return Maps.RUST;

            default:
            return Maps.DUST;
        }
    }

    public string ReturnMap(int map)
    {
        switch (map)
        {
            case 0:
            return M_DUST;

            case 1:
            return M_RUST;

            default:
            return M_DUST;
        }
    }
    public string ReturnMap (Maps map)
    {
        switch (map)
        {
            case Maps.DUST:
            return M_DUST;

            case Maps.RUST:
            return M_RUST;

            default:
            return M_DUST;
        }
    }
    public WeaponFilter ReturnWeaponFilter(string filter)
    {
        switch (filter)
        {
            case(WF_NONE):
            return WeaponFilter.NONE;

            case(WF_ASSAULT):
            return WeaponFilter.ASSUALT;

            case(WF_KNIFE):
            return WeaponFilter.KNIFE;

            case(WF_GERENADE):
            return WeaponFilter.GERENADE;

            default:
            return WeaponFilter.NONE;
        }
    }
    public string ReturnWeaponFilter(WeaponFilter filter)
    {
        switch (filter)
        {
            case(WeaponFilter.NONE):
            return WF_NONE;

            case(WeaponFilter.ASSUALT):
            return WF_ASSAULT;

            case(WeaponFilter.KNIFE):
            return WF_KNIFE;

            case(WeaponFilter.GERENADE):
            return WF_GERENADE;

            default:
            return WF_NONE;
        }
    }

    public void GetItemInDropDown(Dropdown dropdown, string item)
    {
        int tmp = 0;
        foreach (Dropdown.OptionData option in dropdown.options)
        {
            if (option.text == item)
            {
                dropdown.value = tmp;
                break;
            }
            tmp++;
        }
    }

    public void AskYesNoQ (string question, UnityAction yesAction, UnityAction noAction)
    {
        genericYesBtn.onClick.RemoveAllListeners();
        genericYesBtn.onClick.AddListener(yesAction);

        genericNoBtn.onClick.RemoveAllListeners();
        genericNoBtn.onClick.AddListener(noAction);

        genericYesNoModal.GetComponentInChildren<Text>().text = question;
        GoFrom_To(currentPanel, genericYesNoModal, killPreviousPanel: false);
    }
    public void CloseYesNoQ()
    {
        genericYesNoModal.SetActive(false);
    }

    public void ThrowError (string err, UnityAction okAction)
    {
        genericOkBtn.onClick.RemoveAllListeners();
        genericOkBtn.onClick.AddListener(okAction);

        genericErrorModal.GetComponentInChildren<Text>().text = err;
        GoFrom_To(currentPanel, genericErrorModal, killPreviousPanel: false);
    }
    public void CloseError()
    {
        genericErrorModal.SetActive(false);
    }
    public void SelectFromListModal(string msg, UnityAction selectAction, UnityAction cancelAction, UnityAction<string> inputAction)
    {
        genericMessage.text = msg;

        genericCancelBtn.onClick.RemoveAllListeners();
        genericSelectBtn.onClick.AddListener(selectAction);

        genericCancelBtn.onClick.RemoveAllListeners();
        genericCancelBtn.onClick.AddListener(cancelAction);

        genericSearchText.onValueChanged.RemoveAllListeners();
        genericSearchText.onValueChanged.AddListener(inputAction);
        
        GoFrom_To(currentPanel, genericSearch_n_Select, killPreviousPanel: false);
    }
    public void CloseSelectModal()
    {
        genericSearch_n_Select.SetActive(false);
    }


    public void ShowTooltip(string msg)
    {
        toolTipBar.text = msg;
    }
    
    public IEnumerator FillSlider(Slider slider, float finalValue)
    {
        float tmp = 0f;
        slider.value = 0f;
        int times = 10;
        int time = 0;
        if (slider !=null)
        {
            while (true)
            {

                if (time % times != 0)
                {
                    time++;
                    yield return null;
                }
                time++;
                tmp += Time.deltaTime;
                slider.value = Mathf.Lerp(slider.value, finalValue, tmp);
                yield return new WaitForSeconds(waitFactor);

                if (slider.value >= finalValue)
                {
                    yield break;
                }

                // slider.value += Mathf.Lerp()
                // print(slider.value.ToString());
                // print(slider.name);
            }
        }
    }
}
