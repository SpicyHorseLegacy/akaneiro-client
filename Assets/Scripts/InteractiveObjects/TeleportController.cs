using UnityEngine;
using System.Collections;

public class TeleportController : MonoBehaviour {
	
	void OnTriggerEnter(Collider other)
	{
		if(other.transform.GetComponent<Player>())	Active();
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.transform.GetComponent<Player>())	DisActive();
	}
	
	public void Active()
	{
		Renderer[] Renderers = transform.GetComponentsInChildren<Renderer>();
		foreach(Renderer _renderer in Renderers)
		{
			Material mtl =  _renderer.material;
			/*
	    	if(mtl.HasProperty("_EmissiveColor"))
	        {
				if(InteractionShaderColorDefine.Instance)
					mtl.SetColor("_EmissiveColor", InteractionShaderColorDefine.Instance.Color_Teleport);
				else
					mtl.SetColor("_EmissiveColor", Color.cyan);
	        }
			if(mtl.HasProperty("_EdgeWidth"))
				mtl.SetFloat("_EdgeWidth", mtl.GetFloat("_EdgeWidth")/2);
			*/
			if(mtl.HasProperty("_TintColor")){
				if(!_renderer.transform.GetComponent<FadeMTLInTime>())
					_renderer.transform.gameObject.AddComponent<FadeMTLInTime>();
				_renderer.transform.GetComponent<FadeMTLInTime>().StartWithOpacityAndTime(0.25f, 0.2f);
			}
		}
	}
	
	public void DisActive()
	{
		Renderer[] Renderers = transform.GetComponentsInChildren<Renderer>();
		foreach(Renderer _renderer in Renderers)
		{
			Material mtl =  _renderer.material;
			/*
	    	if(mtl.HasProperty("_EmissiveColor"))
	        {
				mtl.SetColor("_EmissiveColor", Color.black);
	        }
			if(mtl.HasProperty("_EdgeWidth"))
				mtl.SetFloat("_EdgeWidth", mtl.GetFloat("_EdgeWidth")*2);
			*/
			if(mtl.HasProperty("_TintColor")){
				if(!_renderer.transform.GetComponent<FadeMTLInTime>())
					_renderer.transform.gameObject.AddComponent<FadeMTLInTime>();
				_renderer.transform.GetComponent<FadeMTLInTime>().StartWithOpacityAndTime(0.125f, 0.2f);
			}
		}
	}
}
