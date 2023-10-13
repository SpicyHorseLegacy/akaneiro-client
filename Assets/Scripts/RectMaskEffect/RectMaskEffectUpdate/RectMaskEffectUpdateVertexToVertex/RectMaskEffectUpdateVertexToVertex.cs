/***********************************************************************
        filename:   RectMaskEffectUpdateVertexToVertex.cs
        created:    2012.05.02
        author:     Twj

        purpose:    矩形从一个顶点到一个顶点遮罩效果的更新基类
*************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RectMaskEffectUpdateVertexToVertex : RectMaskEffectUpdate
{
    // protected variables

    protected struct Info   // 更新信息
    {
        public Vector3      dir;                // 顶点更新方向
        public int[]        arrIndices;         // 所需修改的顶点索引列表
    }
    protected Info[]        infoFirst;          // 遮罩前半段时间内更新信息
    protected Info[]        infoAfter;          // 遮罩后半段时间内更新信息

    protected Mesh          mesh;               // 遮罩网格
    protected float         lastTime;           // 上次遮罩效果更新时间
    protected float         halfTotalTime;      // 遮罩效果总时间的一半
    protected Vector3[]     originalVertices;   // 原始顶点数据
    /////////////////////////////////////////////////////////////////////

    // public functions

    // 构造函数
    // @params: startTime - 遮罩效果开始时间
    // @params: totalTime - 遮罩效果总时间
    // @params: halfHeight - 矩形宽的一半
    // @params: halfWidth - 矩形高的一半
    // @params: mesh - 遮罩网格
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

    // 更新遮罩效果
    // @params: mesh - 遮罩网格
    public override void UpdateEffect( Mesh mesh )
    {
        base.UpdateEffect( mesh );
        
        //
        Info[] arrInfos = null;
        if ( deltaTime >= halfTotalTime )
        {
            if ( infoFirst == arrInfos )
            {
                // 前一帧还是前半段更新，当前这帧进入了后半段更新

                // 进入后半段前的顶点数据修正
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

    // 刷新顶点数据
    // @params: curTime - 当前时间
    // @params: lastTime - 前一次时间
    // @params: arrInfos - 更新信息
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