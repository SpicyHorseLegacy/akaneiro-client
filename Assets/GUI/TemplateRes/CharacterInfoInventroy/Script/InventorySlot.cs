using UnityEngine;
using System.Collections;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private int type;

    [SerializeField]
    private int slot;

    private _ItemInfo data;

    #region Interface
    private bool isEmpty = true;

    public void SetEmptyFlag(bool empty)
    {
        isEmpty = empty;
    }

    public bool GetEmptyFlag()
    {
        return isEmpty;
    }

    public void SetData(_ItemInfo info)
    {
        data = info;
    }
    public _ItemInfo GetData()
    {
        return data;
    }

    public int GetType()
    {
        return type;
    }

    public int GetSlot()
    {
        return slot;
    }

    [SerializeField]
    private UISlicedSprite bg;
    public void ChangeBGColor(Color color)
    {
        bg.color = color;
    }

    [SerializeField]
    private UITexture icon;
    public void UpdateIcon(Texture2D img)
    {
        icon.enabled = false;
        icon.mainTexture = img;
        icon.enabled = true;
    }
    public UITexture GetIcon()
    {
        return icon;
    }

    private int count = 0;
    [SerializeField]
    private UILabel countText;
    public void SetCount(int num)
    {
        count = num;
        if (!IsShowCount())
        {
            countText.text = "";
        }
        else
        {
            countText.text = num.ToString();
        }
    }
    private bool IsShowCount()
    {
        if (isEmpty || data == null)
        {
            return false;
        }
        if (data.localData._TypeID >= 11)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public int GetCount()
    {
        return count;
    }

    public void EmptySlot(Texture2D img)
    {
        SetEmptyFlag(true);
        HideHighLight(true);
        UpdateIcon(img);
        SetCount(0);
        ChangeBGColor(Color.white);
    }

    [SerializeField]
    private UI_Hud_Border_Control highLightObj;
    public void HideHighLight(bool isHide)
    {
        if (highLightObj == null)
        {
            GUILogManager.LogErr("highLightObj is null.type: " + type + "|soly:" + slot);
            return;
        }
        if (isHide)
        {
            NGUITools.SetActive(highLightObj.gameObject, false);
        }
        else
        {
            NGUITools.SetActive(highLightObj.gameObject, true);
            
            if(data != null && data.localData != null)
            {
                highLightObj.ChangeColor((data.localData.info_Level <= PlayerDataManager.Instance.CurLV) ? Color.yellow : Color.red);           
                highLightObj.Pop(1, -1);
            }
        }
    }
    #endregion

    #region Local
    public delegate void Handle_ClickDelegate(int type, int slot);
    public event Handle_ClickDelegate OnClickDelegate;
    private void _ClickDelegate()
    {
        //		GUILogManager.LogErr("_ClickDelegate");
        if (OnClickDelegate != null && !isEmpty)
        {
            OnClickDelegate(type, slot);
        }
    }
    public delegate void Handle_DoubleClickDelegate(int type, int slot);
    public event Handle_DoubleClickDelegate OnDoubleClickDelegate;
    public void _DoubleClickDelegate()
    {
        //		GUILogManager.LogErr("_DoubleClickDelegate");
        if (OnDoubleClickDelegate != null && !isEmpty)
        {
            OnDoubleClickDelegate(type, slot);
        }
    }
    public delegate void Handle_PressDelegate(int type, int slot);
    public event Handle_PressDelegate OnPressDelegate;
    private void _PressDelegate()
    {
        //		GUILogManager.LogErr("_PressDelegate");
        if (OnPressDelegate != null && !isEmpty)
        {
            OnPressDelegate(type, slot);
        }
    }
    public delegate void Handle_ReleaseDelegate(int type, int slot);
    public event Handle_ReleaseDelegate OnReleaseDelegate;
    private void _ReleaseDelegate()
    {
        //		GUILogManager.LogErr("_ReleaseDelegate");
        if (OnReleaseDelegate != null && !isEmpty)
        {
            OnReleaseDelegate(type, slot);
        }
    }
    #endregion

}
