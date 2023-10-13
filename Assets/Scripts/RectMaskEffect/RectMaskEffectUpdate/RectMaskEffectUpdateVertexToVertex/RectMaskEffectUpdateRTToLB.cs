/***********************************************************************
        filename:   RectMaskEffectUpdateRTToLB.cs
        created:    2012.05.02
        author:     Twj

        purpose:    ���δ����ϵ���������Ч���ĸ���
*************************************************************************/

using UnityEngine;
using System.Collections;

public class RectMaskEffectUpdateRTToLB : RectMaskEffectUpdateVertexToVertex
{
    // public functions

    // ���캯��
    // @params: startTime - ����Ч����ʼʱ��
    // @params: totalTime - ����Ч����ʱ��
    // @params: halfHeight - ���ο��һ��
    // @params: halfWidth - ���θߵ�һ��
    // @params: mesh - ��������
    public RectMaskEffectUpdateRTToLB(
            float startTime,
            float totalTime,
            float halfHeight,
            float halfWidth,
            Mesh mesh )
    : base( startTime, totalTime, halfHeight, halfWidth, mesh )
    {
        Vector3[] vertices = mesh.vertices;
        vertices[7] = vertices[8];
        vertices[5] = vertices[4];
        vertices[1] = vertices[9] = vertices[3] = vertices[2];
        mesh.vertices = vertices;

        //
        infoFirst = new Info[3];
        infoFirst[0].dir = new Vector3( -2.0f*halfWidth, 0.0f, 0.0f );
        infoFirst[0].arrIndices = new int[]{ 1, 9 };
        
        infoFirst[1].dir = new Vector3( 0.0f, -2.0f*halfHeight, 0.0f );
        infoFirst[1].arrIndices = new int[]{ 3 };
    
        infoFirst[2].dir = (infoFirst[0].dir + infoFirst[1].dir) * 0.5f;    // ����б�ߵ�һ��
        infoFirst[2].arrIndices = new int[]{ 2 };

        //
        infoAfter = new Info[3];
        infoAfter[0].dir = new Vector3( -2.0f*halfWidth, 0.0f, 0.0f );
        infoAfter[0].arrIndices = new int[]{ 3, 4, 5 };

        infoAfter[1].dir = new Vector3( 0.0f, -2.0f*halfHeight, 0.0f );
        infoAfter[1].arrIndices = new int[]{ 1, 7, 8, 9 };

        infoAfter[2].dir = (infoFirst[0].dir + infoFirst[1].dir) * 0.5f;    // ����б�ߵ�һ��
        infoAfter[2].arrIndices = new int[]{ 0, 2 };
    }
    /////////////////////////////////////////////////////////////////////
}