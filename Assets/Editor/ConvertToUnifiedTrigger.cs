using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ConvertToUnifiedTrigger 
{
    private static List<Object> unifiedTtriggerList_ = new List<Object>();

    [MenuItem("Tools/Convert To Unified Trigger")]
    static void Execute()
    {
        if (Selection.gameObjects.Length == 0)
        {
            Debug.LogError(string.Format(" No Trigger Objects Seleceted "));
            return;
        }

        unifiedTtriggerList_.Clear();

        foreach (GameObject obj in Selection.gameObjects)
        {
            if (obj == null)
            {
                Debug.LogError(" Obj is null ");
                continue;
            }

            TriggerBase script = obj.GetComponent<TriggerBase>();
            if (script == null)
            {
                Debug.LogError(string.Format(" {0} is not a Trigger ", obj.name));
                continue;
            }

            Object newTtrigger = ConvertTrigger(script);
            if (newTtrigger)
                unifiedTtriggerList_.Add(newTtrigger);
        }

        Selection.objects = unifiedTtriggerList_.ToArray();
    }

    static GameObject ConvertTrigger(TriggerBase _trigger)
    {
        GameObject trigger = null;

        if (_trigger is Trigger)
        {
            Trigger_Unified unified = CreateUnifiedTrigger(_trigger.gameObject);
            trigger = unified.gameObject;
            unified.UnifiedType = Trigger_Unified.UnifiedTriggerType.Spawn;
            unified.OnTypeChanged();

            Trigger spawn = _trigger as Trigger;
            unified.TriggerOn = spawn.TriggerOn;
            unified.TriggerType = spawn.TriggerType;
            unified.TriggerDelayTime = spawn.TriggerDelayTime;
            unified.MaxTriggerCount = spawn.MaxTriggerCount;

            UnifiedEventData_Spawn data = unified.gameObject.GetComponent<UnifiedEventData_Spawn>();
            data.spawnerList_.AddRange(spawn.TriggerNpcList);
        }
        else if (_trigger is PlayAnimTrigger)
        {
            Trigger_Unified unified = CreateUnifiedTrigger(_trigger.gameObject);
            trigger = unified.gameObject;
            unified.UnifiedType = Trigger_Unified.UnifiedTriggerType.PlayAnim;
            unified.OnTypeChanged();

            PlayAnimTrigger anim = _trigger as PlayAnimTrigger;
            unified.TriggerOn = anim.TriggerOn;
            unified.TriggerType = anim.TriggerType;
            unified.TriggerDelayTime = anim.TriggerDelayTime;
            unified.MaxTriggerCount = anim.MaxTriggerCount;

            UnifiedEventData_Anim data = unified.gameObject.GetComponent<UnifiedEventData_Anim>();
            data.animDataList_.AddRange(anim.AnimDataArrays);
        }
        else if (_trigger is ActiveCollisionTrigger)
        {
            Trigger_Unified unified = CreateUnifiedTrigger(_trigger.gameObject);
            trigger = unified.gameObject;
            unified.UnifiedType = Trigger_Unified.UnifiedTriggerType.PlayAnim;
            unified.OnTypeChanged();

            ActiveCollisionTrigger collision = _trigger as ActiveCollisionTrigger;
            unified.TriggerOn = collision.TriggerOn;
            unified.TriggerType = collision.TriggerType;
            unified.TriggerDelayTime = collision.TriggerDelayTime;
            unified.MaxTriggerCount = collision.MaxTriggerCount;

            UnifiedEventData_Anim data = unified.gameObject.GetComponent<UnifiedEventData_Anim>();
            data.animDataList_.AddRange(collision.AnimDataArrays);
        }
        else if (_trigger is PF_Speech_Trigger)
        {
            Trigger_Unified unified = CreateUnifiedTrigger(_trigger.gameObject);
            trigger = unified.gameObject;
            unified.UnifiedType = Trigger_Unified.UnifiedTriggerType.Speech;
            unified.OnTypeChanged();

            PF_Speech_Trigger speech = _trigger as PF_Speech_Trigger;
            unified.TriggerOn = speech.TriggerOn;
            unified.TriggerType = speech.TriggerType;
            unified.TriggerDelayTime = speech.TriggerDelayTime;
            unified.MaxTriggerCount = speech.MaxTriggerCount;

            UnifiedEventData_Speech data = unified.gameObject.GetComponent<UnifiedEventData_Speech>();
            data.InsideArea = speech.InsideArea;
            data.SpeechList = speech.SpeechList;
            data.DisableSpawnerList = speech.DisableSpawnerList;
            data.OldEnableTriggerList = speech.EnableTriggerList;
            //data.EnableTriggerList = speech.EnableTriggerList;
        }
        else if (_trigger is TriggerTeleport)
        {
            Trigger_Unified unified = CreateUnifiedTrigger(_trigger.gameObject);
            trigger = unified.gameObject;
            unified.UnifiedType = Trigger_Unified.UnifiedTriggerType.Teleport;
            unified.OnTypeChanged();

            TriggerTeleport teleport = _trigger as TriggerTeleport;
            unified.TriggerOn = teleport.TriggerOn;
            unified.TriggerType = teleport.TriggerType;
            unified.TriggerDelayTime = teleport.TriggerDelayTime;
            unified.MaxTriggerCount = teleport.MaxTriggerCount;

            UnifiedEventData_Teleport data = unified.gameObject.GetComponent<UnifiedEventData_Teleport>();
            data.TeleportPos = teleport.TeleportPos;
            data.CameraFollowAtOnce = teleport.CameraFollowAtOnce;
            data.EntryMesh = teleport.EntryMesh;
            data.FadeInTime = teleport.FadeInTime;
            data.FadeOutTime = teleport.FadeOutTime;

            //Remove Material
            trigger.renderer.enabled = false;

            //Copy Child VFX
            for(int i=0; i<teleport.transform.childCount; i++)
            {
                Transform source = teleport.transform.GetChild(i);
                GameObject clonechild = GameObject.Instantiate(source.gameObject, source.position, source.rotation) as GameObject;
                clonechild.transform.parent = unified.transform;
            }
            
        }
        else
        {
            Debug.LogError(string.Format(" Unknown Trigger {0} ", _trigger.gameObject.name));
        }
        
        return trigger;
    }

    private static Trigger_Unified CreateUnifiedTrigger(GameObject _obj)
    {
        //Create Unified Trigger
        GameObject unified_prefab = AssetDatabase.LoadAssetAtPath(@"Assets\Prefabs\Trigger\Trigger_Unified.prefab", typeof(GameObject)) as GameObject;
        GameObject trigger = GameObject.Instantiate(unified_prefab, _obj.transform.position + Vector3.up, _obj.transform.rotation) as GameObject;
        trigger.name = "UT-" + _obj.name;
        trigger.transform.parent = _obj.transform.parent;
        Trigger_Unified unified = trigger.GetComponent<Trigger_Unified>();

        //Set Size
        trigger.transform.localScale = _obj.transform.localScale;
        
        BoxCollider newComp = trigger.GetComponent<BoxCollider>();
        BoxCollider oldComp = _obj.GetComponent<BoxCollider>();
        newComp.center = oldComp.center;
        newComp.size = oldComp.size;

        return unified;
    }
}
