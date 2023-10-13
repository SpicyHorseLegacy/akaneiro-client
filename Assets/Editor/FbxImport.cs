using UnityEngine;
using System.Collections;
using UnityEditor;

public class FBXScaleFix : AssetPostprocessor
{
    public void OnPreprocessModel()
    {
        ModelImporter modelImporter = (ModelImporter) assetImporter;                    
        modelImporter.globalScale = 0.01f;      
		modelImporter.swapUVChannels=false;
    }   
}