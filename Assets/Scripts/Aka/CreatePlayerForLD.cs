using UnityEngine;
using System.Collections;

public class CreatePlayerForLD : BaseExportNode {

	public Transform PlayerPrefab;
	public Transform CameraPrefab;
	
	void Awake()
	{
		Transform _player = null;
		if(!Player.Instance && PlayerPrefab)
		{
			_player = Instantiate(PlayerPrefab) as Transform;
		}
		if(!GameCamera.Instance && CameraPrefab)
		{
			Transform _cameraObj = Instantiate(CameraPrefab) as Transform;
			GameCamera _camera = _cameraObj.GetComponent<GameCamera>();
			_camera.target = _player;
			GameCamera.EnterNomalState();
		}
		if(_player)
		{
			Vector3 pos = transform.position;
			RaycastHit hitInfo;
	        int layer = 1 << LayerMask.NameToLayer("Walkable");
	        if (Physics.Raycast(pos + Vector3.up * 20, Vector3.down, out hitInfo, 100f, layer))
	            pos.y = hitInfo.point.y + 0.1f;
			_player.position = pos;
			_player.GetComponent<PlayerMoveForLD>().transform.position = pos;
		}
	}
	
	public override string DoExport()
    {
	     XMLStringWriter xmlWriter = new XMLStringWriter();
		
		 xmlWriter.NodeBegin("Player");
		
		 xmlWriter.AddAttribute("SpawnPosX",transform.position.x);
		
		 xmlWriter.AddAttribute("SpawnPosY",transform.position.y);
		
		 xmlWriter.AddAttribute("SpawnPosZ",transform.position.z);
        
		 xmlWriter.NodeEnd("Player");
		
		 return xmlWriter.Result;
    }
}
