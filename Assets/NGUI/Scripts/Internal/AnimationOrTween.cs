//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

namespace AnimationOrTween
{
	public enum NGUITrigger
	{
		OnClick,
		OnHover,
		OnPress,
		OnHoverTrue,
		OnHoverFalse,
		OnPressTrue,
		OnPressFalse,
		OnActivate,
		OnActivateTrue,
		OnActivateFalse,
		OnDoubleClick,
		OnSelect,
		OnSelectTrue,
		OnSelectFalse,
	}

	public enum Direction
	{
		Reverse = -1,
		Toggle = 0,
		Forward = 1,
	}

	public enum EnableCondition
	{
		DoNothing = 0,
		EnableThenPlay,
	}

	public enum DisableCondition
	{
		DisableAfterReverse = -1,
		DoNotDisable = 0,
		DisableAfterForward = 1,
	}
}