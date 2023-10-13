using UnityEngine;
using System.Collections;

public class DamageTextManager : MonoBehaviour {

	static public DamageTextManager Instance;
	
	public Transform damageText;

    public Material PlayerDamageMaterial;
	public Material DamageMaterial;
	public Material HealthMaterial;
	
	public DamageTextTrackType TrackType;
	
	void Awake()
	{
		Instance = this;
	}
	
	public void ShowDamageText(int damage, Transform owner, bool iscrit, EStatusElementType elementType)
	{
		if(damageText)
		{
			Transform dt = Instantiate(damageText, Vector3.zero, Quaternion.identity) as Transform;
			dt.GetComponent<DamageText>().Owner = owner;

            // if it's elemental damage, the track of damage text should go parabolically. otherwise, go linear.
            if (elementType.Get() == EStatusElementType.StatusElement_Invalid)
            {
                dt.GetComponent<DamageText>().TrackType = TrackType;
                dt.GetComponent<DamageText>().RandomOriginalPos();
            }
            else
            {
                dt.GetComponent<DamageText>().TrackType = DamageTextTrackType.parabolic;
            }

            // set color of damage text.
			if(damage < 0)
            {
                if (!owner.GetComponent<Player>())
                {
                    dt.GetComponent<GUIText>().material = DamageMaterial;
					
					if(owner.GetComponent<AllyNpc>())
					    dt.GetComponent<GUIText>().material = PlayerDamageMaterial;
                }else
                {
                    dt.GetComponent<GUIText>().material = PlayerDamageMaterial;
                }
            }
			else
				dt.GetComponent<GUIText>().material = HealthMaterial;
			
			if(iscrit)
				dt.GetComponent<GUIText>().text = "Crit! ";
			else
				dt.GetComponent<GUIText>().text = "";
			
			dt.GetComponent<GUIText>().text += Mathf.Abs(damage);
		}
	}
	
	public void ShowDamageText(int damage, Transform owner)
	{
		ShowDamageText(damage, owner, false, new EStatusElementType());
	}
	
	public void ShowDamageText(string txt, Transform owner)
	{
		if(damageText)
		{
			Transform dt = Instantiate(damageText, Vector3.zero, Quaternion.identity) as Transform;
			dt.GetComponent<DamageText>().Owner = owner;
			dt.GetComponent<DamageText>().TrackType = TrackType;
			
			dt.GetComponent<GUIText>().material = DamageMaterial;
			
			dt.GetComponent<GUIText>().text = txt;
		}
	}
}
