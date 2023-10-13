using UnityEngine;
using System.Collections;

public class LootDropMoving : MonoBehaviour {

	// Use this for initialization
	Quaternion startRotate;

    bool moving;

    void Start()
    {
        startRotate = transform.rotation;
    }
	
    public void JumpToGround(bool isOriginialPos)
    {
        Vector3 _startPos = CS_SceneInfo.pointOnTheGround(transform.position);
        Vector3 _endPos = _startPos;
        if (!isOriginialPos)
        {
            Vector3 _dir = (Player.Instance.transform.position - _startPos).normalized;
            _endPos += _dir;
            _endPos += new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
            _endPos = AstarPath.GetNeasetPos(_endPos);
            _endPos += new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
            _endPos = CS_SceneInfo.pointOnTheGround(_endPos);
        }

        // check if there is box nearby, drop in front of the box.
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1);
        foreach (Collider _collider in colliders)
        {
            if (_collider.GetComponent<InteractiveObjInsideTrigger>())
            {
                _endPos = _collider.transform.position + _collider.transform.forward * Random.Range(1.0f, 1.5f) + _collider.transform.right * (Random.Range(-1.0f, 1.0f));
            }
        }

        Vector3 _middlePos = _startPos + (_endPos - _startPos).normalized * (Vector3.Distance(_startPos, _endPos) / 2) + Vector3.up * 2;

        Vector3[] path = {_startPos, _middlePos, _endPos};

        iTween.MoveTo(gameObject, iTween.Hash("path", path, "time", 0.5f, "easetype", iTween.EaseType.linear));
        iTween.RotateBy(gameObject, iTween.Hash("amount", new Vector3(360, 360, 360), "time", 0.5f, "easetype", iTween.EaseType.linear, "oncomplete", "moveFinished", "oncompletetarget", gameObject));
        LootSoundManager.PlayRollingSound(transform);

        if (_lootvfx)
            DestructAfterTime.DestructGameObjectNow(_lootvfx.gameObject);
    }

    [HideInInspector]
    public ItemDropStruct _iteminfo;
    Transform _lootvfx;

    void moveFinished()
    {
        transform.position = CS_SceneInfo.pointOnTheGround(transform.position) + Vector3.up * 0.1f;
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        float _temprot = transform.eulerAngles.y;
        transform.rotation = startRotate;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, _temprot, transform.eulerAngles.z);

        LootSoundManager.PlayDropSound(transform);

        #region LOOT VFX
        float itemVal = _iteminfo.info_gemVal + _iteminfo.info_eleVal + _iteminfo.info_encVal;
        if (_lootvfx)
            DestructAfterTime.DestructGameObjectNow(_lootvfx.gameObject);

        Transform Prefab = null; ;
        if (GetComponent<Item_Mateiral>())
        {
            Prefab = GetComponent<Item_Mateiral>().LootVFX;
        }
        else
        {
#if NGUI
			switch(PlayerDataManager.Instance.GetItemValLevel(itemVal)) {
			case 1:
				Prefab = ItemVFX.Instance.VFXList[0];
				break;
			case 2:
				Prefab = ItemVFX.Instance.VFXList[1];
				break;
			case 3:
				Prefab = ItemVFX.Instance.VFXList[2];
				break;
			case 4:
				Prefab = ItemVFX.Instance.VFXList[3];
				break;
			default:
				break;
			}
#else
            if (itemVal < _UI_CS_ItemVendor.Instance.greenVal)
            {
            }
            else if ((_UI_CS_ItemVendor.Instance.greenVal - 0.01) < itemVal && itemVal < _UI_CS_ItemVendor.Instance.blueVal)
            {
                if (ItemVFX.Instance != null && ItemVFX.Instance.VFXList != null && ItemVFX.Instance.VFXList.Length > 0)
                    Prefab = ItemVFX.Instance.VFXList[0];
            }
            else if ((_UI_CS_ItemVendor.Instance.blueVal - 0.01) < itemVal && itemVal < _UI_CS_ItemVendor.Instance.purpleVal)
            {
                if (ItemVFX.Instance != null && ItemVFX.Instance.VFXList != null && ItemVFX.Instance.VFXList.Length > 0)
                    Prefab = ItemVFX.Instance.VFXList[1];
            }
            else if ((_UI_CS_ItemVendor.Instance.purpleVal - 0.01) < itemVal && itemVal < _UI_CS_ItemVendor.Instance.brownVal)
            {
                if (ItemVFX.Instance != null && ItemVFX.Instance.VFXList != null && ItemVFX.Instance.VFXList.Length > 0)
                    Prefab = ItemVFX.Instance.VFXList[2];
            }
            else if (itemVal > _UI_CS_ItemVendor.Instance.brownVal - 0.01)
            {
                if (ItemVFX.Instance != null && ItemVFX.Instance.VFXList != null && ItemVFX.Instance.VFXList.Length > 0)
                    Prefab = ItemVFX.Instance.VFXList[3];
            }
#endif
        }

        if (Prefab)
            _lootvfx = (Transform)Instantiate(Prefab, transform.position, Quaternion.identity);
        #endregion
    }

    public void RemoveLootVFX()
    {
        if (_lootvfx)
            DestructAfterTime.DestructGameObjectNow(_lootvfx.gameObject);
    }
}