using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LevelItemManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI starText;
    [SerializeField] private Image levelImage;

    [SerializeField] private Button levelButton;

    private int level, totalStar, archivedStar;

    [SerializeField] private Sprite levelActiveSprite;
    [SerializeField] private Sprite levelInactiveSprite;

    public void Setup(int level, int totalStar, int archivedStar)
    {
        this.level = level;
        this.totalStar = totalStar;
        this.archivedStar = archivedStar;

        levelText.text = "Level " + (level / Player.TOTAL_STAGE_PER_LEVEL + 1);
        starText.text = archivedStar + "/" + totalStar;

        if (Player.Instance.CheckUnlockedLevel(level))
        {
            levelButton.onClick.RemoveAllListeners();
            levelButton.onClick.AddListener(OnClick_LevelButton);
        }
        else
        {
            levelButton.onClick.RemoveAllListeners();
            levelButton.onClick.AddListener(OnClick_UnlockLevelButton);
        }

        levelImage.sprite = Player.Instance.CheckUnlockedLevel(level) ? levelActiveSprite : levelInactiveSprite;
    }

    private void OnClick_LevelButton()
    {
        if (SystemManager.excludeButton)
            return;
        SystemManager.excludeButton = true;
        
        LevelUIManager.SetHeaderText("Level " + (level / Player.TOTAL_STAGE_PER_LEVEL + 1),
            archivedStar + "/" + totalStar);
        LevelUIManager.ShowStage(level);
    }

    private void OnClick_UnlockLevelButton()
    {
        if (SystemManager.excludeButton)
            return;
        SystemManager.excludeButton = true;
        
        string description = "Do you want to unlock this level?";
        ItemBuyUIManager.In(UnlockLevel, description, Player.LEVEL_UNLOCK_PRICE);
    }

    private void UnlockLevel(bool isUnlocked)
    {
        if (isUnlocked)
        {
            Player.Data.levelUnlocked[level / Player.TOTAL_STAGE_PER_LEVEL + 1] = true;
            LevelUIManager.ShowStage(level);
            Player.Instance.AddGold(-Player.LEVEL_UNLOCK_PRICE);
        }
    }
}