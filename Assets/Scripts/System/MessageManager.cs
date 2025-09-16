using System;
using UnityEngine;
using TMPro;


public enum MessageIndex
{
    _DUMMY = 0,

    _FONTTYPE_0 = 1,
    _FONTTYPE_1,
    _FONTTYPE_2,
    _FONTTYPE_1_SIZE_55,


    _HOME_PLAY_BUTTON = 10,
    _HOME_LUCKY_BUTTON,
    _HOME_SHOP_BUTTON,

    _SETTING_TITLE = 20,
    _SETTING_HELP_BUTTON,
    _SETTING_GIFTCODE_BUTTON,

    _SHOP_TITLE = 30,

    _LUCKYWHEEL_ADS_BUTTON = 40,
    _LUCKYWHEEL_DISABLE_BUTTON,
    _LUCKYWHEEL_SPINNING_BUTTON,

    _INGAME_STAGE = 45,

    _RESULT_LOSE_TITLE = 50,
    _RESULT_LOSE_BUTTON,
    _RESULT_WIN_TITLE,
    _RESULT_CLAIM_BUTTON,
    _RESULT_CLAIMX2_BUTTON,
    _RESULT_LOSEBACK_BUTTON,
    _RESULT_WINBACK_BUTTON,

    _LIVE_TITLE = 60,
    _LIVE_DESCRIPTION,
    _LIVE_ADS_BUTTON,

    _REPLAY_TITLE = 70,
    _REPLAY_REPLAY_DESCRIPTION,
    _REPLAY_GOTOHOME_DESCRIPTION,
    _REPLAY_REPLAY_BUTTON,
    _REPLAY_GOTOHOME_BUTTON,

    _ITEMPOPUP_TITLE = 80,
    _ITEMPOPUP_DESCRIPTION,

    _USEITEM_TITLE = 90,
    _USEITEM_DESCRIPTION,
    _USEITEM_ADS_BUTTON,

    _TIME_INFINITE_TITLE = 100,
    _TIME_INFINITE_DESCRIPTION,
    _TIME_NORMAL_TITLE,
    _TIME_NORMAL_DESCRIPTION,
    _TIME_ADS_BUTTON,

    _ITEMDESCRIPTION_0 = 110,
    _ITEMDESCRIPTION_1,
    _ITEMDESCRIPTION_2,
    _ITEMDESCRIPTION_3,
    _ITEMDESCRIPTION_4,
    _ITEMDESCRIPTION_5,
    _ITEMDESCRIPTION_6,
    _ITEMDESCRIPTION_7,
    _ITEMDESCRIPTION_8,

    _TITLE_TUTORIAL_0 = 120,
    _TITLE_TUTORIAL_1,
    _TITLE_TUTORIAL_2,
    _TITLE_TUTORIAL_3,
    _TITLE_TUTORIAL_4,
    _TITLE_TUTORIAL_5,
    _TITLE_TUTORIAL_6,
    _TITLE_TUTORIAL_7,
    _TITLE_TUTORIAL_8,
    _TUTORIAL_FIRSTTEXT,

    _CONTACT_SUBJECT = 130,
    _CONTACT_CONTENT,
    _PRIVACY_POLICY_TITLE,
    _PRIVACY_POLICY_CONTENT,
    _PRIVACY_POLICY_BUTTON,

    _MAX,
}


public class TextData
{
    public int fontIndex = 0;
    public int fontSize = 24;
    public string text = null;
}


public class MessageManager : MonoBehaviour
{
    [SerializeField] TextAsset[] textfile = null;
    private TextData[] data = null;
    static MessageManager Instance = null;
    SystemLanguage language = SystemLanguage.Japanese;


    private void Awake()
    {
        Instance = this;
        language = Application.systemLanguage;
        LoadText();
    }


    bool LoadText()
    {
        int index = 0;

        switch (language)
        {
            case SystemLanguage.English:

                index = 1;
                break;
            case SystemLanguage.Vietnamese:

                index = 0;
                break;

            default:

                index = 1;
                break;
        }

        
#if CE_LANGUAGE_DEBUG_EN
        index = 1;
#endif

#if CE_LANGUAGE_DEBUG_VN
        index = 0;
#endif

        string rawString = textfile[index].text;
        rawString = rawString.Replace("\n", "");
        string[] tmpStr = rawString.Split(char.Parse("\r"));
        data = new TextData[tmpStr.Length];

        for (int i = 0; i < tmpStr.Length; i++)
        {
            string[] eachData = tmpStr[i].Split(char.Parse("\t"));
            data[i] = new TextData();

            if (eachData == null || eachData[0] == "" || eachData[0] == "\n")
            {
                break;
            }

            data[i].fontIndex = int.Parse(eachData[0]);
            data[i].fontSize = int.Parse(eachData[1]);
            data[i].text = eachData[2];
        }

        return true;
    }


    public static void GetStringData(MessageIndex _index, TextMeshProUGUI _textMesh)
    {
        if (_index < 0 || (int)_index >= Instance.data.Length)
        {
            return;
        }

        _textMesh.text = Instance.data[(int)_index].text;
        _textMesh.fontSize = Instance.data[(int)_index].fontSize;
        _textMesh.font = FontManager.GetFontAsset(Instance.data[(int)_index].fontIndex);
    }


    public static string GetStringOnlyText(MessageIndex _index)
    {
        if (_index < 0 || (int)_index >= Instance.data.Length)
        {
            return null;
        }

        return Instance.data[(int)_index].text;
    }

    public static void GetFontData(MessageIndex _index, TextMeshProUGUI _textMesh, bool fetchSize = false)
    {
        _textMesh.font = FontManager.GetFontAsset(Instance.data[(int)_index].fontIndex);

        if (!fetchSize) return;
        _textMesh.fontSize = Instance.data[(int)_index].fontSize;
    }
}