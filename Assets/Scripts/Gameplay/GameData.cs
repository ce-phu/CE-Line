using System;
using System.Collections.Generic;
using UnityEngine;



public class GameData : MonoBehaviour
{
    public static int termOfUseVersion = 0;
    public static int adsBonusRemain = 0;
    public static string privacyPolicyUrl = null;
    public static string termOfUseUrl = null;
    
    public static int interstitialLimit  = 0;
    public static int rewardedLimit      = 0;

    private static GameData instance;

    [SerializeField] private RectTransform rtCanvas;

    public static RectTransform RtCanvas => instance.rtCanvas;


    private void Awake()
    {
        instance = this;
    }
}

[Serializable]
public class RouletteField
{
    /*
    type:
        0: Point,
        1: Coin,
        2~: Items
     *
     */
    public int type = 0;
    public int amount = 0;
}


public class ConfigData
{
    public string contactAddress = "info_poikatsucolorblock@fancsglobal.com";
    public int timerSeconds = 60;
}

[Serializable]
public class LevelData
{
    public int[,] size = new int[9, 7];
    public int row = 0;
    public int column = 0;
    public int difficulty = 0;
    public bool isPrize = false;
    public float timeEstimated = 0;
    
    public class SolvedStep
    {
        public int row = 0;
        public int column = 0;
    }
    
    public List<SolvedStep> solvedSteps = new List<SolvedStep>();
}

[Serializable]
public class NetworkManagerDummyResponse
{
    public static string json = "{\"appVersion\":\"1.0.1\",\"account_id\":142,\"display_id\":\"1286433295\",\"pw\":\"fiEgGXNVTOvrEEPM\",\"os\":0,\"tester\":0,\"banned\":0,\"shadowbanned\":0,\"currentStage\":1,\"stageOrder\":[],\"chainCount\":0,\"currentPoint\":20,\"currentCoin\":0,\"currentStar\":0,\"currentItem0\":3,\"currentItem1\":3,\"currentItem2\":3,\"currentItem3\":3,\"addItem0\":0,\"addItem1\":0,\"addItem2\":0,\"addItem3\":0,\"currentPresent\":0,\"getAdditionalPoint\":0,\"getPresent\":0,\"announce\":[],\"maintenance\":{\"title\":\"\",\"content\":\"\"},\"stageLevelData\":\"\",\"enableReview\":1,\"adsBonusRemain\":0,\"bonusPoint\":0,\"shop\":[],\"buyContent\":0,\"codelist\":[],\"giftcodeerror\":0,\"getPointForLog\":0,\"usePointForLog\":0,\"expPointForLog\":0,\"rouletteCost\":100,\"rouletteFinalIndex\":0,\"rouletteAdsFinalIndex\":0,\"rouletteFields\":[],\"rouletteStatus\":0,\"rouletteAdsStatus\":0,\"rouletteLimit\":5,\"userDailyMission\":{},\"masterDailyMissions\":[],\"termOfUseVersion\":1,\"termOfUseUrl\":\"https://fancsglobal.com/app_terms/poikatsu\",\"privacyPolicyUrl\":\"https://fancsglobal.com/privacypolicy/policy_shogunstudios\",\"logMessage\":\"\",\"isError\":0,\"statusCode\":200,\"interstitialLimit\":1,\"rewardedLimit\":1,\"challengeBonusLimitStage\":10,\"challengeBonusAmount\":5,\"startDushBonus\":{\"startDushBonusList\":[{\"day\":1,\"amount\":10},{\"day\":2,\"amount\":20},{\"day\":3,\"amount\":30}],\"startDushBonusDay\":2,\"startDushBonusDelay\":20}, \"masterDailyMissions\":[{\"daily_mission_id\":1,\"daily_mission_type\":1,\"daily_mission_text\":\"アプリを起動する\",\"daily_mission_goal\":1,\"daily_mission_reward_type\":1,\"daily_mission_reward_amount\":1},{\"daily_mission_id\":2,\"daily_mission_type\":2,\"daily_mission_text\":\"広告を視聴する\",\"daily_mission_goal\":1,\"daily_mission_reward_type\":2,\"daily_mission_reward_amount\":50},{\"daily_mission_id\":3,\"daily_mission_type\":3,\"daily_mission_text\":\"1回クリアする\",\"daily_mission_goal\":1,\"daily_mission_reward_type\":1,\"daily_mission_reward_amount\":1},{\"daily_mission_id\":4,\"daily_mission_type\":3,\"daily_mission_text\":\"2回クリアする\",\"daily_mission_goal\":2,\"daily_mission_reward_type\":1,\"daily_mission_reward_amount\":1},{\"daily_mission_id\":5,\"daily_mission_type\":3,\"daily_mission_text\":\"3回クリアする\",\"daily_mission_goal\":3,\"daily_mission_reward_type\":1,\"daily_mission_reward_amount\":1},{\"daily_mission_id\":6,\"daily_mission_type\":3,\"daily_mission_text\":\"4回クリアする\",\"daily_mission_goal\":4,\"daily_mission_reward_type\":1,\"daily_mission_reward_amount\":1},{\"daily_mission_id\":7,\"daily_mission_type\":3,\"daily_mission_text\":\"5回クリアする\",\"daily_mission_goal\":5,\"daily_mission_reward_type\":1,\"daily_mission_reward_amount\":1},{\"daily_mission_id\":8,\"daily_mission_type\":4,\"daily_mission_text\":\"ブロックを15個消す\",\"daily_mission_goal\":15,\"daily_mission_reward_type\":1,\"daily_mission_reward_amount\":1},{\"daily_mission_id\":9,\"daily_mission_type\":4,\"daily_mission_text\":\"ブロックを20個消す\",\"daily_mission_goal\":20,\"daily_mission_reward_type\":1,\"daily_mission_reward_amount\":1},{\"daily_mission_id\":10,\"daily_mission_type\":4,\"daily_mission_text\":\"ブロックを25個消す\",\"daily_mission_goal\":25,\"daily_mission_reward_type\":1,\"daily_mission_reward_amount\":1},{\"daily_mission_id\":11,\"daily_mission_type\":4,\"daily_mission_text\":\"ブロックを30個消す\",\"daily_mission_goal\":30,\"daily_mission_reward_type\":1,\"daily_mission_reward_amount\":1},{\"daily_mission_id\":12,\"daily_mission_type\":5,\"daily_mission_text\":\"アイテムを1個使用する\",\"daily_mission_goal\":1,\"daily_mission_reward_type\":1,\"daily_mission_reward_amount\":1}]}";
}
