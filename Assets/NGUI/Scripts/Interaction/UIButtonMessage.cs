//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Sends a message to the remote object when something happens.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button Message")]
public class UIButtonMessage : MonoBehaviour
{
	public enum NGUITrigger
	{
        OnClick,
        OnLeftKeyClick,
        OnRightKeyClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		OnDoubleClick,
	}

	public GameObject target;
	public string functionName;
	public NGUITrigger trigger = NGUITrigger.OnClick;
	public bool includeChildren = false;

	bool mStarted = false;
	bool mHighlighted = false;

	void Start () { mStarted = true; }

	void OnEnable () { if (mStarted && mHighlighted) OnHover(UICamera.IsHighlighted(gameObject)); }

	void OnHover (bool isOver)
	{
		if (enabled)
		{
			if (((isOver && trigger == NGUITrigger.OnMouseOver) ||
				(!isOver && trigger == NGUITrigger.OnMouseOut))) Send();
			mHighlighted = isOver;
		}
	}

	void OnPress (bool isPressed)
	{
		if (enabled)
		{
			if (((isPressed && trigger == NGUITrigger.OnPress) ||
				(!isPressed && trigger == NGUITrigger.OnRelease))) Send();
		}
	}

    void OnClick(){ if (enabled && trigger == NGUITrigger.OnClick) Send(); }
    void OnLeftKeyClick() { if (enabled && trigger == NGUITrigger.OnLeftKeyClick) Send(); }
    void OnRightKeyClick() { if (enabled && trigger == NGUITrigger.OnRightKeyClick) Send(); }

	void OnDoubleClick () { if (enabled && trigger == NGUITrigger.OnDoubleClick) Send(); }

	void Send ()
	{
		if (string.IsNullOrEmpty(functionName)) return;
		if (target == null) target = gameObject;

		if (includeChildren)
		{
			Transform[] transforms = target.GetComponentsInChildren<Transform>();

			for (int i = 0, imax = transforms.Length; i < imax; ++i)
			{
				Transform t = transforms[i];
				t.gameObject.SendMessage(functionName, gameObject, SendMessageOptions.DontRequireReceiver);
			}
		}
		else
		{
			target.SendMessage(functionName, gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}
}