using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

public class FindObjects : EditorWindow 
{
	string searchKey = "";
	string typeName = "";
	string propName = "";
	string opSymbol = "";
	string cmpValue = "";
	
	bool keyChanged = false;
	UnityEngine.Object[] results = null;
	int curSelection = 0;
	
	GUIStyle itemStyle;
	GUIStyle activedItemStyle;
	
	Vector2 scrollPos;
	
	void Awake()
	{
		// Normal item style
		Texture2D bgTextrure = new Texture2D(2, 2);
		var colors = new Color[4]{Color.gray, Color.gray, Color.gray, Color.gray};
		bgTextrure.SetPixels(colors);
		bgTextrure.Apply();
		
		itemStyle = new GUIStyle();
		itemStyle.normal.textColor = Color.black;
		itemStyle.active.textColor = Color.white;
		itemStyle.active.background = bgTextrure;
		itemStyle.padding.left = itemStyle.padding.right = itemStyle.padding.top = itemStyle.padding.bottom = 4;
		
		// Actived item style
		activedItemStyle = new GUIStyle(itemStyle);
		activedItemStyle.normal.textColor = Color.white;
		activedItemStyle.normal.background = bgTextrure;
	}
	
	void OnGUI()
	{
		string newKey = EditorGUILayout.TextField(searchKey);
		if(newKey != searchKey)
		{
			keyChanged = true;
			results = null;
			curSelection = -1;
			searchKey = newKey;
			return;
		}
		
		if(results == null)
			return;
		
		// Draw list items
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		int index = 0;
		foreach(var obj in results)
		{
			if(obj == null)
				continue;
			
			bool clicked = false;
			if(index == curSelection)
        		clicked = GUILayout.Button(obj.name, activedItemStyle);
			else
        		clicked = GUILayout.Button(obj.name, itemStyle);
			
			if(clicked)
			{
				curSelection = index;
				Selection.activeObject = obj;
			}
			
			index++;
		}
		EditorGUILayout.EndScrollView();
	}
	
	bool keepInputError = false;
	void Update()
	{
		if(!keyChanged)
			return;
		
		if(!ParseSearchKey(searchKey))
		{
			if(!keepInputError)
			{
				Debug.Log("Cannot parse the search key! User search key like this \"ClassName.PropName == 42\"");
				keepInputError = true;
			}
		}
		else
			keepInputError = false;
		
		keyChanged = false;
		Type componentType = GetComponentType(typeName);
		if(componentType != null)
			results = FindObjectsOfType(componentType);
		
		if(results == null || propName == "")
			return;
		
		List<UnityEngine.Object> cmpResult = new List<UnityEngine.Object>();
		foreach(UnityEngine.Object obj in results)
		{
			object component = Convert(obj, componentType);
			object propValue;
			Type propType;
			if(!GetValueFromComponent(component, componentType, propName, out propValue, out propType))
				continue;
			
			if(CompareValue(propType, opSymbol, propValue, GetCmpValue(cmpValue, propType)))
				cmpResult.Add(obj);
		}
		
		results = cmpResult.ToArray();
	}
	
	bool ParseSearchKey(string key)
	{
		key = key.Trim();
		Regex regClassName = new Regex("[a-zA-Z][a-zA-Z0-9_]*");
		var matchRes = regClassName.Match(key);
		if(matchRes.Value == key)
		{
			typeName = key;
			propName = "";
			opSymbol = "";
			cmpValue = "";
			return true;
		}
		
		Regex regCmpString = new Regex("([a-zA-Z][a-zA-Z0-9_]*)\\.([a-zA-Z][a-zA-Z0-9_]*)(?:\\s*)((?:[<>])|(?:==)|(?:!=))(?:\\s*)(\\S+)");
		matchRes = regCmpString.Match(key);
		if(matchRes.Value == key)
		{
			typeName = matchRes.Groups[1].Value;
			propName = matchRes.Groups[2].Value;
			opSymbol = matchRes.Groups[3].Value;
			cmpValue = matchRes.Groups[4].Value;
			return true;
		}
		
		return false;
	}
	
