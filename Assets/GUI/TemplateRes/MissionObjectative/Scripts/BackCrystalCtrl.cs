using UnityEngine;
using System.Collections;

public class BackCrystalCtrl : MonoBehaviour {
	
	public static BackCrystalCtrl Instance;

	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private Transform move;
	public void Play() {
		TweenDelay.Begin(gameObject,1,"DelayDelegate",null);
	}
	
	private void DelayDelegate() {
		TweenPosition.Begin(move.gameObject,0.5f,new Vector3(0,0,0));
	}
	
	public delegate void Handle_BackCrystalDelegate();
    public event Handle_BackCrystalDelegate OnBackCrystalDelegate;
	private void BackCrystalDelegate() {
		if(OnBackCrystalDelegate != null) {
			OnBackCrystalDelegate();
		}
	}
}
