using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class V_Settings : V_UIElement 
{
	public enum ExpressGraphicsTypes {HIGH = 6, MED = 3, LOW = 0};
	V_AudioController audioController;
	ExpressGraphicsTypes expressType;
	bool isAdvancedGraphicsEnabled = false;

	[HeaderAttribute("Setting's main buttons and panels")]
	// settings buttons and other UI refs
	public Button videoBtn;
	public Button controlsBtn, audioBtn, picBtn, applyBtn, cancelBtn, defaultBtn;
	public GameObject videoPanel;
	public GameObject controlsPanel, audioPanel, picPanel;

	[HeaderAttribute("videoSettings")]
	// videoSettings;	
	public Dropdown resolutionDropDown;
	public Dropdown vSyncDropDown;
	public Slider GammaSlider;
	public Toggle fulScreenToggle;
	public Button expressGraphicBtn, advancedGraphicBtn, expressHighBtn, expressMedBtn, expressLowBtn;
	public GameObject expressGraphicPanel, advancedGraphicPanel;
	public Dropdown textureDropDown, antiAliasingDropDown;
	public Toggle weatherToggle, bloodSpatterToggle;

	[HeaderAttribute("control settings")]
	// control settings
		// mouse
	public Button mouseBtn;
	public Button keyboardBtn;
	public GameObject mousePanel, keyboardPanel;
	public Slider mouseSensitivitySlider, zoomSensitivitySlider;
	public Toggle invertToggle, invertMouseButtonsToggle;
		// keyboard
	public InputField rightInput;
	public InputField leftInput, crouchInput, zoomInput, jumpInput, pickupInput;
		
	[HeaderAttribute("audio Settings")]
	// audio settings
	public Slider themeVolSlider;
	public Slider fxVolSlider, voiceVolSlider;

	[HeaderAttribute("other Settings")]
	public Slider redSlider;
	public Slider greenSlider, blueSlider, crosshairSizeSlider;
	public Image currentCrosshair;
		// cached ref to RectTransform of crosshairs
		private RectTransform crosshairRectTransform;
		// a vector to keep the original size of the crosshair so it wont get distocrosshairRectTransformed when we change its size at runtime
		private Vector2 crosshairOriginalSize;
	private float crosshairSizeFactor = 1f;
	public Image[] crosshairs;
	public Toggle helpToggle, tooltipToggle, friendReqToggle, clanReqToggle, roomInvitationToggle;

	

	new void Awake () 
	{
		base.Awake();
		// dependencies
		audioController = FindObjectOfType <V_AudioController>();
		crosshairRectTransform = currentCrosshair.rectTransform;
		crosshairOriginalSize =  new Vector2 (crosshairRectTransform.rect.width, crosshairRectTransform.rect.height);
		// checking if dependencies are null to throw err
		if (audioController == null)
		{
			UIController.ThrowError("V_Settings: Awake: ome of the dependencies is null", UIController.CloseError);
		}
		// Settings main buttons
		UIController.IfClick_GoTo(videoBtn, ()=> UIController.Enable_DisableUI(videoPanel, controlsPanel, audioPanel, picPanel));
		UIController.IfClick_GoTo(controlsBtn, ()=> UIController.Enable_DisableUI(controlsPanel, videoPanel, audioPanel, picPanel));
		UIController.IfClick_GoTo(audioBtn, ()=> UIController.Enable_DisableUI(audioPanel, videoPanel, controlsPanel, picPanel));
		UIController.IfClick_GoTo(picBtn, ()=> UIController.Enable_DisableUI(picPanel, videoPanel, controlsPanel, audioPanel));

		UIController.IfClick_GoTo(applyBtn, SaveSettings);
		UIController.IfClick_GoTo(cancelBtn, CancelSettings);
		UIController.IfClick_GoTo(defaultBtn, RestoreSettings);
		
		// video settings buttons
		UIController.IfClick_GoTo(expressGraphicBtn, ()=> {isAdvancedGraphicsEnabled = false; UIController.Enable_DisableUI(expressGraphicPanel, advancedGraphicPanel);});
		UIController.IfClick_GoTo(advancedGraphicBtn, ()=> {isAdvancedGraphicsEnabled = true; UIController.Enable_DisableUI(advancedGraphicPanel, expressGraphicPanel);});

		UIController.IfClick_GoTo(expressHighBtn, ()=> expressType = ExpressGraphicsTypes.HIGH); // for fantastic QualitySettings
		UIController.IfClick_GoTo(expressMedBtn, ()=> expressType = ExpressGraphicsTypes.MED); // for medium and good 
		UIController.IfClick_GoTo(expressLowBtn, ()=> expressType = ExpressGraphicsTypes.LOW);  // for low QualitySettings

		// controlsBtns
		UIController.IfClick_GoTo(mouseBtn, ()=> UIController.Enable_DisableUI(mousePanel, keyboardPanel));
		UIController.IfClick_GoTo(keyboardBtn, ()=> UIController.Enable_DisableUI(keyboardPanel, mousePanel));

		// other settings
		UIController.OnSliderChangesValue(crosshairSizeSlider, (value)=> ChangeCrosshairSize(value));
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
		base.OnEnable();
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
		// video
		try
		{
			isAdvancedGraphicsEnabled = PlayerPrefs.GetInt("advancedGraphic") == 0 ? false : true;
			if (isAdvancedGraphicsEnabled)
			{
				UIController.Enable_DisableUI(advancedGraphicPanel, expressGraphicPanel);
			}
			else
			{
				UIController.Enable_DisableUI(expressGraphicPanel, advancedGraphicPanel);
			}
			
			expressType = (ExpressGraphicsTypes) PlayerPrefs.GetInt("beginnerGraphics", 0);

			resolutionDropDown.value = PlayerPrefs.GetInt("resolution", 0);
			vSyncDropDown.value = PlayerPrefs.GetInt("vsync", 0);	
			fulScreenToggle.isOn = (PlayerPrefs.GetInt("fulscreen") == 0) ? false : true;
			StartCoroutine(UIController.FillSlider(GammaSlider, PlayerPrefs.GetFloat("gamma")));
			textureDropDown.value = PlayerPrefs.GetInt("texture", 0);
			antiAliasingDropDown.value = PlayerPrefs.GetInt("antialiasing", 0);
			weatherToggle.isOn = (PlayerPrefs.GetInt("weatherfx")) == 0 ? false : true;
			bloodSpatterToggle.isOn = (PlayerPrefs.GetInt("bloodspatterfx") == 0 ? false : true);
		}
		catch (System.Exception err)
		{
			UIController.ThrowError("no data to load or " + err.Message, UIController.CloseError);

		}

		// control settings
		try
		{
			// mouse settings
			StartCoroutine(UIController.FillSlider(mouseSensitivitySlider, PlayerPrefs.GetFloat("mouseSensitivity", 0)));
			StartCoroutine(UIController.FillSlider(zoomSensitivitySlider, PlayerPrefs.GetFloat("zoomSensitivity", 0)));
			invertToggle.isOn = PlayerPrefs.GetInt("invecrosshairRectTransform") == 1 ? true : false;
			invertMouseButtonsToggle.isOn = PlayerPrefs.GetInt("invecrosshairRectTransformMouseButtons") == 1 ? true : false;
			// keyboard settings

			rightInput.text = PlayerPrefs.GetString("right");
			crouchInput.text = PlayerPrefs.GetString("crouch");
			zoomInput.text = PlayerPrefs.GetString("zoom");
			jumpInput.text = PlayerPrefs.GetString("jump");
			pickupInput.text = PlayerPrefs.GetString("pickup");
		}
		catch (System.Exception)
		{
			
			throw;
		}

		// audioSettings
		try
		{
			StartCoroutine(UIController.FillSlider(themeVolSlider, PlayerPrefs.GetFloat("themeVol", 0)));
			StartCoroutine(UIController.FillSlider(fxVolSlider, PlayerPrefs.GetFloat("fxVol", 0)));
			StartCoroutine(UIController.FillSlider(voiceVolSlider, PlayerPrefs.GetFloat("voiceVol", 0)));
		}
		catch (System.Exception err)
		{
			UIController.ThrowError(err.Message, UIController.CloseError);
		}
		
		// other Settings
		try
		{
			StartCoroutine(UIController.FillSlider(crosshairSizeSlider, PlayerPrefs.GetFloat("crosshairSize")));
			helpToggle.isOn = PlayerPrefs.GetInt("help") == 1 ? true : false;
			tooltipToggle.isOn = PlayerPrefs.GetInt("tooltip") == 1 ? true : false;
			friendReqToggle.isOn = PlayerPrefs.GetInt("friendReq") == 1 ? true : false;
			clanReqToggle.isOn = PlayerPrefs.GetInt("clanReq") == 1 ? true : false;
			roomInvitationToggle.isOn = PlayerPrefs.GetInt("roomInviteReq") == 1 ? true : false;
		}
		catch (System.Exception)
		{
			
			throw;
		}
		finally
		{
			ApplySettings();
		}

	}
	public void SaveSettings()
	{
		// videoSettings
		PlayerPrefs.SetInt("advancedGraphic", isAdvancedGraphicsEnabled ? 1 : 0);
		PlayerPrefs.SetInt("beginnerGraphics",(int) expressType);

		PlayerPrefs.SetInt("resolution", resolutionDropDown.value);
		PlayerPrefs.SetInt("vsync", vSyncDropDown.value);
		PlayerPrefs.SetInt("fulscreen", fulScreenToggle.isOn ? 1 : 0);
	    PlayerPrefs.SetFloat("gamma", GammaSlider.value);
		PlayerPrefs.SetInt("texture", textureDropDown.value);
		PlayerPrefs.SetInt("antialiasing", antiAliasingDropDown.value);
		PlayerPrefs.SetInt("weatherfx", weatherToggle.isOn ? 1 : 0);
		PlayerPrefs.SetInt("bloodspatterfx", bloodSpatterToggle.isOn ? 1 : 0);

		// control settings
			// mouse
			PlayerPrefs.SetFloat("mouseSensitivity", mouseSensitivitySlider.value);
			PlayerPrefs.SetFloat("zoomSensitivity", zoomSensitivitySlider.value);
			PlayerPrefs.SetInt("invecrosshairRectTransform", invertToggle.isOn ? 1 : 0);
			PlayerPrefs.SetInt("invecrosshairRectTransformMouseButtons", invertMouseButtonsToggle.isOn ? 1 : 0);
			// keyboard
			PlayerPrefs.SetString("right", rightInput.text);
			PlayerPrefs.SetString("crouch", crouchInput.text);
			PlayerPrefs.SetString("zoom", zoomInput.text);
			PlayerPrefs.SetString("jump", jumpInput.text);
			PlayerPrefs.SetString("pickup", pickupInput.text);		
		
		// audioSettings
		PlayerPrefs.SetFloat("themeVol", themeVolSlider.value);
		PlayerPrefs.SetFloat("fxVol", fxVolSlider.value);
		PlayerPrefs.SetFloat("voiceVol", voiceVolSlider.value);

		// other settings
		PlayerPrefs.SetFloat("crosshairSize", crosshairSizeSlider.value);
		PlayerPrefs.SetInt("help", helpToggle.isOn ? 1 : 0);
		PlayerPrefs.SetInt("tooltip", tooltipToggle.isOn ? 1 : 0);
		PlayerPrefs.SetInt("friendReq", friendReqToggle.isOn ? 1 : 0);
		PlayerPrefs.SetInt("clanReq", clanReqToggle.isOn ? 1 : 0);
		PlayerPrefs.SetInt("roomInviteReq", roomInvitationToggle.isOn ? 1 : 0);
		// the last step after saving everything is to:
		ApplySettings();
	}

	void ApplySettings()
	{

		// graphic settings
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
			SetQuality((int) expressType);
		}

		// control settings
		// for now we are saving control settings in PlayerPrefs and retrieving it whenever needed.

		// audio settings
		audioController.themeMusicVolume = themeVolSlider.value;
		audioController.fxVolume = fxVolSlider.value;
		audioController.voiceCommandsVolume = voiceVolSlider.value;
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

	void ChangeTextureColor (Texture2D texture)
	{
		// Texture2D newTexture = texture;
		// newTexture.filterMode = FilterMode.Point;
		// newTexture.wrapMode = TextureWrapMode.Clamp;
		// int y = 0;
		// while(y < newTexture.height)
		// {
		// 	int x = 0;
		// 	while(x < newTexture.width)
		// 	{
		// 		newTexture.SetPixel(x, y, new Color(redSlider.value, greenSlider.value, blueSlider.value));
		// 		x++;	
		// 	}
		// 	y++;
		// }
	}
	void ChangeTextureColor(Renderer renderer)
	{
		if (renderer != null)
		{
			renderer.material.color = new Color(redSlider.value, greenSlider.value, blueSlider.value);
		}
	}
	void ChangeCrosshairSize(float value)
	{
		crosshairRectTransform.sizeDelta = new Vector2(crosshairOriginalSize.x * value, crosshairOriginalSize.y * value);
	}
}
