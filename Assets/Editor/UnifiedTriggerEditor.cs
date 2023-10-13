using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Trigger_Unified))]
public class UnifiedTriggerEditor : Editor
{
    private Trigger_Unified trigger_ = null;

    void OnEnable()
    {
        trigger_ = target as Trigger_Unified;
    }
    
    [ExecuteInEditMode]
    public override void OnInspectorGUI()
    {
        Trigger_Unified.UnifiedTriggerType lastType = trigger_.UnifiedType;

        base.OnInspectorGUI();
        EditorGUILayout.LabelField(string.Format("Unity ID: {0}", trigger_.UnityID));

        if (trigger_.UnifiedType != lastType)
        {
            trigger_.OnTypeChanged();
        }
    }

    public void OnSceneGUI()
    {
        Handles.color = Color.red;

        switch (trigger_.UnifiedType)
        {
            case Trigger_Unified.UnifiedTriggerType.Spawn:
                {
                    UnifiedEventData_Spawn data = trigger_.gameObject.GetComponent<UnifiedEventData_Spawn>();
                    foreach (Transform s in data.spawnerList_)
                    {
                        if (s != null)
                        {
                            Handles.DrawLine(trigger_.transform.position, s.position);

                            NpcSpawner spawner = s.GetComponent<NpcSpawner>();
                            spawner.NotifyTriggerLink_Editor(trigger_);
                        }
                    }
                    break;
                }
            case Trigger_Unified.UnifiedTriggerType.PlayAnim:
                {
                    UnifiedEventData_Anim data = trigger_.gameObject.GetComponent<UnifiedEventData_Anim>();

                    foreach (PlayAnimTrigger.AnimationDataStruct AnimData in data.animDataList_)
                    {
                        if (AnimData != null && AnimData.AnimObj)
                            Handles.DrawLine(trigger_.transform.position, AnimData.AnimObj.transform.position);
                    }
                    break;
                }
            case Trigger_Unified.UnifiedTriggerType.Teleport:
                {
                    UnifiedEventData_Teleport data = trigger_.gameObject.GetComponent<UnifiedEventData_Teleport>();
                    if (data.TeleportPos)
                        Handles.DrawLine(trigger_.transform.position, data.TeleportPos.transform.position);
                    break;
                }
            default:
                break;
        }
    }
    


    #region Old

    //private void SetID()
    //{
    //    TriggerBase[] TriggerList = FindObjectsOfType(typeof(TriggerBase)) as TriggerBase[];
    //    Trigger_Unified[] unifiedList = FindObjectsOfType(typeof(Trigger_Unified)) as Trigger_Unified[];

    //    int maxID = 0;
    //    bool bSame = false;
    //    foreach (TriggerBase it in TriggerList)
    //    {
    //        if (it.id > maxID)
    //            maxID = it.id;

    //        if (it != trigger_ && it.id == trigger_.id)
    //            bSame = true;
    //    }
    //    foreach (Trigger_Unified it in unifiedList)
    //    {
    //        if (it.id > maxID)
    //            maxID = it.id;

    //        if (it != trigger_ && it.id == trigger_.id)
    //            bSame = true;
    //    }

    //    Debug.LogWarning(string.Format("MaxID is {0}", maxID));
    //    if (trigger_.id == 0 || bSame)
    //        trigger_.id = ++maxID;
    //}

    //private List<CustomInspectorProp_Base> propList_ = new List<CustomInspectorProp_Base>();

    //void OnEnable()
    //{
    //    propList_.Clear();
    //    propList_.Add(new CustomInspectorProp_Bool(serializedObject, "TriggerOn"));
    //    propList_.Add(new CustomInspectorProp_Int(serializedObject, "id"));
    //    propList_.Add(new CustomInspectorProp_Int(serializedObject, "TriggerType"));
    //    propList_.Add(new CustomInspectorProp_Int(serializedObject, "MaxTriggerCount"));
    //    propList_.Add(new CustomInspectorProp_Float(serializedObject, "TriggerDelayTime"));
    //    propList_.Add(new CustomInspectorProp_Enum(serializedObject, "UnifiedType"));
    //}

    //[ExecuteInEditMode]
    //public override void OnInspectorGUI()
    //{
    //    //base.OnInspectorGUI();

    //    serializedObject.Update();

    //    foreach (CustomInspectorProp_Base prop in propList_)
    //    {
    //        prop.DrawInspector();
    //    }

    //    serializedObject.ApplyModifiedProperties();

    //    Trigger_Unified trigger = (target as Trigger_Unified);
    //    if (trigger.UnifiedType == Trigger_Unified.UnifiedTriggerType.Spawn)
    //    {
    //        //trigger.gameObject. trigger.gameObject.GetComponents<>();
    //        //trigger.gameObject.AddComponent<PF_Speech_Trigger>();
    //    }
    //}

    //#region Prop Class

    //public abstract class CustomInspectorProp_Base
    //{
    //    public SerializedProperty prop_;
    //    public string label_;

    //    public CustomInspectorProp_Base(SerializedObject _obj, string _label)
    //    {
    //        label_ = _label;
    //        prop_ = _obj.FindProperty(label_);
    //    }

    //    public virtual void DrawInspector() { }
    //}

    //public class CustomInspectorProp_Bool : CustomInspectorProp_Base
    //{
    //    public CustomInspectorProp_Bool(SerializedObject _obj, string _label) : base(_obj, _label) { }

    //    public override void DrawInspector()
    //    {
    //        prop_.boolValue = EditorGUILayout.Toggle(label_, prop_.boolValue);
    //    }
    //}

    //public class CustomInspectorProp_Int : CustomInspectorProp_Base
    //{
    //    public CustomInspectorProp_Int(SerializedObject _obj, string _label) : base(_obj, _label) { }

    //    public override void DrawInspector()
    //    {
    //        prop_.intValue = EditorGUILayout.IntField(label_, prop_.intValue);
    //    }
    //}

    //public class CustomInspectorProp_Float : CustomInspectorProp_Base
    //{
    //    public CustomInspectorProp_Float(SerializedObject _obj, string _label) : base(_obj, _label) { }

    //    public override void DrawInspector()
    //    {
    //        prop_.floatValue = EditorGUILayout.FloatField(label_, prop_.floatValue);
    //    }
    //}

    //public class CustomInspectorProp_Enum : CustomInspectorProp_Base
    //{
    //    public CustomInspectorProp_Enum(SerializedObject _obj, string _label) : base(_obj, _label) { }

    //    public override void DrawInspector()
    //    {
    //        prop_.enumValueIndex = (int)(Trigger_Unified.UnifiedTriggerType)EditorGUILayout.EnumPopup(label_, (Trigger_Unified.UnifiedTriggerType)(prop_.enumValueIndex));
    //    }
    //}


    //==========================================================


    //private SerializedProperty triggeronProp;
    //private SerializedProperty idProp;
    //private SerializedProperty triggertypeProp;

    //void OnEnable()
    //{
    //    triggeronProp = serializedObject.FindProperty("TriggerOn");
    //    idProp = serializedObject.FindProperty("id");
    //    triggertypeProp = serializedObject.FindProperty("TriggerType");
    //}

    //public override void OnInspectorGUI()
    //{
    //    serializedObject.Update();

    //    triggeronProp.boolValue = EditorGUILayout.Toggle("Trigger On", triggeronProp.boolValue);
    //    idProp.intValue = EditorGUILayout.IntField("id", idProp.intValue);
    //    triggertypeProp.intValue = EditorGUILayout.IntField("TriggerType", triggertypeProp.intValue);

    //    serializedObject.ApplyModifiedProperties();
    //}

#endregion
}
