using System;
using UnityEngine;


public enum EffectType
{
    GOTGACHA,
    GACHAGLOW,
    GLOW,
    MAX,
}


public class EffectParam
{
    public Vector3 position = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public Vector3 scale = Vector3.one;
    public float speed = 1.0f;
    public EffectType type = (EffectType)0;


    public EffectParam()
    {
    }


    public EffectParam(Vector3 _postion, Quaternion _rotation, Vector3 _scale, float _speed, EffectType _type)
    {
        position = _postion;
        rotation = _rotation;
        scale = _scale;
        speed = _speed;
        type = _type;
    }
}


public class EffectManager : MonoBehaviour
{
    enum ParticleType
    {
        Paritcle,
        Animation,
    }


    [Serializable]
    struct ParticleInfo
    {
        public GameObject particleSystem;
        public int instanceLimit;
        public ParticleType type;
    }


    [SerializeField] ParticleInfo[] particles = null;
    GameObject[][] instancedObject = null;
    ParticleSystem[][] instancedEffect = null;
    Animator[][] instancedAnimator = null;
    public static EffectManager Instance = null;


    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        instancedObject = new GameObject[particles.Length][];
        instancedEffect = new ParticleSystem[particles.Length][];
        instancedAnimator = new Animator[particles.Length][];

        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].particleSystem != null)
            {
                instancedObject[i] = new GameObject[particles[i].instanceLimit];

                switch (particles[i].type)
                {
                    case ParticleType.Paritcle:

                        instancedEffect[i] = new ParticleSystem[particles[i].instanceLimit];

                        for (int j = 0; j < particles[i].instanceLimit; j++)
                        {
                            instancedObject[i][j] = Instantiate(particles[i].particleSystem);
                            instancedObject[i][j].name = i + "_" + j;
                            instancedObject[i][j].transform.parent = transform;
                            instancedObject[i][j].transform.position = new Vector3(-5000.0f, 5000.0f, 0.0f);
                            instancedObject[i][j].SetActive(false);

                            if (!instancedObject[i][j]
                                    .TryGetComponent(out instancedEffect[i][j]))
                            {
                                Debug.LogError("EffectManager.Start : No particle");
                            }
                        }

                        break;

                    case ParticleType.Animation:

                        instancedAnimator[i] = new Animator[particles[i].instanceLimit];

                        for (int j = 0; j < particles[i].instanceLimit; j++)
                        {
                            instancedObject[i][j] = Instantiate(particles[i].particleSystem);
                            instancedObject[i][j].name = i + "_" + j;
                            instancedObject[i][j].transform.parent = transform;
                            instancedObject[i][j].transform.position = new Vector3(-5000.0f, 5000.0f, 0.0f);

                            if (!instancedObject[i][j]
                                    .TryGetComponent(out instancedAnimator[i][j]))
                            {
                                Debug.LogError("EffectManager.Start : No animator");
                            }
                        }

                        break;
                }
            }
        }

        ClearAllEffect();
    }


    public static GameObject Play(Vector3 _position, Quaternion _rotation, Vector3 _scale, float _speed,
        EffectType _type)
    {
        return Instance._Play(_position, _rotation, _scale, _speed, _type);
    }


    GameObject _Play(Vector3 _position, Quaternion _rotation, Vector3 _scale, float _speed, EffectType _type)
    {
        if ((int)_type >= particles.Length)
        {
            return null;
        }

        switch (particles[(int)_type].type)
        {
            case ParticleType.Paritcle:

                for (int i = 0; i < instancedEffect[(int)_type].Length; i++)
                {
                    if (!instancedEffect[(int)_type][i].IsAlive(true))
                    {
                        instancedEffect[(int)_type][i].gameObject.SetActive(true);
                        instancedObject[(int)_type][i].transform.position = _position;
                        instancedObject[(int)_type][i].transform.rotation = _rotation;
                        instancedObject[(int)_type][i].transform.localScale = _scale;
                        instancedEffect[(int)_type][i].Play();

                        return instancedObject[(int)_type][i];
                    }
                }

                break;

            case ParticleType.Animation:

                for (int i = 0; i < instancedAnimator[(int)_type].Length; i++)
                {
                    AnimatorClipInfo[] clipInfo =
                        instancedAnimator[(int)_type][i].GetCurrentAnimatorClipInfo(0);

                    if (clipInfo[0].clip.name == "Def")
                    {
                        instancedObject[(int)_type][i].transform.position = _position;
                        instancedObject[(int)_type][i].transform.rotation = _rotation;
                        instancedObject[(int)_type][i].transform.localScale = _scale;
                        instancedAnimator[(int)_type][i].speed = _speed;
                        instancedAnimator[(int)_type][i].Play("Anim");

                        return instancedObject[(int)_type][i];
                    }
                }

                break;
        }

        return null;
    }


    public static void Stop(GameObject objEff)
    {
        Instance._Stop(objEff);
    }


    private void _Stop(GameObject objEff)
    {
        if (objEff.GetComponent<ParticleSystem>())
        {
            objEff.GetComponent<ParticleSystem>().Stop();
            objEff.SetActive(false);
        }
        else if (objEff.GetComponent<Animator>())
        {
            objEff.GetComponent<Animator>().Play("Def");
        }
    }


    public static void ClearAllEffect() 
    {
        for (int i = 0; i < (int)EffectType.MAX; i++)
        {
            if (Instance.instancedEffect[i] != null)
            {
                for (int j = 0; j < Instance.instancedEffect[i].Length; j++)
                {
                    Instance.instancedEffect[i][j].Stop();
                    Instance.instancedEffect[i][j].gameObject.SetActive(false);
                }
            }

            if (Instance.instancedAnimator[i] != null)
            {
                for (int j = 0; j < Instance.instancedAnimator[i].Length; j++)
                {
                    Instance.instancedAnimator[i][j].Play("Def");
                }
            }
        }
    }
}