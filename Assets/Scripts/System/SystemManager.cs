using UnityEngine;
using System;
using System.Collections;


public enum GameStatus
{
    LOADING,
    HOME,
    INGAME,
    RESULT,
    MAX
}


public class SystemManager : MonoBehaviour
{
    static SystemManager     Instance           = null;
    GameMode[ ]              gameMode           = null;
    GameMode                 currentGameMode    = null;
    static        GameStatus currentStatus      = GameStatus.MAX;
    static        GameStatus previousStatus     = GameStatus.MAX;
    public static bool       networkError       = false;
    public static bool       transitFromLoading = true;
    public static bool       haveGotJson        = false;
    public static bool       isBanned           = false;
    public static bool       inMaintenance      = false;
    public static bool       needUpdate         = false;
    public static bool       fromLoading        = false;
    public        ConfigData configData         = new ConfigData();
    public static bool excludeButton            = false;

    public delegate void CallBackGetReward( );

    static  CallBackGetReward callBackGetReward;
    private IDisposable       disposableOnCloseRewarded;



    private void Awake( )
    {
        Instance = this;

        gameMode      = new GameMode[ ( int )GameStatus.MAX ];
        gameMode[ 0 ] = new Loading( );
        gameMode[ 1 ] = new Home( );
        gameMode[ 2 ] = new Ingame( );
        gameMode[ 3 ] = new Result( );

        isBanned        = false;
        inMaintenance   = false;
        needUpdate      = false;
        fromLoading     = false;
        excludeButton   = false;

        DontDestroyOnLoad( gameObject );
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus) SaveDataManager.SaveData();
    }

    private void OnApplicationQuit()
    {
        SaveDataManager.SaveData();
    }


    void Start( )
    {
        Application.targetFrameRate = 60;

        ChangeStatus( GameStatus.LOADING );
    }



    void Update( )
    {
#if CE_DEBUG

        DebugManager.AddDebugText( "excludeButton", excludeButton.ToString() );
        DebugManager.AddDebugText( "currentGameMode", currentGameMode.ToString( ) );

        if ( Input.GetKeyDown( KeyCode.F12 ) )
        {
            ScreenShotCapture.Capture( );
        }

#else
#endif

        currentGameMode.Update( );
        Player.Proc(  );
    }



    public static void ChangeStatus( GameStatus _gameStatus, bool _resume = false )
    {
        Instance._ChangeStatus( _gameStatus, _resume );
    }



    void _ChangeStatus( GameStatus _gameStatus, bool _resume )
    {
        currentGameMode = gameMode[ ( int )_gameStatus ];
        previousStatus  = currentStatus;
        currentStatus   = _gameStatus;

        if ( _resume )
        {
            return;
        }

        currentGameMode.Init( );
    }



    public static GameStatus GetCurrentStatus( )
    {
        return currentStatus;
    }



    public static GameStatus GetPreviousStatus( )
    {
        return previousStatus;
    }



    public static ConfigData GameConfig {

        get { return Instance.configData; }
    }



    public static bool GetIsAlive( )
    {
        return Instance != null;
    }



    public static IEnumerator Cor_OpenURL( string _url )
    {
        yield return new WaitForSeconds( 0.2f );

        Application.OpenURL( _url );
    }
}