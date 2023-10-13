/***********************************************************************
        filename:   RectMaskEffectUpdateSideToSide.cs
        created:    2012.04.28
        author:     Twj

        purpose:    ���δ�һ�ߵ�һ������Ч���ĸ��»���
*************************************************************************/

using UnityEngine;
using System.Collections;

public class RectMaskEffectUpdateSideToSide : RectMaskEffectUpdate
{
    // protected variables

    protected float     halfTotalTime;      // ����Ч����ʱ���һ��
    protected int[]     arrIndices;         // �����޸ĵĶ��������б�
    protected int[]     arrIndicesFirst;    // ����ǰ���ʱ���������޸ĵĶ��������б�
    protected int[]     arrIndicesAfter;    // ���ֺ���ʱ���������޸ĵĶ��������б�
    /////////////////////////////////////////////////////////////////////
    
    // public functions

    // ���캯��
    // @params: startTime - ����Ч����ʼʱ��
    // @params: totalTime - ����Ч����ʱ��
    // @params: halfHeight - ���ο��һ��
    // @params: halfWidth - ���θߵ�һ��
    public RectMaskEffectUpdateSideToSide(
            float startTime,
            float totalTime,
            float halfHeight,
            float halfWidth )
    : base( startTime, totalTime, halfHeight, halfWidth )
    {
        halfTotalTime = totalTime * 0.5f;
    }

    // ��������Ч��
    // @params: mesh - ��������
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