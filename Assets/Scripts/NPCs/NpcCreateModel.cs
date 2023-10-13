using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

public class NpcCreateModel : MonoBehaviour {
	
	[System.Serializable]
	public class NPCWeaponInfo
	{
		public Transform WeaponPrefab = null;
		public float Size = 1;
        public Vector3 POS = new Vector3();
		public Vector3 ROT = new Vector3();
        public WantedVFXInfo Wanted_VFX_Weapon = null;
	}

    [System.Serializable]
    public class WantedVFXInfo
    {
        public Transform VFX_Prefab = null;
        public float Size = 1;
    }
	
	public Transform ModelPrefab;

    public Vector3 ModelPosition;
    public Vector3 ModelRotation;
    public float ScaleSize = 1.0f;
	
	public NPCWeaponInfo RightWeapon;
	public NPCWeaponInfo LeftWeapon;

    public WantedVFXInfo WantedVFX_Foot;
    public WantedVFXInfo ElementalVFX_Foot;
    public Transform WantedVFX_Foot_Instance;
	
	public Material[] materials;
	
	[HideInInspector]	public NpcBase Owner;
	
	bool isCreateDone = false;
	
	void Awake()
	{
		isCreateDone = false;
	}
	
	// add a traingle to avoid unity culls the animaiton if enmey did any action to jump out of the screen.
	void AddATraingle(Transform _p)
	{
		GameObject newTraingle = new GameObject("Traingle");

		Mesh mesh= new Mesh();
		ArrayList vertices = new ArrayList();
		vertices.Add(new Vector3(0.01f, 0, 0));
		vertices.Add(new Vector3(-0.01f, 0, 0));
		vertices.Add(new Vector3(0, 0.01f, 0));
		ArrayList triangles=new ArrayList();
		for(int i=0;i<vertices.Count;i++)
		{
			triangles.Add(i);
		}
		mesh.vertices=(Vector3[])vertices.ToArray(typeof(Vector3));
		mesh.triangles=(int[])triangles.ToArray(typeof(int));
		MeshFilter _mf = newTraingle.AddComponent<MeshFilter>();
		_mf.mesh = mesh;
		
		newTraingle.AddComponent<MeshRenderer>();
		
		newTraingle.transform.parent = _p;
		newTraingle.transform.position = _p.position;
	}
	
    #region CreateModel

    public void CreateAniamtionModel(NpcBase _o)
    {
		if(isCreateDone) return;
		
        Owner = _o;
        Transform model = Instantiate(ModelPrefab) as Transform;
		model.gameObject.SetActive(true);
		Owner.AnimationModel = model;
		Owner.AnimationModel.parent = transform;
        Owner.AnimationModel.localPosition = ModelPosition;
        Owner.AnimationModel.localEulerAngles = ModelRotation;
        Owner.AnimationModel.gameObject.AddComponent<NpcAnimationEventReceiver>();
        Owner.AnimationModel.GetComponent<NpcAnimationEventReceiver>().Owner = Owner;

        transform.localScale = new Vector3(ScaleSize, ScaleSize, ScaleSize);
        Owner.AvoidanceRadius = Owner.AvoidanceRadius * ScaleSize;

        AddATraingle(model);

        if (!model.GetComponent<Animation>())
            model.gameObject.AddComponent<Animation>();

        if (model.GetComponent<Animation>())
        {
            //model.GetComponent<Animation>().cullingType = AnimationCullingType.AlwaysAnimate;
            model.GetComponent<Animation>().clip = null;
            foreach (AnimationState state in model.GetComponent<Animation>())
            {
                model.GetComponent<Animation>().RemoveClip(state.clip);
            }
            AddAnimation(Owner.GetComponent<NpcBase>().GetType(), Owner.GetComponent<NpcBase>(), model.animation);

            NPCAbilityBaseState[] abis = Owner.GetComponentsInChildren<NPCAbilityBaseState>();
            foreach (NPCAbilityBaseState abi in abis)
            {
                AddAnimation(abi.GetType(), abi, model.animation);
            }
        }
		
		isCreateDone = true;
    }

