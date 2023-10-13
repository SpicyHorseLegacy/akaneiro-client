using UnityEngine;
using System.Collections;

public enum DamageTextTrackType
{
	parabolic,
	linearUp,
	linearDown
}

public class DamageText : MonoBehaviour {
	
	public Transform Owner;
    public Vector3 RandomPos = Vector3.zero;
	public DamageTextTrackType TrackType;
	
	Vector2 offset;
	
	float fontSize = 25;
	
	public float time = 0.2f;
	float time_Total = 0.2f;
	
	int LOR = 1;
	
	void Awake()
	{
		offset = Vector2.zero;
		int chance = Random.Range(0,2);
		if(chance == 0)
			LOR = -1;

        transform.GetComponent<GUIText>().pixelOffset = Vector2.one * -1000;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(time > 0)
		{
			time -= Time.deltaTime;
			
			GUIText text = transform.GetComponent<GUIText>();
			
			if(Owner)
			{
				Vector3 posOnScreen = Vector3.zero;
				if(TrackType == DamageTextTrackType.linearDown)
                    posOnScreen = Camera.mainCamera.WorldToScreenPoint(Owner.position + RandomPos - Vector3.up);
				else
                    posOnScreen = Camera.mainCamera.WorldToScreenPoint(Owner.position + RandomPos + Vector3.up * 2);
				
				Vector2 pos = new Vector2(posOnScreen.x, posOnScreen.y);
				float offsetX = 0;
				float offsetY = 0;
				switch(TrackType)
				{
				case DamageTextTrackType.parabolic:
					offsetX = 30 / time_Total * (time_Total - time) * LOR;
                    offsetY = Mathf.Abs(Mathf.Sin(time_Total * (time_Total - time) * Mathf.Deg2Rad) * 100);
					break;
					
				case DamageTextTrackType.linearUp:
					offsetY = 30 / time_Total * (time_Total - time);
					break;
					
				case DamageTextTrackType.linearDown:
					offsetY = -1 * 30 / time_Total * (time_Total - time);
					break;
				}
				pos.x += offsetX;
				pos.y += offsetY;
				
				text.pixelOffset = pos;
			}
		}else{
			Destroy(gameObject);
		}
	}

    public void RandomOriginalPos()
    {
        RandomPos = new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));
    }
}
