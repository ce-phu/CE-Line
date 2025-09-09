using UnityEngine;



namespace MobileVibration
{
    public class AndroidVibration
    {
        private AndroidJavaClass unityPlayer;
        private AndroidJavaObject currentActivity;
        private AndroidJavaObject vibrator;



        public AndroidVibration()
        {
            unityPlayer         = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" );
            currentActivity     = unityPlayer.GetStatic<AndroidJavaObject>( "currentActivity" );
            vibrator            = currentActivity.Call<AndroidJavaObject>( "getSystemService", "vibrator" );
        }
        


        public void Cancel() {

            vibrator.Call( "cancel" );
		}



		public void Vibrate( long milliseconds, int amplitude ) {

			if ( vibrator == null ) {

				return;
            }

			if ( AndroidVersion() >= 26 ) {

				using ( var vibrationEffectClass = new AndroidJavaClass( "android.os.VibrationEffect" ) ) {

					var effect  = vibrationEffectClass.CallStatic<AndroidJavaObject>( "createOneShot", milliseconds, amplitude );

					vibrator.Call( "vibrate", effect );
				}
			}
			else {

				vibrator.Call( "vibrate", milliseconds );
			}
		}



		private static int AndroidVersion() {

			using ( var version = new AndroidJavaClass( "android.os.Build$VERSION" ) ) {

				return version.GetStatic<int>( "SDK_INT" );
			}
		}
	}
}