    public void CreateModelForOwner()
	{
        #region Create_Material
        if (materials.Length > 0)
		{
            Renderer skinMeshRenderer = Owner.AnimationModel.GetComponentInChildren<Renderer>();
            if (skinMeshRenderer)
                skinMeshRenderer.sharedMaterials = materials;
        }
        #endregion

        #region Create_Weapons
        // equip weapon
        Transform[] childs = Owner.AnimationModel.GetComponentsInChildren<Transform>();

		foreach(Transform it in childs)
		{
			it.gameObject.layer = LayerMask.NameToLayer("NPC");

            if (it.name == "Bip001 Prop1" || it.name == "Bone_WP_Right")
			{
				foreach( Transform weapon in it )
				{
					//Destroy(weapon.gameObject);
				}
				
				if(RightWeapon.WeaponPrefab)
				{
                    if (ModelPrefab.name == "CH_OniShaman")
                    {
                        Transform wp = Owner.AnimationModel.FindChild("WP_OniShaman");
                        if (wp) Destroy(wp.gameObject);
                    }

					//int index = Random.Range(0, Weapons.Length);
					Transform rightWeapon = Instantiate(RightWeapon.WeaponPrefab, it.position, it.rotation)  as Transform;
					rightWeapon.parent = it;
					rightWeapon.localScale = Vector3.one * RightWeapon.Size;
                    rightWeapon.localPosition = RightWeapon.POS;
					rightWeapon.localEulerAngles = RightWeapon.ROT;
                    if (rightWeapon.GetComponent<ScaleParticle>())
                    {
                        rightWeapon.GetComponent<ScaleParticle>().scale = RightWeapon.Size;
                    }
					Owner.EquipWeaponWhichSide(rightWeapon, 0);

                    if (RightWeapon.Wanted_VFX_Weapon.VFX_Prefab)
                    {
                        Transform weapon_wanted_vfx = Instantiate(RightWeapon.Wanted_VFX_Weapon.VFX_Prefab, rightWeapon.position, rightWeapon.rotation) as Transform;
						weapon_wanted_vfx.parent = rightWeapon;
                        weapon_wanted_vfx.gameObject.AddComponent<ScaleParticle>();
                        weapon_wanted_vfx.GetComponent<ScaleParticle>().scale = RightWeapon.Wanted_VFX_Weapon.Size;
                    }
				}
			}
			
			if(it.name == "Bip001 Prop2")
			{
				foreach( Transform weapon in it )
				{
					//Destroy(weapon.gameObject);
				}
				
				if(LeftWeapon.WeaponPrefab)
				{
					//int index = Random.Range(0, Weapons.Length);
					Transform leftWeapon = Instantiate(LeftWeapon.WeaponPrefab, it.position, it.rotation)  as Transform;
					leftWeapon.parent = it;
					leftWeapon.localScale = Vector3.one * LeftWeapon.Size;
                    leftWeapon.localPosition = LeftWeapon.POS;
					leftWeapon.localEulerAngles = LeftWeapon.ROT;
                    if (leftWeapon.GetComponent<ScaleParticle>())
                    {
                        leftWeapon.GetComponent<ScaleParticle>().scale = LeftWeapon.Size;
                    }
					Owner.EquipWeaponWhichSide(leftWeapon, 1);

                    if (LeftWeapon.Wanted_VFX_Weapon.VFX_Prefab)
                    {
                        Transform weapon_wanted_vfx = Instantiate(LeftWeapon.Wanted_VFX_Weapon.VFX_Prefab, leftWeapon.position, leftWeapon.rotation) as Transform;
						weapon_wanted_vfx.parent = leftWeapon;
                        weapon_wanted_vfx.gameObject.AddComponent<ScaleParticle>();
                        weapon_wanted_vfx.GetComponent<ScaleParticle>().scale = LeftWeapon.Wanted_VFX_Weapon.Size;
                    }
				}
			}
        }
        #endregion

        #region Create_Foot_VFX
        // wanted foot vfx
        if (WantedVFX_Foot.VFX_Prefab && (Owner.IsWanted || Owner.IsBoss))
        {
            Vector3 tempPos = ModelPosition + Vector3.up * 0.1f;
            WantedVFX_Foot_Instance = Instantiate(WantedVFX_Foot.VFX_Prefab, tempPos, Quaternion.identity) as Transform;
            WantedVFX_Foot_Instance.parent = Owner.transform;
            WantedVFX_Foot_Instance.localPosition = tempPos;
            if (!WantedVFX_Foot_Instance.GetComponent<ScaleParticle>())
                WantedVFX_Foot_Instance.gameObject.AddComponent<ScaleParticle>();
            WantedVFX_Foot_Instance.GetComponent<ScaleParticle>().scale = WantedVFX_Foot.Size;
            WantedVFX_Foot_Instance.gameObject.AddComponent<SelfSpin>();
            WantedVFX_Foot_Instance.GetComponent<SelfSpin>().SpinSpeed = new Vector3(0, 30, 0);
            WantedVFX_Foot_Instance.GetComponent<SelfSpin>().Life = -1;
        }

        if (ElementalVFX_Foot.VFX_Prefab)
        {
            Vector3 tempPos = ModelPosition + Vector3.up * 0.1f;
            Transform _elementalVFX_foot_instance = Instantiate(ElementalVFX_Foot.VFX_Prefab, tempPos, Quaternion.identity) as Transform;
            _elementalVFX_foot_instance.parent = Owner.transform;
            _elementalVFX_foot_instance.localPosition = tempPos;
            if (!_elementalVFX_foot_instance.GetComponent<ScaleParticle>())
                _elementalVFX_foot_instance.gameObject.AddComponent<ScaleParticle>();
            _elementalVFX_foot_instance.GetComponent<ScaleParticle>().scale = ElementalVFX_Foot.Size;
        }
        #endregion
    }
    #endregion

    void AddAnimation(Type _t, object _c, Animation _animation)
    {
        System.Reflection.FieldInfo[] f_infos = _t.GetFields();
        foreach (System.Reflection.FieldInfo _info in f_infos)
        {
            if (_info.FieldType.BaseType == typeof(System.Array))
            {
                System.Array _tempArray = _info.GetValue(_c) as System.Array;

                if (_tempArray.Length > 0)
                {
                    for (int i = _tempArray.Length - 1; i >= 0; i--)
                    {
                        if (_tempArray.GetValue(i) != null && _tempArray.GetValue(i).GetType() == typeof(UnityEngine.AnimationClip))
                        {
                            AnimationClip _ani = (AnimationClip)_tempArray.GetValue(i);
							//Debug.LogError(_ani.name);
                            _animation.AddClip(_ani, _ani.name);
                        }
                    }
                }
            }
                
            else if (_info.FieldType == typeof(AnimationClip))
            {
                AnimationClip _ani = (AnimationClip)_info.GetValue(_c);
				
                if (_ani != null)
                {
                    //Debug.LogError(_ani.name);
                    _animation.AddClip(_ani, _ani.name);
                }
            }
        }
    }
}
