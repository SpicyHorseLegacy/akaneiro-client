using UnityEngine;
using System.Collections;

public class UI_Amout_Manager : MonoBehaviour {
    [SerializeField] UILabel Label_count;

    public int Max_Count = 99;
    public int CurCount { get { return _curcount; } }
    int _curcount = 1;

    void Start() { _curcount = 1; Label_count.text = "" + _curcount; }

    public void SetValue(int _value)
    {
        _curcount = _value;
        _curcount = Mathf.Clamp(_curcount, 0, Max_Count);
        Label_count.text = "" + _curcount;
        if (Count_Changed_Event != null) Count_Changed_Event(_curcount);
    }

    void PlusBTN_Clicked()
    {
        _curcount++;
        SetValue(_curcount);
    }

    void MimusBTN_Clicked()
    {
        _curcount--;
        SetValue(_curcount);
    }

    public delegate void UI_Amount_Manager_Count_Changed(int _curcount);
    public event UI_Amount_Manager_Count_Changed Count_Changed_Event;
}
