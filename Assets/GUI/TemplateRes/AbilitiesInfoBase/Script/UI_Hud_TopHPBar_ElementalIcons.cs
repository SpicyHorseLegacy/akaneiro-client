using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Hud_TopHPBar_ElementalIcons : MonoBehaviour {

    #region Interface
    [SerializeField]  UISprite FlameIcon;
    [SerializeField]  UISprite FrostIcon;
    [SerializeField]  UISprite BlastIcon;
    [SerializeField]  UISprite StormIcon;
    #endregion

    public void UpdateAllInfo(UI_TypeDefine.UI_GameHud_TopHPBar_ElementalIcons_data _data)
    {
        if (_data == null)
        {
            FlameIcon.gameObject.SetActive(false);
            FrostIcon.gameObject.SetActive(false);
            BlastIcon.gameObject.SetActive(false);
            StormIcon.gameObject.SetActive(false);
            return;
        }

        List<UISprite> _tempactivedicons = new List<UISprite>();
        #region initIcons
        if (_data.IsFlame)
        {
            FlameIcon.gameObject.SetActive(true);
            _tempactivedicons.Add(FlameIcon);
        }
        else
        {
            FlameIcon.gameObject.SetActive(false);
        }
        if (_data.IsFrost)
        {
            FrostIcon.gameObject.SetActive(true);
            _tempactivedicons.Add(FrostIcon);
        }
        else
        {
            FrostIcon.gameObject.SetActive(false);
        }
        if (_data.IsBlast)
        {
            BlastIcon.gameObject.SetActive(true);
            _tempactivedicons.Add(BlastIcon);
        }
        else
        {
            BlastIcon.gameObject.SetActive(false);
        }
        if (_data.IsStorm)
        {
            StormIcon.gameObject.SetActive(true);
            _tempactivedicons.Add(StormIcon);
        }
        else
        {
            StormIcon.gameObject.SetActive(false);
        }
        #endregion

        #region Reposition Icons
        if (_tempactivedicons.Count > 0)
        {
            if (_tempactivedicons.Count == 1)
            {
                _tempactivedicons[0].transform.localScale = new Vector3(35, 35, 1);
                _tempactivedicons[0].transform.localPosition = Vector3.zero;
            }
            else
            {
                for (int i = 0; i < _tempactivedicons.Count; i++)
                {
                    _tempactivedicons[0].transform.localScale = new Vector3(24, 24, 1);
                    _tempactivedicons[0].transform.localPosition = new Vector3(12 - (i % 2) * 24, (int) i / 2 * 24, 0);
                }
            }
            _tempactivedicons.Clear();
        }
        _tempactivedicons = null;
        #endregion
    }

}
