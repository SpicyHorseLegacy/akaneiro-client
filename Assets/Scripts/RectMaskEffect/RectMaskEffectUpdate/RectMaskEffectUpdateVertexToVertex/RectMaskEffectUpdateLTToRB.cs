/***********************************************************************
        filename:   RectMaskEffectUpdateLTToRB.cs
        created:    2012.05.02
        author:     Twj

        purpose:    矩形从左上到右下遮罩效果的更新
*************************************************************************/

using UnityEngine;
using System.Collections;

public class RectMaskEffectUpdateLTToRB : RectMaskEffectUpdateVertexToVertex
{
    // public functions

    // 构造函数
    // @params: startTime - 遮罩效果开始时间
    // @params: totalTime - 遮罩效果总时间
    // @params: halfHeight - 矩形宽的一半
    // @params: halfWidth - 矩形高的一半
    // @params: mesh - 遮罩网格
    public RectMaskEffectUpdateLTToRB(
            float startTime,
            float totalTime,
            float halfHeight,
            float halfWidth,
            Mesh mesh )
    : base( startTime, totalTime, halfHeight, halfWidth, mesh )
    {
        Vector3[] vertices = mesh.vertices;
        vertices[1] = vertices[7] = vertices[9] = vertices[8];
        vertices[3] = vertices[2];
        vertices[5] = vertices[6];
        mesh.vertices = vertices;

        //
        infoFirst = new Info[3];
        infoFirst[0].dir = new Vector3( 2.0f*halfWidth, 0.0f, 0.0f );
        infoFirst[0].arrIndices = new int[]{ 1, 9 };
        
        infoFirst[1].dir = new Vector3( 0.0f, -2.0f*halfHeight, 0.0f );
        infoFirst[1].arrIndices = new int[]{ 7 };
    
        infoFirst[2].dir = (infoFirst[0].dir + infoFirst[1].dir) * 0.5f;    // 矩形斜边的一半
        infoFirst[2].arrIndices = new int[]{ 8 };

        //
        infoAfter = new Info[3];
        infoAfter[0].dir = new Vector3( 2.0f*halfWidth, 0.0f, 0.0f );
        infoAfter[0].arrIndices = new int[]{ 5, 6, 7 };

        infoAfter[1].dir = new Vector3( 0.0f, -2.0f*halfHeight, 0.0f );
        infoAfter[1].arrIndices = new int[]{ 1, 2, 3, 9 };

        infoAfter[2].dir = (infoFirst[0].dir + infoFirst[1].dir) * 0.5f;    // 矩形斜边的一半
        infoAfter[2].arrIndices = new int[]{ 0, 8 };
    }
    /////////////////////////////////////////////////////////////////////
}