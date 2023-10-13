using UnityEngine;
using System.Collections;

public class UI_LevelStar_Single : MonoBehaviour {

    [SerializeField]  UISprite StarSprite;
	[SerializeField]  Transform UI_SFX_StarImpact;

    Vector3 InitSize;

    void Awake()
    {
        InitSize = StarSprite.transform.localScale;
    }

    public void ToggleStar(bool ison)
    {
        if (ison)
        {
            StarSprite.spriteName = "Star_1";
        }else
        {
            StarSprite.spriteName = "Star_2";
        }
    }

    public void Play_Ani_Pop(float _delay)
    {
        TweenDelay.Begin(gameObject, _delay, "Ani_Pop", null);
    }

    void Ani_Pop()
    {
        TweenScale.Begin(StarSprite.gameObject, 0.15f, InitSize * 1.2f);
        TweenDelay.Begin(StarSprite.gameObject, gameObject, 0.15f, "RestoreScale", null);
    }

    public void Play_Ani_Stump(float _delay)
    {
        TweenDelay.Begin(gameObject, _delay, "Ani_Stump", null);
        StarSprite.transform.localScale = Vector3.zero;
        
    }

    void Ani_Stump()
    {
        StarSprite.transform.localScale = InitSize * 2;
        RestoreScale();
		SoundCue.PlayPrefabAndDestroy(UI_SFX_StarImpact);
    }

    void RestoreScale()
    {
        TweenScale.Begin(StarSprite.gameObject, 0.15f, InitSize);
    }
}
