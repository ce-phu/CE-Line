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
    [SerializeField] private TextMeshProUGUI coinGoHome;

    [SerializeField] private Button claimBtn;
    [SerializeField] private Button claimx2Btn;
    [SerializeField] private Button retryBtn;
    [SerializeField] private Button backHomeWinBtn;
    [SerializeField] private Button backHomeLoseBtn;

    [Header("LoseUI")] [SerializeField] private TextMeshProUGUI loseTitleText;
    [SerializeField] private TextMeshProUGUI loseButtonText;
    [SerializeField] private TextMeshProUGUI loseBackButtonText;

    [Header("WinUI")] [SerializeField] private TextMeshProUGUI winTitleText;
    [SerializeField] private TextMeshProUGUI winBackButtonText;
    [SerializeField] private TextMeshProUGUI claimButtonText;
    [SerializeField] private TextMeshProUGUI claimButtonx2Text;
    private bool isCompletedAnimation = false;


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

        switch (GameManager.GetDifficulty())
        {
            case 0:
            {
                coinGoHome.text = Player.RESULT_WIN_PRICE_SUPEREASY.ToString();
                coinText.text = Player.RESULT_WIN_PRICE_SUPEREASY.ToString();
                coinx2Text.text = (Player.RESULT_WIN_PRICE_SUPEREASY * 2).ToString();
                break;
            }
            case 1:
            {
                coinGoHome.text = Player.RESULT_WIN_PRICE_EASY.ToString();
                coinText.text = Player.RESULT_WIN_PRICE_EASY.ToString();
                coinx2Text.text = (Player.RESULT_WIN_PRICE_EASY * 2).ToString();
                break;
            }
            case 2:
            {
                coinGoHome.text = Player.RESULT_WIN_PRICE_NORMAL.ToString();
                coinText.text = Player.RESULT_WIN_PRICE_NORMAL.ToString();
                coinx2Text.text = (Player.RESULT_WIN_PRICE_NORMAL * 2).ToString();
                break;
            }
            case 3:
            {
                coinGoHome.text = Player.RESULT_WIN_PRICE_HARD.ToString();
                coinText.text = Player.RESULT_WIN_PRICE_HARD.ToString();
                coinx2Text.text = (Player.RESULT_WIN_PRICE_HARD * 2).ToString();
                break;
            }
            case 4:
            {
                coinGoHome.text = Player.RESULT_WIN_PRICE_EXTREMELYHARD.ToString();
                coinText.text = Player.RESULT_WIN_PRICE_EXTREMELYHARD.ToString();
                coinx2Text.text = (Player.RESULT_WIN_PRICE_EXTREMELYHARD * 2).ToString();
                break;
            }
            default:
                break;
        }


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

        animator.Play("Def");

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
        MessageManager.GetStringData(MessageIndex._RESULT_LOSE_TITLE, loseTitleText);
        MessageManager.GetStringData(MessageIndex._RESULT_LOSE_BUTTON, loseButtonText);
        MessageManager.GetStringData(MessageIndex._RESULT_WIN_TITLE, winTitleText);
        MessageManager.GetStringData(MessageIndex._RESULT_CLAIM_BUTTON, claimButtonText);
        MessageManager.GetStringData(MessageIndex._RESULT_CLAIMX2_BUTTON, claimButtonx2Text);
        MessageManager.GetStringData(MessageIndex._RESULT_LOSEBACK_BUTTON, loseBackButtonText);
        MessageManager.GetStringData(MessageIndex._RESULT_WINBACK_BUTTON, winBackButtonText);
        LayoutRebuilder.ForceRebuildLayoutImmediate(winBackButtonText.transform.parent as RectTransform);
    }

    void Claim()
    {
        if (SystemManager.excludeButton)
            return;
        SystemManager.excludeButton = true;

        VibrationManager.VibrateTap();
        SoundManager.PlaySE(SE.UI_BTNCLICK);

        switch (GameManager.GetDifficulty())
        {
            case 0:
            {
                Player.Instance.AddGold(Player.RESULT_WIN_PRICE_SUPEREASY, false);
                break;
            }
            case 1:
            {
                Player.Instance.AddGold(Player.RESULT_WIN_PRICE_EASY, false);
                break;
            }
            case 2:
            {
                Player.Instance.AddGold(Player.RESULT_WIN_PRICE_NORMAL, false);
                break;
            }
            case 3:
            {
                Player.Instance.AddGold(Player.RESULT_WIN_PRICE_HARD, false);
                break;
            }
            case 4:
            {
                Player.Instance.AddGold(Player.RESULT_WIN_PRICE_EXTREMELYHARD, false);
                break;
            }
            default:
                break;
        }

        Result.keepPlaying = true;
    }


    void Claimx2()
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
        if (SystemManager.excludeButton)
            return;
        SystemManager.excludeButton = true;

        VibrationManager.VibrateTap();
        SoundManager.PlaySE(SE.UI_BTNCLICK);

        switch (GameManager.GetDifficulty())
        {
            case 0:
            {
                Player.Instance.AddGold(Player.RESULT_WIN_PRICE_SUPEREASY * 2, false);
                break;
            }
            case 1:
            {
                Player.Instance.AddGold(Player.RESULT_WIN_PRICE_EASY * 2, false);
                break;
            }
            case 2:
            {
                Player.Instance.AddGold(Player.RESULT_WIN_PRICE_NORMAL * 2, false);
                break;
            }
            case 3:
            {
                Player.Instance.AddGold(Player.RESULT_WIN_PRICE_HARD * 2, false);
                break;
            }
            case 4:
            {
                Player.Instance.AddGold(Player.RESULT_WIN_PRICE_EXTREMELYHARD * 2, false);
                break;
            }
            default:
                break;
        }

        Result.keepPlaying = true;
    }

    void Retry()
    {
        if (SystemManager.excludeButton)
            return;
        SystemManager.excludeButton = true;

        VibrationManager.VibrateTap();
        SoundManager.PlaySE(SE.UI_BTNCLICK);

        Result.keepPlaying = true;
    }

    void BackToHomeLose()
    {
        if (SystemManager.excludeButton)
            return;
        SystemManager.excludeButton = true;

        VibrationManager.VibrateTap();
        SoundManager.PlaySE(SE.UI_BTNCLICK);

        Result.backToHome = true;
    }

    void BackToHomeWin()
    {
        if (SystemManager.excludeButton)
            return;
        SystemManager.excludeButton = true;

        VibrationManager.VibrateTap();
        SoundManager.PlaySE(SE.UI_BTNCLICK);
        
        switch (GameManager.GetDifficulty())
        {
            case 0:
            {
                Player.Instance.AddGold(Player.RESULT_WIN_PRICE_SUPEREASY, false);
                break;
            }
            case 1:
            {
                Player.Instance.AddGold(Player.RESULT_WIN_PRICE_EASY, false);
                break;
            }
            case 2:
            {
                Player.Instance.AddGold(Player.RESULT_WIN_PRICE_NORMAL, false);
                break;
            }
            case 3:
            {
                Player.Instance.AddGold(Player.RESULT_WIN_PRICE_HARD, false);
                break;
            }
            case 4:
            {
                Player.Instance.AddGold(Player.RESULT_WIN_PRICE_EXTREMELYHARD, false);
                break;
            }
            default:
                break;
        }
        // Player.Instance.AddLive(1);
        // HomeUIManager.SetCurrentHeart();

        Result.backToHome = true;
    }
}