using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectRenderManager : MonoBehaviour
{
    public static            EffectRenderManager Instance;
    [SerializeField]
    private Camera camEff;
    [SerializeField]
    private RawImage imgFrozen;
    [SerializeField]
    private RawImage imgWin;
    [SerializeField]
    private RawImage imgTuto;

    private RenderTexture renderTexture;

    private void Awake( )
    {
        Instance = this;
    }


    public void Start( )
    {
        if ( !renderTexture )
        {
            renderTexture = new RenderTexture( 1080, 1920, 24 )
            {
                format       = RenderTextureFormat.ARGB32,
                filterMode   = FilterMode.Bilinear,
                antiAliasing = 1
            };

            renderTexture.Create( );

            camEff.targetTexture = renderTexture;
            // imgFrozen.texture    = renderTexture;
            // imgWin.texture     = renderTexture;
            // imgTuto.texture      = renderTexture;
        }
    }


    
    public static void ActiveWin( bool _act )
    {
        Instance._ActiveWin( _act );
    }

    void _ActiveWin( bool _act )
    {
        camEff.gameObject.SetActive( _act );
        imgWin.gameObject.SetActive( _act );
    }
    
    
    
    public static void ActiveFrozen( bool _act )
    {
        Instance._ActiveFrozen( _act );
    }

    void _ActiveFrozen( bool _act )
    {
        camEff.gameObject.SetActive( _act );
        imgFrozen.gameObject.SetActive( _act );
    }
    
    
    
    public static void ActiveTutoEff( bool _act )
    {
        Instance._ActiveTutoEff( _act );
    }

    void _ActiveTutoEff( bool _act )
    {
        camEff.gameObject.SetActive( _act );
        imgTuto.gameObject.SetActive( _act );
    }
}