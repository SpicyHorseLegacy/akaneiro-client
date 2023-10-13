/***********************************************************************
        filename:   RectMaskEffectUpdate.cs
        created:    2012.04.28
        author:     Twj

        purpose:    ��������Ч���ĸ���
*************************************************************************/

using UnityEngine;
using System.Collections;

[System.Serializable]
public class RectMaskEffectUpdate
{
    // public variables
    
    // ��������Ч��
    // @params: mesh - ��������
    public virtual void UpdateEffect( Mesh mesh )
    {
        deltaTime = Time.time - startTime;  // Ч���Ѳ��ŵ�ʱ��
    }
    /////////////////////////////////////////////////////////////////////

    // protected variables

    protected float startTime;          // ����Ч����ʼʱ��
    protected float totalTime;          // ����Ч����ʱ��
    protected float halfHeight;         // ���ο��һ��
    protected float halfWidth;          // ���θߵ�һ��
    protected float deltaTime;          // Ч���Ѳ��ŵ�ʱ��
    /////////////////////////////////////////////////////////////////////

    // public functions

    // ���캯��
    // @params: startTime - ����Ч����ʼʱ��
    // @params: totalTime - ����Ч����ʱ��
    // @params: halfHeight - ���ο��һ��
    // @params: halfWidth - ���θߵ�һ��
    public RectMaskEffectUpdate(
            float startTime,
            float totalTime,
            float halfHeight,
            float halfWidth )
    {
        this.startTime = startTime;
        this.totalTime = totalTime;
        this.halfHeight = halfHeight;
        this.halfWidth = halfWidth;
    }
    /////////////////////////////////////////////////////////////////////
}