using UnityEngine;



public class SafeUIObjectController : MonoBehaviour
{
    enum SafeAreaType
    {
        UPPER,
        LOWER,
    }

    [ SerializeField ] SafeAreaType  areaType        = SafeAreaType.UPPER;
    [ SerializeField ] bool          updateForChange = false;
    [ SerializeField ] RectTransform rtBg;

    RectTransform rectTrans       = null;
    Vector2       defaultPosition = Vector2.zero;
    Vector2       sizeBg;



    void Awake( )
    {
        rectTrans       = GetComponent< RectTransform >( );
        defaultPosition = rectTrans.anchoredPosition;
    }



    void Start( )
    {
        if ( rtBg )
        {
            sizeBg = rtBg.sizeDelta;
        }

        AjustPosition( );
    }



    void Update( )
    {
        if ( updateForChange )
        {
            AjustPosition( );
        }
    }



    void AjustPosition( )
    {
        Vector2 marginedPosition = defaultPosition;

        switch ( areaType )
        {
            case SafeAreaType.UPPER:
            {
                marginedPosition.y -= ScreenSizeManager.UpperMargin;
                break;
            }
            case SafeAreaType.LOWER:
            {
                marginedPosition.y += ScreenSizeManager.LowerMargin;
                break;
            }
        }

        rectTrans.anchoredPosition = marginedPosition;

        if ( rtBg )
        {
            rtBg.sizeDelta = sizeBg + new Vector2( Mathf.Abs( marginedPosition.x ), Mathf.Abs( marginedPosition.y ) );
        }
    }
}