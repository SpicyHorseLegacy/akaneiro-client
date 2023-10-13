using UnityEngine;
using System.Collections;

[AddComponentMenu("EZ GUI/Controls/Picture")]
public class UIPicture : AutoSpriteControlBase
{
	protected AutoSprite emptySprite;
	[HideInInspector]
	public TextureAnim[] states = new TextureAnim[]
		{
			new TextureAnim("Picture")
		};

	public override TextureAnim[] States
	{
		get { return states; }
		set { states = value; }
	}

	public override EZTransitionList GetTransitions(int index)
	{
		return null;
	}

	public override EZTransitionList[] Transitions
	{
		get { return null; }
		set { }
	}

	/// <summary>
	/// An array of references to sprites which will
	/// visually represent this control.  Each element
	/// (layer) represents another layer to be drawn.
	/// This allows you to use multiple sprites to draw
	/// a single control, achieving a sort of layered
	/// effect. Ex: You can use a second layer to overlay 
	/// a button with a highlight effect.
	/// </summary>
	public SpriteRoot[] layers = new SpriteRoot[0];


	//---------------------------------------------------
	// State tracking:
	//---------------------------------------------------
	protected int[] stateIndices;

	public override void OnInput(ref POINTER_INFO ptr) { }

	public override void Start()
	{
		if (m_started)
			return;

		base.Start();



		// Runtime init stuff:
		if (Application.isPlaying)
		{
			// Assign our aggregate layers:
			aggregateLayers = new SpriteRoot[1][];
			aggregateLayers[0] = layers;

			stateIndices = new int[layers.Length];

			// Populate our state indices based on if we
			// find any valid states/animations in each 
			// sprite layer:
			for (int i = 0; i < layers.Length; ++i)
			{
				if (layers[i] == null)
				{
					Debug.LogError("A null layer sprite was encountered on control \"" + name + "\". Please fill in the layer reference, or remove the empty element.");
					continue;
				}

				stateIndices[i] = layers[i].GetStateIndex("Picture");
				if (stateIndices[i] != -1)
					layers[i].SetState(stateIndices[i]);
			}

			SetState(0);
		}

		// Since hiding while managed depends on
		// setting our mesh extents to 0, and the
		// foregoing code causes us to not be set
		// to 0, re-hide ourselves:
		if (managed && m_hidden)
			Hide(true);
	}

	public override void SetSize(float width, float height)
	{
		base.SetSize(width, height);

		if (emptySprite == null)
			return;

		emptySprite.SetSize(width, height);
	}

	public override void Copy(SpriteRoot s)
	{
		Copy(s, ControlCopyFlags.All);
	}

	public override void Copy(SpriteRoot s, ControlCopyFlags flags)
	{
		base.Copy(s, flags);

		if (!(s is UIPicture))
			return;

		if (Application.isPlaying)
		{
			UIPicture b = (UIPicture)s;


			if ((flags & ControlCopyFlags.Appearance) == ControlCopyFlags.Appearance)
			{
				if (emptySprite != null)
					emptySprite.Copy(b.emptySprite);
			}
		}
	}


	// Sets the default UVs:
	public override void InitUVs()
	{
		if (states[0].spriteFrames.Length != 0)
			frameInfo.Copy(states[0].spriteFrames[0]);

		base.InitUVs();
	}

	public override IUIContainer Container
	{
		get
		{
			return base.Container;
		}

		set
		{
			if (value != container)
			{
				if (container != null)
					container.RemoveChild(emptySprite.gameObject);

				if (value != null)
					if (emptySprite != null)
						value.AddChild(emptySprite.gameObject);
			}

			base.Container = value;
		}
	}

	/// <summary>
	/// Creates a GameObject and attaches this
	/// component type to it.
	/// </summary>
	/// <param name="name">Name to give to the new GameObject.</param>
	/// <param name="pos">Position, in world space, where the new object should be created.</param>
	/// <returns>Returns a reference to the component.</returns>
	static public UIPicture Create(string name, Vector3 pos)
	{
		GameObject go = new GameObject(name);
		go.transform.position = pos;
		return (UIPicture)go.AddComponent(typeof(UIPicture));
	}

	/// <summary>
	/// Creates a GameObject and attaches this
	/// component type to it.
	/// </summary>
	/// <param name="name">Name to give to the new GameObject.</param>
	/// <param name="pos">Position, in world space, where the new object should be created.</param>
	/// <param name="rotation">Rotation of the object.</param>
	/// <returns>Returns a reference to the component.</returns>
	static public UIPicture Create(string name, Vector3 pos, Quaternion rotation)
	{
		GameObject go = new GameObject(name);
		go.transform.position = pos;
		go.transform.rotation = rotation;
		return (UIPicture)go.AddComponent(typeof(UIPicture));
	}


	public override void Hide(bool tf)
	{
		base.Hide(tf);

		if (emptySprite != null)
			emptySprite.Hide(tf);
	}

	public override void SetColor(Color c)
	{
		base.SetColor(c);

		if (emptySprite != null)
			emptySprite.SetColor(c);
	}
}
