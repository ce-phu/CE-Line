using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public readonly string dataPath = "Assets/LevelData/LevelData.json";

    public int gold = 0;
    public int lastGold = 0;
    public int lives = 5;
    public bool isFreeSpin = true;
    public long lastFreeSpin;
    public int totalStar = 0;

    public int currentStage = 1;
    public int[] starArchived = new int[300];
    public bool[] levelUnlocked = new bool[10];
}

public enum ShopItemTypes
{
    TIMESTOP,
    BOMB,
    HAMMER,
    BUNDLE,
}

public enum ItemType
{
    NONE,
    GOLD,
    LIVES,
    INFLIVES,
    TIMESTOP,
    BOMB,
    HAMMER,
    TIME,
    INFTIME,
    BUNDLE,
}

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private PlayerData data;
    public static PlayerData Data => Instance.data;

    public static readonly int SHOP_TIMESTOP_PRICE = 500;
    public static readonly int SHOP_BOMB_PRICE = 500;
    public static readonly int SHOP_HAMMER_PRICE = 500;
    public static readonly int SHOP_BUNDLE_PRICE = 700;
    public static readonly int SHOP_TIMESTOP_QUANTITY = 5;
    public static readonly int SHOP_BOMB_QUANTITY = 5;
    public static readonly int SHOP_HAMMER_QUANTITY = 5;
    public static readonly int SHOP_BUNDLE_QUANTITY = 3;

    public static readonly int LUCKY_SPIN_PRICE = 20;

    public static readonly int INSTRUCTION_PRICE = 100;

    public static readonly int RESULT_WIN_PRICE_SUPEREASY = 5;
    public static readonly int RESULT_WIN_PRICE_EASY = 10;
    public static readonly int RESULT_WIN_PRICE_NORMAL = 20;
    public static readonly int RESULT_WIN_PRICE_HARD = 30;
    public static readonly int RESULT_WIN_PRICE_EXTREMELYHARD = 50;

    public static readonly int TOTAL_STAR_PER_LEVEL = 300;
    public static readonly int TOTAL_STAR_PER_STAGE = 3;
    public static readonly int TOTAL_STAGE_PER_LEVEL = 100;

    public static readonly float TIME_TO_SOLVE_PER_CELL = 2f;

    public static readonly int LEVEL_UNLOCK_PRICE = 500;

    public System.Action OnGoldChanged;
    [field: SerializeField] public bool IsDataLoaded { get; private set; }


    public void LoadData()
    {
        data = SaveDataManager.data.playerData;
        IsDataLoaded = true;
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public static void Proc()
    {
        Instance._Proc();
    }

    void _Proc()
    {
        // LifeTimeManager.Proc();
    }

    public bool HasEnoughGold(int amount)
    {
        return data.gold >= amount;
    }

    public void UseGold(int amount, bool isInvokeCalled = true)
    {
        if (HasEnoughGold(amount) == false) return;
        data.gold -= amount;
        if (isInvokeCalled) OnGoldChanged?.Invoke();
    }

    public void AddGold(int amount, bool isInvokeCalled = true)
    {
        UpdateLastGold();
        data.gold += amount;
        if (isInvokeCalled) OnGoldChanged?.Invoke();
    }

    public bool HasFirstSpin()
    {
        if (DateTime.Now.Date <= DateTime.FromBinary(data.lastFreeSpin)) return false;
        data.isFreeSpin = true;
        return true;
    }

    public void UseSpin()
    {
        data.isFreeSpin = false;
        data.lastFreeSpin = DateTime.Now.ToBinary();
    }

    public void UpdateLastGold()
    {
        data.lastGold = data.gold;
    }

    public void ArchivedStar(int amount)
    {
        data.starArchived[data.currentStage] = amount;
        data.totalStar += amount;
    }

    public bool CheckUnlockedLevel(int index)
    {
        int group = (index - 1) / TOTAL_STAGE_PER_LEVEL;
        int lowIndex = group * TOTAL_STAGE_PER_LEVEL + 1;
        int highIndex = (group + 1) * TOTAL_STAGE_PER_LEVEL;

        Debug.Log("\\");

        if (!data.levelUnlocked[group + 1])
        {
            for (int i = lowIndex; i <= highIndex; i++)
            {
                Debug.Log(i);
                if (data.starArchived[i - 100] == 0) return false;
            }
        }

        return true;
    }
}