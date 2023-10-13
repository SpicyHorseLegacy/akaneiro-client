/***********************************************************************
        filename:   RectMaskEffectUpdateRToL.cs
        created:    2012.04.28
        author:     Twj

        purpose:    ���δ��ҵ�������Ч���ĸ���
*************************************************************************/

using UnityEngine;
using System.Collections;

public class RectMaskEffectUpdateRToL : RectMaskEffectUpdateSideToSide
{
    // public functions

    // ���캯��
    // @params: startTime - ����Ч����ʼʱ��
    // @params: totalTime - ����Ч����ʱ��
    // @params: halfHeight - ���ο��һ��
    // @params: halfWidth - ���θߵ�һ��
    public RectMaskEffectUpdateRToL(
            float startTime,
            float totalTime,
            float halfHeight,
            float halfWidth )
    : base( startTime, totalTime, halfHeight, halfWidth )
    {
        arrIndicesFirst = new int[]
                            {
                                2, 3, 4,
                            };

        arrIndicesAfter = new int[]
                            {
                                0, 1, 2, 3, 4, 5, 9,
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
            vertices[index].x = (halfTotalTime - deltaTime) / halfTotalTime * halfWidth;
        }

        mesh.vertices = vertices;
    }
    /////////////////////////////////////////////////////////////////////
}