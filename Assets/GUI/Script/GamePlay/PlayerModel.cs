using UnityEngine;
using System.Collections;

public class PlayerModel : MonoBehaviour
{
    public static PlayerModel Instance = null;

    /// <summary>
    /// Rotation speed on drag with mouse or touch
    /// </summary>
    public float m_characterRotationSpeed = 20f;
    /// <summary>
    /// Rotation smoothness when stop dragging used to align the character to the closest section
    /// </summary>
    public float m_characterRotationSmoothness = 0.2f;
    /// <summary>
    /// Rotation section, for example with 90, character will align to 0, 90, 180 or 270 degrees when stopped dragging
    /// </summary>
    public int m_characterRotationSection = 90;

    void Awake()
    {
        Instance = this;
        InitEquipMentManager();
    }

    void Start()
    {
        m_initialLightPosition = this.GetComponentInChildren<Light>().transform.position;
        m_initialLightOrientation = this.GetComponentInChildren<Light>().transform.rotation;
        this.gameObject.AddComponent<BoxCollider>();
    }

    void Update()
    {
        InitCharacterModel();

        float target = m_currentRotation;
        
        // When stopped dragging we align the character to closest section
        if (!m_isDragging && !m_clicked)
        {
            // Closest section at the left or right from the current position
            target = ((int)((m_currentRotation + m_characterRotationSection * (m_lastDirection == -1 ? 0.25f : 0.75f)) / m_characterRotationSection) * m_characterRotationSection);
        }
        else if (!m_isDragging && m_clicked)
        {
            // Next section according to the direction
            target = m_clickDestination;
        }
            
        if (Mathf.Abs(m_currentRotation - target) > 0.1f)
        {
            m_currentRotation = Mathf.SmoothDampAngle(m_currentRotation, target, ref m_currentVelocity, m_characterRotationSmoothness);
            this.transform.rotation = Quaternion.Euler(0f, m_currentRotation, 0f);
            ResetLight();
        }
    }

    public void Hide(bool hide)
    {
        gameObject.SetActive(!hide);
    }

    #region Character Rotation...
    private Vector3 m_initialLightPosition;
    private Quaternion m_initialLightOrientation;
    private float m_currentVelocity;
    private float m_currentRotation = 180f;
    private bool m_isDragging = false;
    private int m_lastDirection;
    private bool m_clicked = false;
    private float m_clickDestination;

    void OnDrag(Vector2 delta)
    {
        m_isDragging = true;
        m_clicked = false;
        m_lastDirection = delta.x >= 0f ? -1 : 1;
        this.transform.rotation *= Quaternion.Euler(0f,  - m_characterRotationSpeed * delta.x * Time.deltaTime, 0f);
        ResetLight();
    }
    
    // Stop dragging
    void OnPress(bool pressed)
    {
        if (!pressed && m_isDragging)
        {
            m_isDragging = false;
            m_clicked = false;
            m_currentRotation = this.transform.rotation.eulerAngles.y;
        }
    }

    void ResetLight()
    {
        Transform lightTransform = this.GetComponentInChildren<Light>().transform;
        lightTransform.position = m_initialLightPosition;
        lightTransform.rotation = m_initialLightOrientation;
    }

    void RotateLeft()
    {
        m_clicked = true;
        m_isDragging = false;
        m_currentRotation = Mathf.Round(this.transform.rotation.eulerAngles.y);
        m_clickDestination = (((int)((int)m_currentRotation / m_characterRotationSection) + 1) * m_characterRotationSection);

        if (m_clickDestination > 360f)
            m_clickDestination -= 360f;
    }

    void RotateRight()
    {
        m_clicked = true;
        m_isDragging = false;
        m_currentRotation = Mathf.Round(this.transform.rotation.eulerAngles.y);
        m_clickDestination = (((int)((int)m_currentRotation / m_characterRotationSection) - 1) * m_characterRotationSection);
        
        if (m_clickDestination < 0f)
            m_clickDestination += 360f;
    }
    #endregion

    #region equipMenut
    public EquipementManager equipmentMan;
    private void InitEquipMentManager()
    {
        if (!gameObject.GetComponent<EquipementManager>())
        {
            equipmentMan = gameObject.AddComponent<EquipementManager>();
            equipmentMan.Owner = transform;
        }
    }
    #endregion

    #region Light
    [SerializeField]
    private Transform light;
    public void OpenLight()
    {
        light.GetComponent<Light>().enabled = true;
    }
    public void CloseLight()
    {
        light.GetComponent<Light>().enabled = false;
    }
    #endregion

    #region InitCharacterModel
    [HideInInspector]
    public CharacterGenerator generator;
    [HideInInspector]
    public bool usingLatestConfig = false;
    public bool newCharacterRequested = true;
    [SerializeField]
    private Shader _newShader;
    [HideInInspector]
    public GameObject character;

