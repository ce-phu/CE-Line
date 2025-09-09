using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[System.Serializable]
public class WheelSegment
{
    public ItemType type;
    public int amount;
    public int weight;
}


public class LuckyWheelUI : MonoBehaviour
{
    public static LuckyWheelUI Instance;
    
    [SerializeField] private Animator animator;
    [SerializeField] private bool isOpen;
    public bool IsOpen => isOpen;
    
    [Header("Wheel Configuration")]
    [SerializeField] private Transform wheelTransform;
    [SerializeField] private WheelSegment[] segments;
    [SerializeField] private float spinDuration = 4f;
    [SerializeField] private int fullSpins = 6;
    
    [Header("Buttons")] 
    [SerializeField] private Button freeSpinButton;
    [SerializeField] private Button coinSpinButton;
    [SerializeField] private Button adsSpinButton;
    [SerializeField] private TextMeshProUGUI adsSpinButtonText;
    [SerializeField] private Button disableSpinButton;
    [SerializeField] private TextMeshProUGUI disableSpinButtonText;
    [SerializeField] private TextMeshProUGUI spinningButtonText;
    [SerializeField] private Button closeButton;

    [SerializeField] private TextMeshProUGUI coinSpinText;
    [SerializeField] private TextMeshProUGUI disableSpinText;

    [Header("Text")] [SerializeField] private TextMeshProUGUI[] wheelItemText;
    
    private bool isSpinning = false;

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
        
        foreach (var itemText in wheelItemText)
        {
            itemText.gameObject.SetActive(false);
            itemText.color = new Color(1, 1, 1, 0);
        }
        
        animator.Play("In");
        isOpen = true;
        
        freeSpinButton.onClick.AddListener(OnClick_FreeSpin);
        coinSpinButton.onClick.AddListener(OnClick_CoinSpin);
        adsSpinButton.onClick.AddListener(OnClick_AdsSpin);
        closeButton.onClick.AddListener(OnClick_CloseButton);
        
        SetupButton();

