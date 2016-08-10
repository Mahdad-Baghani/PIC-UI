using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Random = UnityEngine.Random;

public class V_Settings : V_UIElement 
{
	public enum BeginnerGraphicsType {HIGH = 6, MED = 3, LOW = 0};
	BeginnerGraphicsType beginnerType;
	bool isAdvancedGraphicsEnabled = false;
	// settings buttons and other UI refs
	public Button videoBtn, controlsBtn, audioBtn, picBtn, applyBtn, cancelBtn, defaultBtn;
	public GameObject videoPanel, controlsPanel, audioPanel, picPanel;

	// videoSettings;	
	public Dropdown resolutionDropDown, vSyncDropDown;
	public Slider GammaSlider;
	public Toggle fulScreenToggle;
	public Button beginnerGraphicBtn, advancedGraphicBtn, begHighBtn, begMedBtn, begLowBtn;
	public GameObject beginnerGraphicPanel, advancedGraphicPanel;
	public Dropdown textureDropDown, antiAliasingDropDown;
	public Toggle weatherToggle, bloodSpatterToggle;
	// V_AudioSettings audioSettings;
	// V_ControlSettings controlSettings;
	// V_PICSettings picSettings;


	// Use this for initialization
	new void Awake () 
	{
		base.Awake();
		// Settings main buttons
		UIController.IfClick_GoTo(videoBtn, ()=> UIController.Enable_DisableUI(videoPanel, controlsPanel, audioPanel, picPanel));
		UIController.IfClick_GoTo(controlsBtn, ()=> UIController.Enable_DisableUI(controlsPanel, videoPanel, audioPanel, picPanel));
		UIController.IfClick_GoTo(audioBtn, ()=> UIController.Enable_DisableUI(audioPanel, videoPanel, controlsPanel, picPanel));
		UIController.IfClick_GoTo(picBtn, ()=> UIController.Enable_DisableUI(picPanel, videoPanel, controlsPanel, audioPanel));

		UIController.IfClick_GoTo(applyBtn, SaveSettings);
		UIController.IfClick_GoTo(cancelBtn, CancelSettings);
		UIController.IfClick_GoTo(defaultBtn, RestoreSettings);
		
		// video settings buttons
		UIController.IfClick_GoTo(beginnerGraphicBtn, ()=> {isAdvancedGraphicsEnabled = false; UIController.Enable_DisableUI(beginnerGraphicPanel, advancedGraphicPanel);});
		UIController.IfClick_GoTo(advancedGraphicBtn, ()=> {isAdvancedGraphicsEnabled = true; UIController.Enable_DisableUI(advancedGraphicPanel, beginnerGraphicPanel);});

		UIController.IfClick_GoTo(begHighBtn, ()=> beginnerType = BeginnerGraphicsType.HIGH); // for fantastic QualitySettings
		UIController.IfClick_GoTo(begMedBtn, ()=> beginnerType = BeginnerGraphicsType.MED); // for medium and good 
		UIController.IfClick_GoTo(begLowBtn, ()=> beginnerType = BeginnerGraphicsType.LOW);  // for low QualitySettings
	}

	void SetQuality(int level)
	{
		try
		{
			QualitySettings.SetQualityLevel(level);
		}
		catch (System.Exception err)
		{
			UIController.ThrowError(err.Message, UIController.CloseError);
			throw;
		}
	}

    new void OnEnable()
	{
		LoadSettings();
	}
    private void CancelSettings()
    {
		this.gameObject.SetActive(false);
    }
    private void RestoreSettings()
    {

    }
	public void LoadSettings()
	{
		print(PlayerPrefs.GetFloat("gamma"));
		// video
		try
		{
			isAdvancedGraphicsEnabled = PlayerPrefs.GetInt("advancedGraphic") == 0 ? false : true;
			if (isAdvancedGraphicsEnabled)
			{
				UIController.Enable_DisableUI(advancedGraphicPanel, beginnerGraphicPanel);
			}
			else
			{
				UIController.Enable_DisableUI(beginnerGraphicPanel, advancedGraphicPanel);
			}
			beginnerType = (BeginnerGraphicsType) PlayerPrefs.GetInt("beginnerGraphics", 0);

			resolutionDropDown.value = PlayerPrefs.GetInt("resolution", 0);
			vSyncDropDown.value = PlayerPrefs.GetInt("vsync", 0);	
			fulScreenToggle.isOn = (PlayerPrefs.GetInt("fulscreen") == 0) ? false : true;
			UIController.FillSlider(GammaSlider, PlayerPrefs.GetFloat("gamma"));
			// GammaSlider.value = PlayerPrefs.GetFloat("gamma");
			textureDropDown.value = PlayerPrefs.GetInt("texture", 0);
			antiAliasingDropDown.value = PlayerPrefs.GetInt("antialiasing", 0);
			weatherToggle.isOn = (PlayerPrefs.GetInt("weatherfx")) == 0 ? false : true;
			bloodSpatterToggle.isOn = (PlayerPrefs.GetInt("bloodspatterfx") == 0 ? false : true);
		}
		catch (System.Exception err)
		{
			print("No saved Data to load + " + err.Message);
		}
		finally
		{
			ApplySettings();
		}
	}
	public void SaveSettings()
	{
		PlayerPrefs.SetInt("advancedGraphic", isAdvancedGraphicsEnabled ? 1 : 0);
		PlayerPrefs.SetInt("beginnerGraphics",(int) beginnerType);

		PlayerPrefs.SetInt("resolution", resolutionDropDown.value);
		PlayerPrefs.SetInt("vsync", vSyncDropDown.value);
		PlayerPrefs.SetInt("fulscreen", fulScreenToggle.isOn ? 1 : 0);
	    PlayerPrefs.SetFloat("gamma", GammaSlider.value);
	    print(GammaSlider.value);
		PlayerPrefs.SetInt("texture", textureDropDown.value);
		PlayerPrefs.SetInt("antialiasing", antiAliasingDropDown.value);
		PlayerPrefs.SetInt("weatherfx", weatherToggle.isOn ? 1 : 0);
		PlayerPrefs.SetInt("bloodspatterfx", bloodSpatterToggle.isOn ? 1 : 0);
		
		// the last step after saving everything is to:
		ApplySettings();
	}

	void ApplySettings()
	{
		// #revision: vsync is normally set to 0, 1 and 2;
		QualitySettings.vSyncCount = vSyncDropDown.value;
		PlayerSettings.defaultIsFullScreen = fulScreenToggle.isOn;
		int[] res = ReturnResolution(resolutionDropDown.value);
		RenderSettings.ambientLight = Color.Lerp(Color.black, Color.white, GammaSlider.value);
		if (isAdvancedGraphicsEnabled)
		{
			QualitySettings.antiAliasing = antiAliasingDropDown.value;
			QualitySettings.masterTextureLimit = textureDropDown.value;
			Screen.SetResolution(res[0], res[1], fulScreenToggle.isOn);
		}
		else
		{
			SetQuality((int) beginnerType);
		}
	}

	public int[] ReturnResolution(int i)
	{
		switch (i)
		{
			case 0:
			return new int[] {600, 800};

			case 1:
			return new int[] {400,300}; //!! just for the sake of argument

			case 2:
			return new int[] {1024, 720};
			
			default:
			return new int[] {1366, 748};
		}
	}
}
