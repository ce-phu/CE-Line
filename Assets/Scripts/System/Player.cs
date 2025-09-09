using System;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int gold = 0;
    public int lastGold = 0;
    public int lives = 5;
    public bool isFreeSpin = true;
    public long lastFreeSpin;
    public int timeSpin = 0;
    
    public float liveInfTimer = 0f;
    public float timeInfTimer = 0;
    
    public int currentStage = 1;
}

public enum PowerupTypes
{
    TIMESTOP,
    BOMB,
    HAMMER,
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
    
    public static readonly int MAX_LIVES = 5;
    public static readonly int GOLD_PER_LIVE = 100;
    
    public static readonly int SHOP_TIMESTOP_PRICE = 500;
    public static readonly int SHOP_BOMB_PRICE = 500;
    public static readonly int SHOP_HAMMER_PRICE = 500;
    public static readonly int SHOP_BUNDLE_PRICE = 700;
    public static readonly int SHOP_TIMESTOP_QUANTITY = 5;
    public static readonly int SHOP_BOMB_QUANTITY = 5;
    public static readonly int SHOP_HAMMER_QUANTITY = 5;
    public static readonly int SHOP_BUNDLE_QUANTITY = 3;

    public static readonly int LUCKY_SPIN_PRICE = 100;
    public static readonly int LUCKY_MAXSPIN = 5;

    public static readonly int TIME_RECOVER_PRICE = 100;
    public static readonly int TIME_INFTIME_PRICE = 1000;
    
    public static readonly int RESULT_WIN_PRICE = 60;
    
    private const float LIVE_REGEN_INTERVAL_SECONDS = 1800f; // 30 minutes
    
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


    public static void Proc( )
    {
        Instance._Proc(  );
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



    public void UseLive(int amount)
    {
        if( data.liveInfTimer > 0 ) return;
        if( data.lives        < amount ) return;

        data.lives -= amount;
        data.lives = Mathf.Clamp(data.lives, 0, 5);
    }

    public void AddLive(int amount)
    {
        data.lives += amount;
        data.lives = Mathf.Min(data.lives, MAX_LIVES);
    }

    public void InfLive(float minute)
    {
        data.liveInfTimer = minute * 60;
        // LifeTimeManager.SetLifeInf( true );
    }

    public void InfTime(float minute)
    {
        data.timeInfTimer = minute * 60;
        // LifeTimeManager.SetTimeInf( true );
    }

    public void AddPowerup(PowerupTypes powerupTypes, int amount)
    {
        switch (powerupTypes)
        {
            case PowerupTypes.TIMESTOP:
                SaveDataManager.data.currentItem0 += amount;
                break;
            case PowerupTypes.BOMB:
                SaveDataManager.data.currentItem1 += amount;
                break;
            case PowerupTypes.HAMMER:
                SaveDataManager.data.currentItem2 += amount;
                break;
            default:
                break;
        }
    }

    public bool HasFirstSpin()
    {
        if (DateTime.Now.Date <= DateTime.FromBinary(data.lastFreeSpin)) return false;
        data.isFreeSpin = true;
        return true;
    }

    public void UseSpin()
    {
        if (data.isFreeSpin)
        {
            data.timeSpin++;
            data.isFreeSpin = false;
            data.lastFreeSpin = DateTime.Now.ToBinary();
        }
        else
        {
            data.timeSpin++;
        }
    }

    public void UpdateLastGold()
    {
        data.lastGold = data.gold;
    }
}