using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_SpriteShop_Manager : MonoBehaviour
{
    public static UI_SpriteShop_Manager Instance;

    [SerializeField]  NGUIPanel PetListParent;
    [SerializeField]  UI_SpriteShop_PetItem PetItem_Prefab;
    [SerializeField]  UI_SpriteShop_DetailPanel DetailPanel;

    UI_SpriteShop_PetItem[] petitems = new UI_SpriteShop_PetItem[0];

    UI_TypeDefine.UI_SpriteShop_PetItem_data curDetailPetData;

    void Awake() { Instance = this; }

    public void InitAllPets(UI_TypeDefine.UI_SpriteShop_PetItem_data[] _datas)
    {
        List<UI_SpriteShop_PetItem> _tempitems = new List<UI_SpriteShop_PetItem>();
        for (int i = 0; i < _datas.Length; i++)
        {
            UI_SpriteShop_PetItem _item = GetAPetItem(i);
            if (_item != null)
            {
                _item.UpdateAllInfo(_datas[i]);
                _tempitems.Add(_item);
            }
        }
        for (int i = _datas.Length; i < petitems.Length; i++ )
        {
            Destroy(petitems[i].gameObject);
        }
        petitems = _tempitems.ToArray();
        _tempitems.Clear();
        _tempitems = null;

        RepositionAllitems();

        BTNClicked(petitems[0]);
    }

    public void ShowPetModel(GameObject petModel)
    {
        DetailPanel.ShowPetModel(petModel);
    }

    UI_SpriteShop_PetItem GetAPetItem(int _index)
    {
        if (_index < petitems.Length)
            return petitems[_index];

        UI_SpriteShop_PetItem _newitem = UnityEngine.Object.Instantiate(PetItem_Prefab) as UI_SpriteShop_PetItem;
        _newitem.transform.parent = PetListParent.transform;
        _newitem.transform.localScale = Vector3.one;
        return _newitem;
    }

    void RepositionAllitems()
    {
        for (int i = 0; i < petitems.Length; i++)
        {
            petitems[i].transform.localPosition = new Vector3(-280, 145, -1) + new Vector3(330 * (i % 2), 0, 0) + new Vector3(0, -100 * (i / 2), 0);
        }
    }

    public delegate void Handle_UIShopSpriteCloseBTNClicked();
    public event Handle_UIShopSpriteCloseBTNClicked SpriteShopCloseClicked_Event;
    public delegate void Handle_UIShopSpriteItemBTNClicked(UI_TypeDefine.UI_SpriteShop_PetItem_data petdata);
    public event Handle_UIShopSpriteItemBTNClicked SpriteShopItemClicked_Event;
    public delegate void Handle_UIShopSummonBTNClicked(int petID, int _buytime);
    public event Handle_UIShopSummonBTNClicked SpriteSummonBTNClicked_Event;
    public delegate void Handle_UIShopChooseBTNClicked(int petID);
    public event Handle_UIShopChooseBTNClicked SpriteChooseBTNClicked_Event;
    #region BTN Callback
    public void CloseBTNClicked()
    {
        if (SpriteShopCloseClicked_Event != null)
        {
            SpriteShopCloseClicked_Event();
        }
    }

    public void BTNClicked(UI_SpriteShop_PetItem _btn)
    {
		if(_btn.GetData() != null)
		{
			DetailPanel.UpdateAllInfo(_btn.GetData());
		}
        
        curDetailPetData = _btn.GetData();

        if (SpriteShopItemClicked_Event != null)
        {
            SpriteShopItemClicked_Event(_btn.GetData());
        }
    }

    public void SummonBTNClicked(int _buytime)
    {
        if (SpriteSummonBTNClicked_Event != null)
        {
            SpriteSummonBTNClicked_Event(curDetailPetData.PetID, _buytime);
        }
    }

    public void ChooseBTNClicked()
    {
        if (SpriteChooseBTNClicked_Event != null)
        {
            SpriteChooseBTNClicked_Event(curDetailPetData.PetID);
        }
    }
    #endregion
}
