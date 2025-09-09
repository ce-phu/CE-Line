using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeUIManager : MonoBehaviour
{
    public static TimeUIManager Instance;

    [SerializeField] private Animator anim;

    [SerializeField] private GameObject normalClock;
    [SerializeField] private GameObject infiniteClock;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI adsText;

    [SerializeField] private TextMeshProUGUI clockText;
    [SerializeField] private TextMeshProUGUI coinText;
    
    [SerializeField] private GameObject coinDisablePanel;

    [SerializeField] private Button coinButton;
    [SerializeField] private Button adsButton;
    [SerializeField] private Button closeButton;
    
    private bool isInfinite = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public static void In(bool isInfinite)
    {
        Instance._In(isInfinite);
    }

    private void _In(bool _isInfinite)
    {
        SoundManager.PlaySE(SE.UI_PANELSHOW);
        
        isInfinite = _isInfinite;
        
        anim.Play("In");
        
        SetupButton();
        
        coinButton.onClick.AddListener(OnClick_RecoveryButton);
        adsButton.onClick.AddListener(OnClick_AdsButton);
        closeButton.onClick.AddListener(OnClick_CloseButton);
        
        SystemManager.excludeButton = true;
        
        Result.AddListener(Out);
    }

    private void SetupButton()
    {
        if (isInfinite)
        {
            normalClock.SetActive(false);
            infiniteClock.SetActive(true);
            
            coinText.text = Player.TIME_INFTIME_PRICE.ToString();

            titleText.text = MessageData.TimeUI.InfiniteTitle;
            descText.text = MessageData.TimeUI.InfiniteDescription;
            adsText.text = MessageData.TimeUI.AdsButton;

            coinButton.gameObject.SetActive(true);
            adsButton.gameObject.SetActive(false);
            
            if (Player.Instance.HasEnoughGold(Player.TIME_INFTIME_PRICE))
            {
                coinDisablePanel.SetActive(false);
                coinText.color = Color.white;
            }
            else
            {
                coinDisablePanel.SetActive(true);
                coinText.color = Color.red;
            } 
        }
        else
        {
            normalClock.SetActive(true);
            infiniteClock.SetActive(false);

            clockText.text = "+20s";
            coinText.text = Player.TIME_RECOVER_PRICE.ToString();

            titleText.text = MessageData.TimeUI.NormalTitle;
            descText.text = MessageData.TimeUI.NormalDescription;
            
            coinButton.gameObject.SetActive(true);
            adsButton.gameObject.SetActive(false);
            
            if (Player.Instance.HasEnoughGold(Player.TIME_RECOVER_PRICE))
            {
                coinDisablePanel.SetActive(false);
                coinText.color = Color.white;
                coinButton.interactable = true;
            }
            else
            {
                coinDisablePanel.SetActive(true);
                coinText.color = Color.red;
                coinButton.interactable = false;
            }
        }
    }

    private void OnClick_RecoveryButton()
    {
        if ( SystemManager.excludeButton ) {

            return;
        }

        SystemManager.excludeButton   = true;

        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );

        if (isInfinite)
        {
            Player.Instance.UseGold(Player.TIME_INFTIME_PRICE);
            //Player.Instance.InfTime(0, true);
            //todo Nghia anh add ho em cai khong mat tg o day voi
            // GameManager.AddInfPlayingTime( );
        }
        else
        {
            Player.Instance.UseGold(Player.TIME_RECOVER_PRICE);
            //todo Nghia anh add ho em cai hoi 20s o day voi
            // GameManager.AddPlayingTime( );
            Result.isContinue = true;
        }

        //SystemManager.excludeButton   = false;
        Out(  );
    }

    private void OnClick_AdsButton()
    {
        if ( SystemManager.excludeButton ) {

            return;
        }

        SystemManager.excludeButton   = true;

        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        SystemManager.excludeButton   = false;
        OnClick_CloseButton();
    }
    
    private void OnClick_CloseButton()
    {
        if ( SystemManager.excludeButton ) {

            return;
        }

        SystemManager.excludeButton   = true;

        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_CLOSEBTNCLICK );

        if (isInfinite)
        {
            Out();
        }
        else
        {
            Out();
            
            // SoundManager.PlaySE( SE.SPIN_SPINNING );
            
            ResultUIManager.In(false);
        }
    }
    
    public static void Out()
    {
        Instance._Out();
    }

    private void _Out()
    {
        SoundManager.PlaySE(SE.UI_PANELCLOSE);
        
        anim.Play("Out");

        coinButton.onClick.RemoveAllListeners();
        adsButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
        
        SystemManager.excludeButton = true;
        
        Result.RemoveListener(Out);
    }

    public void AnimationCompleted()
    {
        SystemManager.excludeButton = false;
    }
}
