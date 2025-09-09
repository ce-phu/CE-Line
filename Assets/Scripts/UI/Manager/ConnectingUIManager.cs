using UnityEngine;



public class ConnectingUIManager : MonoBehaviour {

    static ConnectingUIManager Instance     = null;
    Animator anim                           = null;
    bool isPlay                             = false;



    void Awake() {

        Instance    = this;
        anim        = GetComponent<Animator>();
    }



    public static void Play() {

        Instance._Play();
    }



    void _Play() {

        if ( isPlay == false ) {

            anim.Play( "Start" );

            isPlay  = true;
        }
    }



    public static void Stop() {

        Instance._Stop();
    }



    void _Stop() {

        anim.Play( "Def" );

        isPlay  = false;
    }
}
