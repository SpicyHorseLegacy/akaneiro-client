class AssignMaterial extends ScriptableWizard 
{ 
    var theMaterial : Material; 
    
    function OnWizardUpdate() 
    { 
        helpString = "Select Game Obects"; 
        isValid = (theMaterial != null); 
    } 
    
    function OnWizardCreate() 
    { 
        
       var gos = Selection.gameObjects; 
    
       for (var go in gos) 
       { 
			//for()
           go.renderer.material = theMaterial; 
       } 
    } 
    
    @MenuItem("Custom/Assign Material", false, 4) 
    static function assignMaterial() 
    { 
        ScriptableWizard.DisplayWizard( 
            "Assign Material", AssignMaterial, "Assign"); 
    } 
}