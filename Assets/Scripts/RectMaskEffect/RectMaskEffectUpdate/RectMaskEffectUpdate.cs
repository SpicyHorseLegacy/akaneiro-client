/***********************************************************************
        filename:   RectMaskEffectUpdate.cs
        created:    2012.04.28
        author:     Twj

        purpose:    矩形遮罩效果的更新
*************************************************************************/

using UnityEngine;
using System.Collections;

[System.Serializable]
public class RectMaskEffectUpdate
{
    // public variables
    
    // 更新遮罩效果
    // @params: mesh - 遮罩网格
    public virtual void UpdateEffect( Mesh mesh )
    {
        deltaTime = Time.time - startTime;  // 效果已播放的时间
    }
    /////////////////////////////////////////////////////////////////////

    // protected variables

    protected float startTime;          // 遮罩效果开始时间
    protected float totalTime;          // 遮罩效果总时间
    protected float halfHeight;         // 矩形宽的一半
    protected float halfWidth;          // 矩形高的一半
    protected float deltaTime;          // 效果已播放的时间
    /////////////////////////////////////////////////////////////////////

    // public functions

    // 构造函数
    // @params: startTime - 遮罩效果开始时间
    // @params: totalTime - 遮罩效果总时间
    // @params: halfHeight - 矩形宽的一半
    // @params: halfWidth - 矩形高的一半
    public RectMaskEffectUpdate(
            float startTime,
            float totalTime,
            float halfHeight,
            float halfWidth )
    {
        this.startTime = startTime;
        this.totalTime = totalTime;
        this.halfHeight = halfHeight;
        this.halfWidth = halfWidth;
    }
    /////////////////////////////////////////////////////////////////////
}