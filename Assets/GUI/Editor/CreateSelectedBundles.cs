using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

class CreateSelectedBundles{
	
	[MenuItem("GUI/CreateSelectedBundles")]
	
	static void Execute() {
		EditorWindow.GetWindow<UICreateSelectedBundles>(false,"Add file",true);
	}
}
