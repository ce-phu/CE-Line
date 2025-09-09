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
        // GameManager.SetMovingObject(  null );
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
        
        // ReplayUIManager.In(ReplayUIManager.ReplayUITypes.GOTOHOME);
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
        
        // ReplayUIManager.In(ReplayUIManager.ReplayUITypes.REPLAY);
        GameManager.ShowInstructions();
        SystemManager.excludeButton = false;
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