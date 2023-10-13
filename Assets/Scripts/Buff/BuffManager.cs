using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffManager : MonoBehaviour {

    public Transform Owner;
	
	public List<BaseBuff> Buffs = new List<BaseBuff>();

    public void Initial(Transform _owner)
	{
        Owner = _owner;
	}
	
	public void Execute()
	{
		if(Buffs.Count > 0)
		{
			for(int i = 0; i < Buffs.Count; i++)
			{
				Buffs[i].Execute();
			}
		}
	}

    public virtual BaseBuff AddBuffByID(int id, Transform _sourceTransform)
	{
		//Debug.LogError("AddBuff");
		foreach(BaseBuff buff in Buffs)
		{
			if(id == buff.ID)
			{
				//buff.RefreshBuff();
                buff.AddStack();
				return buff;
			}
		}

        BaseBuff newBuff = BuffInfo.Instance.GetBuffPrefab(id);
        if (newBuff)
        {
            newBuff.transform.parent = transform;
            newBuff.transform.localPosition = Vector3.zero;
            newBuff.Owner = Owner;
            newBuff.SourceObj = _sourceTransform;
            newBuff.AddStack();
            Buffs.Add(newBuff);
        }
        return newBuff;
	}
	
	public BaseBuff UpdateBuffByID(int id)
	{
		foreach(BaseBuff buff in Buffs)
		{
			if(id == buff.ID)
			{
				buff.TickExecute();
				return buff;
			}
		}
        return null;
	}

    public BaseBuff GetBuffByID(int id)
    {
        foreach (BaseBuff buff in Buffs)
        {
            if (id == buff.ID)
            {
                return buff;
            }
        }
        return null;
    }
	
	public virtual void DeleteBuffByID(int id)
	{
        for (int i = Buffs.Count - 1; i >= 0; i--)
        {
            if (id == Buffs[i].ID)
            {
                Buffs[i].RemoveStack();
            }
        }
	}
	
	public void RemoveBuffFromList(BaseBuff buff)
	{
		for(int i = Buffs.Count - 1; i >=0; i--)
		{
			if(buff == Buffs[i])
			{
				Buffs.RemoveAt(i);
			}
		}
	}
	
	public void RemoveAllBuffs()
	{
		for(int i = Buffs.Count - 1; i >=0; i--)
		{
			Buffs[i].Exit();
		}
	}
}
