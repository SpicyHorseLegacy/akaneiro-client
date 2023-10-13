using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Hud_BuffBar_Manager : MonoBehaviour {
	
	public static UI_Hud_BuffBar_Manager Instance;

    public enum Enum_BuffSortType
    {
        NONE,
        TopRight,
        TopLeft,
        BottomRight,
        BottomLeft,
        MAX,
    }

    [SerializeField]  Enum_BuffSortType SortType = Enum_BuffSortType.NONE;
    [SerializeField]  int MaxIndex;
    [SerializeField]  UI_Hud_Buff Buff_Prefab;
    [SerializeField]  GameObject Buff_Parent;

    UI_Hud_Buff[] Buffs = new UI_Hud_Buff[0];

	void Awake()
	{
		Instance = this;
	}

    public void UpdateBuff(BaseBuff _buff)
    {
        UI_Hud_Buff _tempbuff = getUIBuffByID(_buff.ID);
        if (_tempbuff != null)
        {
            _tempbuff.UpdateBuff(_buff);
        }
        else
        {
            _tempbuff = UnityEngine.Object.Instantiate(Buff_Prefab) as UI_Hud_Buff;
            _tempbuff.transform.parent = Buff_Parent.transform;
            _tempbuff.transform.localScale = Vector3.one;
			_tempbuff.UpdateBuff(_buff);
            _buff.Buff_Exit_Event += RemoveBuff;
            _buff.Buff_AddStack_Event += UpdateBuff;

            List<UI_Hud_Buff> _temparray = new List<UI_Hud_Buff>();

            for (int i = 0; i < Buffs.Length; i++)
            {
                _temparray.Add(Buffs[i]);
            }
            _temparray.Add(_tempbuff);
            Buffs = _temparray.ToArray();
            _temparray.Clear();
            _temparray = null;

            RepositionBuffs();
        }
    }

    public void RemoveBuff(BaseBuff _buff)
    {
        List<UI_Hud_Buff> _temparray = new List<UI_Hud_Buff>();
        for (int i = 0; i < Buffs.Length; i++)
        {
			if(Buffs[i] != null)
			{
	            if (Buffs[i].ID == _buff.ID)
	            {
	                Destroy(Buffs[i].gameObject);
	            }
	            else
	            {
	                _temparray.Add(Buffs[i]);
	            }
			}
        }
        Buffs = _temparray.ToArray();
        _temparray.Clear();
        _temparray = null;

        RepositionBuffs();
    }
	
	public void RemoveAllBuffs()
	{
		for (int i = 0; i < Buffs.Length; i++)
        {
			if(Buffs[i].gameObject != null)
				Destroy(Buffs[i].gameObject);
		}
		Buffs = new UI_Hud_Buff[0];
	}

    void RepositionBuffs()
    {
        int _restIcons = Buffs.Length;
        int _startBuffIndex = 0;
        int _row = 0;
        int _leftOrRight = (SortType == Enum_BuffSortType.BottomLeft || SortType == Enum_BuffSortType.TopLeft) ? -1 : 1;
        int _bottomOrTop = (SortType == Enum_BuffSortType.BottomLeft || SortType == Enum_BuffSortType.BottomRight) ? -1 : 1;

        while (_restIcons > 0)
        {
            int _lastBuffIndex = _restIcons < MaxIndex ? _restIcons : MaxIndex;
            _lastBuffIndex += _startBuffIndex;
            for (int i = _startBuffIndex; i < _lastBuffIndex; i++)
            {
                int _idx = i % MaxIndex;
                Buffs[i].transform.localPosition = new Vector3(0 + _idx * 30f * _leftOrRight, 0 + _row * 30f * _bottomOrTop, 0);
            }

            _restIcons -= MaxIndex;
            _startBuffIndex += MaxIndex;
            _row++;
        }

    }

    UI_Hud_Buff getUIBuffByID(int _id)
    {
        for (int i = 0; i < Buffs.Length; i++ )
        {
            if (Buffs[i].ID == _id)
            {
                return Buffs[i];
            }
        }
        return null;
    }
}
