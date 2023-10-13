/***********************************************************************
        filename:   RectMaskEffectUpdateBToT.cs
        created:    2012.04.28
        author:     Twj

        purpose:    ���δ��µ�������Ч���ĸ���
*************************************************************************/

using UnityEngine;
using System.Collections;

public class RectMaskEffectUpdateBToT : RectMaskEffectUpdateSideToSide
{
    // public functions

    // ���캯��
    // @params: startTime - ����Ч����ʼʱ��
    // @params: totalTime - ����Ч����ʱ��
    // @params: halfHeight - ���ο��һ��
    // @params: halfWidth - ���θߵ�һ��
    public RectMaskEffectUpdateBToT(
            float startTime,
            float totalTime,
            float halfHeight,
            float halfWidth )
    : base( startTime, totalTime, halfHeight, halfWidth )
    {
        arrIndicesFirst = new int[]
                            {
                                4, 5, 6,
                            };
    
        arrIndicesAfter = new int[]
                            {
                                0, 3, 4, 5, 6, 7,
                            };
    }
    
    // ��������Ч��
    // @params: mesh - ��������
    public override void UpdateEffect( Mesh mesh )
    {
        base.UpdateEffect( mesh );

        // ���¶�������
        Vector3[] vertices = mesh.vertices;
        foreach ( int index in arrIndices )
        {
            vertices[index].y = (deltaTime - halfTotalTime) / halfTotalTime * halfHeight;
        }

        mesh.vertices = vertices;
    }
    /////////////////////////////////////////////////////////////////////
}