	bool CompareValue(Type propType, string opStr, object valLeft, object valRight)
	{
		if(propType == typeof(int))
		{
			switch(opStr)
			{
			case "==":
				return (int)valLeft == (int)valRight;
			case "!=":
				return (int)valLeft != (int)valRight;
			case "<":
				return (int)valLeft < (int)valRight;
			case ">":
				return (int)valLeft > (int)valRight;
			default:
				Debug.Log(propType + " don't surpport operatior " + opStr);
				return false;
			}
		}
		else if(propType == typeof(float))
		{
			switch(opStr)
			{
			case "==":
				return (float)valLeft == (float)valRight;
			case "!=":
				return (float)valLeft != (float)valRight;
			case "<":
				return (float)valLeft < (float)valRight;
			case ">":
				return (float)valLeft > (float)valRight;
			default:
				Debug.Log(propType + " don't surpport operatior " + opStr);
				return false;
			}
		}
		else if(propType.IsEnum)
		{
			switch(opStr)
			{
			case "==":
				return Enum.Equals(valLeft, valRight);
			case "!=":
				return !Enum.Equals(valLeft, valRight);
			default:
				Debug.Log(propType + " don't surpport operatior " + opStr);
				return false;
			}
		}
		
		string opFuncName = "";
		switch(opStr)
		{
		case "==":
			opFuncName = "op_Equality";
			break;
		case "!=":
			opFuncName = "op_Inequality";
			break;
		case "<":
			opFuncName = "op_LessThan";
			break;
		case ">":
			opFuncName = "op_GreaterThan";
			break;
		}
		
		MethodInfo methodInfo = propType.GetMethod(opFuncName);
		if(methodInfo == null)
		{
			Debug.LogError(propType + " have no method " + opFuncName);
			return false;
		}
		
		return (bool)methodInfo.Invoke(null, new object[]{valLeft, valRight});
	}
	
	object GetCmpValue(string valStr, Type valueType)
	{
		if(valueType == typeof(string))
		{
			return (object)valStr;
		}
		else if(valueType.IsEnum)
		{
			try
			{
				return (object)Enum.Parse(valueType, valStr);
			}
			catch(Exception)
			{
				return null;
			}
		}
		
		// Generic types 
		MethodInfo methodInfo = valueType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[]{typeof(string)}, null);
		if(methodInfo == null)
		{
			Debug.LogError(valueType + " have no static Parse method.");
			return null;
		}
		
		return methodInfo.Invoke(null, new object[]{(object)valStr});
	}
	
	static bool GetValueFromComponent(object cp, Type cpType, string propertyName, out object rtValue, out Type rtType)
	{
		// Try field
		FieldInfo fieldInfo = cpType.GetField(propertyName);
		if(fieldInfo != null)
		{
			rtType = fieldInfo.FieldType;
			rtValue = fieldInfo.GetValue(cp);
			
			return true;
		}
		
		// Try property
		PropertyInfo propInfo = cpType.GetProperty(propertyName);
		if(propInfo != null)
		{
			rtType = propInfo.PropertyType;
			rtValue = propInfo.GetGetMethod().Invoke(cp, Type.EmptyTypes);
			
			return true;
		}
		
		Debug.Log(cpType + " don't have property or field named " + propertyName);
		
		rtValue = null;
		rtType = null;
		return false;
	}
	
	static Type GetComponentType(string name)
	{
		if(name == null || name.Length == 0)
			return null;
		
		// Try component
		var assembly = Assembly.Load("UnityEngine");
		Type component = assembly.GetType("UnityEngine." + name);
		if(component != null)
			return component;
		
		// Try custom script
		assembly = Assembly.Load("Assembly-CSharp");
		component = assembly.GetType(name);
		if(component != null && IsTypeInheritFrom(component, "MonoBehaviour"))
			return component;
		
		return null;
	}
	
	static bool IsTypeInheritFrom(Type childclass, string parentClassName)
	{
		Type parentType = childclass;
		do{
			parentType = parentType.BaseType;
			if(parentType.Name == parentClassName)
				return true;
		}
		while(parentType != null);
		
		return false;
	}
	
	static object Convert(object source, Type destinationType)
    {
        if(destinationType == null)
        {
            throw new ArgumentNullException("destinationType");
        }

        if(destinationType.IsGenericType && 
            destinationType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        {
            if (source == null)
            {
                return null;
            }
            destinationType = Nullable.GetUnderlyingType(destinationType);                
        }

        return System.Convert.ChangeType(source, destinationType);
    }
	
	
	[MenuItem("Tools/Find Objects")]
	static void Init()
	{
		FindObjects window = (FindObjects)EditorWindow.GetWindow(typeof(FindObjects));
		window.title = "Find Objects";
	}
}
