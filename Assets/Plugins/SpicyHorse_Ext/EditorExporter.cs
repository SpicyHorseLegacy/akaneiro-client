using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseExportNode : MonoBehaviour
{
	virtual public string DoExport() { return ""; }
	virtual public string GetNodeType() { return ""; }
	[HideInInspector]
	public bool bFinish = false;
}



public class BaseExporter : MonoBehaviour
{
	virtual public bool DoExport(string strOutPath, string strKeyName) { return true; }
	virtual public string GetExporterType() { return ""; }
	virtual public string GetExporterName() { return ""; }
}


