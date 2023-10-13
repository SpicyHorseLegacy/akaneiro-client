
var offset : Vector2;
var tiling : Vector2;
var rot = 45 ;
 
function Update ()
{
	rot= Time.time * 120;
    var matrix = Matrix4x4.TRS (offset, Quaternion.Euler (0, 0, rot), tiling);
    renderer.material.SetMatrix ("_Matrix", matrix);
	
}