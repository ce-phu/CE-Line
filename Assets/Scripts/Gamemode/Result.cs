//using CE.Template.Manager;

using System;
using UnityEngine;
using UnityEngine.Events;


public class Result : GameMode
{
    public enum Status
    {
        INITIALIZE,
        MAIN,
        WAITINANIMATION,
        PREPAREFADEOUT,
        FADEOUT,
        WAITOUTANIMATION,
    }

    Status status = Status.INITIALIZE;

    private static bool isWin;
    private static bool isLose;

    public static bool backToHome;
    public static bool isContinue;
    public static bool keepPlaying;

    private bool  isShowAds     = false;

    public static UnityEvent OnClose = new UnityEvent();



    public override void Init( )
    {
        status = Status.INITIALIZE;
    }



    public override void Update( )
    {
        DebugManager.AddDebugText("status", status.ToString());

        switch ( status )
        {
            case Status.INITIALIZE:
            {
                backToHome = false;
                isContinue = false;
                keepPlaying = false;

                OnClose.Invoke();
                
                if ( isWin )
                {
                    // EffectRenderManager.ActiveWin( true );
                    EffectManager.Play( new Vector3( 0.0f, -3f, 0.0f ), Quaternion.identity, Vector3.one, 1, EffectType.GLOW );
               
                    ResultUIManager.In(true);
                }
                else if ( isLose )
                {
                    TimeUIManager.In(false);
                }

                status = Status.WAITINANIMATION;

                break;
            }
            case Status.WAITINANIMATION:
            {
                if ( ResultUIManager.IsCompleteAnimation() ) {

                    status = Status.MAIN;
                }

                if (isContinue)
                {
                    isContinue = false;
                    Ingame.SetContinue(true, true);
                    SystemManager.ChangeStatus(GameStatus.INGAME);
                }
                
                break;
            }
            case Status.MAIN:
            {
                if ( backToHome || keepPlaying)
                {
                    status = Status.PREPAREFADEOUT;
                }

                break;
            }
            case Status.PREPAREFADEOUT:
            {
                FadeManager.FadeOut( Color.black, 0.5f );

                status = Status.FADEOUT;

                break;
            }
            case Status.FADEOUT:
            {
                if ( FadeManager.IsFading( ) == false )
                {
                    if (backToHome)
                    {
                        if ( !isShowAds )
                        {
                            isShowAds       = true;

                            // AdManager.I.ShowInterstitial( ( isSuccess ) =>
                            // {
                            //     isShowAds                   = false;
                            //     SystemManager.excludeButton = false;
                            //
                            //     EffectRenderManager.ActiveWin( false );
                            //     
                            //     ResultUIManager.Def();
                            //
                            //     IngameUIManager.Def();
                            //
                            //     GameManager.NextLevel();
                            //
                            //     EffectManager.ClearAllEffect();
                            //
                            //     SystemManager.ChangeStatus( GameStatus.HOME );
                            //
                            //     isWin      = false;
                            //     isLose     = false;
                            //
                            //     status  = Status.INITIALIZE;
                            // } );
                            isShowAds                   = false;
                            SystemManager.excludeButton = false;

                            // EffectRenderManager.ActiveWin( false );
                                
                            ResultUIManager.Def();

                            IngameUIManager.Def();

                            // GameManager.NextLevel();

                            EffectManager.ClearAllEffect();

                            SystemManager.ChangeStatus( GameStatus.HOME );

                            RemoveALlListener();
                            
                            isWin  = false;
                            isLose = false;

                            status  = Status.INITIALIZE;
                        }
                    }

                    if (keepPlaying)
                    {
                        isShowAds                   = false;
                        SystemManager.excludeButton = false;

                        // EffectRenderManager.ActiveWin( false );
                                
                        ResultUIManager.Def();

                        IngameUIManager.Def();

                        // GameManager.NextLevel();

                        EffectManager.ClearAllEffect();

                        SystemManager.ChangeStatus( GameStatus.INGAME );

                        RemoveALlListener();
                        
                        isWin  = false;
                        isLose = false;

                        status  = Status.INITIALIZE;
                    }
                }

                break;
            }
        }
    }



    public static void SetWin( bool _isWin )
    {
        isWin = _isWin;
    }



    public static void SetLose( bool _isLose )
    {
        isLose = _isLose;
    }

    public static void AddListener(UnityAction action)
    {
        OnClose.AddListener(action);
    }

    public static void RemoveListener(UnityAction action)
    {
        OnClose.RemoveListener(action);
    }

    public static void RemoveALlListener()
    {
        OnClose.RemoveAllListeners();
    }
}