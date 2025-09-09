using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
//using CE.Template.Manager;



public class ResultUIManager : MonoBehaviour
{
    public static ResultUIManager Instance;
    
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI coinx2Text;
    
    [SerializeField] private Button claimBtn;
    [SerializeField] private Button claimx2Btn;
    [SerializeField] private Button retryBtn;
    [SerializeField] private Button backHomeWinBtn;
    [SerializeField] private Button backHomeLoseBtn;

    [Header("LoseUI")]
    [SerializeField] private TextMeshProUGUI loseTitleText;
    [SerializeField] private TextMeshProUGUI loseButtonText;
    [SerializeField] private TextMeshProUGUI loseBackButtonText;

    [Header("WinUI")]
    [SerializeField] private TextMeshProUGUI winTitleText;
    [SerializeField] private TextMeshProUGUI winBackButtonText;
    [SerializeField] private TextMeshProUGUI claimButtonText;
    [SerializeField] private TextMeshProUGUI claimButtonx2Text;
    private bool  isCompletedAnimation = false;



    private void Awake()
    {
        Instance = this;
    }

    
    
    public static void In(bool isWin)
    {
        Instance._In(isWin);
    }

    private void _In(bool _isWin)
    {
        SetText();
            
        claimBtn.onClick.AddListener(Claim);
        claimx2Btn.onClick.AddListener(Claimx2);
        retryBtn.onClick.AddListener(Retry);
        backHomeWinBtn.onClick.AddListener(BackToHomeWin);
        backHomeLoseBtn.onClick.AddListener(BackToHomeLose);
        
        animator.Play(_isWin ? "Win_In" : "Lose_In");
        IngameUIManager.Out();
        SoundManager.PauseBGM();
        SoundManager.PlaySE(_isWin ? SE.LEVEL_CLEAR : SE.LEVEL_FAIL);

        coinText.text   = Player.RESULT_WIN_PRICE.ToString();
        coinx2Text.text = (Player.RESULT_WIN_PRICE * 2).ToString();

        isCompletedAnimation = false;
    }

    
    
    public static void Def()
    {
        Instance._Def();
    }

    private void _Def()
    {
        claimBtn.onClick.RemoveListener(Claim);
        claimx2Btn.onClick.RemoveListener(Claimx2);
        retryBtn.onClick.RemoveListener(Retry);
        backHomeWinBtn.onClick.RemoveListener(BackToHomeWin);
        backHomeLoseBtn.onClick.RemoveListener(BackToHomeLose);
        
        animator.Play( "Def" );

        isCompletedAnimation = false;
    }
    
    
    
    public static bool IsCompleteAnimation()
    {
        return Instance.isCompletedAnimation;
    }



    public void SetAnimationCompleted()
    {
        isCompletedAnimation = true;
    }

    private void SetText()
    {
        loseTitleText.text = MessageData.ResultUI.LoseTitle;
        loseButtonText.text = MessageData.ResultUI.LoseButton;
        winTitleText.text = MessageData.ResultUI.WinTitle;
        claimButtonText.text = MessageData.ResultUI.ClaimButton;
        claimButtonx2Text.text = MessageData.ResultUI.Claimx2Button;
        loseBackButtonText.text = MessageData.ResultUI.LoseBackButton;
        winBackButtonText.text = MessageData.ResultUI.WinBackButton;
    }

    void Claim( )
    {
        if( SystemManager.excludeButton )
            return;
        SystemManager.excludeButton = true;
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        Player.Instance.AddGold(60, false);
        Player.Instance.AddLive(1);
        Result.keepPlaying   = true;
    }
    
    
    
    void Claimx2( )
    {
        // AdManager.I.ShowRewarded( ( isSuccess ) =>
        // {
        //     if ( isSuccess ) {
        //
        //         SaveDataManager.data.currentCoin += 120;
        //         SaveDataManager.SaveData(  );
        //         Result.backToHome   = true;
        //     }
        // } );
        if( SystemManager.excludeButton )
            return;
        SystemManager.excludeButton = true;
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );

        Player.Instance.AddGold(120, false);
        Player.Instance.AddLive(1);
        Result.keepPlaying   = true;
    }

    void Retry()
    {
        if( SystemManager.excludeButton )
            return;
        SystemManager.excludeButton = true;
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );

        Result.keepPlaying   = true;
    }

    void BackToHomeLose( )
    { 
        if( SystemManager.excludeButton )
            return;
        SystemManager.excludeButton = true;
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        Result.backToHome   = true;
    }
    
    void BackToHomeWin( )
    { 
        if( SystemManager.excludeButton )
            return;
        SystemManager.excludeButton = true;
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        Player.Instance.AddGold(60, false);
        Player.Instance.AddLive(1);
        // HomeUIManager.SetCurrentHeart();
        
        Result.backToHome   = true;
    }
}