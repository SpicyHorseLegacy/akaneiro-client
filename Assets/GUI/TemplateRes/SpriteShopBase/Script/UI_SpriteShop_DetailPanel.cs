using UnityEngine;
using System.Collections;

public class UI_SpriteShop_DetailPanel : MonoBehaviour
{
    [SerializeField]  UISprite Pet_Icon;
    [SerializeField]  UILabel Label_PetName;
    [SerializeField]  UILabel Label_PetDescription;
    [SerializeField]  UI_CoolDownCycle_Control RemainTimeGroup;
    [SerializeField]  UI_MoneyGroup[] SummonPrice;
    [SerializeField]  GameObject PetModelParent;
	[SerializeField]  GameObject DescriptionGroup;
	[SerializeField]  GameObject CallGroup;
	[SerializeField]  GameObject CallBTN;
    [SerializeField]  GameObject SummonBTNPanel;
	[SerializeField]  UI_CoolDownCycle_Control Cooldown;
	[SerializeField]  UILabel Label_UnlockHint;

    [SerializeField]  GameObject petmodel;

    public void UpdateAllInfo(UI_TypeDefine.UI_SpriteShop_PetItem_data data)
    {
        Pet_Icon.spriteName = data.PetSimpleIconName;
        Label_PetName.text = data.PetName;
        Label_PetDescription.text = data.PetDetailDescription;

        Label_UnlockHint.gameObject.SetActive(data.isLocked);
        if (data.isLocked)
        {
            Label_UnlockHint.text = "You need to reach level " + data.RequirdLevel + " to unlock this spirit.";

            CallGroup.SetActive(false);
            SummonBTNPanel.SetActive(false);
            DescriptionGroup.transform.localPosition = new Vector3(DescriptionGroup.transform.localPosition.x, -275, DescriptionGroup.transform.localPosition.z);
            Label_PetDescription.text = "?????????????????";
        }
        else
        {
            SummonPrice[0].UpdateAllInfo(data.Price1HourType.Get() == EMoneyType.eMoneyType_SK, data.Price1HourValue.ToString());
            SummonPrice[1].UpdateAllInfo(data.Price1DayType.Get() == EMoneyType.eMoneyType_SK, data.Price1DayValue.ToString());
            SummonPrice[2].UpdateAllInfo(data.Price1WeekType.Get() == EMoneyType.eMoneyType_SK, data.Price1WeekValue.ToString());

            CallGroup.SetActive(data.LastTime > 0 || data.isKSPet);
			Cooldown.gameObject.SetActive(data.isKSPet == false);
            if (data.LastTime > 0)
            {
				UI_TypeDefine.UI_LearnAbilityCoolDown_data _data = new UI_TypeDefine.UI_LearnAbilityCoolDown_data();
				_data.CurAbiCooldown = data.LastTime;
				_data.MaxAbiCooldown = data.MaxTime;
				Cooldown.UpdateAllInfo(_data);
				Cooldown.StartCoolDown();
                CallBTN.SetActive(!data.IsCalled);
                Label_UnlockHint.gameObject.SetActive(data.IsCalled);
                if (data.IsCalled)
                    Label_UnlockHint.text = "You have called this spirit!";
            }
			SummonBTNPanel.SetActive(data.LastTime <= 0 && !data.isKSPet);
			
			Vector3 descriptionPos = DescriptionGroup.transform.localPosition;
			descriptionPos.y = (data.LastTime > 0 || data.isKSPet) ? -250f : -275f;
            DescriptionGroup.transform.localPosition = descriptionPos;
        }
    }

    public void ShowPetModel(GameObject _petModel)
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
    }

    void CallPet_BTNClicked()
    {
        UI_SpriteShop_Manager.Instance.ChooseBTNClicked();
    }

    void OneHourBTNClicked()
    {
        UI_SpriteShop_Manager.Instance.SummonBTNClicked(buypetTime.Prop_ksOneH_money);
    }
    void OneDayBTNClicked()
    {
        UI_SpriteShop_Manager.Instance.SummonBTNClicked(buypetTime.Prop_ks1D_money);
    }
    void OneWeekBTNClicked()
    {
        UI_SpriteShop_Manager.Instance.SummonBTNClicked(buypetTime.Prop_fk7D_RealMoney);
    }
}
