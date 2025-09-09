using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class SettingUIManager : MonoBehaviour 
{

    public static SettingUIManager Instance;

    [SerializeField] private TextMeshProUGUI titleText;
    
    [Header("Setting")] 
    [SerializeField] private Button bgmButton;
    [SerializeField] private GameObject bgmDisable;
    [SerializeField] private Image bgmImage;
    [SerializeField] private Button seButton;
    [SerializeField] private GameObject seDisable;
    [SerializeField] private Image seImage;
    [SerializeField] private Button vibrationButton;
    [SerializeField] private GameObject vibrationDisable;
    [SerializeField] private Image vibrationImage;

    [Header("Buttons")] 
    [SerializeField] private TextMeshProUGUI helpButtonText;
    [SerializeField] private Button helpButton;
    [SerializeField] private TextMeshProUGUI giftCodeButtonText;
    [SerializeField] private Button giftCodeButton;
    [SerializeField] private TextMeshProUGUI joinUsButtonText;
    [SerializeField] private Button joinUsButton;
    [SerializeField] private Button closeButton;
    
    [Header("Animator")]
    [SerializeField] private Animator animator;

    private Color disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.9019608f);
    public static bool isOpen = false;

    private void Awake() {

        Instance    = this;
    }
    


    public static void In() {

        Instance._In();
    }



    private void _In() {
        
        SoundManager.PlaySE(SE.UI_PANELSHOW);
        
        bgmButton.onClick.AddListener(OnClick_BGMButton);
        seButton.onClick.AddListener(OnClick_SEButton);
        vibrationButton.onClick.AddListener(OnClick_VibrationButton);
        
        helpButton.onClick.AddListener(OnClick_HelpButton);
        giftCodeButton.onClick.AddListener(OnClick_GiftCodeButton);
        joinUsButton.onClick.AddListener(OnClick_JoinUsButton);
        closeButton.onClick.AddListener(OnClick_CloseButton);

        // titleText.text = MessageData.SettingUI.Title;
        
        animator.Play( "In" );
    }

    public static void Ingame_In()
    {
        Instance._Ingame_In();
    }

    private void _Ingame_In()
    {
        bgmButton.onClick.AddListener(OnClick_BGMButton);
        seButton.onClick.AddListener(OnClick_SEButton);
        vibrationButton.onClick.AddListener(OnClick_VibrationButton);
        closeButton.onClick.AddListener(OnClick_IngameCloseButton);
        
        // titleText.text = MessageData.SettingUI.Title;
        
        animator.Play( "Ingame_In" );
        isOpen = true;
        
        Result.AddListener(Ingame_Out);
    }


    public static void Out() {

        Instance._Out();
    }



    private void _Out()
    {
        SoundManager.PlaySE(SE.UI_PANELCLOSE);
        
        bgmButton.onClick.RemoveAllListeners();
        seButton.onClick.RemoveAllListeners();
        vibrationButton.onClick.RemoveAllListeners();
        
        helpButton.onClick.RemoveAllListeners();
        giftCodeButton.onClick.RemoveAllListeners();
        joinUsButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
        
        animator.Play( "Out" );

        SaveDataManager.SaveData();
    }
    
    public static void Ingame_Out()
    {
        Instance._Ingame_Out();
    }

    private void _Ingame_Out()
    {
        bgmButton.onClick.RemoveAllListeners();
        seButton.onClick.RemoveAllListeners();
        vibrationButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
        
        SaveDataManager.SaveData();
        Ingame.SetPause(false);
        
        animator.Play( "Ingame_Out" );
        isOpen = false;
        
        Result.RemoveListener(Ingame_Out);
    }

    
    public void OnClick_BGMButton()
    {
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );


        if (SaveDataManager.data.BGMvalue == 0)
        {
            SaveDataManager.data.BGMvalue = 1;
            bgmImage.color = Color.white;
            bgmDisable.SetActive(false);
        }
        else
        {
            SaveDataManager.data.BGMvalue = 0;
            bgmImage.color = disabledColor;
            bgmDisable.SetActive(true);
        }

        SoundManager.SetBGMVolume();

        SystemManager.excludeButton = false;
    }

    public void OnClick_SEButton()
    {
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );


        if (SaveDataManager.data.soundvalue == 0)
        {
            SaveDataManager.data.soundvalue = 1;
            seImage.color = Color.white;
            seDisable.SetActive(false);
        }
        else
        {
            SaveDataManager.data.soundvalue = 0;
            seImage.color = disabledColor;
            seDisable.SetActive(true);
        }
        
        SystemManager.excludeButton = false;
    }
    
    
    public void OnClick_VibrationButton()
    {
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );


        if (!SaveDataManager.data.enableVib)
        {
            SaveDataManager.data.enableVib = true;
            vibrationImage.color = Color.white;
            vibrationDisable.SetActive(false);
        }
        else
        {
            SaveDataManager.data.enableVib = false;
            vibrationImage.color = disabledColor;
            vibrationDisable.SetActive(true);
        }
        
        SystemManager.excludeButton = false;
    }

    public void OnClick_HelpButton()
    {
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        Debug.LogWarning("Pressed on Help And Support Button");
        StartCoroutine( SystemManager.Cor_OpenURL( "https://cubicegg.asia/PrivacyPolicy_en.html" ) );
        
        SystemManager.excludeButton = false;
    }


    public void OnClick_GiftCodeButton()
    {
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        Debug.LogWarning("Pressed on Gift Code Button");
        StartCoroutine( SystemManager.Cor_OpenURL( "https://cubicegg.asia/TermOfUse_en.html" ) );

        SystemManager.excludeButton = false;
    }
    
    
    public void OnClick_JoinUsButton()
    {
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        Debug.LogWarning("Pressed on Join Us Button");
     
        SystemManager.excludeButton = false;
    }

    public void UpdateSetting()
    {
        if (SaveDataManager.data.BGMvalue == 0)
        {
            bgmImage.color = disabledColor;
            bgmDisable.SetActive(true);
        }
        else
        {
            bgmImage.color = Color.white;
            bgmDisable.SetActive(false);
        }

        if (SaveDataManager.data.soundvalue == 0)
        {
            seImage.color = disabledColor;
            seDisable.SetActive(true);
        }
        else
        {
            seImage.color = Color.white;
            seDisable.SetActive(false);
        }
        
        if (!SaveDataManager.data.enableVib)
        {
            vibrationImage.color = disabledColor;
            vibrationDisable.SetActive(true);
        }
        else
        {
            vibrationImage.color = Color.white;
            vibrationDisable.SetActive(false);
        }

        
        // helpButtonText.text = MessageData.SettingUI.HelpButton;
        // giftCodeButtonText.text = MessageData.SettingUI.GiftCodeButton;
    }
    
    private void OnClick_CloseButton() {

        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_CLOSEBTNCLICK );

        Out();
    }

    private void OnClick_IngameCloseButton() {

        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_CLOSEBTNCLICK );

        Ingame_Out();
    }
    
    public void CompleteAnimation() {

        SystemManager.excludeButton = false;
    }
}

