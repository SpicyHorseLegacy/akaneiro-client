using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;

public class EnemyAbilityBuilder : EditorWindow
{
    List<NpcBase> abiEnemies = new List<NpcBase>();
    List<NpcBase> newAbiEnemies = new List<NpcBase>();
    NpcBase curEnemy;

    GameObject tempOBJ;

    Vector2 scrollPosLeft;
    Vector2 scrollPosRight;

    Color changedContentColor = Color.yellow;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Tools/Enemy Abilitys Builder")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        EnemyAbilityBuilder window = (EnemyAbilityBuilder)EditorWindow.GetWindow(typeof(EnemyAbilityBuilder));
    }

    void OnDestroy()
    {
        if (tempOBJ)
        {
            DestroyImmediate(tempOBJ);
        }
    }

    #region Draw_GUI
    void OnGUI()
    {
        if (GUILayout.Button("Refresh"))
        {
            Refresh();
        }

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("box", GUILayout.Width(200));

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("New Ability Enemy"))
            {
                NewAbilityEnemySelectionWindow.Init(this);
            }
            GUI.backgroundColor = Color.white;

            DrawSortPanel();

            scrollPosLeft = EditorGUILayout.BeginScrollView(scrollPosLeft);
            foreach (NpcBase abienemy in abiEnemies)
            {
                GUI.contentColor = Color.white;
                if (abienemy == curEnemy)
                {
                    GUI.contentColor = Color.green;
                }
                if (GUILayout.Button(abienemy.NpcName))
                {
                    GetCurEnemy(abienemy);
                }
            }

            GUI.contentColor = Color.white;
            GUILayout.Label("New Ability Enemy");

            foreach (NpcBase abienemy in newAbiEnemies)
            {
                GUI.contentColor = Color.white;
                if (abienemy == curEnemy)
                {
                    GUI.contentColor = Color.green;
                }
                if (GUILayout.Button(abienemy.NpcName))
                {
                    GetCurEnemy(abienemy);
                }
            }


            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            GUI.contentColor = Color.white;
            EditorGUILayout.BeginVertical("box");

            if (tempOBJ == null)
            {
                EditorGUILayout.LabelField("none!");
            }
            else
            {
                NPCAbilityBaseState[] abis = tempOBJ.GetComponents<NPCAbilityBaseState>();
                scrollPosRight = EditorGUILayout.BeginScrollView(scrollPosRight);
                if(curEnemy)
                    EditorGUILayout.HelpBox(curEnemy.NpcName, MessageType.Warning);

                DrawAddNewAbilityPanel();

                foreach (NPCAbilityBaseState abi in abis)
                {
                    EditorGUILayout.BeginVertical("box");

                        EditorGUI.indentLevel = 0;
                        EditorGUILayout.Space();
                        GUI.backgroundColor = Color.yellow;
                        EditorGUILayout.BeginHorizontal("box");
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.LabelField(abi.GetType().ToString());
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();

                        EditorGUI.indentLevel = 1;

                        abi.IsPlayImpactSound = EditorGUILayout.Toggle("Is Play Impact SFX", abi.IsPlayImpactSound);

                        if (abi.IsPlayImpactSound)
                            abi.ImpactSoundPrefab = EditorGUILayout.ObjectField("    Impact Sound Prefab" ,abi.ImpactSoundPrefab, typeof(Transform)) as Transform;

                        abi.IsPlayImpactVFX = EditorGUILayout.Toggle("Is Play Impact VFX", abi.IsPlayImpactVFX);

                        if (abi.IsPlayImpactVFX)
                            abi.ImpactVFXPrefab = EditorGUILayout.ObjectField("    Impact VFX Prefab", abi.ImpactVFXPrefab, typeof(Transform)) as Transform;

                        EditorGUILayout.Space();

                        abi.id = EditorGUILayout.IntField("Ability ID", abi.id);
                        if (abi.id == 0)
                        {
                            EditorGUILayout.HelpBox("You should set a ID for this Ability!", MessageType.Error);
                        }

                        abi.AbilityCoolDown = EditorGUILayout.FloatField("Ability CoolDown", abi.AbilityCoolDown);
                        abi.AbilityCoolDownDif = EditorGUILayout.FloatField("Ability CoolDown Dif", abi.AbilityCoolDownDif);
				

						EditorGUILayout.BeginHorizontal();
							EditorGUILayout.BeginHorizontal("box");
							string _abiConditionString = "";
							for(int i = 0 ; i < abi.AbilityConditions.Count; i++ )
							{
								NPCAbilityBaseState.AbilityCondition _abicon = abi.AbilityConditions[i];
								_abiConditionString += "[" + (i+1) + "] " + _abicon.AbiCondition.ToString() +  " || " + _abicon.Num;
								if(i != abi.AbilityConditions.Count - 1)
								_abiConditionString += "\n";
							}
							if(_abiConditionString == "")
								_abiConditionString = "No Condition";
							GUILayout.Label(_abiConditionString);
							EditorGUILayout.EndHorizontal();
						if(GUILayout.Button("+", GUILayout.Width(50)))
						{
							EnmeyAbilityConditionBuilderWindow.Init(abi);
						}
						EditorGUILayout.EndHorizontal();

                        abi.PosType = (NPCAbilityBaseState.AbilityPositionType)EditorGUILayout.EnumPopup("Postion Type", abi.PosType);

                        EditorGUILayout.Space();

                        abi.CastAnimation = EditorGUILayout.ObjectField(new GUIContent("Casting animation", "casting animation tooltip"), abi.CastAnimation, typeof(AnimationClip)) as AnimationClip;
                        if (abi.CastAnimation == null)
                        {
                            EditorGUILayout.HelpBox("No animation!", MessageType.Error);
                        }
                        abi.CastSoundPrefab = EditorGUILayout.ObjectField("Casting Sound" ,abi.CastSoundPrefab, typeof(Transform)) as Transform;

                        EditorGUILayout.Space();

                        DrawMoreInfoForAbi(abi);

                        EditorGUILayout.Space();

                        EditorGUILayout.BeginHorizontal();

                        GUI.backgroundColor = Color.red;
                        if (GUILayout.Button("Delete", GUILayout.Width(50)))
                        {
                            if (EditorUtility.DisplayDialog("Delete Ability", "Are you sure delete ability " + abi.name + " from " + curEnemy.NpcName, "Yap!", "No"))
                                RemoveAbiFromTempOBJ(abi);
                        }
                        GUI.backgroundColor = Color.white;

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.BeginHorizontal("box");
                    if (GUILayout.Button("Revert", GUILayout.Width(50)))
                    {
                        RefreshTempOBJ();
                    }

                    if (GUILayout.Button("OK", GUILayout.Width(50)))
                    {
                        if (EditorUtility.DisplayDialog("Replace Prefab", "Are you sure replace ability properties to " + curEnemy.NpcName, "Yap!", "No"))
                            UpdateToPrefab();
                    }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.EndVertical();

            
         
        EditorGUILayout.EndHorizontal();
    }

    string[] options = { "NPC_ALL", "NPC_MeteorRainState", "NPC_ShockWaveState", "NPC_Slow", "NPC_Toss", "NPC_RainOfBlow", "NPC_WhirleWind", "NPC_SkyStrike", "NPC_AreaHoT" };
    int index = 0;
    int indexForSort = 0;
    string sortString = "NPC_ALL";

    void DrawSortPanel()
    {
        EditorGUILayout.Space();

        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        indexForSort = EditorGUI.Popup(
            rect,
            "sorts : ",
            indexForSort,
            options);
        sortString = options[indexForSort];

        EditorGUILayout.Space();
    }

    void DrawAddNewAbilityPanel()
    {
        Color tempColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.blue;
        EditorGUILayout.BeginHorizontal("box");
        GUI.backgroundColor = tempColor;
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        index = EditorGUI.Popup(
            rect,
            "Component:",
            index,
            options);

        if (GUILayout.Button("Add Component"))
            AddComponentToObjects();
        EditorGUILayout.EndHorizontal();
    }

    void DrawCheckAbleTransform(Transform sourceObj, Transform targetObj, GUIContent content)
    {
        Color baseColor = GUI.contentColor;
        if (sourceObj != targetObj)
            GUI.contentColor = changedContentColor;
        targetObj = EditorGUILayout.ObjectField(content, targetObj, typeof(Transform)) as Transform;
        GUI.contentColor = baseColor;
    }

    void DrawMoreInfoForAbi(NPCAbilityBaseState abi)
    {
        if (abi.GetType() == typeof(NPC_ShockWaveState) || abi.GetType() == typeof(NPC_RainOfBlow) || abi.GetType() == typeof(NPC_Slow) || abi.GetType() == typeof(NPC_WhirlWind) ||
			abi.GetType() == typeof(NPC_SkyStrike) || abi.GetType() == typeof(NPC_AreaHoT))
        {
            abi.AbilityImpactVFXPrefab = EditorGUILayout.ObjectField("Impact VFX Prefab", abi.AbilityImpactVFXPrefab, typeof(Transform)) as Transform;
            abi.AbilityImpactPosition = EditorGUILayout.ObjectField("Impact Position", abi.AbilityImpactPosition, typeof(Transform)) as Transform;
            abi.AbilityImpactSoundPrefab = EditorGUILayout.ObjectField("Impact sound Prefab", abi.AbilityImpactSoundPrefab, typeof(Transform)) as Transform;
        }
		
        if (abi.GetType() == typeof(NPC_MeteorRainState))
        {
            NPC_MeteorRainState tempAbi = (NPC_MeteorRainState)abi;
            tempAbi.MeteorRainPrefab = EditorGUILayout.ObjectField("Meteor Rain OBJ Prefab", tempAbi.MeteorRainPrefab, typeof(Transform)) as Transform;
        }
		
		if(abi.GetType() == typeof(NPC_AreaHoT))
		{
			NPC_AreaHoT tempAbi = (NPC_AreaHoT)abi;
			tempAbi.HoTVFXPrefab = EditorGUILayout.ObjectField("Area OBJ Prefab", tempAbi.HoTVFXPrefab, typeof(Transform)) as Transform;
		}
		
        if (abi.GetType() == typeof(NPC_Toss))
        {
            NPC_Toss tempAbi = (NPC_Toss)abi;
            tempAbi.ChargingVFXPrefab = EditorGUILayout.ObjectField("Charging VFX Prefab", tempAbi.ChargingVFXPrefab, typeof(Transform)) as Transform;
            tempAbi.ThrowSoundPrefab = EditorGUILayout.ObjectField("Throwing Sound Prefab", tempAbi.ThrowSoundPrefab, typeof(Transform)) as Transform;
            string[] options = {"Left Hand" , "Right Hand"};
            tempAbi.ChargingHand = (NPC_Toss.ChangingHand)EditorGUILayout.Popup("Postion Type", (int)tempAbi.ChargingHand, options);
            tempAbi.ProjectilePrefab = EditorGUILayout.ObjectField("Projectile Prefab", tempAbi.ProjectilePrefab, typeof(Transform)) as Transform;
        }
		
		if(abi.GetType() == typeof(NPC_SkyStrike))
		{
			NPC_SkyStrike tempAbi = (NPC_SkyStrike)abi;
			tempAbi.LoopAnimation = EditorGUILayout.ObjectField("Loop AnimationClip", tempAbi.LoopAnimation, typeof(AnimationClip)) as AnimationClip;
			tempAbi.EndAnimation = EditorGUILayout.ObjectField("End AnimationClip", tempAbi.EndAnimation, typeof(AnimationClip)) as AnimationClip;
			tempAbi.ProtectVFXPrefab = EditorGUILayout.ObjectField("ProtectVFX Prefab", tempAbi.ProtectVFXPrefab, typeof(Transform)) as Transform;
			tempAbi.ProjectileVFXPrefab = EditorGUILayout.ObjectField("Projctile Prefab", tempAbi.ProjectileVFXPrefab, typeof(Transform)) as Transform;
		}
    }
    #endregion

    #region List_Control
    void Refresh()
    {
        abiEnemies.Clear();
        newAbiEnemies.Clear();

        if (indexForSort == 0)
        {
            sortString = "NPCAbilityBaseState";
        }

        string[] existingMonsters = new string[0];
        for (int i = 1; i < 10; i++)
        {

            if (i == 1)
                existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies/Area1_Common", "*.prefab", SearchOption.AllDirectories);
            else if (i == 2)
                existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies/Area2_Common", "*.prefab", SearchOption.AllDirectories);
            else if (i == 3)
                existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies/Area3_Common", "*.prefab", SearchOption.AllDirectories);
            else if (i == 4)
                existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies/Area4_Common", "*.prefab", SearchOption.AllDirectories);
            else if (i == 5)
                existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies/Area5_Common", "*.prefab", SearchOption.AllDirectories);
            else if (i == 6)
                existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies/Area6_Common", "*.prefab", SearchOption.AllDirectories);
            else if (i == 7)
                existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies/Area7_Common", "*.prefab", SearchOption.AllDirectories);
            else if (i == 8)
                existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies/Area8_Common", "*.prefab", SearchOption.AllDirectories);
            else if (i == 9)
                existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies/Mission", "*.prefab", SearchOption.AllDirectories);


            foreach (string MonsterFile in existingMonsters)
            {
                NpcBase theMonster = AssetDatabase.LoadAssetAtPath(MonsterFile, typeof(NpcBase)) as NpcBase;

                if (theMonster && theMonster.abilityManager)
                {
                    if (theMonster.abilityManager.transform.GetComponent(sortString))
                    {
                        abiEnemies.Add(theMonster);
                    }
                }
                else
                {
                    //Debug.LogError("Name : " + theMonster.transform.name + " don't have an ability manager!");
                }

            }
        }
        if (abiEnemies.Count > 0 && !abiEnemies.Contains(curEnemy))
        {
            GetCurEnemy(abiEnemies[0]);
        }
    }

    void RefreshTempOBJ()
    {
        if (!tempOBJ)
        {
            tempOBJ = new GameObject();
            tempOBJ.name = "TEMP OBJECT FOR ENEMY ABILITY BUILDING! IT WILL BE DELETED BY CLOSING BUILDING WINDOW AUTOMATICALLY!";
        }

        NPCAbilityBaseState[] tempAbis = tempOBJ.GetComponents<NPCAbilityBaseState>();
        foreach (NPCAbilityBaseState abi in tempAbis)
        {
            DestroyImmediate(abi);
        }

        if (curEnemy)
        {
            NPCAbilityBaseState[] abis = curEnemy.abilityManager.transform.GetComponents<NPCAbilityBaseState>();
            foreach (NPCAbilityBaseState abi in abis)
            {
                AddAbiToTempOBJ(abi);
            }
        }
    }

    void GetCurEnemy(NpcBase abiEnemy)
    {
        curEnemy = abiEnemy;
        RefreshTempOBJ();
    }

    public void GetANewAbiEnemy(NpcBase newabiEnemy)
    {
        newAbiEnemies.Add(newabiEnemy);
        GetCurEnemy(newabiEnemy);
    }

    #endregion

    #region Ability_Component_Control

    void AddComponentToObjects()
    {
        if (!tempOBJ)
        {
            RefreshTempOBJ();
        }
        if (!tempOBJ.GetComponent(options[index]))
            tempOBJ.AddComponent(options[index]);
    }


    void AddAbiToTempOBJ(NPCAbilityBaseState abi)
    {
        if (abi.GetType() == typeof(NPC_ShockWaveState))
        {
            NPC_ShockWaveState tempAbi = (NPC_ShockWaveState)abi;
            NPC_ShockWaveState newTempAbi = tempOBJ.AddComponent<NPC_ShockWaveState>();
            CopyPropertiesToAnother(tempAbi, newTempAbi);
        }
        if (abi.GetType() == typeof(NPC_Toss))
        {
            NPC_Toss tempAbi = (NPC_Toss)abi;
            NPC_Toss newTempAbi = tempOBJ.AddComponent<NPC_Toss>();
            CopyPropertiesToAnother(tempAbi, newTempAbi);
        }
        if (abi.GetType() == typeof(NPC_MeteorRainState))
        {
            NPC_MeteorRainState tempAbi = (NPC_MeteorRainState)abi;
            NPC_MeteorRainState newTempAbi = tempOBJ.AddComponent<NPC_MeteorRainState>();
            CopyPropertiesToAnother(tempAbi, newTempAbi);
        }
        if (abi.GetType() == typeof(NPC_Slow))
        {
            NPC_Slow tempAbi = (NPC_Slow)abi;
            NPC_Slow newTempAbi = tempOBJ.AddComponent<NPC_Slow>();
            CopyPropertiesToAnother(tempAbi, newTempAbi);
        }
        if (abi.GetType() == typeof(NPC_RainOfBlow))
        {
            NPC_RainOfBlow tempAbi = (NPC_RainOfBlow)abi;
            NPC_RainOfBlow newTempAbi = tempOBJ.AddComponent<NPC_RainOfBlow>();
            CopyPropertiesToAnother(tempAbi, newTempAbi);
        }
        if (abi.GetType() == typeof(NPC_WhirlWind))
        {
            NPC_WhirlWind tempAbi = (NPC_WhirlWind)abi;
            NPC_WhirlWind newTempAbi = tempOBJ.AddComponent<NPC_WhirlWind>();
            CopyPropertiesToAnother(tempAbi, newTempAbi);
        }
		if(abi.GetType() == typeof(NPC_SkyStrike))
		{
			NPC_SkyStrike tempAbi = (NPC_SkyStrike)abi;
			NPC_SkyStrike newTempAbi = tempOBJ.AddComponent<NPC_SkyStrike>();
			CopyPropertiesToAnother(tempAbi, newTempAbi);
		}
		if(abi.GetType() == typeof(NPC_AreaHoT))
		{
			NPC_AreaHoT tempAbi = (NPC_AreaHoT)abi;
			NPC_AreaHoT newTempAbi = tempOBJ.AddComponent<NPC_AreaHoT>();
			CopyPropertiesToAnother(tempAbi, newTempAbi);
		}
    }

    void RemoveAbiFromTempOBJ(NPCAbilityBaseState abi)
    {
        DestroyImmediate(abi);
    }

    void UpdateToPrefab()
    {
        NpcBase tempPrefab = Instantiate(curEnemy) as NpcBase;
        NPCAbilityBaseState[] tempPrefabAbis = tempPrefab.abilityManager.transform.GetComponents<NPCAbilityBaseState>();
        foreach (NPCAbilityBaseState abi in tempPrefabAbis)
        {
            DestroyImmediate(abi);
        }

        NPCAbilityBaseState[] tempAbis = tempOBJ.GetComponents<NPCAbilityBaseState>();
        foreach(NPCAbilityBaseState abi in tempAbis)
        {
            UpdateToTEMPPrefab(tempPrefab, abi);
        }

        PrefabUtility.ReplacePrefab(tempPrefab.gameObject, curEnemy, ReplacePrefabOptions.ReplaceNameBased);
        DestroyImmediate(tempPrefab.gameObject);
        Refresh();
    }

    void UpdateToTEMPPrefab(NpcBase tempPrefab, NPCAbilityBaseState abi)
    {
        if (abi.GetType() == typeof(NPC_ShockWaveState))
        {
            NPC_ShockWaveState tempAbi = (NPC_ShockWaveState)abi;
            NPC_ShockWaveState curEnemyPrefabAbi = tempPrefab.abilityManager.transform.GetComponent<NPC_ShockWaveState>();
            if (!curEnemyPrefabAbi)
                curEnemyPrefabAbi = tempPrefab.abilityManager.gameObject.AddComponent<NPC_ShockWaveState>();
            CopyPropertiesToAnother(abi, curEnemyPrefabAbi);
        }
        if (abi.GetType() == typeof(NPC_Toss))
        {
            NPC_Toss tempAbi = (NPC_Toss)abi;
            NPC_Toss curEnemyPrefabAbi = tempPrefab.abilityManager.transform.GetComponent<NPC_Toss>();
            if (!curEnemyPrefabAbi)
                curEnemyPrefabAbi = tempPrefab.abilityManager.gameObject.AddComponent<NPC_Toss>();
            CopyPropertiesToAnother(abi, curEnemyPrefabAbi);
        }
        if (abi.GetType() == typeof(NPC_MeteorRainState))
        {
            NPC_MeteorRainState tempAbi = (NPC_MeteorRainState)abi;
            NPC_MeteorRainState curEnemyPrefabAbi = tempPrefab.abilityManager.transform.GetComponent<NPC_MeteorRainState>();
            if (!curEnemyPrefabAbi)
                curEnemyPrefabAbi = tempPrefab.abilityManager.gameObject.AddComponent<NPC_MeteorRainState>();
            CopyPropertiesToAnother(abi, curEnemyPrefabAbi);
        }
        if (abi.GetType() == typeof(NPC_Slow))
        {
            NPC_Slow tempAbi = (NPC_Slow)abi;
            NPC_Slow curEnemyPrefabAbi = tempPrefab.abilityManager.transform.GetComponent<NPC_Slow>();
            if (!curEnemyPrefabAbi)
                curEnemyPrefabAbi = tempPrefab.abilityManager.gameObject.AddComponent<NPC_Slow>();
            CopyPropertiesToAnother(abi, curEnemyPrefabAbi);
        }
        if (abi.GetType() == typeof(NPC_RainOfBlow))
        {
            NPC_RainOfBlow tempAbi = (NPC_RainOfBlow)abi;
            NPC_RainOfBlow curEnemyPrefabAbi = tempPrefab.abilityManager.transform.GetComponent<NPC_RainOfBlow>();
            if (!curEnemyPrefabAbi)
                curEnemyPrefabAbi = tempPrefab.abilityManager.gameObject.AddComponent<NPC_RainOfBlow>();
            CopyPropertiesToAnother(abi, curEnemyPrefabAbi);
        }
        if (abi.GetType() == typeof(NPC_WhirlWind))
        {
            NPC_WhirlWind tempAbi = (NPC_WhirlWind)abi;
            NPC_WhirlWind curEnemyPrefabAbi = tempPrefab.abilityManager.transform.GetComponent<NPC_WhirlWind>();
            if (!curEnemyPrefabAbi)
                curEnemyPrefabAbi = tempPrefab.abilityManager.gameObject.AddComponent<NPC_WhirlWind>();
            CopyPropertiesToAnother(abi, curEnemyPrefabAbi);
        }
		if (abi.GetType() == typeof(NPC_SkyStrike))
        {
            NPC_SkyStrike tempAbi = (NPC_SkyStrike)abi;
            NPC_SkyStrike curEnemyPrefabAbi = tempPrefab.abilityManager.transform.GetComponent<NPC_SkyStrike>();
            if (!curEnemyPrefabAbi)
                curEnemyPrefabAbi = tempPrefab.abilityManager.gameObject.AddComponent<NPC_SkyStrike>();
            CopyPropertiesToAnother(abi, curEnemyPrefabAbi);
        }
		if (abi.GetType() == typeof(NPC_AreaHoT))
        {
            NPC_AreaHoT tempAbi = (NPC_AreaHoT)abi;
            NPC_AreaHoT curEnemyPrefabAbi = tempPrefab.abilityManager.transform.GetComponent<NPC_AreaHoT>();
            if (!curEnemyPrefabAbi)
                curEnemyPrefabAbi = tempPrefab.abilityManager.gameObject.AddComponent<NPC_AreaHoT>();
            CopyPropertiesToAnother(abi, curEnemyPrefabAbi);
        }
    }

    void CopyPropertiesToAnother(NPCAbilityBaseState sourceAbi, NPCAbilityBaseState targetAbi)
    {
        if (targetAbi.GetType() != sourceAbi.GetType()) return;

        if (sourceAbi.GetType() == typeof(NPC_Toss))
        {
            NPC_Toss tempSourceAbi = (NPC_Toss)sourceAbi;
            NPC_Toss tempTargetAbi = (NPC_Toss)targetAbi;
            tempTargetAbi.ChargingVFXPrefab = tempSourceAbi.ChargingVFXPrefab;
            tempTargetAbi.ThrowSoundPrefab = tempSourceAbi.ThrowSoundPrefab;
            tempTargetAbi.ChargingHand = tempSourceAbi.ChargingHand;
            tempTargetAbi.ProjectilePrefab = tempSourceAbi.ProjectilePrefab;
        }
        if (sourceAbi.GetType() == typeof(NPC_MeteorRainState))
        {
            NPC_MeteorRainState tempSourceAbi = (NPC_MeteorRainState)sourceAbi;
            NPC_MeteorRainState tempTargetAbi = (NPC_MeteorRainState)targetAbi;
            tempTargetAbi.MeteorRainPrefab = tempSourceAbi.MeteorRainPrefab;
        }
        if (sourceAbi.GetType() == typeof(NPC_Slow))
        {
            NPC_Slow tempSourceAbi = (NPC_Slow)sourceAbi;
            NPC_Slow tempTargetAbi = (NPC_Slow)targetAbi;
            tempTargetAbi.SlowDownVFXPrefab = tempSourceAbi.SlowDownVFXPrefab;
        }
        if (sourceAbi.GetType() == typeof(NPC_WhirlWind))
        {
        }
		if (sourceAbi.GetType() == typeof(NPC_SkyStrike))
        {
			NPC_SkyStrike tempSourceAbi = (NPC_SkyStrike)sourceAbi;
            NPC_SkyStrike tempTargetAbi = (NPC_SkyStrike)targetAbi;
			tempTargetAbi.LoopAnimation = tempSourceAbi.LoopAnimation;
			tempTargetAbi.EndAnimation = tempSourceAbi.EndAnimation;
			tempTargetAbi.ProtectVFXPrefab = tempSourceAbi.ProtectVFXPrefab;
			tempTargetAbi.ProjectileVFXPrefab = tempSourceAbi.ProjectileVFXPrefab;
        }
		if (sourceAbi.GetType() == typeof(NPC_AreaHoT))
        {
			NPC_AreaHoT tempSourceAbi = (NPC_AreaHoT)sourceAbi;
            NPC_AreaHoT tempTargetAbi = (NPC_AreaHoT)targetAbi;
            tempTargetAbi.HoTVFXPrefab = tempSourceAbi.HoTVFXPrefab;
        }
		
		targetAbi.AbilityConditions = sourceAbi.AbilityConditions;
        targetAbi.IsPlayImpactSound = sourceAbi.IsPlayImpactSound;
        targetAbi.ImpactSoundPrefab = sourceAbi.ImpactSoundPrefab;
        targetAbi.IsPlayImpactVFX = sourceAbi.IsPlayImpactVFX;
        targetAbi.ImpactVFXPrefab = sourceAbi.ImpactVFXPrefab;
        targetAbi.id = sourceAbi.id;
        targetAbi.AbilityCoolDown = sourceAbi.AbilityCoolDown;
        targetAbi.AbilityCoolDownDif = sourceAbi.AbilityCoolDownDif;
        targetAbi.PosType = sourceAbi.PosType;
        targetAbi.PositionOffset = sourceAbi.PositionOffset;
        targetAbi.CastAnimation = sourceAbi.CastAnimation;
        targetAbi.CastSoundPrefab = sourceAbi.CastSoundPrefab;
        targetAbi.AbilityImpactVFXPrefab = sourceAbi.AbilityImpactVFXPrefab;
        targetAbi.AbilityImpactPosition = sourceAbi.AbilityImpactPosition;
        targetAbi.AbilityImpactSoundPrefab = sourceAbi.AbilityImpactSoundPrefab;
    }
    #endregion
}
