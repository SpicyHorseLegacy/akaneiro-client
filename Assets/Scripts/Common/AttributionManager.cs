using UnityEngine;
using System.Collections;

public class AttributionManager : MonoBehaviour {

    public Transform Owner;

    public int[] Attrs = new int[EAttributeType.ATTR_Max];
	public float[] EleAttrs = new float[EStatusElementType.StatusElement_Max];
	public float[] EleChanceAttrs = new float[EStatusElementType.StatusElement_Max];

    public virtual void UpdateAttrs(vectorAttrChange playerAttrVec)
    {
        foreach (SAttributeChange c in playerAttrVec)
        {
            Attrs[c.attributeType.Get()] = c.value;
        }
    }

    public virtual void UpdateEleAttrs(vectorElementValue elementAtrrVec)
    {
        foreach (SElementValue attr in elementAtrrVec)
        {
            EleAttrs[attr.specialType.Get()] = attr.value;
        }
    }

	public virtual void UpdateEleChanceAttrs(vectorElementValue elementAtrrVec)
	{
		foreach (SElementValue attr in elementAtrrVec)
		{
			EleChanceAttrs[attr.specialType.Get()] = attr.value;
		}
	}
}