        SystemManager.excludeButton = true;
    }

    public void InFinish()
    {
        
    }

    public static void Out()
    {
        Instance._Out();        
    }
    
    private void _Out()
    {
        SoundManager.PlaySE(SE.UI_PANELCLOSE);
        animator.Play("Out");
        isOpen = false;
        
        freeSpinButton.onClick.RemoveAllListeners();
        coinSpinButton.onClick.RemoveAllListeners();
        adsSpinButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();

        SystemManager.excludeButton = true;
    }

    public void OutFinish()
    {
        
    }

    private void SetupButton()
    {
        if (Player.Instance.HasFirstSpin())
        {
            freeSpinButton.gameObject.SetActive(true);
            coinSpinButton.gameObject.SetActive(false);
            adsSpinButton.gameObject.SetActive(false);
            disableSpinButton.gameObject.SetActive(false);
        }
        else
        {
            coinSpinText.text = Player.LUCKY_SPIN_PRICE.ToString();

            if (Player.Data.timeSpin < Player.LUCKY_MAXSPIN)
            {
                if (Player.Instance.HasEnoughGold(Player.LUCKY_SPIN_PRICE))
                {
                    freeSpinButton.gameObject.SetActive(false);
                    coinSpinButton.gameObject.SetActive(true);
                    adsSpinButton.gameObject.SetActive(false);
                    disableSpinButton.gameObject.SetActive(false);
                }
                else
                {
                    freeSpinButton.gameObject.SetActive(false);
                    coinSpinButton.gameObject.SetActive(false);
                    adsSpinButton.gameObject.SetActive(true);
                    disableSpinButton.gameObject.SetActive(false);
                    // adsSpinButtonText.text = MessageData.LuckyWheelUI.AdsButton;
                }
            }
            else
            {
                freeSpinButton.gameObject.SetActive(false);
                coinSpinButton.gameObject.SetActive(false);
                adsSpinButton.gameObject.SetActive(false);
                disableSpinButton.gameObject.SetActive(true);
                // disableSpinButtonText.text = MessageData.LuckyWheelUI.DisableButton;
            }
        }
    }

    private void OnClick_FreeSpin()
    {
        if (isSpinning) return;
        
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;        
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        Player.Instance.UseSpin();

        // spinningButtonText.text = MessageData.LuckyWheelUI.SpinningButton;
        animator.Play("SpinningButtonIn");
        
        Spin();
    }

    private void OnClick_CoinSpin()
    {
        if (isSpinning) return;
        
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;        
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        Player.Instance.UseGold(Player.LUCKY_SPIN_PRICE);
        Player.Instance.UseSpin();
        
        animator.Play("SpinningButtonIn");
        
        Spin();
    }

    private void OnClick_AdsSpin()
    {
        if (isSpinning) return;
        
        if (SystemManager.excludeButton) return;
        SystemManager.excludeButton = true;        
        
        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        Player.Instance.UseSpin();
        
        animator.Play("SpinningButtonIn");

        Spin();
    }
    
    /// <summary>
    /// Attempts to spin the wheel, checking costs first.
    /// </summary>
    /// 
    private void Spin()
    {
        SoundManager.PlaySE(SE.SPIN_START);
        if (isSpinning || segments.Length == 0) return;

        isSpinning = true;
        coinSpinButton.interactable = false;
        adsSpinButton.interactable = false;

        int randomIndex = Random.Range(0, segments.Length);
        
        // HideText();
        StartCoroutine(SpinTheWheelCoroutine(GetRandomItem()));
    }

    private int GetRandomItem()
    {
        int totalWeight = 0;
        foreach (var item in segments)
        {
            totalWeight += item.weight;
        }
        
        int randomNumber = Random.Range(0, totalWeight);
        int sum = 0;
        
        for (int i = 0; i < segments.Length; i++)
        {
            sum += segments[i].weight;
            if (randomNumber < sum)
            {
                return i;
            }
        }

        return 0;
    }

    private IEnumerator SpinTheWheelCoroutine(int segmentIndex)
    {
        SoundManager.PlaySE( SE.SPIN_SPINNING, 1, true);
        
        float segmentAngle = 360f / segments.Length;
        // float randomOffset = Random.Range(-segmentAngle / 2 * 0.8f, segmentAngle / 2 * 0.8f);
        float randomOffset = Random.Range(-40f, -5f);
        // Debug.Log(randomOffset);
        float targetAngle = (segmentIndex * segmentAngle + randomOffset);
        float fullRotations = fullSpins * 360f;
        float finalAngle = (targetAngle - fullRotations);

        float timer = 0f;
        float startAngle = wheelTransform.eulerAngles.z;

        while (timer < spinDuration)
        {
            timer += Time.deltaTime;

            float t = 1 - Mathf.Pow(1 - timer / spinDuration, 4);

            float currentAngle = Mathf.LerpUnclamped(startAngle, finalAngle, t);
            wheelTransform.eulerAngles = new Vector3(0, 0, currentAngle);
            
            if (timer >= spinDuration / 3)
            {
                SoundManager.StopSE(SE.SPIN_SPINNING);
            }
            
            yield return null;
        }

        wheelTransform.eulerAngles = new Vector3(0, 0, targetAngle);

        // yield return new WaitForSeconds(0.5f);
        
        // ShowText();        
        GrantReward(segments[segmentIndex]);
        isSpinning = false;
        coinSpinButton.interactable = true;
        adsSpinButton.interactable = true;

        SystemManager.excludeButton = false;
    }

    private void GrantReward(WheelSegment reward)
    {
        SoundManager.PlaySE( SE.SPIN_PRIZE );
        
        switch (reward.type)
        {
            case ItemType.GOLD:
                Player.Instance.AddGold(reward.amount);
                ItemPopupUI.In(ItemType.GOLD, reward.amount);
                break;
            case ItemType.INFLIVES:
                Player.Instance.InfLive(reward.amount);
                ItemPopupUI.In(ItemType.INFLIVES, reward.amount);
                break;
            case ItemType.INFTIME:
                Player.Instance.InfTime(reward.amount);
                ItemPopupUI.In(ItemType.INFTIME, reward.amount);
                break;
            case ItemType.TIMESTOP:
                Player.Instance.AddPowerup(PowerupTypes.TIMESTOP, 1);
                ItemPopupUI.In(ItemType.TIMESTOP, 1);
                break;
            case ItemType.BOMB:
                Player.Instance.AddPowerup(PowerupTypes.BOMB, 1);
                ItemPopupUI.In(ItemType.BOMB, 1);
                break;
            case ItemType.HAMMER:
                Player.Instance.AddPowerup(PowerupTypes.HAMMER, 1);
                ItemPopupUI.In(ItemType.HAMMER, 1);
                break;
        }
        
        animator.Play("SpinningButtonOut");
    }

    public void ShowText()
    {
        IEnumerator ShowTextCoroutine()
        {
            int i = 0;
            while (i < wheelItemText.Length && wheelItemText[i].color.a < .8f)
            {
                wheelItemText[i].gameObject.SetActive(true);
                
                float duration = .02f;
                float elapsedTime = 0f;

                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / duration;
                    wheelItemText[i].color = new Color(1, 1, 1,(float)Mathf.Lerp(0, 1, t));
                    yield return null;
                }       
                
                wheelItemText[i].color = Color.white;
                i++;
            }
        }

        StartCoroutine(ShowTextCoroutine());
    }

    public void HideText()
    {
        IEnumerator HideTextCoroutine()
        {
            int i = 0;
            while (i < wheelItemText.Length && wheelItemText[i].color.a > .2f)
            {
                Debug.Log("aa");
                wheelItemText[i].gameObject.SetActive(true);
                
                float duration = .1f;
                float elapsedTime = 0f;

                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / duration;
                    wheelItemText[i].color = new Color(1, 1, 1,(float)Mathf.Lerp(1, 0, t));
                    yield return null;
                }

                wheelItemText[i].color = new Color(1, 1, 1, 0);
                i++;
            }
        }

        StartCoroutine(HideTextCoroutine());
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