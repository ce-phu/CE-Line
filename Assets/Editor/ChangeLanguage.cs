using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class ChangeLanguage : EditorWindow
{
    private int selectedLanguageIndex;

    private readonly string[] languageDefines =
    {
        "CE_LANGUAGE_DEBUG_EN",
        "CE_LANGUAGE_DEBUG_VN",
    };

    private readonly string[] languageNames =
    {
        "English",
        "Vietnamese",
    };

    [MenuItem("Tools/Change Language")]
    public static void Open()
    {
        GetWindow<ChangeLanguage>("Change Language");
    }

    private void OnEnable()
    {
        string defines = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Android);

        for (int i = 0; i < languageDefines.Length; i++)
        {
            if (defines.Contains(languageDefines[i]))
            {
                selectedLanguageIndex = i;
                break;
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Choose language for debugging (define symbol):");

        selectedLanguageIndex = EditorGUILayout.Popup(selectedLanguageIndex, languageNames);

        GUILayout.Space(20);
        if (GUILayout.Button("Apply"))
        {
            ApplyLanguageDefine(languageDefines[selectedLanguageIndex]);
            Close();
        }
    }

    private void ApplyLanguageDefine(string defineSymbol)
    {
        // Get current defines
        string defines = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Android);
        
        List<string> defineList = defines.Split(';')
                                .Where(d => !string.IsNullOrWhiteSpace(d))
                                .ToList();

        // Remove old language defines
        foreach (var lang in languageDefines)
        {
            defineList.Remove(lang);
        }

        // Add the selected one
        defineList.Add(defineSymbol);

        PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Android, defineList.ToArray());

        Debug.Log($"[ChangeLanguage] Applied define symbol: {defineSymbol}");
    }
}
