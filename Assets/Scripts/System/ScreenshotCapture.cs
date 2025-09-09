using UnityEngine;
using UnityEditor;
using System;
using System.IO;



public class ScreenShotCapture {

    private const string Path = "ScreenShot/";



    public static void Capture() {

        string assetPath    = string.Format( System.IO.Path.Combine( Path, "ScreenShot_{0}.png" ), DateTime.Now.ToString( "yyyyMMddHHmmss" ) );
        
        string fullPath = ( Application.dataPath ).Replace( "Assets", "" ) + Path;

        if ( !Directory.Exists( fullPath ) ) {
            
            Directory.CreateDirectory( fullPath );
        }

        ScreenCapture.CaptureScreenshot( string.Format( assetPath ) );
        Debug.Log( $"Capture screenshot ÅF {( Application.dataPath ).Replace( "Assets", "" ) + assetPath}" );
    }
}