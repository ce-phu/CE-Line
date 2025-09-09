namespace MobileVibration
{
    using System.Collections;
    using System.Runtime.InteropServices;

    public class IOSVibration
    {
#if UNITY_IOS
        [ DllImport( "__Internal" ) ]
        private static extern void _Vibrate( );

        [ DllImport( "__Internal" ) ]
        private static extern void _VibratePop( );

        [ DllImport( "__Internal" ) ]
        private static extern void _VibratePeek( );

        [ DllImport( "__Internal" ) ]
        private static extern void _VibrateNope( );

        public void Vibrate( )
        {
            _Vibrate( );
        }

        public void VibratePop( )
        {
            _VibratePop( );
        }

        public void VibratePeek( )
        {
            _VibratePeek( );
        }

        public void VibrateNope( )
        {
            _VibrateNope( );
        }
#else
        public void Vibrate()
        {

        }

        public void VibratePop()
        {

        }

        public void VibratePeek()
        {

        }

        public void VibrateNope()
        {

        }

#endif
    }
}