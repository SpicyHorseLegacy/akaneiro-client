using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EquipementManager : MonoBehaviour {

    public Transform Owner;
    public CharacterGenerator generator;

    public enum EEquipmentType
    {
        RightHand_Weapon = 0,
        LeftHand_Weapon = 1,
        Helm,
        Cloak,
        Breastplate,
        Breeches,
        Ring,
        Necklace,
        Boots,
        Gloves,
        CloakHood,
        Max,
    }

	[HideInInspector]
	public List<Transform> MyWeapons;
	[HideInInspector]
	public Transform sheath1,sheath2;

    //Weapon
    public Transform LeftHandWeapon;
    public Transform RightHandWeapon;
		
    //Armor
    public Transform Helm;
    public Transform Cloak;
    public Transform Breastplate;
    public Transform Breeches;
		
    //Accessory
    public Transform Ring;
    public Transform Necklace;
    public Transform Boots;
    public Transform Gloves;

    public Transform CloakHood;

	Transform SocketPrefab;

    Transform _enchant_Healthy;
    Transform _enchant_Focus;
    Transform _enchant_Fortified;
	
	GameObject[] Assumbles = new GameObject[0];

    ESex gender;
	
	void Awake()
	{
		GameObject tempObj = new GameObject();
		SocketPrefab = tempObj.transform;
        SocketPrefab.position = Vector3.one * -5000;
		SocketPrefab.parent = transform;
	}

    public WeaponBase.EWeaponType GetWeaponType()
    {
        WeaponBase.EWeaponType wt = WeaponBase.EWeaponType.WT_NoneWeapon;

        if (RightHandWeapon != null)
        {
            if (RightHandWeapon)
            {
                if (RightHandWeapon.GetComponent<WeaponBase>() != null)
                    wt = RightHandWeapon.GetComponent<WeaponBase>().WeaponType;
            }

            if (wt == WeaponBase.EWeaponType.WT_OneHandWeapon && LeftHandWeapon)
            {
                wt = WeaponBase.EWeaponType.WT_DualWeapon;
            }
        }
        return wt;
    }

    public void UpdateItemInfoBySlot(uint slot, Transform itemEx, SItemInfo equipItemInfo, bool isAttach, ESex _gender)
    {
        gender = _gender;
        if(itemEx)
            itemEx.GetComponent<Item>().ItemInfo = equipItemInfo;
        EquipementManager.EEquipmentType itemType = EquipementManager.EEquipmentType.Max;
        switch (slot)
        {
            case 0:
                itemType = EquipementManager.EEquipmentType.Helm;
                break;
            case 2:
                itemType = EquipementManager.EEquipmentType.Breastplate;
                break;
            case 3:
                itemType = EquipementManager.EEquipmentType.Cloak;
                break;
            case 6:
                itemType = EquipementManager.EEquipmentType.RightHand_Weapon;
                if (isAttach)
                {
#if NGUI
#else
                    if (Owner == Player.Instance.transform)
                        _UI_CS_DebugInfo.Instance.SetWpn1Info(equipItemInfo, true);
#endif
                }
                else
                {
#if NGUI
#else
                    if (Owner == Player.Instance.transform)
                        _UI_CS_DebugInfo.Instance.SetWpn1Info(null, false);
#endif
                }
                break;
            case 7:
                itemType = EquipementManager.EEquipmentType.LeftHand_Weapon;
                if (isAttach)
                {
#if NGUI
#else
                    if(Owner == Player.Instance.transform)
                        _UI_CS_DebugInfo.Instance.SetWpn2Info(equipItemInfo, true);
#endif
                }
                else
                {
#if NGUI
#else
                    if (Owner == Player.Instance.transform)
                        _UI_CS_DebugInfo.Instance.SetWpn2Info(null, false);
#endif
                }
                break;
            case 8:
                itemType = EquipementManager.EEquipmentType.Breeches;
                break;
            default:
                break;
        }
        DetachItem(itemType, gender);
        RecordEquip(itemType, itemEx);

        if (itemType == EEquipmentType.Cloak)
        {
            Transform _cloakhood = null;
            if (Cloak)
            {
                _cloakhood = itemEx;
                _cloakhood.GetComponent<Item>().ItemInfo = Cloak.GetComponent<Item>().ItemInfo;
            }
            DetachItem(EEquipmentType.CloakHood, gender);
            if (_cloakhood && _cloakhood.GetComponent<ArmorBase>().CharactorName != "")
                RecordEquip(EEquipmentType.CloakHood, _cloakhood);
        }
    }

    public void UpdateEquipment(ESex _gender)
    {
        if(generator == null)
            generator = CharacterGenerator.CreateWithRandomConfig("ch_aka_f");	

        gender = _gender;
		
		DestroyAllAssumbles();
		
        AttachItem(EEquipmentType.LeftHand_Weapon, LeftHandWeapon);
        AttachItem(EEquipmentType.RightHand_Weapon, RightHandWeapon);

        if (CloakHood)
        {
			AttachItem(EEquipmentType.Helm, CloakHood);
        }
        else
        {
			AttachItem(EEquipmentType.Helm, Helm);
        }

        AttachItem(EEquipmentType.Cloak, Cloak);
        AttachItem(EEquipmentType.Breastplate, Breastplate);
        AttachItem(EEquipmentType.Breeches, Breeches);
        AttachItem(EEquipmentType.Ring, Ring);
        AttachItem(EEquipmentType.Necklace, Necklace);
        AttachItem(EEquipmentType.Boots, Boots);
        AttachItem(EEquipmentType.Gloves, Gloves);
		CheckAssumbles();
    }
	
	void DestroyAllAssumbles()
	{
		for(int i = Assumbles.Length - 1; i >= 0; i--)
			Destroy(Assumbles[i]);
		Assumbles = new GameObject[0];
	}
	
	void CheckAssumbles()
	{
		List<GameObject> _newassumbles = new List<GameObject>();
		CheckAssumble(RightHandWeapon, _newassumbles);
		CheckAssumble(LeftHandWeapon, _newassumbles);
		CheckAssumble(Helm, _newassumbles);
		CheckAssumble(Breastplate, _newassumbles);
		CheckAssumble(Breeches, _newassumbles);
		CheckAssumble(CloakHood, _newassumbles);
		CheckAssumble(Ring, _newassumbles);
		CheckAssumble(Necklace, _newassumbles);
		CheckAssumble(Boots, _newassumbles);
		CheckAssumble(Boots, _newassumbles);
		Assumbles = _newassumbles.ToArray();
	}
	
	void CheckAssumble(Transform _part, List<GameObject> _list)
	{
		if(_part && _part.GetComponent<AssumbleManager>())	
		{
			GameObject _newassumble = _part.GetComponent<AssumbleManager>().AttachToObject(Owner.gameObject, gender);
			if(_newassumble != null)
				_list.Add(_newassumble);
		}
	}

    void AttachItem(EquipementManager.EEquipmentType ItemType, Transform Item)
    {
        if (Item)
        {
            SItemInfo _iteminfo = null;
            if (Item) _iteminfo = Item.GetComponent<Item>().ItemInfo;
            else _iteminfo = new SItemInfo();
            AttachItem(ItemType, Item, _iteminfo, gender);
        }
        else
        {
            //Debug.LogWarning(ItemType + " : null!");
            DetachItem(ItemType, gender);
        }
    }

    void AttachItem(EquipementManager.EEquipmentType ItemType, Transform Item, SItemInfo itemInfo, ESex _gender)
    {
        //Debug.LogError("id : " + itemInfo.ID);
        if (!Item)
            return;

        if (Item.GetComponent<Item>())
        {
            Item.GetComponent<Item>().ItemInfo = itemInfo;
        }
        else
        {
            Debug.LogError("Item : " + Item.name + " dont have component <Item>!");
        }

        if (Item.collider)
            Destroy(Item.collider);

        //Item.transform.localScale = new Vector3(1.0f,1.0f,1.0f);

        Transform[] all = Owner.GetComponentsInChildren<Transform>();
        
        // Find the socket and init the item (weapon or cloak)
        Transform socket = null;
		bool isMirrorWeapon = false;
        foreach (Transform T in all)
        {
            if (T.name == "Bip001 Prop2")
            {
                if (ItemType == EquipementManager.EEquipmentType.LeftHand_Weapon &&
					Item.GetComponent<Item>().TypeID != 1011 &&
					Item.GetComponent<Item>().TypeID != 1012 &&
					Item.GetComponent<Item>().TypeID != 4003 &&
					Item.GetComponent<Item>().TypeID != 4004 &&
					Item.GetComponent<Item>().TypeID != 4005 &&
					Item.GetComponent<Item>().TypeID != 4006 &&
					Item.GetComponent<Item>().TypeID != 4050 &&
					Item.GetComponent<Item>().TypeID != 4053 &&
					Item.GetComponent<Item>().TypeID != 4060)
                {
                    Item.gameObject.SetActive(true);
                    LeftHandWeapon = Item;
                    LeftHandWeapon.GetComponent<WeaponBase>().InitVFXWithItemInfo(itemInfo, Vector3.zero);
                    socket = T.transform;
                }
            }
            else if (T.name == "Bip001 Prop1")
            {
                if (ItemType == EquipementManager.EEquipmentType.RightHand_Weapon &&
					Item.GetComponent<Item>().TypeID != 1011 &&
					Item.GetComponent<Item>().TypeID != 1012 &&
					Item.GetComponent<Item>().TypeID != 4003 &&
					Item.GetComponent<Item>().TypeID != 4004 &&
					Item.GetComponent<Item>().TypeID != 4005 &&
					Item.GetComponent<Item>().TypeID != 4006 &&
					Item.GetComponent<Item>().TypeID != 4050 &&
					Item.GetComponent<Item>().TypeID != 4053 &&
					Item.GetComponent<Item>().TypeID != 4060)
                {
                    Item.gameObject.SetActive(true);
                    RightHandWeapon = Item;
                    RightHandWeapon.GetComponent<WeaponBase>().InitVFXWithItemInfo(itemInfo,Vector3.zero);
                    socket = T.transform;
                }
            }
            else if (T.name == "Bip001 L Forearm")
            {
                if (ItemType == EquipementManager.EEquipmentType.LeftHand_Weapon &&
					(Item.GetComponent<Item>().TypeID == 1011 ||
					Item.GetComponent<Item>().TypeID == 1012 ||
					Item.GetComponent<Item>().TypeID == 4003 ||
					Item.GetComponent<Item>().TypeID == 4004 ||
					Item.GetComponent<Item>().TypeID == 4005 ||
					Item.GetComponent<Item>().TypeID == 4006 ||
					Item.GetComponent<Item>().TypeID == 4050 ||
					Item.GetComponent<Item>().TypeID == 4053 ||
					Item.GetComponent<Item>().TypeID == 4060))
                {
                    Item.gameObject.SetActive(true);
                    LeftHandWeapon = Item;
                    LeftHandWeapon.GetComponent<WeaponBase>().InitVFXWithItemInfo(itemInfo, Vector3.up * -90f);
                    socket = T.transform;
                }
            }
            else if (T.name == "Bip001 R Forearm")
            {
                if (ItemType == EquipementManager.EEquipmentType.RightHand_Weapon && 
					(Item.GetComponent<Item>().TypeID == 1011 ||
					Item.GetComponent<Item>().TypeID == 1012 ||
					Item.GetComponent<Item>().TypeID == 4003 ||
					Item.GetComponent<Item>().TypeID == 4004 ||
					Item.GetComponent<Item>().TypeID == 4005 ||
					Item.GetComponent<Item>().TypeID == 4006 ||
					Item.GetComponent<Item>().TypeID == 4050 ||
					Item.GetComponent<Item>().TypeID == 4053 ||
					Item.GetComponent<Item>().TypeID == 4060))
                {
                    Item.gameObject.SetActive(true);
                    RightHandWeapon = Item;
                    RightHandWeapon.GetComponent<WeaponBase>().InitVFXWithItemInfo(itemInfo, Vector3.up * -90f);
                    socket = T.transform;
					isMirrorWeapon = true;
                }
            }
            else if (T.name == "Bip001 Cloak")
            {
                T.gameObject.layer = LayerMask.NameToLayer("Player");
                if (ItemType == EquipementManager.EEquipmentType.Cloak)
                {
                    ArmorBase theCloak = Item.GetComponent<ArmorBase>();
                    theCloak.gameObject.layer = LayerMask.NameToLayer("Player");

                    if (theCloak != null && theCloak.armortype == ArmorBase.Armortype.isCloak)
                    {
                        if (theCloak.ReplaceTransform != null && theCloak.ReplaceInstance == null)
                        {
                            theCloak.ReplaceInstance = Instantiate(theCloak.ReplaceTransform) as Transform;
                            theCloak.ReplaceInstance.gameObject.SetActive(true);
                        }

                        if (theCloak.ReplaceInstance)
                        {
                            theCloak.ReplaceInstance.gameObject.AddComponent<ItemDownLoading>();

                            if (theCloak.ReplaceInstance.GetComponent<ItemDownLoading>() != null)
                            {
                                theCloak.ReplaceInstance.GetComponent<ItemDownLoading>().MaterialBundle = Item.GetComponent<ItemDownLoading>().MaterialBundle;
                                theCloak.ReplaceInstance.GetComponent<ItemDownLoading>().bStartDownLoadModel = true;
                            }
                            theCloak.ReplaceInstance.gameObject.SetActive(true);

                            if (!theCloak.ReplaceInstance.GetComponent<ArmorBase>())
                                theCloak.ReplaceInstance.gameObject.AddComponent<ArmorBase>();
                            theCloak.ReplaceInstance.GetComponent<Item>().ItemInfo = itemInfo;
                            theCloak.ReplaceInstance.parent = T.transform;
                            theCloak.ReplaceInstance.localPosition = theCloak.ReplaceTransform.localPosition;
                            theCloak.ReplaceInstance.localRotation = theCloak.ReplaceTransform.localRotation;
                            theCloak.gameObject.layer = Owner.gameObject.layer;
                            theCloak.ReplaceInstance.gameObject.layer = Owner.gameObject.layer;
                            theCloak.ReplaceInstance.localScale = Vector3.one;
                            SetLayerRecursively(theCloak.ReplaceInstance, Owner.gameObject.layer);
                        }
                    }
                }
            }
        }


        switch (ItemType)
        {
            case EquipementManager.EEquipmentType.Helm:
            case EquipementManager.EEquipmentType.Breastplate:
            case EquipementManager.EEquipmentType.Breeches:
                if (Item.GetComponent<ArmorBase>() && generator != null)
                {
                    generator.ConfigComponent(ItemType, Item.GetComponent<ArmorBase>().CharactorName, _gender, itemInfo.gem);
                }
                break;
        }

        Item.GetComponent<Item>().SetOwner(Owner);

        if (!socket)
        {
            socket = SocketPrefab;
        }

        Item.transform.parent = socket;
        Item.transform.localPosition = Vector3.zero;
        Item.transform.localRotation = Quaternion.identity;
        
		// when equip some weapon like fist and claw on right hand, we need to mirror the object.
		if (isMirrorWeapon)
			Item.transform.localScale = new Vector3(1.0f, 1.0f, -1.0f);
		else
			Item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        if (!Owner)
        {
            ;// Debug.Log("no owner " + transform.parent.name);
        }
        foreach (Transform mr in Item.gameObject.transform.GetComponentsInChildren<Transform>())
        {
            mr.gameObject.layer = Owner.gameObject.layer;
        }
    }

    public void DetachAllItems(ESex _gender)
    {
        //Debug.LogError("[Equipment] Detach all equipments!");
        for (int i = 0; i < (int)EquipementManager.EEquipmentType.Max; i++)
        {
            DetachItem((EquipementManager.EEquipmentType)i, _gender);
        }
    }

    public void DetachItem(EquipementManager.EEquipmentType ItemType, ESex _gender)
	{
        // detaching helm/breastplat/breeches means change to no wearing model.
		switch(ItemType)
		{
			case EquipementManager.EEquipmentType.Helm:
			case EquipementManager.EEquipmentType.Breastplate:
			case EquipementManager.EEquipmentType.Breeches:
                if (generator != null)
                    generator.ConfigNoneComponent(ItemType, _gender);
				break;
		}

		DropItem(ItemType);
	}

    public void Equip(EEquipmentType type, Transform item, Transform socket)
    {
        if (!item) return;
        item.GetComponent<Item>().SetOwner(Owner);

        if (!socket)
        {
            socket = SocketPrefab;
        }

		if(!Owner)
		{
            ;// Debug.Log("no owner " + transform.parent.name);
		}
        foreach (Transform mr in item.gameObject.transform.GetComponentsInChildren<Transform>())
        {
            mr.gameObject.layer = Owner.gameObject.layer;
        }

        RecordEquip(type, item);
		
		item.transform.parent = socket;
		
		item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		
        //attach sheath
        if (item.GetComponent<WeaponBase>())
        {
            Transform sheath = item.GetComponent<WeaponBase>().sheath;
            if (sheath)
            {
                Component[] all = Owner.GetComponentsInChildren<Component>();
                foreach (Component T in all)
                {
                    if (T && T.name == "Bip001 Bone_Back_01")
                    {
                        if (item.GetComponent<WeaponBase>().Sheahth1)
                        {
                            item.GetComponent<WeaponBase>().Sheahth1.parent = T.transform;
                            item.GetComponent<WeaponBase>().Sheahth1.position = T.transform.position;
                            item.GetComponent<WeaponBase>().Sheahth1.rotation = T.transform.rotation;
                            item.GetComponent<WeaponBase>().Sheahth1.gameObject.SetActive(true);
                        }
                    }
                    else if (T && T.name == "Bip001 Bone_Back_02")
                    {
                        if (item.GetComponent<WeaponBase>().Sheahth2)
                        {
                            item.GetComponent<WeaponBase>().Sheahth2.parent = T.transform;
                            item.GetComponent<WeaponBase>().Sheahth2.position = T.transform.position;
                            item.GetComponent<WeaponBase>().Sheahth2.rotation = T.transform.rotation;
                            item.GetComponent<WeaponBase>().Sheahth2.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }

	public void DropItem(EEquipmentType ItemType)
	{
        Transform item = GetEquip(ItemType);
        if (item)
        {
            //hide sheath
            if (item.GetComponent<WeaponBase>())
            {
                if (item.GetComponent<WeaponBase>().Sheahth1)
                {
                    item.GetComponent<WeaponBase>().Sheahth1.gameObject.SetActive(false);
                }
                if (item.GetComponent<WeaponBase>().Sheahth2)
                {
                    item.GetComponent<WeaponBase>().Sheahth2.gameObject.SetActive(false);
                }
                if (item.GetComponent<MeleeWeaponTrail>())
                {
                    item.GetComponent<MeleeWeaponTrail>().DestroyMesh();
                }
            }
            ArmorBase theCloak = item.GetComponent<ArmorBase>();
			if( theCloak != null && theCloak.armortype == ArmorBase.Armortype.isCloak)
			{
				if( theCloak.ReplaceInstance)
				{
                    Destroy(theCloak.ReplaceInstance.gameObject);	  
				}
			}
            Destroy(item.gameObject);
        }

        RecordEquip(ItemType, null);
	}

    void RecordEquip(EEquipmentType type, Transform item)
    {
        if (item)
        {
            item.transform.parent = SocketPrefab;
            item.transform.localPosition = Vector3.zero;
        }
        switch (type)
        {
            case EEquipmentType.RightHand_Weapon:
                RightHandWeapon = item;
                break;
            case EEquipmentType.LeftHand_Weapon:
                LeftHandWeapon = item;
                break;
            case EEquipmentType.Helm:
                Helm = item;
                break;
            case EEquipmentType.Cloak:
                Cloak = item;
                break;
            case EEquipmentType.Breastplate:
                Breastplate = item;
                break;
            case EEquipmentType.Breeches:
                Breeches = item;
                break;
            case EEquipmentType.Ring:
                Ring = item;
                break;
            case EEquipmentType.Necklace:
                Necklace = item;
                break;
            case EEquipmentType.Boots:
                Boots = item;
                break;
            case EEquipmentType.Gloves:
                Gloves = item;
                break;
            case EEquipmentType.CloakHood:
                CloakHood = item;
                break;
        }
    }

    Transform GetEquip(EEquipmentType type)
    {
        Transform equip = null;
        switch (type)
        {
            case EEquipmentType.RightHand_Weapon:
                equip = RightHandWeapon;
                break;
            case EEquipmentType.LeftHand_Weapon:
                equip = LeftHandWeapon;
                break;
            case EEquipmentType.Helm:
                equip = Helm;
                break;
            case EEquipmentType.Cloak:
                equip = Cloak;
                break;
            case EEquipmentType.Breastplate:
                equip = Breastplate;
                break;
            case EEquipmentType.Breeches:
                equip = Breeches;
                break;
            case EEquipmentType.Ring:
                equip = Ring;
                break;
            case EEquipmentType.Necklace:
                equip = Necklace;
                break;
            case EEquipmentType.Boots:
                equip = Boots;
                break;
            case EEquipmentType.Gloves:
                equip = Gloves;
                break;
            case EEquipmentType.CloakHood:
                equip = CloakHood;
                break;
        }
        return equip;
    }

    public void UpdateVFXForArmorEnchant()
    {
        if (_enchant_Healthy)
        {
            if (!_enchant_Healthy.GetComponent<DestructAfterTime>())
                _enchant_Healthy.gameObject.AddComponent<DestructAfterTime>();
            _enchant_Healthy.GetComponent<DestructAfterTime>().DestructNow();
            _enchant_Healthy = null;
        }
        if (_enchant_Focus)
        {
            if (!_enchant_Focus.GetComponent<DestructAfterTime>())
                _enchant_Focus.gameObject.AddComponent<DestructAfterTime>();
            _enchant_Focus.GetComponent<DestructAfterTime>().DestructNow();
            _enchant_Focus = null;
        }
        if (_enchant_Fortified)
        {
            if (!_enchant_Fortified.GetComponent<DestructAfterTime>())
                _enchant_Fortified.gameObject.AddComponent<DestructAfterTime>();
            _enchant_Fortified.GetComponent<DestructAfterTime>().DestructNow();
            _enchant_Fortified = null;
        }

        if (Helm && Helm.GetComponent<Item>())
        {
            InitVFXWithEnchantID(Helm.GetComponent<Item>().ItemInfo.enchant);
        }
        if (Cloak && Cloak.GetComponent<Item>())
        {
            InitVFXWithEnchantID(Cloak.GetComponent<Item>().ItemInfo.enchant);
        }
        if (Breastplate && Breastplate.GetComponent<Item>())
        {
            InitVFXWithEnchantID(Breastplate.GetComponent<Item>().ItemInfo.enchant);
        }
        if (Breeches && Breeches.GetComponent<Item>())
        {
            InitVFXWithEnchantID(Breeches.GetComponent<Item>().ItemInfo.enchant);
        }
        if (Ring && Ring.GetComponent<Item>())
        {
            InitVFXWithEnchantID(Ring.GetComponent<Item>().ItemInfo.enchant);
        }
        if (Necklace && Necklace.GetComponent<Item>())
        {
            InitVFXWithEnchantID(Necklace.GetComponent<Item>().ItemInfo.enchant);
        }
        if (Boots && Boots.GetComponent<Item>())
        {
            InitVFXWithEnchantID(Boots.GetComponent<Item>().ItemInfo.enchant);
        }
        if (Gloves && Gloves.GetComponent<Item>())
        {
            InitVFXWithEnchantID(Gloves.GetComponent<Item>().ItemInfo.enchant);
        }
    }

    void InitVFXWithEnchantID(int enchantID)
    {
        //Debug.LogError("Armor Enchant ID : " + enchantID);
        Transform prefab = null;
        Vector3 pos = Vector3.zero;
        if (enchantID == 101 || enchantID == 102 || enchantID == 103 || enchantID == 104 || enchantID == 105 || enchantID == 106 || enchantID == 107 || enchantID == 108 || enchantID == 109 || enchantID == 110 )
        {
            if (_enchant_Healthy) return;
            _enchant_Healthy = Object.Instantiate(ItemPrefabs.Instance.Enchant_Armor_HealthyPrefab) as Transform;
            pos = Owner.position + Vector3.up * 0.1f;
            prefab = _enchant_Healthy;
        }
        if (enchantID == 201 || enchantID == 202 || enchantID == 203 || enchantID == 204 || enchantID == 205 || enchantID == 206 || enchantID == 207 || enchantID == 208 || enchantID == 209 || enchantID == 210)
        {
            if (_enchant_Focus) return;
            _enchant_Focus = Object.Instantiate(ItemPrefabs.Instance.Enchant_Armor_FocusedPrefab) as Transform;
            pos = Owner.position + Vector3.up * 1.9f;
            prefab = _enchant_Focus;
        }
        if (enchantID == 301 || enchantID == 302 || enchantID == 303 || enchantID == 304 || enchantID == 305 || enchantID == 306 || enchantID == 307 || enchantID == 308 || enchantID == 309 || enchantID == 310)
        {
            if (_enchant_Fortified) return;
            _enchant_Fortified = Object.Instantiate(ItemPrefabs.Instance.Enchant_Armor_FortifiedPrefab) as Transform;
            pos = Owner.position + Vector3.up * 0.7f;
            prefab = _enchant_Fortified;
        }
        if (prefab)
        {
            prefab.position = pos;
            prefab.parent = Owner;
            Transform[] children = prefab.GetComponentsInChildren<Transform>();
        }
    }

	void SetLayerRecursively(Transform temp,int layer)
    {
		temp.gameObject.layer = layer;
		for(int i = 0; i < temp.GetChildCount();i++)
		{
			SetLayerRecursively(temp.GetChild(i),layer);
		}
    }
}