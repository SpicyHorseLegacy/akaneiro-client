using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_LevelStars_Manager : MonoBehaviour {

    public int CurLevel { get { return _curlevel; } set { _curlevel = value; } }
    int _curlevel;

    [SerializeField]  UI_LevelStar_Single Star_Prefab;
    [SerializeField]  int Distance = 20;
    [SerializeField]  UI_LevelStar_Single[] Starss = new UI_LevelStar_Single[0];

    public void UpdateLevel(int _level, int _maxlevel)
    {
        _curlevel = _level;
        if (_level < 0) _level = 0;

        List<UI_LevelStar_Single> _tempstars = new List<UI_LevelStar_Single>();

        for (int i = 0; i < _level; i++ )
        {
            UI_LevelStar_Single _newstar = FindAStarByID(i);
            _newstar.ToggleStar(true);
            _tempstars.Add(_newstar);
        }

        for (int i = _level; i < _maxlevel; i++)
        {
            UI_LevelStar_Single _newstar = FindAStarByID(i);
            _newstar.ToggleStar(false); 
            _tempstars.Add(_newstar);
        }

        for (int i = _maxlevel; i < Starss.Length; i++)
        {
            if (Starss[i] != null)
                Destroy(Starss[i].gameObject);
        }

        Starss = _tempstars.ToArray();
        RepostionStars();
    }

    public void PopAllStars()
    {
        for (int i = 0; i < Starss.Length;i++ )
        {
            Starss[i].Play_Ani_Pop((i+1) * 0.1f);
        }
    }

    public void StumpAllStars()
    {
        for (int i = 0; i < Starss.Length; i++)
        {
            Starss[i].Play_Ani_Stump((i + 1) * 0.1f);
        }
    }

    #region Local

    UI_LevelStar_Single FindAStarByID(int _index)
    {
        if (_index < Starss.Length)
        {
            return Starss[_index];
        }

        UI_LevelStar_Single _newstar = UnityEngine.Object.Instantiate(Star_Prefab) as UI_LevelStar_Single;
        _newstar.transform.parent = transform;
        _newstar.transform.localPosition = Vector3.zero;
        _newstar.transform.localScale = Vector3.one;
        return _newstar;
    }

    void RepostionStars()
    {
        for (int i = 0; i < Starss.Length; i++)
        {
            Vector3 _temppos = Starss[i].transform.localPosition;
            _temppos.x = i * Distance;
            Starss[i].transform.localPosition = _temppos;
        }
    }

    #endregion
}
 