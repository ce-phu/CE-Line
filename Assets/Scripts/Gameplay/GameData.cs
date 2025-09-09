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


public class MessageData
{
    //	お問い合わせメール
    public static string contactSubject = "ポイ活カラーブロック - サポート問い合わせ";

    public static string contactContent =
        "■お問い合わせ内容を記載してください\n\n\n\n\n\n-----------------------------------------\n※不具合等の調査のため、可能な限りゲーム画面のスクリーンショットの添付をお願いいたします。\n※楽天ポイントギフトコードの使用有無に関するお問い合わせの場合は、該当する楽天ポイントギフトコードも合わせてお知らせください。\n※お問い合わせへの回答をさせていただくため、@fancsglobal.comからのメールを受信できるよう設定してください。\n※頂いたメールアドレスは、お問い合わせ対応でのみ利用いたします。\n-----------------------------------------\n以下の情報はお問い合わせ内容の調査に必要なため、削除せずにそのまま送信してください\n-----------------------------------------\n・アプリ名[ ポイ活カラーブロック ]\n・お使いの機種名[ {0} ]\n・Androidバージョン[ {1} ]\n・アプリバージョン[ {2} ]\n[ {3} ]\n-----------------------------------------\n\n";



    //	アイテムゲット
    public static string useItemTitle = "Watch the commercial";
    public static string useItemContent = "Would you like to watch the commercial and receive?";
    public static string useItemButton = "Watch";

    //	アイテム名
    public static string[] itemName =
    {
        "「時間停止」",
        "「ロケット」",
        "「ハンマー」",
        "「バキューム」",
    };

    //	アイテム交換確認
    public static string buyItemTitle = "アイテム交換";
    public static string buyItemContent = "ポイントをアイテムに交換しますか？";
    
    

    //	利用規約誘導
    public static string newPrivacyPolicyTitle = "利用規約";

    public static string newPrivacyPolicyContent =
        "<color=#00ffffff>ポイ活カラーブロックを利用する為には<u><link={0}>利用規約</link></u>への同意および<u><link={1}>プライバシーポリシー</link></u>の確認が必要です。</color>\nアプリを利用する為に「次へ」をクリックしてください。";

    public static string newPrivacyPolicyButton = "次へ";

    //	スタートダッシュボーナス
    public static string getStartDushBonusTitle = "スタートダッシュボーナス！！";
    public static string getStartDushBonusContent = "ポイントを獲得しました！";

    //	ルーレットボタン
    public static string[] rouletteButtonText =
    {
        "<size=40>ガチャを開始</size><br><color=#ffff00ff>１日１回<color=#ff0000ff>無料</color>",
        "　　  {0}コインでまわす！<br><color=#ffff00ff>　　 本日あと{1}回</color>",
        "<color=#ff0000ff>　　     {0}コイン<color=#bbbbbbff>でまわす！<br>　　   本日あと{1}回</color>",
        "<color=#bbbbbbff>本日の分は終了しました。<br>また明日挑戦してね！</color>",
    };

    //	ルーレットポップアップタイトル
    public static string[] roulettePopupTitleText =
    {
        "ポイントゲット",
        "コインゲット",
        "スターゲット",
        "アイテムゲット",
    };


    
    public static string[] itemDescription =
    {
        "For a certain period of time\nthe remaining time will not decrease.",
        "You can only move in\nthe direction of the arrow.",
        "The rocket will destroy one of the selected boxes",
        "You need to erase the base color and the top color in order.",
        "Hammer destroys one selected box.\nIf you select Frozen Block,\nthe defrosting step will proceed one step.",
        "You cannot move it until you have erased\nthe other blocks the number of times shown.",
        "Starred blocks can only be removed\nfrom starred gates.",
        "Once you have removed the required number of key blocks\nyou can remove the lock and move it.",
        "Connected blocks will\nmove together when dragged."
    };
    
    // Tutorial item description 

    public static string[] titleTuto    = {
        
    /* Stage 06 */    "Time Stop",
    /* Stage 07 */    "Arrow Box",
    /* Stage 11 */    "Rocket",
    /* Stage 15 */    "Two-color Box",
    /* Stage 21 */    "Hammer",
    /* Stage 26 */    "Frozen Box",
    /* Stage 35 */    "Star Box",
    /* Stage 44 */    "Key Box",
    /* Stage 52 */    "Connecting Boxes",
    };
    
    
    public static class TimeUI
    {
        public static string InfiniteTitle = "NO TIME LIMIT";
        public static string InfiniteDescription = "No time limit in this level!";
        public static string NormalTitle = "OUT OF TIME";
        public static string NormalDescription = "Get another one to keep playing";
        public static string AdsButton = "FREE";
    }

    public static class SettingUI
    {
        public static string Title = "SETTING";
        public static string HelpButton = "Privacy Policy";
        public static string GiftCodeButton = "Term of Use";
    }

    public static class ShopUI
    {
        public static string Title = "SHOP";
    }

    public static class ResultUI
    {
        public static string LoseTitle = "YOU FAILED!";
        public static string LoseButton = "Let's retry!";
        public static string WinTitle = "LEVEL COMPLETED!";
        public static string ClaimButton = "Claim";
        public static string Claimx2Button = "Claim x2";
        public static string LoseBackButton = "Go home";
        public static string WinBackButton = "Go home and receive";
    }

    public static class LiveUI
    {
        public static string Title = "REFILL YOUR LIVES";
        public static string Description = "Get another one to keep playing";
        public static string AdsButton = "FREE";
    }

    public static class ReplayUI
    {
        public static string Title = "ARE YOU SURE?";
        public static string ReplayDescription = "You will lose a heart by replay!";
        public static string GoToHomeDescription = "You will lose a heart by go to home!";
        public static string ReplayButton = "GIVE UP";
        public static string GoToHomeButton = "GO TO HOME";
    }

    public static class ItemPopupUI
    {
        public static string Title = "ITEM RECEIVED";
        public static string Description = "You just receive an item!";
    }

    public static class UseItemUI
    {
        public static string Title = "WATCH THE COMMERCIAL";
        public static string Description = "Would you like to watch an advertise to use item?";
        public static string AdsButton = "WATCH ADS";
    }

    public static class LuckyWheelUI
    {
        public static string AdsButton = "Watch Ads";
        public static string DisableButton = "Enable After 12:00AM";
        public static string SpinningButton = "Spinning...";
    }

    public static class HomeUI
    {
        public static string PlayButton = "PLAY";
        public static string LuckyButton = "Lucky Spin";
        public static string ShopButton = "Shop";
    }
}


[Serializable]
public class LevelData
{
    public int[,] size = new int[9, 7];
    public int row = 0;
    public int column = 0;

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
