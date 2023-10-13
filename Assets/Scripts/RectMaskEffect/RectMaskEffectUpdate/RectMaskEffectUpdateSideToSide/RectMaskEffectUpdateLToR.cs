/***********************************************************************
        filename:   RectMaskEffectUpdate.cs
        created:    2012.04.28
        author:     Twj

        purpose:    ���δ���������Ч���ĸ���
*************************************************************************/

using UnityEngine;
using System.Collections;

public class RectMaskEffectUpdateLToR : RectMaskEffectUpdateSideToSide
{
    // public functions

    // ���캯��
    // @params: startTime - ����Ч����ʼʱ��
    // @params: totalTime - ����Ч����ʱ��
    // @params: halfHeight - ���ο��һ��
    // @params: halfWidth - ���θߵ�һ��
    public RectMaskEffectUpdateLToR(
            float startTime,
            float totalTime,
            float halfHeight,
            float halfWidth )
    : base( startTime, totalTime, halfHeight, halfWidth )
    {
        arrIndicesFirst = new int[]
                            {
                                6, 7, 8,
                            };

        arrIndicesAfter = new int[]
                            {
                                0, 1, 5, 6, 7, 8, 9,
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
            vertices[index].x = (deltaTime - halfTotalTime) / halfTotalTime * halfWidth;
        }

        mesh.vertices = vertices;
    }
    /////////////////////////////////////////////////////////////////////
}