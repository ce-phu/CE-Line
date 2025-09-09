using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopupUI : MonoBehaviour
{
    public static ItemPopupUI Instance;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    
    [SerializeField] private Animator anim;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject[] itemImage;
    [SerializeField] private TextMeshProUGUI[] itemText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public static void In(ItemType itemType, int amount = 0)
    {
        Instance._In(itemType, amount);
    }

    private void _In(ItemType _itemType, int _amount = 0)
    {
        anim.Play("In");

        closeButton.onClick.AddListener(OnClick_CloseButton);
        SetupItem(_itemType, _amount);        
        
        SystemManager.excludeButton = true;
    }

    private void SetText()
    {
        // titleText.text = MessageData.ItemPopupUI.Title;
        // descriptionText.text = MessageData.ItemPopupUI.Description;
    }
    
    private void SetupItem(ItemType itemType, int amount = 0)
    {
        foreach (var item in itemImage)
        {
            item.gameObject.SetActive(false);
        }
        
        switch (itemType)
        {
            case ItemType.GOLD:
            {
                itemImage[7].gameObject.SetActive(true);
                itemText[7].gameObject.SetActive(true);
                itemText[7].text = "x" + amount.ToString();
                break;
            }
            case ItemType.LIVES:
            {
                itemImage[3].gameObject.SetActive(true);
                itemText[3].gameObject.SetActive(true);
                itemText[3].text = "x" + amount.ToString();
                break;
            }
            case ItemType.INFLIVES:
            {
                itemImage[4].gameObject.SetActive(true);
                itemText[4].gameObject.SetActive(true);
                itemText[4].text = amount.ToString() + "m";
                break;
            }
            case ItemType.TIMESTOP:
            {
                itemImage[0].gameObject.SetActive(true);
                itemText[0].gameObject.SetActive(true);
                itemText[0].text = "x" + amount.ToString();
                break;
            }
            case ItemType.BOMB:
            {
                itemImage[1].gameObject.SetActive(true);
                itemText[1].gameObject.SetActive(true);
                itemText[1].text = "x" + amount.ToString();
                break;
            }
            case ItemType.HAMMER:
            {
                itemImage[2].gameObject.SetActive(true);
                itemText[2].gameObject.SetActive(true);
                itemText[2].text = "x" + amount.ToString();
                break;
            }
            case ItemType.TIME:
            {
                itemImage[5].gameObject.SetActive(true);
                itemText[5].gameObject.SetActive(false); //No point of using this one
                itemText[5].text = amount.ToString();
                break;
            }
            case ItemType.INFTIME:
            {
                itemImage[6].gameObject.SetActive(true);
                itemText[6].gameObject.SetActive(true);
                itemText[6].text = amount + "m";
                break;
            }
            case ItemType.BUNDLE:
            {
                itemImage[0].gameObject.SetActive(true);
                itemText[0].gameObject.SetActive(true);
                itemText[0].text = "x" + amount.ToString();
                itemImage[1].gameObject.SetActive(true);
                itemText[1].gameObject.SetActive(true);
                itemText[1].text = "x" + amount.ToString();
                itemImage[2].gameObject.SetActive(true);
                itemText[2].gameObject.SetActive(true);
                itemText[2].text = "x" + amount.ToString();
                break;
            }
            default:
                break;
        }
    }

    public static void Out()
    {
        Instance._Out();
    }

    private void _Out()
    {
        anim.Play("Out");
        
        closeButton.onClick.RemoveAllListeners();
        
        SystemManager.excludeButton = true;
    }

    private void OnClick_CloseButton()
    {
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;        
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_CLOSEBTNCLICK );
        
        Out();
    }
    
    public void AnimationCompleted()
    {
        SystemManager.excludeButton = false;
    }
}
