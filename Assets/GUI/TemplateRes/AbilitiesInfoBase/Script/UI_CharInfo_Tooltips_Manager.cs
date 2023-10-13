using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_CharInfo_Tooltips_Manager : MonoBehaviour {

    public static UI_CharInfo_Tooltips_Manager Instance = null;

    void Awake()
    {
        Instance = this;
        GUIManager.Instance.AddTemplateInitEnd();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RePositionAllGroup();
        }
    }

    #region Interface

    [SerializeField]
    GameObject Border;

    [SerializeField]
    GameObject Tooltips;

    [SerializeField]
    UISlicedSprite BG;

    [SerializeField]
    GameObject Frames;

    [SerializeField]
    UI_CharInfo_Tooltips_AbiNameGroup AbiNameGroup;

    [SerializeField]
    UI_CharInfo_Tooltips_AbiDetailGroup AbiDetailGroup;

    [SerializeField]
    UI_CharInfo_Tooltips_BaseGroup BottomGroup;

    [SerializeField]
    List<UI_CharInfo_Tooltips_BaseGroup> AllGroup = new List<UI_CharInfo_Tooltips_BaseGroup>();

    public Vector3 RelativeSize;

    bool isMoveToPos = false;

    public void UpdatePosition(Vector3 _pos, Vector3 _borderpos)
    {
        if (_pos.x + RelativeSize.x > 640-215) _pos.x -= RelativeSize.x + 100f;
        if (_pos.y - RelativeSize.y < -360-8) _pos.y += RelativeSize.y;

        if (isMoveToPos)
            TweenPosition.Begin(Tooltips.gameObject, 0.1f, _pos);
        else
            Tooltips.transform.localPosition = _pos;

        Border.transform.localPosition = _borderpos;

        isMoveToPos = true;
    }

    public void Show(UI_CharInfo_Tooltips_Abi.CharInfo_Tooltips_Ability_Data _data)
    {
        Border.SetActive(true);
        Border.GetComponent<UI_Hud_Border_Control>().Pop(0.5f, -1);
        Tooltips.SetActive(true);
        UpdateInfoAsAbi(_data);
    }

    public void Dispose()
    {
        Border.GetComponent<UI_Hud_Border_Control>().Dispose();
        Border.SetActive(false);
        Tooltips.SetActive(false);
        isMoveToPos = false;
    }

    public void UpdateInfoAsAbi(UI_CharInfo_Tooltips_Abi.CharInfo_Tooltips_Ability_Data _data)
    {
        CleanAllGroup();

        UpdateFrameColor(_data.AbiColor);

        AbiNameGroup.gameObject.SetActive(true);
        AbiNameGroup.UpdateAllInfo(_data);
        AllGroup.Add(AbiNameGroup);

        AbiDetailGroup.gameObject.SetActive(true);
        AbiDetailGroup.UpdateAllInfo(_data);
        AllGroup.Add(AbiDetailGroup);

        RePositionAllGroup();
    }

    #endregion

    #region Local

    #region Delegate

    void CloseBTNClicked()
    {
        Dispose();
    }

    #endregion

    void UpdateFrameColor(Color _color)
    {
        foreach (UIWidget _w in Frames.GetComponentsInChildren<UIWidget>())
        {
            _w.color = _color;
        }
    }

    void CleanAllGroup()
    {
        foreach (UI_CharInfo_Tooltips_BaseGroup _group in AllGroup)
        {
            _group.gameObject.SetActive(false);
        }
        AllGroup.Clear();
    }

    void RePositionAllGroup()
    {
        Vector3 _pos = new Vector3(0, -80f, 0);
        if (AllGroup.Count > 0)
        {
            _pos = new Vector3(0, -22.5f, 0);
            for (int i = 0; i < AllGroup.Count; i++)
            {
                UI_CharInfo_Tooltips_BaseGroup _group = AllGroup[i];
                if (_group != null)
                {
                    _group.transform.localPosition = _pos;
                    _pos.y -= _group.GetVisualHeight();
                }
            }
        }

        BG.transform.localScale = new Vector3(BG.transform.localScale.x, -_pos.y, BG.transform.localScale.z);
        BottomGroup.transform.localPosition = _pos;

        Bounds _b = NGUIMath.CalculateRelativeWidgetBounds(Tooltips.transform);
        RelativeSize = new Vector3(_b.extents.x * 2, _b.extents.y * 2, 1);
    }

    #endregion
}
