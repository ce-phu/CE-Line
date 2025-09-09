using MobileVibration;
using UnityEngine;



public class VibrationManager : MonoBehaviour {

    enum PhoneOs {

        NONE,
        ANDROID,
        IOS
    }

    static VibrationManager Instance    = null;
    private PhoneOs platform            = PhoneOs.NONE;
    bool isVibrationSupported           = false;

    AndroidVibration androidVibration   = null;
    IOSVibration iosVibration           = null;
    


    void Awake() {

        Instance            = this;

#if !UNITY_EDITOR
#if UNITY_ANDROID
        
        platform            = PhoneOs.ANDROID;
        androidVibration    = new AndroidVibration();

#elif UNITY_IOS

        platform            = PhoneOs.IOS;
        iosVibration        = new IOSVibration();
#endif
#endif

        isVibrationSupported    = SystemInfo.supportsVibration && Application.isMobilePlatform;
    }



    public static void VibrateTap() {

        Instance._VibrateTap();
    }
     


    void _VibrateTap() {

        if ( SaveDataManager.data.enableVib == false ) {
        
            return;
        }
        
        if ( isVibrationSupported == false ) {
            
            return;
        }
        
        switch ( platform ) {
        
            case PhoneOs.ANDROID:
            {
				//androidVibration.Vibrate( 0x00000005 );
				androidVibration.Vibrate( 10L, 255 );
				break;
            }
            case PhoneOs.IOS:
            {
                iosVibration.VibrateNope();
                break;
            }
            case PhoneOs.NONE:
            default:
            {
                Handheld.Vibrate();
                break;
            }
        }
	}



    /// <summary>
    /// Vibrate default, duration parameter only works on Android
    /// </summary>
    /// <param name="duration">in miliseconds</param>
    public static void Vibrate(long _duration, int _amplitude ) {

        Instance._Vibrate( _duration, _amplitude );
    }



    void _Vibrate( long _duration, int _amplitude ) {

        if ( SaveDataManager.data.enableVib == false ) {

            return;
        }

        if ( isVibrationSupported == false ) {

            return;
        }

        switch ( platform ) {

            case PhoneOs.ANDROID:
            {
				androidVibration.Vibrate( _duration, _amplitude );
				break;
            }
            case PhoneOs.IOS:
            {
                iosVibration.Vibrate();
                break;
            }
            case PhoneOs.NONE:
            default:
            {
                Handheld.Vibrate();
                break;
            }
        }
    }



    /// <summary>
    /// Only on android
    /// </summary>
    public static void Cancel() {

        Instance._Cancel();
    }



    void _Cancel()
    {
        if ( isVibrationSupported == false ) {
            
            return;
        }

        if ( platform == PhoneOs.ANDROID ) {

            androidVibration.Cancel();
        }
    }



    public static bool IsVibrationSupported {

        get { return Instance.isVibrationSupported; }
    }
}
