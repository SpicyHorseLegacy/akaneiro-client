//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's color.
/// </summary>

[AddComponentMenu("NGUI/Tween/Color")]
public class TweenColor : UITweener
{
	public Color from = Color.white;
	public Color to = Color.white;

	Transform mTrans;
	UIWidget mWidget;
    UIWidget[] mWidgets;
	Material mMat;
	Light mLight;

    bool isContainChildren;

	/// <summary>
	/// Current color.
	/// </summary>

	public Color color
	{
		get
		{
			if (mWidget != null) return mWidget.color;
			if (mLight != null) return mLight.color;
			if (mMat != null) return mMat.color;
			return Color.black;
		}
		set
		{
			if (mWidget != null) mWidget.color = value;
			if (mMat != null) mMat.color = value;

            if (isContainChildren && mWidgets.Length > 0)
            {
                foreach (UIWidget _w in mWidgets)
                {
                    _w.color = value;
                }
            }

			if (mLight != null)
			{
				mLight.color = value;
				mLight.enabled = (value.r + value.g + value.b) > 0.01f;
			}
		}
	}

	/// <summary>
	/// Find all needed components.
	/// </summary>

	void Awake ()
	{
		mWidget = GetComponentInChildren<UIWidget>();
        mWidgets = GetComponentsInChildren<UIWidget>();
		Renderer ren = renderer;
		if (ren != null) mMat = ren.material;
		mLight = light;
	}

	/// <summary>
	/// Interpolate and update the color.
	/// </summary>

	override protected void OnUpdate(float factor, bool isFinished) { color = Color.Lerp(from, to, factor); }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

    static public TweenColor Begin(GameObject go, float duration, Color color)
    {
        TweenColor comp = UITweener.Begin<TweenColor>(go, duration);
        comp.from = comp.color;
        comp.to = color;
        comp.isContainChildren = false;
        return comp;
    }

    static public TweenColor Begin(GameObject go, float duration, Color color, bool containChildren)
    {
        TweenColor comp = UITweener.Begin<TweenColor>(go, duration);
        comp.from = comp.color;
        comp.to = color;
        comp.isContainChildren = containChildren;
        return comp;
    }
}