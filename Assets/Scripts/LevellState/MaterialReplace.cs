//Author:jiangwei
//date:2011-9-6
// need to replaceMaterial for adaptting the variation of time.
using UnityEngine;
using System.Collections;

public class MaterialReplace : MonoBehaviour {

	// Use this for initialization
	public Material[] s1Materials;
	
	public Material[] s2Materials;
	
	public Material[] s3Materials;
	
	public Material[] s4Materials;
	

	private Material[] tempMaterials = null;
	

	void Start () {
	  
	}
	
	// Update is called once per frame
	void Update () {
		
		if( tempMaterials == null)
		{
			tempMaterials = transform.renderer.materials;
		}
		
		if(GlobalGameState.state == "s1" && s1Materials != null)
		{
		   int icount = Mathf.Min(s1Materials.Length,transform.renderer.materials.Length);
		   for(int i = 0; i < icount;i++)
		     tempMaterials[i]  = s1Materials[i];
		}
		else if(GlobalGameState.state == "s2" && s2Materials != null)
		{
		   int icount = Mathf.Min(s2Materials.Length,transform.renderer.materials.Length);
		   for(int i = 0; i < icount;i++)
		     tempMaterials[i] = s2Materials[i];
		}
		else if(GlobalGameState.state == "s3" && s3Materials != null)
		{
		   int icount = Mathf.Min(s3Materials.Length,transform.renderer.materials.Length);
		   for(int i = 0; i < icount ;i++)
		     tempMaterials[i] = s3Materials[i];
		}
		else if(GlobalGameState.state == "s4" && s4Materials != null)
		{
		   int icount = Mathf.Min(s4Materials.Length,transform.renderer.materials.Length);
		   for(int i = 0; i < icount ;i++)
		     tempMaterials[i] = s4Materials[i];
		}
		
		if( tempMaterials != null )
		{
			transform.renderer.materials = tempMaterials;
		}
	
	}
}