using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KarmaGroupManager : MonoBehaviour {

    public enum EnumKarmaState
    {
        Spawn,
        CanChase,
        Chasing,
    }

    public enum EnumKarmaType
    {
        Normal = 1,
        PurpleKarma,
    }

    public SMoneyEnter Info;
    public EnumKarmaState KState
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
            if (_state == EnumKarmaState.Chasing)
            {
                foreach (KarmaController _k in Karmas)
                {
                    iTween.Stop(_k.gameObject, "MoveTo");
                }
            }
        }
    }
    EnumKarmaState _state;

    public KarmaController[] KarmaPrefabs;

    public List<KarmaController> Karmas = new List<KarmaController>();

    public float SpawnTime = 0.5f;
    public float LifeTime = 15;

    public Transform Target;

    protected virtual void Awake()
    {
        KState = EnumKarmaState.Spawn;
    }

    void Update()
    {
        #region Timer Control
        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0)
        {
            DestroyKarmas();
        }

        if (KState == EnumKarmaState.Spawn)
        {
            SpawnTime -= Time.deltaTime;
            if (SpawnTime <= 0)
                KState = EnumKarmaState.CanChase;
        }
        #endregion
    }

    public virtual void PlayerGetKarma(KarmaController _karma)
    {
        for (int i = Karmas.Count - 1; i >= 0; i--)
        {
            KarmaController _k = Karmas[i];
            if (_karma == _k)
            {
                _k.DestroyKarma();
                Karmas.RemoveAt(i);
                Player.GetKarma();
                break;
            }
        }

        if (Karmas.Count == 0) DestroyKarmas();
    }

    public void CreateKarmaWithMoneyInfo(SMoneyEnter _info)
    {
        Info = _info;
        for(int i = 0; i < Info.Distribution.Count;i++)
		{
			SServerMapMoney temp = (SServerMapMoney)Info.Distribution[i];
			
            KarmaController _kPrefab =null;
			foreach( KarmaController prefab in KarmaPrefabs)
			{
				if((int)prefab.KarmaType == temp.ID)
				{
					_kPrefab = prefab;
					break;
				}
			}
			if( _kPrefab != null)
			{
				for(int j = 0; j < temp.Value;j++)
		        {
			        KarmaController _newKarma  = Instantiate(_kPrefab) as KarmaController;
                    Vector3 _pos = Info.pos;
                    _pos.x += Random.Range(-1.0f, 1.0f);
                    _pos.z += Random.Range(-1.0f, 1.0f);
                    _pos.y = CS_SceneInfo.pointOnTheGround(_pos).y + 1;
                    _newKarma.transform.position = _pos;
                    _newKarma.Parent = this;
                    Karmas.Add(_newKarma);
		        }
			}
		}
    }

    public void DestroyKarmas()
    {
        foreach(KarmaController _karma in Karmas)
        {
            _karma.DestroyKarma();
        }
		if(gameObject)
        	Destroy(gameObject);
    }
}