    public ESex Gender
    {
        get
        {
            return m_gender;
        }
        set
        {
            m_gender = value;
        }
    }
    ESex m_gender = new ESex();

    private void InitCharacterModel()
    {
        if (!CharacterGenerator.ReadyToUse)
        {
            return;
        }
        if (equipmentMan.generator == null)
        {
            equipmentMan.generator = CharacterGenerator.CreateWithRandomConfig("ch_aka_f");
        }
        if (equipmentMan.generator == null) return;
        if (newCharacterRequested)
        {
            if (!equipmentMan.generator.ConfigReady)
                return;
            newCharacterRequested = false;
            character = equipmentMan.generator.Generate();
            character.transform.position = transform.position;
            character.transform.rotation = transform.rotation;
            character.transform.localScale = transform.localScale;
            character.transform.parent = transform;
            character.name = "Aka_Model";
            character.layer = LayerMask.NameToLayer("NGUI");
            character.animation["Aka_1H_Idle_1"].wrapMode = WrapMode.Loop;
            character.animation.CrossFade("Aka_1H_Idle_1");

            if (gameObject.GetComponent<AddWidgetCompleteCount>())
                gameObject.GetComponent<AddWidgetCompleteCount>().AddWidgetComplete();

            equipmentMan.UpdateEquipment(m_gender);
            RestAnimation();
        }
        else
        {
            if (usingLatestConfig)
            {
                if (!equipmentMan.generator.ConfigReady)
                    return;
                usingLatestConfig = false;
                character = equipmentMan.generator.Generate(character);
                RestAnimation();
            }
        }

        // Local collider will fit character bounding box, used to detect drag on character and make rotation
        BoxCollider collider = (BoxCollider)this.collider;
        collider.center = transform.InverseTransformPoint(character.renderer.bounds.center);
        collider.size = new Vector3(character.renderer.bounds.size.x / transform.lossyScale.x
                            , character.renderer.bounds.size.y / transform.lossyScale.y
                            , character.renderer.bounds.size.z / transform.lossyScale.z);
    }

    private void RestAnimation()
    {
        if (equipmentMan.RightHandWeapon != null)
        {
            if (equipmentMan.RightHandWeapon.GetComponent<WeaponBase>().WeaponType == WeaponBase.EWeaponType.WT_TwoHandWeaponSword)
            {
                character.animation["Aka_2HNodachi_Idle_1"].wrapMode = WrapMode.Loop;
                character.animation.CrossFade("Aka_2HNodachi_Idle_1");
            }
            else if (equipmentMan.RightHandWeapon.GetComponent<WeaponBase>().WeaponType == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
            {
                character.animation["Aka_2H_Idle_1"].wrapMode = WrapMode.Loop;
                character.animation.CrossFade("Aka_2H_Idle_1");
            }
            else
            {
                character.animation["Aka_1H_Idle_1"].wrapMode = WrapMode.Loop;
                character.animation.CrossFade("Aka_1H_Idle_1");
            }
        }
        else
        {
            character.animation["Aka_1H_Idle_1"].wrapMode = WrapMode.Loop;
            character.animation.CrossFade("Aka_1H_Idle_1");
        }

        foreach (Renderer _render in GetComponentsInChildren<Renderer>())
        {
            foreach (Material _mat in _render.sharedMaterials)
            {
                if (_mat == null)
                {
                    continue;
                }
                if (_mat.HasProperty("_EdgeWidth"))
                {
                    _mat.SetFloat("_EdgeWidth", 0.5f);
                }
            }
        }
    }

    public void UpdatePlayerAllEquipment(vectorSItemuuid equipList, ESex _gender)
    {
        m_gender = _gender;
        equipmentMan.DetachAllItems(m_gender);
        foreach (itemuuid equip in equipList)
        {
            Transform item = UnityEngine.Object.Instantiate(ItemPrefabs.Instance.GetItemPrefab(equip.itemID, 0, equip.prefabID)) as Transform;
            SItemInfo itemInfo = new SItemInfo();
            itemInfo.gem = equip.gemID;
            itemInfo.element = equip.elementID;
            itemInfo.enchant = equip.enchantID;
            equipmentMan.UpdateItemInfoBySlot((uint)equip.slotPart, item, itemInfo, true, m_gender);
        }
        equipmentMan.UpdateEquipment(m_gender);
        usingLatestConfig = true;
        ResetLayerForModel();
    }

    void ResetLayerForModel()
    {
        foreach (Transform _tran in transform)
        {
            _tran.gameObject.layer = LayerMask.NameToLayer("NGUI");
        }
    }
    #endregion
}
