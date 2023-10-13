using UnityEngine;
using System.Collections;

public enum EArmorCaterogy
{
	light,
	medium,
	heavy
};

public class ArmorBase : Item 
{
	public enum Armortype
	{
		isHelmet = 0,
		isCloak,
		isBreastplate,
		isBreeches,
	};
	public Armortype armortype;
	public EArmorCaterogy ArmorCaterogy;
	
	[HideInInspector] public string Armor_Name;
	[HideInInspector] public int ArmorAmount;

	public string CharactorName;
	
	public Transform ReplaceTransform;
	
    //[HideInInspector]
	public Transform ReplaceInstance;

    void Awake()
    {
        ItemType = Item.EItem_Type.EItem_Armor;
    }
	
	public override void Start ()
	{
		base.Start ();
	}
	/*
	public void OnGUI ()
	{
		if(IsEquipted) return;
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		int layer = 1<<LayerMask.NameToLayer("DropItem");
		if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layer))
		{
			if(hit.transform == transform)
			{
				//mouse over me, show item info
				GUI.Box(new Rect(190,85,300,300),"");
				GUI.Label(new Rect(200,90,200,20),Prefix1_Name+" "+Prefix2_Name+" "+ Armor_Name);
				GUI.Label(new Rect(200,110,200,20),"Required level: "+Prefix1_Level);
				GUI.Label(new Rect(200,130,200,20),"Armor amount: " + ArmorAmount);
				GUI.Label(new Rect(200,150,200,20),"Color: " + PrefixColor_Color);
				if(Prefix1_Level>4)
				{
					GUI.Label(new Rect(200,170,200,20),Prefix2_Description+Prefix2_Modifier_1+"%");
				}
			}
		}
	}
	*/

    void Update()
    {
        if (Camera.main)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layer = 1 << LayerMask.NameToLayer("DropItem");
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, layer))
            {
                if (hit.transform == transform)
                {
                    ShowArmorText();
                    ShowTip();
                }
            }
            else
            {
                HideTip();
            }
        }
    }
	
	public void ShowArmorText()
	{
//		if(IsEquipted) return;
//		
//		UIButton tipTransform = InGameUIControl.Instance.LootToolTip;
//		if( tipTransform == null)
//		   return;
//		
//		string Context = Prefix1_Name+" "+Prefix2_Name+" "+Armor_Name + "\n";
//		Context += "Required level: "+Prefix1_Level + "\n";
//		Context += "Armor amount: "+  ArmorAmount + "\n";
//		Context += "Color: " + PrefixColor_Color + "\n";
//		if(Prefix1_Level>4)
//	    {
//		    Context += Prefix2_Description+Prefix2_Modifier_1+"%" + "\n";
//	    }
//		
//		tipTransform.Text = Context;
	}
	
	public void InitArmorColorWithGemID(int gemID)
	{
        if (ArmorGemColorManager.Instance)
        {
            Color[] targetColors = new Color[2];

            for (int i = 0; i < targetColors.Length; i++)
            {
				if(gemID == 0)
				{
					targetColors[i] = ArmorGemColorManager.Instance.None[i];
					continue;
				}
				
				if(gemID == 501 || gemID == 502 || gemID == 503 || gemID == 504 || gemID == 505 || 
					gemID == 506 || gemID == 507 || gemID == 508 || gemID == 509|| gemID == 510)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Ruby[i];
					continue;
				}
				
				if(gemID == 601 || gemID == 602 || gemID == 603 || gemID == 604 || gemID == 605 || 
					gemID == 606 || gemID == 607 || gemID == 608 || gemID == 609|| gemID == 610)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Sapphire[i];
					continue;
				}
				
				if(gemID == 701 || gemID == 702 || gemID == 703 || gemID == 704 || gemID == 705 || 
					gemID == 706 || gemID == 707 || gemID == 708 || gemID == 709|| gemID == 710)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Emerald[i];
					continue;
				}
				
				if(gemID == 801 || gemID == 802 || gemID == 803 || gemID == 804 || gemID == 805 || 
					gemID == 806 || gemID == 807 || gemID == 808 || gemID == 809|| gemID == 810)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Garnet[i];
					continue;
				}
				
				if(gemID == 901 || gemID == 902 || gemID == 903 || gemID == 904 || gemID == 905 || 
					gemID == 906 || gemID == 907 || gemID == 908 || gemID == 909|| gemID == 910)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Amethyst[i];
					continue;
				}
				
				if(gemID == 1001 || gemID == 1002 || gemID == 1003 || gemID == 1004 || gemID == 1005 || 
					gemID == 1006 || gemID == 1007 || gemID == 1008 || gemID == 1009|| gemID == 1010)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Malachite[i];
					continue;
				}
				
				if(gemID == 1101 || gemID == 1102 || gemID == 1103 || gemID == 1104 || gemID == 1105 || 
					gemID == 1106 || gemID == 1107 || gemID == 1108 || gemID == 1109|| gemID == 1110)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Obsidian[i];
					continue;
				}
				
				if(gemID == 1201 || gemID == 1202 || gemID == 1203 || gemID == 1204 || gemID == 1205 || 
					gemID == 1206 || gemID == 1207 || gemID == 1208 || gemID == 1209|| gemID == 1210)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Golden[i];
					continue;
				}
				
				if(gemID == 1301 || gemID == 1302 || gemID == 1303 || gemID == 1304 || gemID == 1305 || 
					gemID == 1306 || gemID == 1307 || gemID == 1308 || gemID == 1309|| gemID == 1310)
				{
					targetColors[i] = ArmorGemColorManager.Instance.Jade[i];
					continue;
				}
            }

            Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                for (int j = 0; j < renderers[i].materials.Length; j++)
                {
					// if it's better gem, like golden or jade, use luxury shader.
	                if (gemID > 1200)
	                {
	                    renderers[i].materials[j].shader = ArmorGemColorManager.Instance.LuxuryShader;
	                }
	                else
	                {
	                    renderers[i].materials[j].shader = ArmorGemColorManager.Instance.NoramlShader;
	                }
	
	                Material mtl =  renderers[i].materials[j];
	                if (mtl.HasProperty("_TintColorR"))
	                {
	                    mtl.SetColor("_TintColorR", targetColors[0]);
	                }
	                if (mtl.HasProperty("_TintColorG"))
	                {
	                    mtl.SetColor("_TintColorG", targetColors[1]);
	                }
	                if (mtl.HasProperty("_PulsingTex"))
	                {
						// only Golden gem use luxxry pusling tex02
	                    if (gemID > 1200 && gemID < 1300)
	                        mtl.SetTexture("_PulsingTex", ArmorGemColorManager.Instance.LuxxryPuslingTex02);
	                    else
	                        mtl.SetTexture("_PulsingTex", ArmorGemColorManager.Instance.LuxuryPuslingTex01);
	                }
					if(mtl.HasProperty("_PulsingSpeed"))
					{
						mtl.SetFloat("_PulsingSpeed", 0.41f);
					}
					if(mtl.HasProperty("_PulsingBrightness"))
					{
						mtl.SetFloat("_PulsingBrightness", 0.55f);
					}
				}
            }
        }
	}
	
	public void InitArmorColorWithItemInfo(SItemInfo itemInfo)
	{
        if(itemInfo != null)
		    InitArmorColorWithGemID(itemInfo.gem);
	}
}
