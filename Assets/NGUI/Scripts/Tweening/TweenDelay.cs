using UnityEngine;
using System.Collections;

public class TweenDelay : MonoBehaviour {

    public static void Begin(GameObject _gameobject, GameObject _eventReceiver, float _delay, string _func, object _param)
    {
        TweenDelay _tween = _gameobject.GetComponent<TweenDelay>();
        if (!_tween)
            _tween = _gameobject.AddComponent<TweenDelay>();

        _tween._eventReceiver = _eventReceiver;
        _tween._Delay = _delay;
        _tween._Func = _func;
        _tween._Param = _param;
        _tween.enabled = true;
    }

    public static void Begin(GameObject _gameobject, float _delay, string _func, object _param)
    {
        Begin(_gameobject, _gameobject, _delay, _func, _param);
    }

    GameObject _eventReceiver;
    public float _Delay = -1f;
    public string _Func;
    object _Param;

    void Update()
    {
        if (_Delay != -1 && _Delay > 0)
        {
            _Delay -= Time.deltaTime;
            if (_Delay < 0)
            {
				this.enabled = false;
                _eventReceiver.SendMessage(_Func, _Param, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
