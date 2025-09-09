using UnityEngine;
using UnityEngine.UI;



public class FadeManager : MonoBehaviour {

    static FadeManager Instance = null;
    [SerializeField]
    Image fadeImage             = null;
    [SerializeField]
    Animator anim               = null;
    bool isFading               = false;
    bool whileFade              = false;
    bool willOpen               = false;

    enum Anim {

        DEF,
        IN,
        OUT,
        KEEP,
        CLEAR,
    }

    int[] animNameHash      = {

        Animator.StringToHash( "Def" ),
        Animator.StringToHash( "In" ),
        Animator.StringToHash( "Out" ),
        Animator.StringToHash( "Keep" ),
        Animator.StringToHash( "Clear" ),
    };



	void Awake() {

        Instance    = this;
    }



	public static void FadeIn( Color _color, float _time ) {

        Instance._FadeIn( _color, _time );
    }
    
    
    
    void _FadeIn( Color _color, float _time ) {

        fadeImage.color    = _color;
        anim.speed         = 1.0f / _time;
        anim.Play( animNameHash[ (int)Anim.IN ] );
        anim.Play( "In" );

        isFading   = true;
        willOpen   = true;
    }



    public static void FadeOut( Color _color, float _time ) {

        Instance._FadeOut( _color, _time );
    }



    void _FadeOut( Color _color, float _time ) {

        fadeImage.color    = _color;
        anim.speed         = 1.0f / _time;
        anim.Play( animNameHash[ (int)Anim.OUT ] );

        isFading   = true;
        whileFade  = true;
    }



    public static void Clear() {

        Instance._Clear();
    }



    void _Clear() {

        anim.Play( animNameHash[ (int)Anim.CLEAR ] );

        isFading    = false;
        whileFade   = false;
        willOpen    = false;
    }



    public static void KeepColor( Color _color ) {

        Instance._KeepColor( _color );
    }



    void _KeepColor( Color _color ) {

        fadeImage.color    = _color;
        anim.Play( animNameHash[ (int)Anim.KEEP ] );

        isFading   = true;
        whileFade  = true;
        willOpen   = false;
    }



    void CompleteAnimation() {
        // Debug.Log("finished");
        isFading    = false;

        if ( willOpen ) {

            whileFade   = false;
            willOpen    = false;
        }
    }



    public static bool IsFading() {

        return Instance.isFading;
    }



    public static bool WhileFade() {

        return Instance.whileFade;
    }



    public static bool isAlive() {

        return Instance != null;
    }
}
