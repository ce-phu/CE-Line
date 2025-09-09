using TMPro;
using UnityEngine;

public class DebugLevel : MonoBehaviour
{
    [ SerializeField ] private TMP_InputField level;



    public void Enter( )
    {
        int lv = int.Parse( level.text );

        // GameManager.DebugLevel( lv );
    }
}
