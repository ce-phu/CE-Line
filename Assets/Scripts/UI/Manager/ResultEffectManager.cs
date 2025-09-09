using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ResultEffectManager : MonoBehaviour
{
    [SerializeField] private Transform startCoinPos;
    [SerializeField] private Transform startStarDifficultPos;
    [SerializeField] private Transform startStarClearChainPos;
    [SerializeField] private Transform midCoinPos;
    [SerializeField] private Transform midDifficultStarPos;
    [SerializeField] private Transform midClearChainStarPos;
    [SerializeField] private Transform endCoinPos;
    [SerializeField] private Transform endStarPos;
    private List<EffectUIController> listEffect = new List<EffectUIController>();
    [SerializeField] GameObjectPool poolStarDifficult    = null;
    [SerializeField] GameObjectPool poolStarClearChain    = null;
    [SerializeField] GameObjectPool poolCoin    = null;



    private void Awake()
    {
        poolStarDifficult.Init();
        poolStarClearChain.Init();
        poolCoin.Init();
    }



    public void PlayCoinEffect( float _duration, Action _onComplete = null )
    {
		PlayEffectMovement( poolCoin, startCoinPos, midCoinPos, endCoinPos, _duration, _onComplete );
    }



    public void PlayStarDifficultEffect( float _duration, Action _onComplete = null )
    {
		PlayEffectMovement( poolStarDifficult, startStarDifficultPos, midDifficultStarPos, endStarPos, _duration, _onComplete );
    }



    public void PlayStarClearChainEffect( float _duration, Action _onComplete = null )
    {
		PlayEffectMovement( poolStarClearChain, startStarClearChainPos, midClearChainStarPos, endStarPos, _duration, _onComplete );
    }



    void PlayEffectMovement( GameObjectPool _pool, Transform _startPos, Transform _midTargetPos, Transform _endPos, float _duration, Action _onComplete = null )
    {
        GameObject obj              = _pool.Get();
        EffectUIController effect   = obj.GetComponent<EffectUIController>();
        listEffect.Add( effect );

        // Random offset around mid point
        Quaternion rotation = Quaternion.Euler( 0f, 0f, Random.Range( -180f, 180f ) );
        Vector3 offset      = ( rotation * Vector3.up ) * Random.Range( 5f, 50f );

        effect.MoveToPoint(

            _startPos.position,
			_midTargetPos.position + offset,
			_endPos.position,
			_duration,
            0f,
            () =>
            {
				_onComplete?.Invoke();
                listEffect.Remove( effect );
                _pool.Release( effect.gameObject );
            });
    }
}
