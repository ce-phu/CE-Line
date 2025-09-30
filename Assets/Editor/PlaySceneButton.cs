using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;
using System.Reflection;



public class PlaySceneButton {

    static readonly System.Type ToolbarType = typeof( UnityEditor.Editor ).Assembly.GetType( "UnityEditor.Toolbar" );



    [InitializeOnLoadMethod]
    static void InitializeOnLoad() {

        EditorApplication.update -= OnUpdate;
        EditorApplication.update += OnUpdate;
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }



    static ScriptableObject currentToolbar = null;



    static void OnUpdate() {

        if ( currentToolbar == null ) {

            var toolbars = Resources.FindObjectsOfTypeAll( ToolbarType );

            currentToolbar  = toolbars.Length > 0 ? (ScriptableObject)toolbars[ 0 ] : null;

            if ( currentToolbar != null ) {

                AddHandler( currentToolbar );
            }
        }
    }



    static void AddHandler( object toolbar ) {

        FieldInfo root = toolbar.GetType().GetField( "m_Root", BindingFlags.NonPublic | BindingFlags.Instance );
        VisualElement concreteRoot = root.GetValue( toolbar ) as VisualElement;

        VisualElement toolbarZone = concreteRoot.Q( "ToolbarZoneLeftAlign" );
        VisualElement parent = new VisualElement() {

            style = {
                    flexGrow        = 1,
                    flexDirection   = FlexDirection.RowReverse,
                }
        };

        IMGUIContainer container = new IMGUIContainer();
        container.onGUIHandler      += OnToolbarGUI;

        parent.Add( container );
        toolbarZone.Add( parent );
    }



    static void OnToolbarGUI() {

        using ( new GUILayout.HorizontalScope() ) {

            GUIContent buttonContent    = EditorGUIUtility.IconContent( "UnityEditor.GameView" );
            buttonContent.tooltip       = "Play From Boot";

            if ( GUILayout.Button(

                    buttonContent,

                    new GUIStyle( "Command" ) {

                        fontSize        = 16,
                        alignment       = TextAnchor.MiddleCenter,
                        imagePosition   = ImagePosition.ImageAbove,
                        fontStyle       = FontStyle.Bold
                    }
                )
            ) {

                string scenePath = EditorBuildSettings.scenes[ 0 ].path;
                SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>( scenePath );

                if ( sceneAsset == null ) {

                    Debug.Log( $"{scenePath} scene assets does not exist" );

                    return;
                }

                EditorSceneManager.playModeStartScene   = sceneAsset;

                EditorApplication.isPlaying             = true;
            }
        }
    }



    static void OnPlayModeChanged( PlayModeStateChange state ) {

        if ( state == PlayModeStateChange.ExitingPlayMode ) {

            EditorSceneManager.playModeStartScene   = null;
        }
    }
}
