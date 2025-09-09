using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HomeEffectManager : MonoBehaviour
{
    public static  HomeEffectManager Instance;
    
    private Animator anim;
    [SerializeField] private Transform coinTf;
    [SerializeField] private Transform effCoinPos;
    [SerializeField] private Button[] listBtn;
    private bool isCompleteAnimation;
    private List<EffectUIController> listEffect = new List<EffectUIController>();
    [SerializeField] GameObjectPool poolCoin    = null;



    void Awake()
    {
        Instance = this;
        
        anim = GetComponent<Animator>();
        poolCoin.Init();
    }


    public void Play() {

        StartCoroutine( Cor_Play() );
    }



    IEnumerator Cor_Play() {

        anim.Play( "HalfFadeIn" );
        
        int coinCount   = Player.Data.gold - Player.Data.lastGold;
        Player.Instance.UseGold(coinCount, false);
        int loopCount   = Mathf.Min( coinCount, 10 );
        int incPerCoin  = coinCount < 10 ? 1 : coinCount / 10;
        int lastCoin    = coinCount - incPerCoin * ( loopCount - 1 );

        for ( int i = 0; i < loopCount; i++ ) {

            GameObject obj              = poolCoin.Get();
            EffectUIController effect   = obj.GetComponent<EffectUIController>();
            listEffect.Add( effect );

            Quaternion rotation         = Quaternion.Euler( 0.0f, 0.0f, Random.Range( -180.0f, 180.0f ) );
            Vector3 offset              = ( rotation * Vector3.up ) * Random.Range( 5.0f, 50.0f );
            int incCoin                 = i == loopCount - 1 ? lastCoin : incPerCoin;

            effect.MoveToPoint( transform.position, effCoinPos.position + offset, coinTf.position, 0.5f, 0.0f,
                () => {

                    // SoundManager.PlaySE( SE.SPIN_PRIZE );

                    // GameManager.prevCoin += incCoin;
                    // HomeUIManager.SetCoin( GameManager.prevCoin );
                    Player.Instance.AddGold(incCoin);

                    listEffect.Remove( effect );
                    poolCoin.Release( effect.gameObject );
                } );

            yield return new WaitForSeconds( 0.025f );
        }
        
        yield return new WaitForSeconds( 0.75f );
        anim.Play("HalfFadeOut");

        HomeUIManager.Instance.AnimationCompleted();
    }



    public void CompleteAnimation()
    {
        isCompleteAnimation = true;
    }
}