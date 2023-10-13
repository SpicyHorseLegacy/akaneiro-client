using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trigger_Unified : BaseExportNode
{
    public enum UnifiedTriggerType
    { Invalid = -1, Spawn, PlayAnim, Speech, Teleport }
    //{ Invalid=-1, Spawn, ChangeMap, Teleport, Item, PlayAnim, Speech, Max }

    public bool TriggerOn = true;
    public int TriggerType = 0;
    public int MaxTriggerCount = 1; //0 means trigger infinite
    public float TriggerDelayTime = 0f;

    public UnifiedTriggerType UnifiedType = UnifiedTriggerType.Invalid;

    
    [HideInInspector]
    [SerializeField]
    private int unityID_ = -1;
    public int UnityID { get { return unityID_; } }

    public void OnTypeChanged()
    {
        UnifiedEventData_Base[] datas = gameObject.GetComponents<UnifiedEventData_Base>();
        foreach (UnifiedEventData_Base data in datas)
        {
            DestroyImmediate(data,true);
        }

        switch (UnifiedType)
        {
            case Trigger_Unified.UnifiedTriggerType.Spawn:
                {
                    gameObject.AddComponent<UnifiedEventData_Spawn>();
                    renderer.material.color = new Color(1, 0, 0, 0.37f);
                    break;
                }
            case Trigger_Unified.UnifiedTriggerType.PlayAnim:
                {
                    gameObject.AddComponent<UnifiedEventData_Anim>();
                    renderer.material.color = new Color(0, 1, 0, 0.37f);
                    break;
                }
            case Trigger_Unified.UnifiedTriggerType.Speech:
                {
                    gameObject.AddComponent<UnifiedEventData_Speech>();
                    renderer.material.color = new Color(0.85f, 0.59f, 0.1f, 0.37f);
                    break;
                }
            case Trigger_Unified.UnifiedTriggerType.Teleport:
                {
                    gameObject.AddComponent<UnifiedEventData_Teleport>();
                    gameObject.AddComponent<RandomEffectTrigger>();
                    renderer.material.color = new Color(0, 0, 1, 0.37f);
                    break;
                }
            default:
                break;
        }
    }

    public override string DoExport()
    {
        XMLStringWriter xmlWriter = new XMLStringWriter();
        xmlWriter.NodeBegin("Triggers");
        xmlWriter.AddAttribute("id", UnityID);
        xmlWriter.AddAttribute("TriggerType", TriggerType);
        xmlWriter.AddAttribute("PosX", transform.position.x);
        xmlWriter.AddAttribute("PosY", transform.position.y);
        xmlWriter.AddAttribute("PosZ", transform.position.z);
	    xmlWriter.AddAttribute("rot",  transform.eulerAngles.y);
	
		if(gameObject.GetComponent<BoxCollider>())
		{
			float sizex = gameObject.GetComponent<BoxCollider>().size.x;
			sizex *= transform.localScale.x;
			
			float sizey = gameObject.GetComponent<BoxCollider>().size.y;
			sizey *= transform.localScale.y;
			
			float sizez = gameObject.GetComponent<BoxCollider>().size.z;
			sizez *= transform.localScale.z;
			
			
			xmlWriter.AddAttribute("SizeX",sizex);
			xmlWriter.AddAttribute("SizeY",sizey);
			xmlWriter.AddAttribute("SizeZ",sizez);
		}

        xmlWriter.AddAttribute("TriggerOn", TriggerOn);
        xmlWriter.AddAttribute("DelayTime", TriggerDelayTime);
        xmlWriter.AddAttribute("TriggerCount", MaxTriggerCount);

        ExportEventData(UnifiedType, xmlWriter);

        xmlWriter.NodeEnd("Triggers");
		
		return xmlWriter.Result;
    }

    public bool IsLinkToSpawner(NpcSpawner _spawner)
    {
        if (UnifiedType == UnifiedTriggerType.Spawn)
        {
            UnifiedEventData_Spawn data = GetComponent<UnifiedEventData_Spawn>();
            if (data.spawnerList_.Contains(_spawner.transform))
                return true;
        }

        return false;
    }

    public void SetUnityID(int _id)
    {
        unityID_ = _id;
    }

    private void ExportEventData(UnifiedTriggerType _type, XMLStringWriter _xmlWriter)
    {
        switch (_type)
        {
            case UnifiedTriggerType.PlayAnim:
                {
                    ExportData_Anim(_xmlWriter);
                    break;
                }
            case UnifiedTriggerType.Spawn:
                {
                    ExportData_Spawn(_xmlWriter);
                    break;
                }
            case UnifiedTriggerType.Speech:
                {
                    ExportData_Speech(_xmlWriter);
                    break;
                }
            case UnifiedTriggerType.Teleport:
                {
                    ExportData_Teleport(_xmlWriter);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void ExportData_Spawn(XMLStringWriter _xmlWriter)
    {
        UnifiedEventData_Spawn data = GetComponent<UnifiedEventData_Spawn>();

        foreach (Transform it in data.spawnerList_)
        {
            if (it == null) 
                continue;

            NpcSpawner mMonsterSpawner = it.GetComponent<NpcSpawner>();
            if (mMonsterSpawner != null)
            {
                _xmlWriter.NodeBegin("TriggerResult");
                _xmlWriter.AddAttribute("ID", mMonsterSpawner.id);
                _xmlWriter.AddAttribute("Name", mMonsterSpawner.name);
                _xmlWriter.AddAttribute("Type", 0);
                _xmlWriter.NodeEnd("TriggerResult");
            }
        }
    }

    private void ExportData_Anim(XMLStringWriter _xmlWriter)
    {
        UnifiedEventData_Anim data = GetComponent<UnifiedEventData_Anim>();

        foreach (PlayAnimTrigger.AnimationDataStruct it in data.animDataList_)
        {
            if (it == null)
                continue;

            _xmlWriter.NodeBegin("TriggerResult");
            _xmlWriter.AddAttribute("ID", it.AnimObj.id + UnityID);
            _xmlWriter.AddAttribute("Name", it.AnimObj.transform.name);
            _xmlWriter.AddAttribute("Type", 4);
            _xmlWriter.NodeBegin("AnimationInfo");
            _xmlWriter.AddAttribute("id", it.AnimObj.id + UnityID);
            _xmlWriter.AddAttribute("realID", it.AnimObj.id);
            _xmlWriter.AddAttribute("Loop", it.IsLoopAnim);
            _xmlWriter.AddAttribute("AnimName", it.anim == null ? "" : it.anim.name);
            _xmlWriter.AddAttribute("AnimDelayTime", it.AnimDelayTime);
            _xmlWriter.AddAttribute("bCollision", it.bCollision);
            _xmlWriter.AddAttribute("bCanShow", it.bCanShow);
            _xmlWriter.NodeEnd("AnimationInfo");
            _xmlWriter.NodeEnd("TriggerResult");
        }
    }

    private void ExportData_Speech(XMLStringWriter _xmlWriter)
    { 
        UnifiedEventData_Speech data = GetComponent<UnifiedEventData_Speech>();

        foreach (NpcSpawner it in data.DisableSpawnerList)
        {
            if (it == null) continue;

            _xmlWriter.NodeBegin("TriggerResult");
            _xmlWriter.AddAttribute("ID", it.id);
            _xmlWriter.AddAttribute("kind", 2);
            _xmlWriter.AddAttribute("Type", 0);

            _xmlWriter.NodeEnd("TriggerResult");

        }

        foreach (Trigger_Unified it in data.EnableTriggerList)
        {
            if (it == null) continue;

            _xmlWriter.NodeBegin("EnableTrigger");
            _xmlWriter.AddAttribute("ID", it.UnityID);
            _xmlWriter.NodeEnd("EnableTrigger");
        }

        foreach (PF_Speech_Trigger.cSpeechObj it in data.SpeechList)
        {
            if (it == null) continue;

            _xmlWriter.NodeBegin("TriggerResult");

            _xmlWriter.AddAttribute("ID", it.NpcID);
            _xmlWriter.AddAttribute("isRandom", it.bRandom);
            _xmlWriter.AddAttribute("Type", 5);
            _xmlWriter.AddAttribute("isLoop", it.bLoop);
            _xmlWriter.AddAttribute("InsideArea", data.InsideArea);
            _xmlWriter.AddAttribute("SpeakTime", it.SpeakTime);

            List<int> resultList = new List<int>();

            foreach (int index in it.WordList)
                resultList.Add(index);

            foreach (int index in resultList)
            {
                _xmlWriter.NodeBegin("Word");
                _xmlWriter.AddAttribute("WhichWord", index);
                _xmlWriter.NodeEnd("Word");
            }

            _xmlWriter.NodeEnd("TriggerResult");
        }
    }

    private void ExportData_Teleport(XMLStringWriter _xmlWriter)
    { 
        UnifiedEventData_Teleport data = GetComponent<UnifiedEventData_Teleport>();

        _xmlWriter.NodeBegin("TriggerResult");
        _xmlWriter.AddAttribute("ID", UnityID);
        _xmlWriter.AddAttribute("Name", gameObject.name);
        _xmlWriter.AddAttribute("Type", 2);

        if (data.TeleportPos != null)
        {
            _xmlWriter.NodeBegin("DesPos");
            _xmlWriter.AddAttribute("id", UnityID);
            _xmlWriter.AddAttribute("PosX", data.TeleportPos.position.x);
            _xmlWriter.AddAttribute("PosY", data.TeleportPos.position.y);
            _xmlWriter.AddAttribute("PosZ", data.TeleportPos.position.z);

            _xmlWriter.AddAttribute("FadeInTime", data.FadeInTime);
            _xmlWriter.AddAttribute("FadeOutTime", data.FadeOutTime);
            _xmlWriter.AddAttribute("CameraFollow", data.CameraFollowAtOnce);

            _xmlWriter.NodeEnd("DesPos");
        }

        _xmlWriter.NodeEnd("TriggerResult");
    }
    
    public override string GetNodeType()
    {
        return "Trigger";
    }
}
