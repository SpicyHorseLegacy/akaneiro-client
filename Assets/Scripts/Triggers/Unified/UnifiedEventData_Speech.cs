using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnifiedEventData_Speech : UnifiedEventData_Base
{
    public bool InsideArea;

    public PF_Speech_Trigger.cSpeechObj[] SpeechList = new PF_Speech_Trigger.cSpeechObj[0];

    public NpcSpawner[] DisableSpawnerList = new NpcSpawner[0];

    public Trigger_Unified[] EnableTriggerList = new Trigger_Unified[0];

    public TriggerBase[] OldEnableTriggerList = new TriggerBase[0];
}
