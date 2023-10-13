using UnityEngine;
using UnityEditor;
using System.Collections;


public class ChangeAudioImportSettings : EditorWindow {

    [HideInInspector]
    public static Rect guiRect = new Rect(0, 0, 400, 500);

    [MenuItem("Custom/Change AudioImport Settings")]

    public static void InspectSelectionMenuSelected()
    {
		EditorWindow.GetWindowWithRect(typeof(ChangeAudioImportSettings), guiRect, true, "Change AudioImport Settings");
    }

	const int minCompressionRate = 45000;
	const int maxCompressionRate = 500000;
    private AudioImporterFormat format = AudioImporterFormat.Native;
    private bool bThreeD = true;
	private bool bforceToMono = false;
	private AudioImporterLoadType loadtype = AudioImporterLoadType.CompressedInMemory;
	private int compressionBitRate = minCompressionRate;

	private string[] formatNames = new string[] {
        AudioImporterFormat.Native.ToString(),
        AudioImporterFormat.Compressed.ToString(),
    };

	private string[] loadtypeNames = new string[]{
		AudioImporterLoadType.CompressedInMemory.ToString(),
		AudioImporterLoadType.DecompressOnLoad.ToString(),
		AudioImporterLoadType.StreamFromDisc.ToString(),
	};

	private AudioImporterFormat[] formats = new AudioImporterFormat[] {
        AudioImporterFormat.Native,
        AudioImporterFormat.Compressed
    };

	private AudioImporterLoadType[] loadTypes = new AudioImporterLoadType[]
	{
		AudioImporterLoadType.CompressedInMemory,
		AudioImporterLoadType.DecompressOnLoad,
		AudioImporterLoadType.StreamFromDisc
	};

    public ChangeAudioImportSettings() { }

    public void OnSelectionChange() {
        this.Repaint();
    }

    public void OnGUI() {
        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);

        GUILayout.BeginArea(guiRect);
        GUILayout.BeginVertical();

        DrawCurrentValues();

        GUILayout.Space(10F);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Close")) {
            this.Close();
        }
        if (GUILayout.Button("Get Values!")) {
            HandleButtonClickGetValues();
        }
        if (GUILayout.Button("Apply!")) {
            HandleButtonClickApply();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(15F);

        DrawCurrentSelection();

        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();
    }

    private void HandleButtonClickGetValues() {

        if (Selection.activeObject != null && Selection.activeObject is AudioClip) {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            AudioImporter audioImporter = (AudioImporter)AssetImporter.GetAtPath(path);
            this.format = audioImporter.format;
            this.bThreeD = audioImporter.threeD;
			this.bforceToMono = audioImporter.forceToMono;
			this.loadtype = audioImporter.loadType;
            this.compressionBitRate = audioImporter.compressionBitrate;
			this.Repaint();
        }
    }

    private void HandleButtonClickApply() {
        if (Selection.objects != null) {
            foreach (Object obj in Selection.objects) {
                if (obj is AudioClip) {
                    string path = AssetDatabase.GetAssetPath(obj);
                    AudioImporter audioImporter 
                        = (AudioImporter)AssetImporter.GetAtPath(path);
                    audioImporter.format = this.format;
                    audioImporter.threeD = this.bThreeD;
					audioImporter.forceToMono = this.bforceToMono;
					audioImporter.loadType = this.loadtype;
                    audioImporter.compressionBitrate = this.compressionBitRate;
					AssetDatabase.ImportAsset(path); 
                }
            }
        }
    }

    private void DrawCurrentValues() {

		//Format value
        int formatIndex = 0;
        for (int i = 0; i < formats.Length; i++) {
            if (formats[i] == this.format) {
                formatIndex = i;
            }
        }
        formatIndex = EditorGUILayout.Popup("Format:", formatIndex, formatNames);
        this.format = formats[formatIndex];

		//3D value
        this.bThreeD = EditorGUILayout.Toggle("3D Sound", this.bThreeD);

		//force to mono value
		this.bforceToMono = EditorGUILayout.Toggle("Force to mono", this.bforceToMono);

		//Load Type value
		int loadTypeIndex = 0;
		for (int i = 0; i < this.loadTypes.Length; i++)
		{
			if (this.loadTypes[i] == this.loadtype)
			{
				loadTypeIndex = i;
			}
		}
		loadTypeIndex = EditorGUILayout.Popup("Load type:", loadTypeIndex, loadtypeNames);
		this.loadtype = this.loadTypes[loadTypeIndex];

		//Compression Bit Rate value
		this.compressionBitRate = (int)(1000f * System.Math.Round(EditorGUILayout.Slider("CompressionBitrate:", (this.compressionBitRate * 0.001f), minCompressionRate * 0.001f, maxCompressionRate * 0.001f)));
		Debug.Log("this.compressionBitRate = " + this.compressionBitRate);
    }

    private void DrawCurrentSelection() {
        if (Selection.objects != null) {
            GUILayout.Label("Current selection:");
            foreach (Object obj in Selection.objects) {
                GUILayout.Label(string.Format("{0}: {1}", obj.GetType().Name, obj.name));
            }
        }
    }


}
