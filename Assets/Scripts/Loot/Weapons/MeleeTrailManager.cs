using UnityEngine;
using System.Collections;

public class MeleeTrailManager : MonoBehaviour {
	
	static public MeleeTrailManager Instance;
	
	public Color[] None;
	public Color[] Ruby;
	public Color[] Sapphire;
	public Color[] Emerald;
	public Color[] Garnet;
	public Color[] Amethyst;
	public Color[] Malachite;
	public Color[] Obsidian;
	public Color[] Golden;
	public Color[] Jade;
	
	void Awake () {
		Instance = this;
	}
}
