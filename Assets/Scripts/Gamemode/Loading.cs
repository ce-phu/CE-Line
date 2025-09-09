using UnityEngine;
using UnityEngine.SceneManagement;
//using CE.Template.Manager;



public class Loading : GameMode
{
    enum Status
    {
        INITIALIZE,
        FADEIN,
        WAITLOADING,
        WAITSTARTPLAY,
        FADEOUT,
        FINALIZE,
    }

    Status         status     = Status.INITIALIZE;
    AsyncOperation asyncOp    = null;
    float          timer      = 0.0f;
    float          retryTimer = 0.0f;



    public override void Init( )
    {
        SaveDataManager.LoadData( );
        
        asyncOp                      = SceneManager.LoadSceneAsync( "Main" );
        asyncOp.allowSceneActivation = false;

        string verText = "v." + Application.version + "." + GitRevision.CURRENT_GIT_REVISION_STR;
        
#if !CE_PRODUCT_SERVER

        verText   += "_Dev";

#endif
        
        LoadingUIManager.SetVersion( verText );
        
        status = Status.INITIALIZE;
        timer  = 0.0f;
    }



    public override void Update( )
    {

#if CE_DEBUG

        DebugManager.AddDebugText( "status", status.ToString() );

#endif

        switch ( status )
        {
            case Status.INITIALIZE:
            {
                SoundManager.PlayBGM(BGM.MENU);
                FadeManager.FadeIn( Color.white, 0.5f );

                status = Status.FADEIN;

                break;
            }
            case Status.FADEIN:
            {
                if ( FadeManager.IsFading( ) == false )
                {
                    timer  = 0.0f;
                    status = Status.WAITLOADING;
                }

                break;
            }
            case Status.WAITLOADING:
            {
                if ( asyncOp.progress >= 0.89f && timer >= 2.0f ) {

                    if ( SaveDataManager.data.termOfUseVersion < GameData.termOfUseVersion ) {

                        PopupUIManager.In( PopupType.ONEBUTTON, MessageData.newPrivacyPolicyTitle,
                            string.Format( MessageData.newPrivacyPolicyContent, GameData.termOfUseUrl, GameData.privacyPolicyUrl ),
                            MessageData.newPrivacyPolicyButton, null, () => {

                                SoundManager.PlaySE( SE.UI_BTNCLICK );
                                VibrationManager.VibrateTap();

                                SaveDataManager.data.termOfUseVersion   = GameData.termOfUseVersion;
                                SaveDataManager.SaveData();

                                if ( Player.Data.currentStage == 1 ) {

                                    status  = Status.WAITSTARTPLAY;
                                }
                                else {

                                    FadeManager.FadeOut( Color.black, 0.5f );
                                    status  = Status.FADEOUT;
                                }
                            },
                            null );
                    }
                    else {

                        if ( Player.Data.currentStage == 1 ) {

                            status  = Status.WAITSTARTPLAY;
                        }
                        else {

                            FadeManager.FadeOut( Color.black, 0.5f );
                            status  = Status.FADEOUT;
                        }
                    }
                }
                else {

                    timer       += Time.unscaledDeltaTime;
    
                    float limit = 2f;
                    //limit       += AdManager.I.CanShowInterstitial() ? 0.5f : 0.0f;
                    //limit       += AdManager.I.CanShowRewarded() ? 0.5f : 0.0f;
                    
                    timer       = Mathf.Min( timer, limit );

                    LoadingUIManager.SetLoadingGauge( Mathf.Min( asyncOp.progress / 0.89f, timer / 2.0f ) );
                }

                break;
            }
            case Status.WAITSTARTPLAY:
            {/*
                if ( true ) {

                    NetworkManager.SetBehaviorFinishConnection( ( ) => {
                                                                    
                        FadeManager.FadeOut( Color.black, 0.5f );
                        status = Status.FADEOUT;
                    }, 
                    ( ) => {

                        retryTimer = 5.0f;
                        timer      = 0.0f;
                        LoadingUIManager.SetLoadingGauge( Mathf.Min( asyncOp.progress / 0.89f, timer / 2.0f ) );

                    } );
                }
                else {

                    timer += Time.unscaledDeltaTime;
                    LoadingUIManager.SetLoadingGauge( Mathf.Min( asyncOp.progress / 0.89f, timer / 2.0f ) );
                }*/
                FadeManager.FadeOut( Color.black, 0.5f );
                status = Status.FADEOUT;
                break;
            }
            case Status.FADEOUT:
            {
                if ( FadeManager.IsFading( ) == false )
                {
                    asyncOp.allowSceneActivation = true;
                    status                       = Status.FINALIZE;
                }

                break;
            }
            case Status.FINALIZE:
            {
                if ( SceneManager.GetActiveScene( ).name == "Main" )
                {
                    // //  TODO Okada check what's for
                    // NetworkManager.GetDailyMission();

                     if ( Player.Data.currentStage == 1 ) {
                    
                         SystemManager.fromLoading        = true;
                         SystemManager.ChangeStatus( GameStatus.INGAME );
                         FadeManager.FadeIn( Color.black, 0.5f );
                     }
                     else {
                    
                         SystemManager.ChangeStatus( GameStatus.HOME );
                     }
                     
                     status = Status.INITIALIZE;
                }

                break;
            }
        }
    }
}