using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerLevelText;
    [SerializeField] private TextMeshProUGUI levelStarText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject levelItemPrefabs;
    [SerializeField] private GameObject stageItemPrefabs;

    [SerializeField] private Transform levelItemContainer;
    [SerializeField] private Transform stageItemContainer;
    [SerializeField] private GameObject levelDisplay;
    [SerializeField] private GameObject stageDisplay;

    [SerializeField] private Button backButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private Animator animator;

    public static LevelUIManager Instance;

    private bool isShowLevel = true;

    private bool isAnimationCompleted = false;
    
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
        isAnimationCompleted = false;
        animator.Play("In");

        backButton.onClick.AddListener(OnClick_BackButton);
        closeButton.onClick.AddListener(OnClick_CloseButton);
        Player.Instance.OnGoldChanged += () => { coinText.text = Player.Data.gold.ToString(); };

        SetHeaderText();
        ShowLevel();
    }

    public static void SetHeaderText(string headerLevelText = "", string levelStarText = "")
    {
        Instance._SetHeaderText(headerLevelText, levelStarText);
    }

    private void _SetHeaderText(string headerLevelText = "", string levelStarText = "")
    {
        if (headerLevelText == "" && levelStarText == "")
        {
            this.headerLevelText.text = "Levels";
            this.levelStarText.text = Player.Data.totalStar + "/" + GameManager.GetTotalStar();
            coinText.text = Player.Data.gold.ToString();
        }
        else
        {
            this.headerLevelText.text = headerLevelText;
            this.levelStarText.text = levelStarText;
            coinText.text = Player.Data.gold.ToString();
        }
    }

    private void ShowLevel()
    {
        SystemManager.excludeButton = false;

        ClearPrefabs();
        backButton.gameObject.SetActive(false);
        isShowLevel = true;
        levelDisplay.SetActive(true);
        stageDisplay.SetActive(false);

        SetHeaderText();

        List<LevelData> masterLevel = GameManager.GetMasterLevel();

        for (int i = 1;
             i < (int)(masterLevel.Count / Player.TOTAL_STAGE_PER_LEVEL) * Player.TOTAL_STAGE_PER_LEVEL;
             i += Player.TOTAL_STAGE_PER_LEVEL)
        {
            LevelItemManager temp = Instantiate(levelItemPrefabs, levelItemContainer)
                .GetComponent<LevelItemManager>();
            temp.Setup(i, Player.TOTAL_STAR_PER_LEVEL, GetArchivedStar(i));
            // Debug.Log(i);
        }

        int GetArchivedStar(int firstIndex)
        {
            int total = 0;
            for (int i = firstIndex; i < firstIndex + Player.TOTAL_STAGE_PER_LEVEL; i++)
            {
                total += Player.Data.starArchived[i];
            }

            return total;
        }
    }

    public static void ShowStage(int firstIndex)
    {
        Instance._ShowStage(firstIndex);
    }

    private void _ShowStage(int _firstIndex)
    {
        SystemManager.excludeButton = false;

        ClearPrefabs();
        backButton.gameObject.SetActive(true);
        isShowLevel = false;
        levelDisplay.SetActive(false);
        stageDisplay.SetActive(true);

        List<LevelData> masterLevel = GameManager.GetMasterLevel();

        for (int i = _firstIndex; i < _firstIndex + Player.TOTAL_STAGE_PER_LEVEL; i++)
        {
            StageItemManager temp = Instantiate(stageItemPrefabs, stageItemContainer)
                .GetComponent<StageItemManager>();
            if (Player.Data.starArchived[i] == 0)
            {
                temp.Setup(i, masterLevel[i].isPrize, Player.Data.starArchived[i],
                    (Player.Data.starArchived[i - 1] != 0) || (i == _firstIndex),
                    (Player.Data.starArchived[i - 1] != 0) || (i == _firstIndex));
            }
            else
            {
                temp.Setup(i, masterLevel[i].isPrize, Player.Data.starArchived[i], true, false);
            }
        }
    }

    private void ClearPrefabs()
    {
        foreach (Transform child in levelItemContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in stageItemContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnClick_BackButton()
    {
        if (SystemManager.excludeButton)
            return;
        SystemManager.excludeButton = true;

        ShowLevel();
        stageDisplay.SetActive(false);
        levelDisplay.SetActive(true);
    }

    private void OnClick_CloseButton()
    {
        if (SystemManager.excludeButton)
            return;
        SystemManager.excludeButton = true;
        
        Out();
    }

    public static void Out()
    {
        Instance._Out();
    }

    private void _Out()
    {
        backButton.onClick.RemoveAllListeners();
        isAnimationCompleted = false;
        animator.Play("Out");
    }

    public void CompletedAnimation()
    {
        isAnimationCompleted = true;
        SystemManager.excludeButton = false;
    }
}