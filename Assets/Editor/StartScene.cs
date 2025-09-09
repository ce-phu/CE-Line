using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;



public class SettingStartScene : EditorWindow {

    private const string SAVE_KEY     = "StartScenePathKey";



    [MenuItem( "Tools/Setting Start Scene" )]
    public static void Open (){

        GetWindow<SettingStartScene>( typeof( SettingStartScene ) );
    }



    private void OnEnable() {

        string startScenePath   = EditorUserSettings.GetConfigValue( SAVE_KEY );

        if ( string.IsNullOrEmpty( startScenePath ) == false ) {

            SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>( startScenePath );

            if ( sceneAsset == null ) {

                Debug.LogWarning( startScenePath + " is not existed." );
            }
            else {

                EditorSceneManager.playModeStartScene   = sceneAsset;
            }
        }
    }



    private void OnGUI(){

        string beforeScenePath  = "";

        if ( EditorSceneManager.playModeStartScene != null ) {

            beforeScenePath     = AssetDatabase.GetAssetPath( EditorSceneManager.playModeStartScene );
        }

        EditorSceneManager.playModeStartScene = (SceneAsset)EditorGUILayout.ObjectField( new GUIContent( "Start Scene" ), EditorSceneManager.playModeStartScene, typeof( SceneAsset ), false );

        string afterScenePath   = "";

        if ( EditorSceneManager.playModeStartScene != null ) {

            afterScenePath = AssetDatabase.GetAssetPath( EditorSceneManager.playModeStartScene );
        }

        if ( beforeScenePath != afterScenePath ) {

            EditorUserSettings.SetConfigValue( SAVE_KEY, afterScenePath );
        }
    }
}
