/***********************************************************************
        filename:   RectMaskEffectUpdate.cs
        created:    2012.04.28
        author:     Twj

        purpose:    ����Բ������Ч���ĸ���
*************************************************************************/

using UnityEngine;
using System.Collections;

public class RectMaskEffectUpdateCircle : RectMaskEffectUpdate
{
    // private variables

    private float timePerTriangle;    // ÿ�����������ֵ�ʱ��(= ������ʱ�� / 8(���ηָ�Ϊ8��������))
    /////////////////////////////////////////////////////////////////////

    // public functions

    // ���캯��
    // @params: startTime - ����Ч����ʼʱ��
    // @params: totalTime - ����Ч����ʱ��
    // @params: halfHeight - ���ο��һ��
    // @params: halfWidth - ���θߵ�һ��
    public RectMaskEffectUpdateCircle(
            float startTime,
            float totalTime,
            float halfHeight,
            float halfWidth )
    : base( startTime, totalTime, halfHeight, halfWidth )
    {
        timePerTriangle = totalTime * 0.125f;   // 0.125��ʾ1/8
    }
    
    // ��������Ч��
    // @params: mesh - ��������
    public override void UpdateEffect( Mesh mesh )
    {
        float deltaTime = Time.time - startTime;    // Ч���Ѳ��ŵ�ʱ��
        
        // ���¶�������
        Vector3[] vertices = mesh.vertices;
        float percent = (deltaTime % timePerTriangle) / timePerTriangle;  // ÿ�����������ֵİٷֱ�
        int index = (int)(deltaTime / timePerTriangle) + 1; // ��ǰ�޸ĵĶ�������
        for ( int i = 1; i <= index; ++i )
        {
            switch ( index )
            {
            case 1:
                vertices[i].x = halfWidth * percent;
                break;

            case 2:
                vertices[i].y = halfHeight - halfHeight*percent;
                break;

            case 3:
                vertices[i].y = -halfHeight * percent;
                break;

            case 4:
                vertices[i].x = halfWidth * (1 - percent);
                break;

            case 5:
                vertices[i].x = -halfWidth * percent;
                break;

            case 6:
                vertices[i].y = -halfHeight * (1 - percent);
                break;

            case 7:
                vertices[i].y = halfHeight * percent;
                break;

            case 8:
                vertices[i].x = -halfWidth + halfWidth*percent;
                break;
            }
        }

        mesh.vertices = vertices;
    }
    /////////////////////////////////////////////////////////////////////
}