using UnityEngine;
using System.Collections;

public class MaterialObject : MonoBehaviour{
	
	[SerializeField]
	private UITexture icon;
	[SerializeField]
	private UILabel count;
	
	public UITexture GetIcon() {
		return icon;
	}
	
	public void SetCount(int _count){
		count.text = _count.ToString();
		
	}
	
	
}
