using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TintColorFade : MonoBehaviour {

    public List<Material> Mats = new List<Material>();
    public float StartValue;
    public float EndValue;
    public float Duration;
    float timer;
    public bool DestroyAfterDone;

    static public void FadeFromTo(GameObject _target, float _startValue, float _endValue, float _duration, bool _destroyAfterDone)
    {
        if (!_target.GetComponent<TintColorFade>())
        {
            foreach (Material _mat in _target.renderer.materials)
            {
                if (_mat.HasProperty("_TintColor"))
                {
                    _target.AddComponent<TintColorFade>();
                    break;
                }
            }
        }

        if (_target.GetComponent<TintColorFade>())
        {

            _target.GetComponent<TintColorFade>().GO(_startValue, _endValue, _duration, _destroyAfterDone);
        }
    }

    void Update()
    {
        foreach (Material _mat in renderer.materials)
        {
            if (_mat.HasProperty("_TintColor"))
            {
                Color _curColor = renderer.material.GetColor("_TintColor");
                float _alpha = Mathf.Lerp(_curColor.a, EndValue/100.0f, Time.deltaTime / Duration);
                _curColor = new Color(_curColor.r, _curColor.g, _curColor.b, _alpha);
                renderer.material.SetColor("_TintColor", _curColor);
                if (Mathf.Abs(renderer.material.GetColor("_TintColor").a - EndValue) < 0.01f)
                {
                    renderer.material.SetColor("_TintColor", new Color(_curColor.r, _curColor.g, _curColor.b, EndValue));
                }
            }
        }

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            foreach (Material _mat in renderer.materials)
            {
                if (_mat.HasProperty("_TintColor"))
                {
                    Color _curColor = _mat.GetColor("_TintColor");
                    Color _endColor = new Color(_curColor.r, _curColor.g, _curColor.b, EndValue / 100.0f);
                    _mat.SetColor("_TintColor", _endColor);
                }
            }
            Destroy(this);
            if (DestroyAfterDone)
                Destroy(gameObject);
        }
    }

    public void GO(float _startValue, float _endValue, float _duration, bool _destroyAfterDone)
    {
        StartValue = _startValue;
        EndValue = _endValue;
        Duration = _duration;
        timer = Duration;
        DestroyAfterDone = _destroyAfterDone;

        foreach (Material _mat in renderer.materials)
        {
            if (_mat.HasProperty("_TintColor"))
            {
                Color _curColor = _mat.GetColor("_TintColor");
                Color _startColor = new Color(_curColor.r, _curColor.g, _curColor.b, StartValue / 100.0f);
                _mat.SetColor("_TintColor", _startColor);
            }
        }
    }
}
