using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance;
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private GameObject debugPanel;

    private Dictionary<string, string> displayText = new Dictionary<string, string>();
    private float timer;
    private int frameCounter;
    


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

#if CE_DEBUG

        debugPanel.SetActive( true );

#else

        debugPanel.SetActive( false );

#endif

    }



    private void Update()
    {

#if CE_DEBUG

        if ( fpsText != null ) 
        {

            timer   += Time.deltaTime;
            frameCounter++;

            if (timer >= 1.0f) 
            {

                float fps		= frameCounter / timer;
                fpsText.text    = "FPS : " + fps.ToString( "F2" );

                timer           -= 1.0f;
                frameCounter      = 0;
            }
        }

        debugText.text = "";
        foreach (KeyValuePair<string, string> pair in displayText)
        {
            debugText.text += pair.Key + " : " + pair.Value + "\n";
        }

#endif

    }



    public static void AddDebugText(string key, string text)
    {

#if CE_DEBUG

        Instance._AddDebugText(key, text);

#endif

    }



    private void _AddDebugText(string _key, string _text)
    {
        if (displayText.ContainsKey(_key))
        {
            displayText[_key] = _text;
        }
        else
        {
            displayText.Add(_key, _text);
        }
    }



    public static void RemoveDebugText(string key)
    {

#if CE_DEBUG

        Instance._RemoveDebugText(key);
        
#endif

    }

    private void _RemoveDebugText(string _key)
    {
#if CE_DEBUG

        if ( displayText.ContainsKey( _key ) ) {

            displayText.Remove( _key );
        }

#endif
    }
}
