using UnityEngine;
using UnityEngine.UI;


public class ScreenSizeManager : MonoBehaviour
{
    static             ScreenSizeManager Instance           = null;
    [ SerializeField ] CanvasScaler      scaler             = null;
    Vector2                              canvasDefaultSize  = Vector2.zero;
    float                                defaultAspectRatio = 1.0f;
    float                                upperMargin        = 0.0f;
    float                                lowerMargin        = 0.0f;

    float safeRatio      = 16.0f  / 9.0f;
    float maxUpperMargin = 234.0f / 2436.0f;
    float maxLowerMargin = 102.0f / 2436.0f;

    public static bool IsHeightScreen = false;



    void Awake( )
    {
        Instance           = this;
        canvasDefaultSize  = scaler.referenceResolution;
        defaultAspectRatio = ( float )canvasDefaultSize.y / canvasDefaultSize.x;

        Update( );
    }



    void Update( )
    {
        float screenAspectRatio = ( float )Screen.height / Screen.width;

        if ( screenAspectRatio > defaultAspectRatio )
        {
            scaler.matchWidthOrHeight = 0.0f;
        }
        else
        {
            scaler.matchWidthOrHeight = 1.0f;
        }

        if ( screenAspectRatio > safeRatio )
        {
            float remainSpace     = ( screenAspectRatio - safeRatio ) / 2.0f;
            float upperMarginRate = Mathf.Min( remainSpace, maxUpperMargin );
            upperMargin = upperMarginRate * canvasDefaultSize.x;

            float lowerMarginRate = Mathf.Min( remainSpace, maxLowerMargin );
            lowerMargin = lowerMarginRate * canvasDefaultSize.x;
#if UNITY_IOS
            lowerMargin += 25;
#endif

            IsHeightScreen = !( upperMargin > 5 );
        }
        else
        {
            IsHeightScreen = true;
            upperMargin    = 0.0f;
            lowerMargin    = 0.0f;
        }
    }



    public static float UpperMargin
    {
        get { return Instance.upperMargin; }
    }



    public static float LowerMargin
    {
        get { return Instance.lowerMargin; }
    }
}