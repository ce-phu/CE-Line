    
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



public class HomeUIManager : MonoBehaviour
{
    public static HomeUIManager Instance;

    [Header("Buttons")] [SerializeField] private Button settingButton;
    [SerializeField] private Button luckyButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button playButton;

    [Header("Text")] [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI luckyText;
    [SerializeField] private TextMeshProUGUI shopText;
    [SerializeField] private TextMeshProUGUI playText;
    [SerializeField] private TextMeshProUGUI stage1Text;
    [SerializeField] private TextMeshProUGUI stage2Text;
    [SerializeField] private TextMeshProUGUI stage3Text;

    [Header("Animator")] [SerializeField] private Animator anim;

    [SerializeField] private HomeEffectManager effect;

    private bool isAnimationCompleted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        SetCoin(Player.Data.gold);
        Player.Instance.OnGoldChanged += UpdateCoin;
    }

    public static void In()
    {
        Instance._In();
    }

    private void _In()
    {
        anim.Play("In");
        isAnimationCompleted = false;
        
        settingButton.onClick.AddListener(OnClick_SettingButton);
        luckyButton.onClick.AddListener(OnClick_LuckyButton);
        shopButton.onClick.AddListener(OnClick_ShopButton);
        playButton.onClick.AddListener(OnClick_PlayButton);
        
        SetTextAndStage();
    }

    public static void Out()
    {
        Instance._Out();
    }

    private void _Out()
    {
        anim.Play("Out");
        isAnimationCompleted = false;
        
        settingButton.onClick.RemoveAllListeners();
        luckyButton.onClick.RemoveAllListeners();
        shopButton.onClick.RemoveAllListeners();
        playButton.onClick.RemoveAllListeners();
    }

    public static void Def()
    {
        Instance._Def();
    }

    private void _Def()
    {
        anim.Play("Def");
    }



    public static void SetCoin(int amount)
    {
        Instance.coinText.text = amount.ToString();
    }

    private void UpdateCoin()
    {
        coinText.text = Player.Data.gold.ToString();
    }



    private void SetTextAndStage()
    {
        // playText.text = MessageData.HomeUI.PlayButton;

        stage1Text.text = Player.Data.currentStage.ToString();
        stage2Text.text = (Player.Data.currentStage + 1).ToString();
        stage3Text.text = (Player.Data.currentStage + 2).ToString();

        // luckyText.text = MessageData.HomeUI.LuckyButton;
        // shopText.text = MessageData.HomeUI.ShopButton;
    }
    
    private void OnClick_SettingButton()
    {        
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;        
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        SettingUIManager.In();
    }

    private void OnClick_LuckyButton()
    {
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;        
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        LuckyWheelUI.In();
    }

    private void OnClick_ShopButton()
    {
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;        
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        ShopUIManager.In();
    }

    public static void OnClick_PlayButton()
    {
        if ( SystemManager.excludeButton ) {

            return;
        }
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        SystemManager.excludeButton   = true;
        
        SoundManager.PlaySE( SE.LEVEL_START );

        Home.goToIngame = true;
    }
    
    public static void PlayEffect( )
    {
        Instance.isAnimationCompleted = false;
        Instance.effect.Play();
    }

    
    public static bool IsCompleteAnimation()
    {
        return Instance.isAnimationCompleted;
    }

    public void AnimationCompleted()
    {
        isAnimationCompleted = true;

        SystemManager.excludeButton = false;
    }
}
