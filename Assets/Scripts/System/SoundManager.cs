using System.Collections;
using UnityEngine;



public enum SE
{
    UI_BTNCLICK,
    UI_CLOSEBTNCLICK,
    UI_PANELSHOW,
    UI_PANELCLOSE,
    SPIN_START,
    SPIN_SPINNING,
    SPIN_PRIZE,
    SHOP_PURCHASED,
    ITEM_FREEZESTART,
    ITEM_FREEZEEND,
    ITEM_BOMBSTART,
    ITEM_BOMBEND,
	ITEM_HAMMERSTART,
	ITEM_HAMMEREND,
	BLOCK_STARTDRAG,
    BLOCK_ENDDRAG,
	BLOCK_TRANSFORM,
	BLOCK_MOCHIJUMP,
	BLOCK_COLLECTSUCCESS,
	BLOCK_WHOOSH,
	LEVEL_CLEAR,
	LEVEL_FAIL,
    LEVEL_START,
    COIN_APPEAR,
    COIN_REACHED,
}

public enum BGM
{
    MENU,
    GAMEPLAY,
}


public class SoundManager : MonoBehaviour
{
    static             SoundManager   Instance       = null;
    [ SerializeField ] AudioClip[ ]   seClip         = null;
    [ SerializeField ] AudioSource[ ] seSource       = null;
    [ SerializeField ] AudioClip[ ]      bgmClip        = null;
    [ SerializeField ] AudioSource    bgmSource      = null;
    int                               currentASIndex = 0;



    void Awake( )
    {
        Instance = this;
    }



    public static void PlaySE( SE _se )
    {
        PlaySE( _se, 1.0f, false );
    }



    public static void PlaySE( SE _se, float _pitch, bool _isLoop )
    {
        Instance._PlaySE( _se, _pitch, _isLoop );
    }



    void _PlaySE( SE _se, float _pitch, bool _isLoop )
    {
        if ( ( int )_se >= seClip.Length )
        {
            Debug.LogWarning( "SoundManager._PlaySE -> index is out of bounds." );
            return;
        }

        if ( seClip[ ( int )_se ] == null )
        {
            Debug.LogWarning( "SoundManager._PlaySE -> se " + _se.ToString( ) + " is null." );
            return;
        }

        if ( seSource[ currentASIndex ].isPlaying )
        {
            seSource[ currentASIndex ].Stop( );
        }

        seSource[ currentASIndex ].clip      = seClip[ ( int )_se ];
        seSource[ currentASIndex ].panStereo = 0.0f;
        seSource[ currentASIndex ].volume    = SaveDataManager.data.soundvalue;
        seSource[ currentASIndex ].pitch     = _pitch;
        seSource[ currentASIndex ].loop      = _isLoop;
        seSource[ currentASIndex ].Play( );

        currentASIndex++;

        if ( currentASIndex >= seSource.Length )
        {
            currentASIndex = 0;
        }
    }



    public static void StopSE( SE _se ) {

        Instance._StopSE( _se );
	}



	void _StopSE( SE _se ) {

		if ( (int)_se >= seClip.Length ) {
			Debug.Log( "SoundManager._PlaySE -> index is out of bounds." );
			return;
		}

		if ( seClip[ (int)_se ] == null ) {
			Debug.Log( "SoundManager._PlaySE -> se " + _se.ToString() + " is null." );
			return;
		}

        for ( int i = 0; i < seSource.Length; i++ ) {

			if ( seSource[ i ].clip != null && seSource[ i ].clip.name == seClip[ (int)_se ].name ) {

				seSource[ i ].Stop();
			}
		}
	}



	public static void PlayBGM( BGM bgmSound )
    {
        Instance._PlayBGM( bgmSound );
    }



    void _PlayBGM( BGM _bgmSound )
    {
        if (bgmSource.isPlaying)
        {
            StartCoroutine(Wait());
            IEnumerator Wait()
            {
                float duration = .5f;
                float elapsed = 0;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    bgmSource.volume = Mathf.Lerp(SaveDataManager.data.BGMvalue, 0, elapsed / duration);
                    yield return null;
                }

                bgmSource.volume = 0;

                bgmSource.clip   = bgmClip[ (int)_bgmSound ];
                bgmSource.loop   = true;
                bgmSource.Play( );

                elapsed = 0;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    bgmSource.volume = Mathf.Lerp(0, SaveDataManager.data.BGMvalue, elapsed / duration);
                    yield return null;
                }

                bgmSource.volume = SaveDataManager.data.BGMvalue;
            }
        }
        else
        {
            bgmSource.clip   = bgmClip[ (int)_bgmSound ];
            bgmSource.loop   = true;
            bgmSource.volume = 0;
            bgmSource.Play( ); 
           
            StartCoroutine(Wait());
            IEnumerator Wait()
            {
                float duration = .5f;
                float elapsed = 0;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    bgmSource.volume = Mathf.Lerp(0, SaveDataManager.data.BGMvalue, elapsed / duration);
                    yield return null;
                }

                bgmSource.volume = SaveDataManager.data.BGMvalue;
            }
        }
    }



    public static bool IsPlayMenuBGM( )
    {
        return Instance._IsPlayMenuBGM( );
    }



    bool _IsPlayMenuBGM( )
    {
        return bgmSource.clip == bgmClip[0];
    }
    
    public static bool IsPlayIngameBGM( )
    {
        return Instance._IsPlayIngameBGM( );
    }



    bool _IsPlayIngameBGM( )
    {
        return bgmSource.clip == bgmClip[1];
    }



    public static void PauseBGM( )
    {
        Instance._PauseBGM( );
    }



    void _PauseBGM( )
    {
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            float duration = .5f;
            float elapsed = 0;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(SaveDataManager.data.BGMvalue, 0, elapsed / duration);
                yield return null;
            }

            bgmSource.volume = 0;
            bgmSource.Pause( );
        }
    }

    public static void ResumeBGM()
    {
        Instance._ResumeBGM();
    }
    
    void _ResumeBGM( )
    {
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            bgmSource.UnPause( );
            float duration = .5f;
            float elapsed = 0;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(0, SaveDataManager.data.BGMvalue, elapsed / duration);
                yield return null;
            }

            bgmSource.volume = SaveDataManager.data.BGMvalue;
        }
    }
    public static void SetBGMVolume( )
    {
        Instance._SetBGMVolume( );
    }



    void _SetBGMVolume( )
    {
        var startValue = bgmSource.volume;
        
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            float duration = .5f;
            float elapsed = 0;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(startValue, SaveDataManager.data.BGMvalue, elapsed / duration);
                yield return null;
            }

            bgmSource.volume = SaveDataManager.data.BGMvalue;
        }
    }
}