using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class ItemBuyUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI cancelButtonText;
    [SerializeField] private TextMeshProUGUI buyButtonPriceText;
    [SerializeField] private GameObject iconsHolder;
    [SerializeField] private GameObject[] itemImage;
    [SerializeField] private TextMeshProUGUI[] itemText;
    [SerializeField] private GameObject shadow;
    [SerializeField] private Animator animator;
    [SerializeField] private Sprite disableBuyButton;
    [SerializeField] private Sprite enableBuyButton;
    [SerializeField] private Image buyButtonImage;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button cancelButton;

    public static ItemBuyUIManager Instance;

    private Action<bool> callback;

    private Color activeColor = Color.white;
    private Color inactiveColor = Color.red;

    private bool isAnimationCompleted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public static void In(Action<bool> callback, string description, int price, ItemType itemType = ItemType.NONE,
        int amount = 0)
    {
        Instance._In(callback, description, price, itemType, amount);
    }

    private void _In(Action<bool> _callback, string _description, int _price, ItemType _itemType = ItemType.NONE,
        int _amount = 0)
    {
        this.callback = _callback;

        isAnimationCompleted = false;
        animator.Play("In");

        titleText.text = "BUY ITEM";
        descriptionText.text = _description;

        if (_itemType == ItemType.NONE)
        {
            iconsHolder.SetActive(false);
            shadow.SetActive(false);
        }
        else
        {
            iconsHolder.SetActive(false);
            shadow.SetActive(false);

            SetupItem(_itemType, _amount);
        }

        cancelButtonText.text = "CANCEL";
        buyButtonPriceText.text = _price.ToString();

        buyButtonImage.sprite = Player.Instance.HasEnoughGold(_price) ? enableBuyButton : disableBuyButton;
        buyButtonPriceText.color = Player.Instance.HasEnoughGold(_price) ? activeColor : inactiveColor;

        cancelButton.onClick.AddListener(OnClick_CancelButton);
        if (Player.Instance.HasEnoughGold(_price))
        {
            buyButton.onClick.AddListener(OnClick_BuyButton);
        }
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

    private void OnClick_CancelButton()
    {
        if (SystemManager.excludeButton)
            return;
        SystemManager.excludeButton = true;

        callback?.Invoke(false);
        Out();
    }

    private void OnClick_BuyButton()
    {
        if (SystemManager.excludeButton)
            return;
        SystemManager.excludeButton = true;

        callback?.Invoke(true);
        Out();
    }

    public static void Out()
    {
        Instance._Out();
    }

    private void _Out()
    {
        isAnimationCompleted = false;
        animator.Play("Out");
    }

    public void AnimationCompleted()
    {
        isAnimationCompleted = true;
        SystemManager.excludeButton = false;
    }
}