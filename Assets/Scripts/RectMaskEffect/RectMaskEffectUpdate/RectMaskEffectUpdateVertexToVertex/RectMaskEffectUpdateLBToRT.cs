/***********************************************************************
        filename:   RectMaskEffectUpdateLBToRT.cs
        created:    2012.05.02
        author:     Twj

        purpose:    矩形从左下到右上遮罩效果的更新
*************************************************************************/

using UnityEngine;
using System.Collections;

public class RectMaskEffectUpdateLBToRT : RectMaskEffectUpdateVertexToVertex
{
    // public functions

    // 构造函数
    // @params: startTime - 遮罩效果开始时间
    // @params: totalTime - 遮罩效果总时间
    // @params: halfHeight - 矩形宽的一半
    // @params: halfWidth - 矩形高的一半
    // @params: mesh - 遮罩网格
    public RectMaskEffectUpdateLBToRT(
            float startTime,
            float totalTime,
            float halfHeight,
            float halfWidth,
            Mesh mesh )
    : base( startTime, totalTime, halfHeight, halfWidth, mesh )
    {
        Vector3[] vertices = mesh.vertices;
        vertices[5] = vertices[7] = vertices[6];
        vertices[1] = vertices[9] = vertices[8];
        vertices[3] = vertices[4];
        mesh.vertices = vertices;

        //
        infoFirst = new Info[3];
        infoFirst[0].dir = new Vector3( 2.0f*halfWidth, 0.0f, 0.0f );
        infoFirst[0].arrIndices = new int[]{ 5 };
        
        infoFirst[1].dir = new Vector3( 0.0f, 2.0f*halfHeight, 0.0f );
        infoFirst[1].arrIndices = new int[]{ 7 };
    
        infoFirst[2].dir = (infoFirst[0].dir + infoFirst[1].dir) * 0.5f;    // 矩形斜边的一半
        infoFirst[2].arrIndices = new int[]{ 6 };

        //
        infoAfter = new Info[3];
        infoAfter[0].dir = new Vector3( 2.0f*halfWidth, 0.0f, 0.0f );
        infoAfter[0].arrIndices = new int[]{ 1, 7, 8, 9 };

        infoAfter[1].dir = new Vector3( 0.0f, 2.0f*halfHeight, 0.0f );
        infoAfter[1].arrIndices = new int[]{ 3, 4, 5 };

        infoAfter[2].dir = (infoFirst[0].dir + infoFirst[1].dir) * 0.5f;    // 矩形斜边的一半
        infoAfter[2].arrIndices = new int[]{ 0, 6 };
    }
    /////////////////////////////////////////////////////////////////////
}