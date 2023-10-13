class AssignShader extends ScriptableWizard 
{ 
    var theShader : Shader; 
    
    function OnWizardUpdate() 
    { 
        helpString = "Select Game Obects"; 
        isValid = (theShader != null); 
    } 
    
    function OnWizardCreate() 
    { 
        
       var gos = Selection.gameObjects; 
    
       for (var go in gos) 
       { 
          // go.renderer.material = theMaterial; 
           
           for(var mtl in go.renderer.sharedMaterials)
           {
              mtl.shader =  theShader; //Shader.Find ("Transparent/Diffuse");
           }
           
       } 
    } 
    
    @MenuItem("Custom/Assign Shader", false, 4) 
    static function assignShader() 
    { 
        ScriptableWizard.DisplayWizard( 
            "Assign Material", AssignShader, "Assign"); 
    } 
}

