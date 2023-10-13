using UnityEngine;
using System.Collections;


/// <summary>
/// UIWindowsDrag.
/// </summary>/
public class UIWindowsDrag : MonoBehaviour {

    public Transform target;//拖拽的窗体的根节点//
    public Vector3 scale = Vector3.one;//希望拖拽时窗体移动随着拖拽的方向移动的幅度的一个决定因素//
    //public float padding = 0.1f;

    Plane mPlane;//参考平面//
    bool mPressed = false;//是否按下//
    Vector3 mLastPos = Vector3.zero;//上一次NGUI摄像机沿着屏幕上的鼠标点方向发射射线与参考平面的交点//
	
    void OnPress(bool pressed)
    {
        mPressed = pressed;
        if(pressed)//如果鼠标被按下//
        {
             mLastPos = UICamera.lastHit.point;
             Transform trans = UICamera.currentCamera.transform;
             mPlane = new Plane(trans.rotation * Vector3.back,mLastPos);//注意，此平面的方向与NGUI正交摄像机裁剪面平行//
        }else{
			
		}
     }

     void OnDrag(Vector2 delta)
     {
        Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);//发射射线//
        float dist = 0f;
        if(mPlane.Raycast(ray,out dist))//与参考平面相交//
        {
            Vector3 currentPos = ray.GetPoint(dist);//取得相交点//
            Vector3 offset = currentPos - mLastPos;//计算鼠标单次拖拽（OnDrag是由UICamera脚本中的Update函数的某个子程序在恰当的时候利用SendMessage发送过来的）移动的步长分量//
            mLastPos = currentPos;//更新mLastPos为当前currentPos，一边下一次的计算//
            if (offset.x != 0f || offset.y != 0f)
            {
                offset = target.InverseTransformDirection(offset);//将计算出的鼠标移动步长转为根节点的坐标系下的分量//
                offset.Scale(scale);//我们可以在检视面板中修改此scale的值，可以看到不同的效果//
                offset = target.TransformDirection(offset);//再转回世界坐标系//
            }
            Vector3 pos = target.position;
            pos += offset;//先将target的预期位置给计算出来。下面才是最重要的限制窗体的部分：//
            Camera curcam = UICamera.currentCamera;
            Bounds bs = NGUIMath.CalculateAbsoluteWidgetBounds(target);//根据根节点计算窗体在世界坐标系中的大小//
            Vector3 _lb =new Vector3(target.position.x - bs.size.x/2,target.position.y-bs.size.y/2,0f);//求出窗体的左下角的世界坐标//
            Vector3 lb = curcam.WorldToScreenPoint(_lb);//转屏幕坐标//
            Vector3 _rt = new Vector3(target.position.x+bs.size.x/2,target.position.y+bs.size.y/2,0f);//求出窗体的右上角世界坐标//
            Vector3 rt = curcam.WorldToScreenPoint(_rt);//转屏幕坐标//

            float width = rt.x - lb.x;//求出窗体的屏幕坐标系宽度//
            float height = rt.y - lb.y;//求出窗体的屏幕坐标系高度//
            Vector3 ClampVector1;
            Vector3 ClampVector2;
			if(Screen.width>width&&Screen.height>height){
				ClampVector1 = new Vector3(width / 2, height / 2, 0f);//窗体的左下角的最远点（相对于屏幕）//
				ClampVector2 = new Vector3(Screen.width - width / 2, Screen.height - height / 2, 0f);//窗体的右上角的最远点（相对于屏幕）//
			}
			else{
				ClampVector1 = new Vector3(width / 2-(width-Screen.width), height / 2-(height-Screen.height), 0f);
				ClampVector2 = new Vector3(Screen.width- width / 2+(width-Screen.width), Screen.height - height / 2+(height-Screen.height), 0f);//窗体的右上角的最远点（相对于屏幕）//
			}
			
            Vector3 scrPos = curcam.WorldToScreenPoint(pos);//将pos转为屏幕坐标//
            //将pos的屏幕坐标限制在屏幕的两个最远点范围之内//
            scrPos.x = Mathf.Clamp(scrPos.x, ClampVector1.x, ClampVector2.x);
            scrPos.y = Mathf.Clamp(scrPos.y, ClampVector1.y, ClampVector2.y);
            target.position = curcam.ScreenToWorldPoint(scrPos);
         }
     }
 }