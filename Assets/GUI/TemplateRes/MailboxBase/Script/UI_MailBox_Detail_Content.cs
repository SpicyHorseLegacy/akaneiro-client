using UnityEngine;
using System.Collections;

public class UI_MailBox_Detail_Content : MonoBehaviour
{

    [SerializeField]
    float offsetY = 50;

    [SerializeField]
    UISprite _contentbackground;
    [SerializeField]
    UISprite _contentbottomline;

    [SerializeField]
    UILabel _contentlabel;

    public void UpdateMailContent(string _content)
    {
        string _tempcontent = _content;
        if (_tempcontent == "")
            _tempcontent = "NONE.";
        _contentlabel.text = _tempcontent;
        _contentbackground.transform.localScale = _contentlabel.relativeSize * _contentlabel.transform.localScale.x + new Vector2(20, 60);
        Vector3 _pos = _contentbottomline.transform.localPosition;
        _pos.y = -_contentbackground.transform.localScale.y + 22;
        _contentbottomline.transform.localPosition = _pos;
    }

    public float GetItemGridPosY()
    {
        return _contentbackground.transform.localPosition.y - _contentbackground.transform.localScale.y - offsetY;
    }
}
