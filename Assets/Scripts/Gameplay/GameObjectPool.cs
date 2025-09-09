using System.Collections.Generic;
using UnityEngine;



public class GameObjectPool : MonoBehaviour
{
    [ SerializeField ] GameObject prefab       = null;
    [ SerializeField ] int        initialLimit = 0;
    HashSet< GameObject >         used         = null;
    Queue< GameObject >           pool         = null;



    public void Init( )
    {
        if ( used == null )
        {
            used = new HashSet< GameObject >( );
        }

        if ( pool == null )
        {
            pool = new Queue< GameObject >( );
        }

        Dispose( );

        for ( int i = 0; i < initialLimit; i++ )
        {
            GameObject obj = Instantiate( prefab, transform );

            obj.SetActive( false );

            pool.Enqueue( obj );
        }
    }



    public GameObject Get( )
    {
        if ( pool.Count == 0 )
        {
            GameObject result = Instantiate( prefab, transform );

            if ( result == null )
            {
                return null;
            }

            pool.Enqueue( result );
        }

        GameObject resultComponent = pool.Dequeue( );

        if ( resultComponent == null )
        {
            return resultComponent;
        }

        used.Add( resultComponent );

        if ( resultComponent.activeSelf == false )
        {
            resultComponent.SetActive( true );
        }

        return resultComponent;
    }



    public bool Release( GameObject _object )
    {
        if ( used.Contains( _object ) )
        {
            _object.SetActive( false );

            pool.Enqueue( _object );
            used.Remove( _object );

            return true;
        }

        return false;
    }



    public void Dispose( )
    {
        ReleaseAllInstances( );

        foreach ( GameObject gameObject in pool )
        {
            GameObject.Destroy( gameObject );
        }

        pool.Clear( );
    }



    public void ReleaseAllInstances( )
    {
        foreach ( GameObject instance in used )
        {
            instance.SetActive( false );

            pool.Enqueue( instance );
        }

        used.Clear( );
    }



    public int GetRemainCount( )
    {
        return pool.Count;
    }
}