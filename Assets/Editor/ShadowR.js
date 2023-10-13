@MenuItem("Scripts/Shadow Recv off") 
static function ShadowRecv() { 
    Undo.RegisterSceneUndo("Shadow Recv off"); 
	var rds : Component[];
   for(var gb:GameObject in Selection.gameObjects){ 
	   if(gb.renderer)
	      gb.renderer.receiveShadows = false;
      
      rds = gb.GetComponentsInChildren(Renderer);
      for(var rd:Renderer in rds)
      {
      	rd.receiveShadows = false;
      }	
   } 
} 

@MenuItem("Scripts/Shadow Cast off") 
static function ShadowCast() { 
    Undo.RegisterSceneUndo("Shadow Cast off"); 
	var rds : Component[];
   for(var gb:GameObject in Selection.gameObjects){ 
   if(gb.renderer)
      gb.renderer.castShadows = false;
      
      rds = gb.GetComponentsInChildren(Renderer);
      for(var rd:Renderer in rds)
      {
      	rd.castShadows = false;
      }
   } 
} 

@MenuItem("Scripts/Hide Off") 
static function HideOff() { 
    Undo.RegisterSceneUndo("Hide Off"); 

   for(var gb:GameObject in Selection.gameObjects)
   { 
//	   if(gb.renderer)
//		  gb.renderer.enabled = !gb.renderer.enabled;
      var rds : Component[];
      rds = gb.GetComponentsInChildren(Renderer);
      for(var rd:Renderer in rds)
      {
      	rd.enabled = false;
      }		  
   } 
} 

@MenuItem("Scripts/Hide On") 
static function HideOn() { 
    Undo.RegisterSceneUndo("Hide On"); 

   for(var gb:GameObject in Selection.gameObjects)
   { 
//	   if(gb.renderer)
//		  gb.renderer.enabled = !gb.renderer.enabled;
      var rds : Component[];
      rds = gb.GetComponentsInChildren(Renderer);
      for(var rd:Renderer in rds)
      {
      	rd.enabled = true;
      }		  
   } 
} 

@MenuItem("Scripts/Ground") 
static function Ground() { 
    Undo.RegisterSceneUndo("Ground"); 

   for(var gb:GameObject in Selection.gameObjects)
   { 
		  gb.layer = LayerMask.NameToLayer("Ground");
   } 
} 

@MenuItem("Scripts/tagFence") 
static function tagFence() { 
    Undo.RegisterSceneUndo("tagFence"); 

   for(var gb:GameObject in Selection.gameObjects)
   { 
		  gb.tag = "Fence";
   } 
} 