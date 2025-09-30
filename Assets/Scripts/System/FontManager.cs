using UnityEngine;
using TMPro;


public class FontManager : MonoBehaviour
{
    [SerializeField] TMP_FontAsset[] fontVN = null;
    [SerializeField] TMP_FontAsset[] fontEN = null;
    TMP_FontAsset[] fontAssets = null;
    static FontManager Instance = null;


    void Awake()
    {
        Instance = this;

        PrepareFont();
    }


    void PrepareFont()
    {
        SystemLanguage lang = Application.systemLanguage;

        switch (lang)
        {
            case SystemLanguage.English:

                fontAssets = fontEN;
                break;

            case SystemLanguage.Vietnamese:

                fontAssets = fontVN;
                break;

            default:

                fontAssets = fontEN;
                break;
        }
    
#if CE_LANGUAGE_DEBUG_EN
                fontAssets = fontEN;
#endif

#if CE_LANGUAGE_DEBUG_VN
                fontAssets = fontVN;
#endif
    }


    public static TMP_FontAsset GetFontAsset(int _index)
    {
        if (Instance == null)
        {
            return null;
        }

        return Instance.fontAssets[_index];
    }
}