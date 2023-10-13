/***********************************************************************
        filename:   RectMaskEffectUpdateRBToLT.cs
        created:    2012.05.02
        author:     Twj

        purpose:    ���δ����µ���������Ч���ĸ���
*************************************************************************/

using UnityEngine;
using System.Collections;

public class RectMaskEffectUpdateRBToLT : RectMaskEffectUpdateVertexToVertex
{
    // public functions

    // ���캯��
    // @params: startTime - ����Ч����ʼʱ��
    // @params: totalTime - ����Ч����ʱ��
    // @params: halfHeight - ���ο��һ��
    // @params: halfWidth - ���θߵ�һ��
    // @params: mesh - ��������
    public RectMaskEffectUpdateRBToLT(
            float startTime,
            float totalTime,
            float halfHeight,
            float halfWidth,
            Mesh mesh )
    : base( startTime, totalTime, halfHeight, halfWidth, mesh )
    {
        Vector3[] vertices = mesh.vertices;
        vertices[3] = vertices[5] = vertices[4];
        vertices[1] = vertices[9] = vertices[2];
        vertices[7] = vertices[6];
        mesh.vertices = vertices;

        //
        infoFirst = new Info[3];
        infoFirst[0].dir = new Vector3( -2.0f*halfWidth, 0.0f, 0.0f );
        infoFirst[0].arrIndices = new int[]{ 5 };
        
        infoFirst[1].dir = new Vector3( 0.0f, 2.0f*halfHeight, 0.0f );
        infoFirst[1].arrIndices = new int[]{ 3 };
    
        infoFirst[2].dir = (infoFirst[0].dir + infoFirst[1].dir) * 0.5f;    // ����б�ߵ�һ��
        infoFirst[2].arrIndices = new int[]{ 4 };

        //
        infoAfter = new Info[3];
        infoAfter[0].dir = new Vector3( -2.0f*halfWidth, 0.0f, 0.0f );
        infoAfter[0].arrIndices = new int[]{ 1, 2, 3, 9 };

        infoAfter[1].dir = new Vector3( 0.0f, 2.0f*halfHeight, 0.0f );
        infoAfter[1].arrIndices = new int[]{ 5, 6, 7 };

        infoAfter[2].dir = (infoFirst[0].dir + infoFirst[1].dir) * 0.5f;    // ����б�ߵ�һ��
        infoAfter[2].arrIndices = new int[]{ 0, 4 };
    }
    /////////////////////////////////////////////////////////////////////
}