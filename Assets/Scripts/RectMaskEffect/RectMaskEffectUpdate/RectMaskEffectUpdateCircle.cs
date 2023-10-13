/***********************************************************************
        filename:   RectMaskEffectUpdate.cs
        created:    2012.04.28
        author:     Twj

        purpose:    矩形圆周遮罩效果的更新
*************************************************************************/

using UnityEngine;
using System.Collections;

public class RectMaskEffectUpdateCircle : RectMaskEffectUpdate
{
    // private variables

    private float timePerTriangle;    // 每个三角面遮罩的时间(= 遮罩总时间 / 8(矩形分割为8个三角面))
    /////////////////////////////////////////////////////////////////////

    // public functions

    // 构造函数
    // @params: startTime - 遮罩效果开始时间
    // @params: totalTime - 遮罩效果总时间
    // @params: halfHeight - 矩形宽的一半
    // @params: halfWidth - 矩形高的一半
    public RectMaskEffectUpdateCircle(
            float startTime,
            float totalTime,
            float halfHeight,
            float halfWidth )
    : base( startTime, totalTime, halfHeight, halfWidth )
    {
        timePerTriangle = totalTime * 0.125f;   // 0.125表示1/8
    }
    
    // 更新遮罩效果
    // @params: mesh - 遮罩网格
    public override void UpdateEffect( Mesh mesh )
    {
        float deltaTime = Time.time - startTime;    // 效果已播放的时间
        
        // 更新顶点数据
        Vector3[] vertices = mesh.vertices;
        float percent = (deltaTime % timePerTriangle) / timePerTriangle;  // 每个三角面遮罩的百分比
        int index = (int)(deltaTime / timePerTriangle) + 1; // 当前修改的顶点索引
        for ( int i = 1; i <= index; ++i )
        {
            switch ( index )
            {
            case 1:
                vertices[i].x = halfWidth * percent;
                break;

            case 2:
                vertices[i].y = halfHeight - halfHeight*percent;
                break;

            case 3:
                vertices[i].y = -halfHeight * percent;
                break;

            case 4:
                vertices[i].x = halfWidth * (1 - percent);
                break;

            case 5:
                vertices[i].x = -halfWidth * percent;
                break;

            case 6:
                vertices[i].y = -halfHeight * (1 - percent);
                break;

            case 7:
                vertices[i].y = halfHeight * percent;
                break;

            case 8:
                vertices[i].x = -halfWidth + halfWidth*percent;
                break;
            }
        }

        mesh.vertices = vertices;
    }
    /////////////////////////////////////////////////////////////////////
}