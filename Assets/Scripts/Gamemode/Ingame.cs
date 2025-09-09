//using CE.Template.Manager;
using UnityEngine;



public class Ingame : GameMode
{
    public enum Status
    {
        INITIALIZE,
        FADEIN,
        WAITINANIMATION,
        TUTORIAL,
        PLAYING,
        WAITCONNECTION,
        WAITOUTANIMATION,
        OUT,
        FADEOUT,
    }

    Status status = Status.INITIALIZE;

    private static bool isGameOver;
    private static bool isNextLevel;
    private static bool isPause;
    private static bool isFrozen; //this one use for the ads popup of the powerup
    private static bool goToHome;
    private static bool isContinue;
    private static bool addOneMinute;
    private static bool retry;
    private static bool noInstAds;

    private bool isShowAds;



    public override void Init( )
    {
        status = Status.INITIALIZE;
    }



    public override void Update( )
    {
        DebugManager.AddDebugText( "status", status.ToString() );

        switch ( status )
        {
            case Status.INITIALIZE:
            {
                if (!SoundManager.IsPlayIngameBGM())
                {
                    SoundManager.PlayBGM(BGM.GAMEPLAY);
                }
                else
                {
                    SoundManager.ResumeBGM();
                }
                isGameOver  = false;
                isNextLevel = false;
                goToHome    = false;
                retry       = false;
                noInstAds   = false;

                if ( !isContinue ) {

                    GameManager.Init();
                    FadeManager.FadeIn( Color.black, 0.5f );
                    EffectManager.ClearAllEffect();
                }

                status = Status.FADEIN;

                break;
            }
            case Status.FADEIN:
            {
                if ( FadeManager.IsFading( ) == false ) {
                    
                    if ( !isContinue ) {

                        IngameUIManager.In();
                    }

                    status = Status.WAITINANIMATION;
                }

                break;
            }
            case Status.WAITINANIMATION:
            {
                if ( IngameUIManager.IsCompleteAnimation() )
                {
                    if ( isContinue ) {
                    
                        if ( addOneMinute ) {
                    
                            // GameManager.AddPlayingTime();
                        }
                    }

                    status  = Status.TUTORIAL;

                }

                break;
            }
            case Status.TUTORIAL:
            {
                if ( isContinue == false ) {

                    // GameManager.PlayTutorial();
                }

                isContinue   = false;
                addOneMinute = false;

                status  = Status.PLAYING;

                break;
            }
            case Status.PLAYING:
            {
                if ( isNextLevel )
                {
                    Player.Data.currentStage++;
                    
                    SaveDataManager.SaveData( );
                    
                    status = Status.WAITCONNECTION;

                    Home.haveCoinEff = true;
                }
                else if ( isGameOver )
                {
                    Result.SetLose( true );
                    // GameManager.SetMovingObject( null );

                    SystemManager.ChangeStatus( GameStatus.RESULT );

                    isGameOver      = false;

                    status = Status.OUT;
                }
                else if ( isPause )
                {
					if ( !SettingUIManager.isOpen ) { //it was PauseUIManager, but now use SettingUIManager instead

						// TutorialUIManager.Stop();

                        SettingUIManager.Ingame_In();
					}
                }
                else if (isFrozen)
                {
                    
                }
                else if ( retry || goToHome )
                {
                    // IngameUIManager.ActiveObjCombo( false );
                    IngameUIManager.Out( );
                    status = Status.WAITOUTANIMATION;
                }
                else
                {
                    // if ( TutorialUIManager.Finished() == false ) {
                    //
                    //     if ( TutorialUIManager.IsCompleteAnimation() ) {
                    //
                    //         if ( Input.GetMouseButtonDown( 0 ) ) {
                    //
                    //             TutorialUIManager.Stop();
                    //         }
                    //     }
                    // }
                    //
                    // GameManager.Proc();
                    // IngameUIManager.Proc();
                }

                break;
            }
            case Status.WAITCONNECTION:
            {
                Result.SetWin( true );

                SystemManager.ChangeStatus( GameStatus.RESULT );
                    
                isNextLevel = false;

                break;
            }
            case Status.WAITOUTANIMATION:
            {
                if ( IngameUIManager.IsCompleteAnimation( ) )
                {
                    EffectManager.ClearAllEffect( );

                    status = Status.OUT;
                }

                break;
            }
            case Status.OUT:
            {
                if ( retry || goToHome )
                {
                    FadeManager.FadeOut( Color.black, 0.5f );

                    status = Status.FADEOUT;
                }
                else if ( isContinue )
                {
                    // GameManager.AddPlayingTime();

                    status = Status.PLAYING;
                }

                break;
            }
            case Status.FADEOUT:
            {
                if ( FadeManager.IsFading( ) == false ) {

                    if ( noInstAds ) {
                    
                        if ( goToHome ) {

                            SystemManager.ChangeStatus( GameStatus.HOME );
                        }

                        retry       = false;
                        goToHome    = false;
                        isShowAds   = false;

                        IngameUIManager.Def();
                    
                        status  = Status.INITIALIZE;
                    }
                    else {
                    
                        if ( !isShowAds ) {
                    
                            isShowAds       = true;

                            // AdManager.I.ShowInterstitial( ( isSuccess ) =>
                            // {
                            //     isShowAds   = false;
                            //
                            //     if ( goToHome ) {
                            //
                            //         SystemManager.ChangeStatus( GameStatus.HOME );
                            //     }
                            //
                            //     retry       = false;
                            //     goToHome    = false;
                            //
                            //     GameManager.NextLevel();
                            //     IngameUIManager.Def();
                            //
                            //     status  = Status.INITIALIZE;
                            // } );
                            isShowAds   = false;

                            if ( goToHome ) {

                                SystemManager.ChangeStatus( GameStatus.HOME );
                            }

                            retry    = false;
                            goToHome = false;

                            // GameManager.NextLevel();
                            IngameUIManager.Def();

                            status  = Status.INITIALIZE;
                        }
                    }
                }

                break;
            }
        }
    }



    public static void SetGameOver( bool _isGameOver )
    {
        isGameOver = _isGameOver;
    }



    public static void SetNextLevel( bool _isNextLevel )
    {
        isNextLevel = _isNextLevel;
    }



    public static void SetPause( bool _isPause )
    {
        isPause = _isPause;
    }

    
    
    public static bool GetPause()
    {
        return isPause;
    }
    
    public static void SetFrozen( bool _isFrozen )
    {
        isFrozen = _isFrozen;
    }

    
    public static void SetGoToHome( bool _goToHome )
    {
        goToHome = _goToHome;
    }



    public static void SetNoInstAds( bool _noInstAds ) {

        noInstAds   = _noInstAds;
    }



    public static void SetRetry( bool _retry )
    {
        retry = _retry;
    }



    public static void SetContinue( bool _continue, bool _addOneMinute )
    {
        isContinue      = _continue;
        addOneMinute    = _addOneMinute;
    }
}