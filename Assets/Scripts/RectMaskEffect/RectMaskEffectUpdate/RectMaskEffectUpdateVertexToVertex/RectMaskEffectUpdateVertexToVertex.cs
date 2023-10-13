/***********************************************************************
        filename:   RectMaskEffectUpdateVertexToVertex.cs
        created:    2012.05.02
        author:     Twj

        purpose:    ���δ�һ�����㵽һ����������Ч���ĸ��»���
*************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RectMaskEffectUpdateVertexToVertex : RectMaskEffectUpdate
{
    // protected variables

    protected struct Info   // ������Ϣ
    {
        public Vector3      dir;                // ������·���
        public int[]        arrIndices;         // �����޸ĵĶ��������б�
    }
    protected Info[]        infoFirst;          // ����ǰ���ʱ���ڸ�����Ϣ
    protected Info[]        infoAfter;          // ���ֺ���ʱ���ڸ�����Ϣ

    protected Mesh          mesh;               // ��������
    protected float         lastTime;           // �ϴ�����Ч������ʱ��
    protected float         halfTotalTime;      // ����Ч����ʱ���һ��
    protected Vector3[]     originalVertices;   // ԭʼ��������
    /////////////////////////////////////////////////////////////////////

    // public functions

    // ���캯��
    // @params: startTime - ����Ч����ʼʱ��
    // @params: totalTime - ����Ч����ʱ��
    // @params: halfHeight - ���ο��һ��
    // @params: halfWidth - ���θߵ�һ��
    // @params: mesh - ��������
    public RectMaskEffectUpdateVertexToVertex(
            float startTime,
            float totalTime,
            float halfHeight,
            float halfWidth,
            Mesh mesh )
    : base( startTime, totalTime, halfHeight, halfWidth )
    {
        this.mesh = mesh;
        lastTime = startTime;
        halfTotalTime = totalTime * 0.5f;
    }

    // ��������Ч��
    // @params: mesh - ��������
    public override void UpdateEffect( Mesh mesh )
    {
        base.UpdateEffect( mesh );
        
        //
        Info[] arrInfos = null;
        if ( deltaTime >= halfTotalTime )
        {
            if ( infoFirst == arrInfos )
            {
                // ǰһ֡����ǰ��θ��£���ǰ��֡�����˺��θ���

                // �������ǰ�Ķ�����������
                RefreshVertices( halfTotalTime, lastTime, infoFirst );

                lastTime = halfTotalTime;
            }

            arrInfos = infoAfter;
        }
        else
        {
            arrInfos = infoFirst;
        }

        if ( null != arrInfos )
        {
            RefreshVertices( Time.time, lastTime, arrInfos );

            lastTime = Time.time;
        }
    }
    /////////////////////////////////////////////////////////////////////

    // private functions

    // ˢ�¶�������
    // @params: curTime - ��ǰʱ��
    // @params: lastTime - ǰһ��ʱ��
    // @params: arrInfos - ������Ϣ
    private void RefreshVertices( float curTime, float lastTime, Info[] arrInfos )
    {
        Vector3[] vertices = mesh.vertices;
        float percent = (curTime - lastTime) / halfTotalTime;
        foreach ( Info info in arrInfos )
        {
            foreach ( int index in info.arrIndices )
            {
                vertices[index] += (percent * info.dir);
            }
        }

        mesh.vertices = vertices;
    }
    /////////////////////////////////////////////////////////////////////
}