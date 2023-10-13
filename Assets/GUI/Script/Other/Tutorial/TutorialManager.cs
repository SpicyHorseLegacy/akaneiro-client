using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TutorialManager {
	
	private static List<TutorialObject> tutorialList = new List<TutorialObject>();

	#region Interface
	public static void Next(string name){
		foreach(TutorialObject obj in tutorialList){
			if(obj.objKey.Equals(name)){
				obj.gameObject.SetActive(true);
				obj.GetComponent<TutorialObject>().Play(false);
				DelList(obj);
			}
		}		
	}
	
	public static List<TutorialObject> GetTutorialList(){
		return tutorialList;
	}

	public static void CleanList(){
		tutorialList.Clear();
	}
	
	public static void AddList(TutorialObject obj){
		tutorialList.Add(obj);
	}
	
	public static void DelList(TutorialObject obj){
		tutorialList.Remove(obj);
	}

	#endregion
}
