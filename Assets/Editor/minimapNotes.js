#pragma strict

@CustomEditor(minimapUseNotes)
class minimapNotes extends Editor {
	override function OnInspectorGUI () {
		GUILayout.Label ("----------------------------------------------------------- \n *Here listed the notes of how to implement \n a map quickly to new level: \n -----------------------------------------------------------  \n 1-Put this prefab in a new level  \n 2-Dublicate the material used for the map \n and name it with the new level name \n 3-Assign the new level map textures to the material \n 4-Adjust the [Posion/Scale] of the map object \n called \"levelMap\" \n 5-Put the object called \"playerPoint\" on the same \n position of the player, because this object represent the player \n position on the map  \n \n *NOTE \n -Never apply the changes to the prefab :)");
	}
}

