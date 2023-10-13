/***********************************************************************
        filename:   MaskEffect.cs
        created:    2012.04.28
        author:     Twj

        purpose:    遮罩效果基类
*************************************************************************/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public abstract class MaskEffect : MonoBehaviour
{
    // public variables

    // true网格数据将在开始时更新，false网格数据每帧都会更新
    public bool     isStatic = true;

    public Color    color = new Color( 0.0f, 0.0f, 0.0f, 0.5f );    // 遮罩顶点颜色
    /////////////////////////////////////////////////////////////////////

    // protected variables

    protected bool          playing;    // 是否正在播放遮罩效果
    protected float         startTime;  // 遮罩效果开始时间
    protected float         totalTime;  // 遮罩效果总时间
    protected MeshFilter    meshFilter;

    protected delegate void PreStartCallback();
    protected PreStartCallback  preStartCallback;   // 遮罩开始前的回调函数

    protected delegate void EndCallback();
    protected EndCallback       endCallback;        // 遮罩结束的回调函数
    /////////////////////////////////////////////////////////////////////

    // public functions

    // 开始遮罩效果
    // @params: time - 遮罩效果总时间
    public virtual void StartEffect( float time )
    {
        playing     = true;
        startTime   = Time.time;
        totalTime   = time;

        //
        if ( null != preStartCallback )
        {
            preStartCallback();
        }

        //
        renderer.enabled = true;
    }
    /////////////////////////////////////////////////////////////////////

    // protected functions

    protected virtual void Awake()
    {
        InitMesh();

        renderer.castShadows = false;
        renderer.receiveShadows = false;
    }

    protected virtual void Update()
    {
        if ( !isStatic )
        {
            InitMeshColors( meshFilter.mesh );
        }

        //
        if ( playing )
        {
            float deltaTime = Time.time - startTime;    // 效果已播放的时间
            if ( deltaTime > totalTime )
            {
                playing = false;

                //
                if ( null != endCallback )
                {
                    endCallback();
                }

                //
                renderer.enabled = false;
            }
            else
            {
                UpdateEffect();
            }
        }
    }

    // 初始化Mesh
    // @return: true初始化Mesh成功，false初始化Mesh失败
    public virtual bool InitMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
		
		if(null == meshFilter.mesh){
       	 	meshFilter.mesh = new Mesh();
		}
        InitMeshVertices( meshFilter.mesh );
        InitMeshIndices( meshFilter.mesh );
        InitMeshColors( meshFilter.mesh );
		
	    renderer.enabled = false;
        return true;
    }

    // 初始化Mesh顶点
    // @return: true初始化Mesh顶点成功，false初始化Mesh顶点失败
    protected virtual bool InitMeshVertices( Mesh mesh )
    {
        return true;
    }

    // 初始化Mesh索引
    // @return: true初始化Mesh索引成功，false初始化Mesh索引失败
    protected virtual bool InitMeshIndices( Mesh mesh )
    {
        return true;
    }

    // 初始化Mesh颜色
    // @return: true初始化Mesh颜色成功，false初始化Mesh颜色失败
    protected virtual bool InitMeshColors( Mesh mesh )
    {
        Color[] colors = new Color[mesh.vertices.Length];
        for ( int i = 0; i < colors.Length; ++i )
        {
            colors[i] = color;
        }

        mesh.colors = colors;

        return true;
    }

    // 更新遮罩效果
    protected virtual void UpdateEffect()
    {}
    /////////////////////////////////////////////////////////////////////
}