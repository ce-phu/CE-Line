using System;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReplayUIManager : MonoBehaviour
{
    public enum ReplayUITypes
    {
        INSTRUCTION,
        GOTOHOME,
    }

    public static ReplayUIManager Instance;

    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private Animator anim;
    [SerializeField] private Button coinButton;
    [SerializeField] private Button goHomeButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI goHomeButtonText;

    [SerializeField] private Button adsButton;
    [SerializeField] private TextMeshProUGUI instructionCoinText;
    [SerializeField] private GameObject disablePanel;

    private bool isClose = false;

    private ReplayUITypes replayUIType;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public static void In(ReplayUITypes type)
    {
        Instance.replayUIType = type;

        Instance._In();
    }

    private void _In()
    {
        isClose = false;
        SoundManager.PlaySE(SE.UI_PANELSHOW);

        anim.Play("In");

        coinButton.onClick.AddListener(OnClick_ProcessButton);
        goHomeButton.onClick.AddListener(OnClick_ProcessButton);
        closeButton.onClick.AddListener(OnClick_CloseButton);
        adsButton.onClick.AddListener(OnClick_AdsButton);

        // Ingame.SetPause( true ); 
        Ingame.SetFrozen(true); //call this instead of SetPause, because SetPause will automatically call SettingUIManager.In()
        SystemManager.excludeButton = true;

        MessageManager.GetStringData(MessageIndex._REPLAY_TITLE, titleText);

        if (replayUIType == ReplayUITypes.INSTRUCTION)
        {
            coinButton.gameObject.SetActive(true);
            goHomeButton.gameObject.SetActive(false);
            MessageManager.GetStringData(MessageIndex._REPLAY_REPLAY_DESCRIPTION, descText);
            instructionCoinText.text = Player.INSTRUCTION_PRICE.ToString();

            adsButton.gameObject.SetActive(true);
            coinButton.interactable = Player.Instance.HasEnoughGold(Player.INSTRUCTION_PRICE);
            disablePanel.SetActive(!Player.Instance.HasEnoughGold(Player.INSTRUCTION_PRICE));
            instructionCoinText.color =
                Player.Instance.HasEnoughGold(Player.INSTRUCTION_PRICE) ? Color.white : Color.red;
        }
        else if (replayUIType == ReplayUITypes.GOTOHOME)
        {
            coinButton.gameObject.SetActive(false);
            goHomeButton.gameObject.SetActive(true);
            adsButton.gameObject.SetActive(false);
            disablePanel.SetActive(false);

            MessageManager.GetStringData(MessageIndex._REPLAY_GOTOHOME_DESCRIPTION, descText);
            MessageManager.GetStringData(MessageIndex._REPLAY_GOTOHOME_BUTTON, goHomeButtonText);
        }
        else
        {
            MessageManager.GetStringData(MessageIndex._REPLAY_REPLAY_DESCRIPTION, descText);
        }

        Result.AddListener(Out);
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
        goHomeButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
        adsButton.onClick.RemoveAllListeners();

        SystemManager.excludeButton = true;

        Result.RemoveListener(Out);
    }

    private void OnClick_ProcessButton()
    {
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;

        VibrationManager.VibrateTap();
        SoundManager.PlaySE(SE.UI_BTNCLICK);

        if (replayUIType == ReplayUITypes.INSTRUCTION)
        {
            Out();
            Player.Instance.UseGold(Player.INSTRUCTION_PRICE);
        }
        else if (replayUIType == ReplayUITypes.GOTOHOME)
        {
            Out();
            IngameUIManager.Out();
            Ingame.SetFrozen(false);
            Ingame.SetGoToHome(true);
        }
        else
        {
            Out();
            IngameUIManager.Out();
            Ingame.SetFrozen(false);
            Ingame.SetRetry(true);
        }
    }

    private void OnClick_AdsButton()
    {
        Out();
    }

    private void OnClick_CloseButton()
    {
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;

        isClose = true;

        VibrationManager.VibrateTap();
        SoundManager.PlaySE(SE.UI_CLOSEBTNCLICK);

        Out();
        Ingame.SetFrozen(false);
    }

    public void CompleteAnimation()
    {
        SystemManager.excludeButton = false;
    }

    public void CompleteOutAnimation()
    {
        if (!isClose && replayUIType == ReplayUITypes.INSTRUCTION)
        {
            GameManager.ShowInstructions();
        }
    }
}