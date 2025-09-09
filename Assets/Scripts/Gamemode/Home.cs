using UnityEngine;
//using CE.Template.Manager;


public class Home : GameMode
{
    public enum Status
    {
        INITIALIZE,
        FADEIN,
        WAITINANIMATION,
        OPENSHOP,
        SELECT,
        WAITOUTANIMATION,
        FADEOUT,
        WAITCONNECTION,
    }
 
    static        Status status     = Status.INITIALIZE;
    public static bool   goToIngame = false;
    public static bool   haveCoinEff;
    public static bool   isShowAds  = false;



    public override void Init( )
    {
        status = Status.INITIALIZE;
    }



    public override void Update( )
    {
#if CE_DEBUG

        DebugManager.AddDebugText( "status",       status.ToString());
        DebugManager.AddDebugText( "display_id",   SaveDataManager.data.display_id);
        DebugManager.AddDebugText( "account_id",   SaveDataManager.data.account_id.ToString());
        DebugManager.AddDebugText( "currentStage", Player.Data.currentStage.ToString());

#endif

        switch ( status )
        {
            case Status.INITIALIZE:
            {
                if (!SoundManager.IsPlayMenuBGM())
                {
                    SoundManager.PlayBGM( BGM.MENU );
                }
                goToIngame = false;
                isShowAds  = false;

                HomeUIManager.In( );

                FadeManager.FadeIn( Color.black, 0.5f );

                status = Status.FADEIN;

                break;
            }
            case Status.FADEIN:
            {
                if ( FadeManager.IsFading( ) == false )
                {
                    status = Status.WAITINANIMATION;
                }

                break;
            }
            case Status.WAITINANIMATION:
            {
                if ( HomeUIManager.IsCompleteAnimation( ) )
                {
                    if ( haveCoinEff )
                    {
                        HomeUIManager.PlayEffect( );
                        haveCoinEff = false;
                    }
                    else
                    {
                        status = Status.OPENSHOP;
                    }
                }

                break;
            }
            case Status.OPENSHOP:
            {
                if ( HomeUIManager.IsCompleteAnimation( ) )
                {
                    status = Status.SELECT;
                }

                break;
            }
            case Status.SELECT:
            {
                //if ( Player.Data.currentStage >= 11 && SaveDataManager.data.reviewed == false && GameData.enableReview )
                if ( Player.Data.currentStage >= 11 && SaveDataManager.data.reviewed == false )
                {
                    //AppReview.Review();
                    //TODO appReview
                    SaveDataManager.data.reviewed = true;
                    SaveDataManager.SaveData( );
                }

                if ( goToIngame )
                {
                    HomeUIManager.Out( );

                    status = Status.WAITOUTANIMATION;
                }

                break;
            }
            case Status.WAITOUTANIMATION:
            {
                if ( HomeUIManager.IsCompleteAnimation( ) )
                {
                    SystemManager.excludeButton     = false;

                    FadeManager.FadeOut( Color.black, 0.5f );

                    status = Status.FADEOUT;
                }

                break;
            }
            case Status.FADEOUT:
            {
                if ( FadeManager.IsFading( ) == false )
                {
                    if ( Player.Data.currentStage >= 7 )
                    {
                        if ( !isShowAds )
                        {
                            isShowAds       = true;

                            // AdManager.I.ShowInterstitial( ( isSuccess ) =>
                            // {
                            //     HomeUIManager.Def();
                            //
                            //     goToIngame  = false;
                            //
                            //     SystemManager.ChangeStatus( GameStatus.INGAME );
                            //
                            //     status      = Status.INITIALIZE;
                            // } );
                            HomeUIManager.Def();

                            goToIngame  = false;

                            SystemManager.ChangeStatus( GameStatus.INGAME );

                            status      = Status.INITIALIZE;
                        }
                    }
                    else {

                        HomeUIManager.Def();

                        goToIngame  = false;

                        SystemManager.ChangeStatus( GameStatus.INGAME );

                        status      = Status.INITIALIZE;
                    }
                }

                break;
            }
        }
    }
}