/***********************************************************************
        filename:   RectMaskEffectUpdateSideToSide.cs
        created:    2012.04.28
        author:     Twj

        purpose:    矩形从一边到一边遮罩效果的更新基类
*************************************************************************/

using UnityEngine;
using System.Collections;

public class RectMaskEffectUpdateSideToSide : RectMaskEffectUpdate
{
    // protected variables

    protected float     halfTotalTime;      // 遮罩效果总时间的一半
    protected int[]     arrIndices;         // 所需修改的顶点索引列表
    protected int[]     arrIndicesFirst;    // 遮罩前半段时间内所需修改的顶点索引列表
    protected int[]     arrIndicesAfter;    // 遮罩后半段时间内所需修改的顶点索引列表
    /////////////////////////////////////////////////////////////////////
    
    // public functions

    // 构造函数
    // @params: startTime - 遮罩效果开始时间
    // @params: totalTime - 遮罩效果总时间
    // @params: halfHeight - 矩形宽的一半
    // @params: halfWidth - 矩形高的一半
    public RectMaskEffectUpdateSideToSide(
            float startTime,
            float totalTime,
            float halfHeight,
            float halfWidth )
    : base( startTime, totalTime, halfHeight, halfWidth )
    {
        halfTotalTime = totalTime * 0.5f;
    }

    // 更新遮罩效果
    // @params: mesh - 遮罩网格
    public override void UpdateEffect( Mesh mesh )
    {
        base.UpdateEffect( mesh );
        
        //
        if ( deltaTime >= halfTotalTime )
        {
            arrIndices = arrIndicesAfter;
        }
        else
        {
            arrIndices = arrIndicesFirst;
        }
    }
    /////////////////////////////////////////////////////////////////////
}