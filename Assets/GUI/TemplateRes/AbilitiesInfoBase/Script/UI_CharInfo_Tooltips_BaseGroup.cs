using UnityEngine;
using System.Collections;

public class UI_CharInfo_Tooltips_BaseGroup : MonoBehaviour 
{
    public virtual float GetVisualHeight()
    {
        return NGUIMath.CalculateRelativeWidgetBounds(transform).extents.y * 2 + 15f;
    }
}
