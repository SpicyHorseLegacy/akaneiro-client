using UnityEngine;
using System.Collections;

public class BaseScreenCtrl : MonoBehaviour {

    protected virtual void Awake()
    {
        RegisterInitEvent();
    }

    // start registering delegate from templates after how many templates are ready.
    protected int initDelegateCount = 3;

    /// <summary>
    /// Register init evetn from GUIManager.
    /// </summary>
    protected virtual void RegisterInitEvent()
    {
        GUIManager.Instance.OnInitEndDelegate += TemplateInitEnd;
        GUIManager.Instance.OnScreenManagerDestroy += DestoryAllEvent;
        GUIManager.Instance.OnTemplateInit += RegisterSingleTemplateEvent;
        GUIManager.Instance.OnSingleTemplateDestroy += UnregisterSingleTemplateEvent;
    }

    /// <summary>
    /// When this screen is going to be destroied, release all delegates.
    /// Notice : call base function after release all delegates from templates first.
    /// </summary>
    protected virtual void DestoryAllEvent()
    {
//        Debug.Log(GUIManager.Instance.GetTemplateInitEndCount());
        GUIManager.Instance.OnInitEndDelegate -= TemplateInitEnd;
        GUIManager.Instance.OnScreenManagerDestroy -= DestoryAllEvent;
		GUIManager.Instance.OnTemplateInit -= RegisterSingleTemplateEvent;
        GUIManager.Instance.OnSingleTemplateDestroy -= UnregisterSingleTemplateEvent;
    }

    /// <summary>
    /// If any template in this screen is ready, check if it is the last one. If so, register all delegates from templates.
    /// </summary>
    protected virtual void TemplateInitEnd()
    {
        if (GUIManager.Instance.GetTemplateInitEndCount() == initDelegateCount)
        {
            RegisterTemplateEvent();
        }
    }

    /// <summary>
    /// Register delegates from templates. If something changed in templates, listen and do sth.
    /// </summary>
    protected virtual void RegisterTemplateEvent()
    {
    }

    /// <summary>
    /// When a templated is loaded, call this function to register all template event.
    /// </summary>
    /// <param name="_templateName"> the template's name.</param>
    protected virtual void RegisterSingleTemplateEvent(string _templateName)
    {
    }

    protected virtual void UnregisterSingleTemplateEvent(string _templateName)
    {
    }
}
