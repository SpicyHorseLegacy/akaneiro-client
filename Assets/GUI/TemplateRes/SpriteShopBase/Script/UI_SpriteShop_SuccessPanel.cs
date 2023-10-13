using UnityEngine;
using System.Collections;

public class UI_SpriteShop_SuccessPanel : MonoBehaviour {

    public static UI_SpriteShop_SuccessPanel Instance;

	[SerializeField] GameObject PetModelParent;
	[SerializeField] UILabel Label_PetName;
	[SerializeField] GameObject SuccessVFX;
	GameObject petmodel;

    void Awake() { Instance = this; }
	
	public void ShowPetModel(GameObject _petModel, string _petname)
    {
        if (petmodel)
        {
            Destroy(petmodel);
            petmodel = null;
        }
        petmodel = _petModel;
        petmodel.transform.parent = PetModelParent.transform;
        petmodel.transform.localScale = Vector3.one;
        petmodel.transform.localEulerAngles = Vector3.zero;
        petmodel.transform.localPosition = Vector3.zero;

		Label_PetName.text = _petname;

		UnityEngine.Object.Instantiate(SuccessVFX, transform.position, transform.rotation);
    }

    void BTN_OK_Clicked()
    {
        if(InGameScreenShopSpriteCtrl.Instance)
            Player.Instance.CallOnSpirit((SpiritBase.eSpiriteType)InGameScreenShopSpriteCtrl.Instance.choosePetID);

        UI_SpriteShop_Manager.Instance.CloseBTNClicked();
    }
}
