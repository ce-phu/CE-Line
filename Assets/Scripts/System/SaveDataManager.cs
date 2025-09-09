using System;
using System.Collections.Generic;
using UnityEngine;



public class SaveData
{
    public int    account_id   = 0;
    public string display_id   = "";
    public int    tutorialStep = 0;
    public int    currentCoin  = 0;
    public int[ ] fillLifeTime = null;

    public int    currentItem0 = 3;
    public int    currentItem1 = 3;
    public int    currentItem2 = 3;

    public PlayerData playerData = new PlayerData();
    
    // SYSTEM
    public float       soundvalue       = 1;
    public float       BGMvalue         = 1;
    public bool        enableVib        = true;
    public bool        reviewed         = false;
    public bool        shopOpend        = false;
    public List< int > checkedAnnounce  = new List< int >( );
    public int         termOfUseVersion = 0;
}



public class SaveDataManager
{
    public static SaveData data            = new SaveData( );
    static        int      saveDataVersion = 0;

    public static void LoadData( )
    {
        string rawData = PlayerPrefs.GetString( "saveData" );

        if ( string.IsNullOrEmpty( rawData ) )
        {
            data = new SaveData( );
        }
        else
        {
            saveDataVersion = PlayerPrefs.GetInt( "saveDataVersion" );
            string decrypted = EncryptionManager.DecryptData( rawData, saveDataVersion );
            data = JsonUtility.FromJson< SaveData >( decrypted );

            Player.Instance.LoadData();
        }
    }


    
    public static void SaveData( )
    {
        data.playerData = Player.Data;
        
        string json      = JsonUtility.ToJson( data );
        string encrypted = EncryptionManager.EncryptData( json, saveDataVersion );
        PlayerPrefs.SetString( "saveData", encrypted );
        PlayerPrefs.SetInt( "saveDataVersion", saveDataVersion );
        PlayerPrefs.Save( );
    }
}