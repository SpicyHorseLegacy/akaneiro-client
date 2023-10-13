using UnityEngine;
using System.Collections;

public class UI_Hud_Border_Control : MonoBehaviour {

    [SerializeField]  GameObject Corner_TL;
    [SerializeField]  GameObject Corner_TR;
    [SerializeField]  GameObject Corner_BL;
    [SerializeField]  GameObject Corner_BR;

    Vector3 _initPos_TL;
    Vector3 _initPos_RL;
    Vector3 _initPos_BL;
    Vector3 _initPos_BR;

    public int poprepeat = -1;

    void Awake()
    {
        _initPos_TL = Corner_TL.transform.localPosition;
        _initPos_RL = Corner_TR.transform.localPosition;
        _initPos_BL = Corner_BL.transform.localPosition;
        _initPos_BR = Corner_BR.transform.localPosition;
		
		if (poprepeat > 0 || poprepeat == -1)
        {
            Pop(1, poprepeat);
        }
    }
	
	public enum CornerType{
		TL = 0,
		TR,
		BL,
		BR,
		MAX
	}
	
	public void SetPosY(CornerType type,float y) {
		switch(type) {
		case CornerType.TL:
			Corner_TL.transform.localPosition = new Vector3(Corner_TL.transform.localPosition.x,y,Corner_TL.transform.localPosition.z);
			break;
		case CornerType.TR:
			Corner_TR.transform.localPosition = new Vector3(Corner_TR.transform.localPosition.x,y,Corner_TR.transform.localPosition.z);
			break;
		case CornerType.BL:
			Corner_BL.transform.localPosition = new Vector3(Corner_BL.transform.localPosition.x,y,Corner_BL.transform.localPosition.z);
			break;
		case CornerType.BR:
			Corner_BR.transform.localPosition = new Vector3(Corner_BR.transform.localPosition.x,y,Corner_BR.transform.localPosition.z);
			break;
		}
	}
	
    public void Pop(float _time, int repeat)
    {
        poprepeat = repeat;

        Corner_TL.transform.localPosition = _initPos_TL;
        Corner_TR.transform.localPosition = _initPos_RL;
        Corner_BL.transform.localPosition = _initPos_BL;
        Corner_BR.transform.localPosition = _initPos_BR;
        TweenPosition.Begin(Corner_TL, _time / 2f, _initPos_TL * 1.1f);
        TweenPosition.Begin(Corner_TR, _time / 2f, _initPos_RL * 1.1f);
        TweenPosition.Begin(Corner_BL, _time / 2f, _initPos_BL * 1.1f);
        TweenPosition.Begin(Corner_BR, _time / 2f, _initPos_BR * 1.1f);

        TweenDelay.Begin(gameObject, _time / 2f, "RestoreSelf", _time);

        if (repeat != -1)
            poprepeat--;
    }

    void RestoreSelf(float _time)
    {
        TweenPosition.Begin(Corner_TL, _time / 2f, _initPos_TL);
        TweenPosition.Begin(Corner_TR, _time / 2f, _initPos_RL);
        TweenPosition.Begin(Corner_BL, _time / 2f, _initPos_BL);
        TweenPosition.Begin(Corner_BR, _time / 2f, _initPos_BR);

        TweenDelay.Begin(gameObject, _time / 2f, "PopFinish", _time);
    }

    void PopFinish(float _time)
    {
        if (poprepeat > 0 || poprepeat == -1)
        {
            Pop(_time, poprepeat);
        }
    }

    public void Dispose()
    {
        Corner_TL.transform.localPosition = _initPos_TL;
        Corner_TR.transform.localPosition = _initPos_RL;
        Corner_BL.transform.localPosition = _initPos_BL;
        Corner_BR.transform.localPosition = _initPos_BR;
    }

    public void ChangeColor(Color _color)
    {
        TweenColor.Begin(gameObject, 0.25f, _color, true);
    }

    public void SetAlpha(float _a)
    {
        foreach (UIWidget _w in GetComponentsInChildren<UIWidget>())
        {
            _w.alpha = _a;
        }
    }
}
