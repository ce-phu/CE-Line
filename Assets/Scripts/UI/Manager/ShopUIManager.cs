using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    public static ShopUIManager Instance;

    [SerializeField] private TextMeshProUGUI titleText;
    
    [SerializeField] private Button closeButton;
    
    [SerializeField] private Animator animator;
    
    [SerializeField] private Button addCoinButton;
    [SerializeField] private Button subtractCoinButton;

    [SerializeField] private ShopItemController[] shopItems;

    private bool isCompleteAnimation = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public static void In()
    {
        Instance._In();
    }
    
    private void _In()
    {
        SoundManager.PlaySE(SE.UI_PANELSHOW);
        
        titleText.text = MessageData.ShopUI.Title;
        
        animator.Play("In");
        
        closeButton.onClick.AddListener(OnClick_CloseButton);
        
        //test
        addCoinButton.onClick.AddListener((() =>
        {
            Player.Instance.AddGold(30);
        }));
        
        subtractCoinButton.onClick.AddListener(() =>
        {
            Player.Instance.UseGold(30);
        });
    }

    public static void Out()
    {
        Instance._Out();
    }
    
    private void _Out()
    {
        SoundManager.PlaySE(SE.UI_PANELCLOSE);
        
        animator.Play("Out");
        
        closeButton.onClick.RemoveAllListeners();
        
        addCoinButton.onClick.RemoveAllListeners();
        subtractCoinButton.onClick.RemoveAllListeners();
    }

    private void OnClick_CloseButton()
    {
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;        
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_CLOSEBTNCLICK );
        
        Out();
    }

    public void CompleteAnimation()
    {
        isCompleteAnimation = true;
        SystemManager.excludeButton = false;
    }

    public static void UpdateShopItem()
    {
        foreach (var item in Instance.shopItems)
        {
            item.SetupEnable();
        }
    }
}
