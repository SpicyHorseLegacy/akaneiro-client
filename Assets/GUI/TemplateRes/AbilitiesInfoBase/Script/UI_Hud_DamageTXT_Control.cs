using UnityEngine;
using System.Collections;

public class UI_Hud_DamageTXT_Control : MonoBehaviour {

    [SerializeField] UILabel DamageLabel;

    // we need a world pos object to put into the world, otherwise, the damage text couldn't move when camera moves. damage text should follow the damage source.
    [SerializeField] GameObject WorldPos;

    UI_TypeDefine.UI_GameHud_DamageTXT_data DamageData;

    float _lifeTime = -1;

    Vector3 RandomPos;
    int LOR = 0;

    void Update()
    {
        if (DamageData != null)
        {
            if (_lifeTime > 0)
            {
                _lifeTime -= Time.deltaTime;
                Vector3 _screenPos = Camera.main.WorldToScreenPoint(WorldPos.transform.position);
                Vector3 _uiPos = UICamera.currentCamera.ScreenToWorldPoint(_screenPos);

                float offsetX = 0;
                float offsetY = 0;
                switch (DamageData.AniType)
                {
                    case UI_TypeDefine.UI_GameHud_DamageTXT_data.EnumDamageTXTAnimationType.Jump:
                        offsetX = 175 / DamageData.LifeTime * (DamageData.LifeTime - _lifeTime) * LOR;
                        offsetY = Mathf.Abs(Mathf.Sin((DamageData.LifeTime - _lifeTime) / DamageData.LifeTime * 180 * 0.75f * Mathf.Deg2Rad) * 175);
                        break;

                    case UI_TypeDefine.UI_GameHud_DamageTXT_data.EnumDamageTXTAnimationType.LinearUp:
                        offsetY = 175 / DamageData.LifeTime * (DamageData.LifeTime - _lifeTime);
                        break;

                    case UI_TypeDefine.UI_GameHud_DamageTXT_data.EnumDamageTXTAnimationType.LinearDown:
                        offsetY = -1 * 175 / DamageData.LifeTime * (DamageData.LifeTime - _lifeTime);
                        break;
                }

                _uiPos.z = transform.position.z;
                _uiPos = transform.parent.InverseTransformPoint(_uiPos);
                _uiPos += RandomPos + new Vector3(offsetX, offsetY, 0);
                transform.localPosition = _uiPos;
            }
            else
            {
                Dispose();
            }
        }
    }

    public void UpdateAllInfo(UI_TypeDefine.UI_GameHud_DamageTXT_data _data, Vector3 _worldPos)
    {
        UpdateDamageLabel(_data.DamageTEXT, _data.DamageColor);

        DamageData = _data;
        _lifeTime = _data.LifeTime;
        RandomPos = new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0);
        WorldPos.transform.position = _worldPos;
        WorldPos.transform.parent = null;

        LOR = Random.Range(0, 2) == 0 ? -1 : 1;
    }

    void UpdateDamageLabel(string _damage, Color _color)
    {
        DamageLabel.text = _damage;
        DamageLabel.color = _color;
    }

    void Dispose()
    {
        Destroy(WorldPos);
        Destroy(gameObject);
    }
}
