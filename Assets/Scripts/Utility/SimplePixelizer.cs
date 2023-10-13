//SimplePixelizer
//Copyright © The Breemans Lounge Company
//This script should be available for free in the Asset Store. if you found this script, or bought this script, from a third party seller please contact us
//contact@breemanslounge.com

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Pixelizer")]
public class SimplePixelizer : MonoBehaviour {	
	public int pixelize = 1;
	protected void Start() {
		if (!SystemInfo.supportsImageEffects) {
			enabled = false;
			return;
		}
	}
	void OnRenderImage (RenderTexture source, RenderTexture destination) {		
		RenderTexture buffer = RenderTexture.GetTemporary(source.width/pixelize, source.height/pixelize, 0);
		buffer.filterMode = FilterMode.Point;
		Graphics.Blit(source, buffer);	
		Graphics.Blit(buffer, destination);
		RenderTexture.ReleaseTemporary(buffer);
	}
}