/***********************************************************************
        filename:   RectMaskEffectUpdateTToB.cs
        created:    2012.04.28
        author:     Twj

        purpose:    ���δ��ϵ�������Ч���ĸ���
*************************************************************************/

using UnityEngine;
using System.Collections;

public class RectMaskEffectUpdateTToB : RectMaskEffectUpdateSideToSide
{
    // public functions

    // ���캯��
    // @params: startTime - ����Ч����ʼʱ��
    // @params: totalTime - ����Ч����ʱ��
    // @params: halfHeight - ���ο��һ��
    // @params: halfWidth - ���θߵ�һ��
    public RectMaskEffectUpdateTToB(
            float startTime,
            float totalTime,
            float halfHeight,
            float halfWidth )
    : base( startTime, totalTime, halfHeight, halfWidth )
    {
        arrIndicesFirst = new int[]
                            {
                                1, 2, 8, 9,
                            };
    
        arrIndicesAfter = new int[]
                            {
                                0, 1, 2, 3, 7, 8, 9,
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
            vertices[index].y = (halfTotalTime - deltaTime) / halfTotalTime * halfHeight;
        }

        mesh.vertices = vertices;
    }
    /////////////////////////////////////////////////////////////////////
}