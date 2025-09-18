using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    static IngameUIManager Instance;
    
    [SerializeField] private Button          pauseButton;
    [SerializeField] private Button          backHomeButton;
    [SerializeField] private Button          replayButton;
    [SerializeField] private TextMeshProUGUI stageText;

    [SerializeField] private Animator animator;

    [SerializeField] private TMP_InputField debugLevel;

    private bool isCompleteAnimation = false;

    private void Awake( )
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    
    
    public static void In( )
    {
        Instance._In( );
    }

    
    
    private void _In( )
    {
        pauseButton.onClick.AddListener( Pause );
        backHomeButton.onClick.AddListener( BackHome );
        replayButton.onClick.AddListener( Replay );
        debugLevel.onSubmit.AddListener(LoadDebugLevel);
        
        animator.Play( "In" );
        // LifeTimeManager.In(  );
        isCompleteAnimation = false;
        SystemManager.excludeButton = true;

        stageText.text = "STAGE " + Player.Data.currentStage;
    }
    


    public static void Out( )
    {
        Instance._Out( );
    }

    private void _Out( )
    {
        pauseButton.onClick.RemoveListener( Pause );
        backHomeButton.onClick.RemoveListener( BackHome );
        replayButton.onClick.RemoveListener( Replay );
        debugLevel.onSubmit.RemoveListener(LoadDebugLevel);

        animator.Play( "Out" );
        // LifeTimeManager.Out(  );
        SystemManager.excludeButton = true;

        isCompleteAnimation = false;
    }


    public static void Def( )
    {
        Instance._Def( );
    }


    void _Def( )
    {
        animator.Play( "Def" );
    }


    private void Pause( )
    {
        if( SystemManager.excludeButton )
        {
            return;
        }

        SystemManager.excludeButton = true;

        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );

        Ingame.SetPause( true );
    }    
    
    
    
    private void BackHome( )
    {
        if( SystemManager.excludeButton )
        {
            return;
        }

        SystemManager.excludeButton = true;

        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        ReplayUIManager.In(ReplayUIManager.ReplayUITypes.GOTOHOME);
        SystemManager.excludeButton = false;
    }    
    
    
    
    private void Replay( )
    {
        if( SystemManager.excludeButton )
        {
            return;
        }

        SystemManager.excludeButton = true;

        VibrationManager.VibrateTap();
        SoundManager.PlaySE( SE.UI_BTNCLICK );
        
        ReplayUIManager.In(ReplayUIManager.ReplayUITypes.INSTRUCTION);
    }

    private void LoadDebugLevel(string text)
    {
        GameManager.debugLevel = int.Parse(text);
        GameManager.Init();
    }

    public void HardLevelAnimationCompleted()
    {
        Ingame.SetFrozen(false);
        SystemManager.excludeButton = false;
    }
    
    public void AnimationCompleted( )
    {
        isCompleteAnimation = true;
        SystemManager.excludeButton = false;
    }

    public static bool IsCompleteAnimation( )
    {
        return Instance.isCompleteAnimation;
    }
}