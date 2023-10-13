using UnityEngine;
using System.Collections;

public class AssumbleManager : MonoBehaviour
{
	public string Bone_Name;
	public Vector3 Offset_Pos;
	public Vector3 Offset_Rot;
	public Vector3 Offset_Scale =  Vector3.one;
	public GameObject AssumbleObj_M;
	public GameObject AssumbleObj_F;
	
	public GameObject AttachToObject(GameObject _gameobj, ESex _gender)
	{
		Transform[] _all = _gameobj.GetComponentsInChildren<Transform>();
		for(int i = 0; i< _all.Length; i++)
		{
			Transform _temp = _all[i];
			if(_temp.name == Bone_Name)
			{
				GameObject _newAssumble = null;
				if(_gender.Get() == ESex.eSex_Male)
					_newAssumble = AssumbleObj_M;
				if(_gender.Get() == ESex.eSex_Female)
					_newAssumble = AssumbleObj_F;

				if(_newAssumble != null)
				{
					_newAssumble = UnityEngine.Object.Instantiate(_newAssumble) as GameObject;
					_newAssumble.transform.parent = _temp;
					_newAssumble.transform.localPosition = Offset_Pos;
					_newAssumble.transform.localEulerAngles = Offset_Rot;
					_newAssumble.transform.localScale = Offset_Scale;
					_newAssumble.layer = _gameobj.layer;
					foreach(Transform _tran in _newAssumble.transform)
						_tran.gameObject.layer = _gameobj.layer;
				}
				return _newAssumble;
			}
		}
		return null;
	}
}
