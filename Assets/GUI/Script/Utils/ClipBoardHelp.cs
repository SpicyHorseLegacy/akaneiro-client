using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

public class ClipBoardHelp : MonoBehaviour {
	
	public static ClipBoardHelp Instance;

    private static PropertyInfo m_systemCopyBufferProperty = null;
    private static PropertyInfo GetSystemCopyBufferProperty()
    {
       if (m_systemCopyBufferProperty == null)
       {
         Type T = typeof(GUIUtility);
         m_systemCopyBufferProperty = T.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.NonPublic);
         if (m_systemCopyBufferProperty == null)
          throw new Exception("Can't access internal member 'GUIUtility.systemCopyBuffer' it may have been removed / renamed");
       }
       return m_systemCopyBufferProperty;
    }
	
    public static string clipBoard
    {
       get 
       {
         PropertyInfo P = GetSystemCopyBufferProperty();
         return (string)P.GetValue(null,null);
       }
       set
       {
         PropertyInfo P = GetSystemCopyBufferProperty();
         P.SetValue(null,value,null);
       }
    }
	
	void Awake(){
		Instance = this;
	}

	private Transform currentInput;
	public void SetCurrentObj(Transform obj) {
		currentInput = obj;
	}

	
	void Update () {
		 if (Input.GetKey(KeyCode.LeftControl)){
			if(Input.GetKeyDown(KeyCode.V)){
				if(currentInput != null){
#if NGUI
#else
	        		currentInput.GetComponent<UITextField>().Text = (currentInput.GetComponent<UITextField>().Text + clipBoard);
#endif
				}
			}
     	 }
		//TEST USE THIS, 因为edit中不能使用ctrl+V//
		 if (Input.GetKeyDown(KeyCode.H)){
			if(currentInput != null){
#if NGUI
#else
        		currentInput.GetComponent<UITextField>().Text = (currentInput.GetComponent<UITextField>().Text + clipBoard);
#endif
			}
		 }
	}
	
}
