using System;
using UnityEngine;
using TMPro;

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