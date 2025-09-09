using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;



public class EffectUIController : MonoBehaviour
{
    enum Status
    {
        NONE,
        WAITING,
        SPREAD,
        GOTOTARGET,
        FINISH,
    }

    Status     status        = Status.NONE;
    Vector3    startPosition = Vector3.zero;
    Vector3    waitPosition  = Vector3.zero;
    Vector3    goalPosition  = Vector3.zero;
    Quaternion addRot        = Quaternion.identity;
    float      delay         = 0.0f;
    float      duration      = 0.5f;
    float      timer         = 0.0f;
    Image      image         = null;
    Color      imageCol      = Color.white;
    Action     callback      = null;

    [ SerializeField ] GameObject trail = null;


    private RectTransform rt;



    private void Awake( )
    {
        rt = GetComponent< RectTransform >( );
    }



    void Update( )
    {
        switch ( status )
        {
            case Status.WAITING:
            {
                if ( timer < 1.0f )
                {
                    timer += Time.deltaTime / delay;
                }
                else
                {
                    transform.localScale = Vector3.one;
                    status               = Status.SPREAD;
                    timer                = 0.0f;
                }

                break;
            }
            case Status.SPREAD:
            {
                if ( timer < 1.0f )
                {
                    timer += Time.deltaTime / duration;
                    float lerpVal = Mathf.Pow( Mathf.Clamp01( timer ), 1.0f / 3.0f );
                    transform.localScale =  Vector3.Lerp( Vector3.one * 0.5f, Vector3.one,  lerpVal );
                    transform.position   =  Vector3.Lerp( startPosition,      waitPosition, lerpVal );
                    transform.rotation   *= addRot;
                    imageCol.a           =  lerpVal;
                    image.color          =  imageCol;
                }
                else
                {
                    if ( trail != null )
                    {
                        trail.SetActive( true );
                    }

                    imageCol.a           = 1.0f;
                    image.color          = imageCol;
                    transform.localScale = Vector3.one;
                    timer                = 0.0f;
                    status               = Status.GOTOTARGET;
                }

                break;
            }
            case Status.GOTOTARGET:
            {
                if ( timer < 1.0f )
                {
                    timer += Time.deltaTime / duration;
                    float lerpVal = Mathf.Pow( Mathf.Clamp01( timer ), 3.0f / 1.0f );
                    transform.localScale =  Vector3.Lerp( Vector3.one,  Vector3.one * 0.5f, lerpVal );
                    transform.position   =  Vector3.Lerp( waitPosition, goalPosition,       lerpVal );
                    transform.rotation   *= addRot;
                    imageCol.a           =  1.0f - lerpVal;
                    image.color          =  imageCol;
                }
                else
                {
                    imageCol.a           = 0.0f;
                    image.color          = imageCol;
                    transform.localScale = Vector3.one * 0.5f;
                    timer                = 0.0f;
                    status               = Status.FINISH;
                }

                break;
            }
            case Status.FINISH:
            {
                callback?.Invoke( );

                if ( trail != null )
                {
                    trail.SetActive( false );
                }

                status = Status.NONE;

                break;
            }
        }
    }



    public void MoveToPoint( Vector3 _startPosition, Vector3 _waitPosition, Vector3 _goalPosition, float _duration,
        float                        _delay,         Action  _callback = null )
    {
        if ( image == null )
        {
            image = GetComponent< Image >( );
        }

        transform.localScale = Vector3.one * 0.5f;
        startPosition        = _startPosition;
        waitPosition         = _waitPosition;
        goalPosition         = _goalPosition;
        delay                = _delay;
        duration             = _duration;
        addRot               = Quaternion.Euler( 0.0f, 0.0f, Random.Range( -3.0f, 3.0f ) );
        imageCol.a           = 0.0f;
        image.color          = imageCol;

        callback = _callback;
        timer    = 0.0f;
        status   = Status.WAITING;
    }
}