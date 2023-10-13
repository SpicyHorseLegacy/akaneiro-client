using Facebook;
using UnityEngine;
using UnityEditor;
using UnityEditor.FacebookEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(FBSettings))]
public class FacebookSettingsEditor : Editor
{
    bool showFacebookInitSettings = false;
    bool showAndroidUtils = (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android);

    GUIContent appNameLabel = new GUIContent("App Name [?]:", "For your own use and organization.\n(ex. 'dev', 'qa', 'prod')");
    GUIContent appIdLabel = new GUIContent("App Id [?]:", "Facebook App Ids can be found at https://developers.facebook.com/apps");
    
    GUIContent cookieLabel = new GUIContent("Cookie [?]", "Sets a cookie which your server-side code can use to validate a user's Facebook session");
    GUIContent loggingLabel = new GUIContent("Logging [?]", "(Web Player only) If true, outputs a verbose log to the Javascript console to facilitate debugging.");
    GUIContent statusLabel = new GUIContent("Status [?]", "If 'true', attempts to initialize the Facebook object with valid session data.");
    GUIContent xfbmlLabel = new GUIContent("Xfbml [?]", "(Web Player only If true) Facebook will immediately parse any XFBML elements on the Facebook Canvas page hosting the app");
    GUIContent frictionlessLabel = new GUIContent("Frictionless Requests [?]", "Use frictionless app requests, as described in their own documentation.");

    GUIContent packageNameLabel = new GUIContent("Package Name [?]", "aka: the bundle identifier");
    GUIContent classNameLabel = new GUIContent("Class Name [?]", "aka: the activity name");
    GUIContent debugAndroidKeyLabel = new GUIContent("Debug Android Key Hash [?]", "Copy this key to the Facebook Settings in order to test a Facebook Android app");

    GUIContent sdkVersion = new GUIContent("SDK Version [?]", "This Unity Facebook SDK version.  If you have problems or compliments please include this so we know exactly what version to look out for.");
    GUIContent buildVersion = new GUIContent("Build Version [?]", "This Unity Facebook SDK version.  If you have problems or compliments please include this so we know exactly what version to look out for.");

    private FBSettings instance;

    public override void OnInspectorGUI()
    {
        instance = (FBSettings)target;

        AppIdGUI();
        FBParamsInitGUI();
        AndroidUtilGUI();
        AboutGUI();
    }

    private void AppIdGUI()
    {
        EditorGUILayout.HelpBox("1) Add the Facebook App Id(s) associated with this game", MessageType.None);
        if (instance.AppIds.Length == 0 || instance.AppIds[instance.SelectedAppIndex] == "0")
        {
            EditorGUILayout.HelpBox("Invalid App Id", MessageType.Error);
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(appNameLabel);
        EditorGUILayout.LabelField(appIdLabel);
        EditorGUILayout.EndHorizontal();
        for (int i = 0; i < instance.AppIds.Length; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            instance.SetAppLabel(i, EditorGUILayout.TextField(instance.AppLabels[i]));
            GUI.changed = false;
            instance.SetAppId(i, EditorGUILayout.TextField(instance.AppIds[i]));
            if (GUI.changed)
                ManifestMod.GenerateManifest();
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Another App Id"))
        {
            var appLabels = new List<string>(instance.AppLabels);
            appLabels.Add("New App");
            instance.AppLabels = appLabels.ToArray();

            var appIds = new List<string>(instance.AppIds);
            appIds.Add("0");
            instance.AppIds = appIds.ToArray();
        }
        if (instance.AppLabels.Length > 1)
        {
            if (GUILayout.Button("Remove Last App Id"))
            {
                var appLabels = new List<string>(instance.AppLabels);
                appLabels.RemoveAt(appLabels.Count - 1);
                instance.AppLabels = appLabels.ToArray();

                var appIds = new List<string>(instance.AppIds);
                appIds.RemoveAt(appIds.Count - 1);
                instance.AppIds = appIds.ToArray();
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        if (instance.AppIds.Length > 1)
        {
            EditorGUILayout.HelpBox("2) Select Facebook App Id to be compiled with this game", MessageType.None);
            GUI.changed = false;
            instance.SetAppIndex(EditorGUILayout.Popup("Selected App Id", instance.SelectedAppIndex, instance.AppLabels));
            if (GUI.changed)
                ManifestMod.GenerateManifest();
            EditorGUILayout.Space();
        }
        else
        {
            instance.SetAppIndex(0);
        }
    }

    private void FBParamsInitGUI()
    {
        EditorGUILayout.HelpBox("(Optional) Edit the FB.Init() parameters", MessageType.None);
        showFacebookInitSettings = EditorGUILayout.Foldout(showFacebookInitSettings, "FB.Init() Parameters");
        if (showFacebookInitSettings)
        {
            FBSettings.Cookie = EditorGUILayout.Toggle(cookieLabel, FBSettings.Cookie);
            FBSettings.Logging = EditorGUILayout.Toggle(loggingLabel, FBSettings.Logging);
            FBSettings.Status = EditorGUILayout.Toggle(statusLabel, FBSettings.Status);
            FBSettings.Xfbml = EditorGUILayout.Toggle(xfbmlLabel, FBSettings.Xfbml);
            FBSettings.FrictionlessRequests = EditorGUILayout.Toggle(frictionlessLabel, FBSettings.FrictionlessRequests);
        }
        EditorGUILayout.Space();
    }

    private void AndroidUtilGUI()
    {
        showAndroidUtils = EditorGUILayout.Foldout(showAndroidUtils, "Android Build Facebook Settings");
        if (showAndroidUtils)
        {
            if (!FacebookAndroidUtil.HasAndroidSDK())
            {
                var msg = "You don't have the Android SDK setup!  Go to "+(Application.platform == RuntimePlatform.OSXEditor?"Unity":"Edit")+"->Preferences... and set your Android SDK Location under External Tools";
                EditorGUILayout.HelpBox(msg, MessageType.Warning);
            }
            EditorGUILayout.HelpBox("Copy and Paste these into your \"Native Android App\" Settings on developers.facebook.com/apps", MessageType.None);
            SelectableLabelField(packageNameLabel, PlayerSettings.bundleIdentifier);
            SelectableLabelField(classNameLabel, ManifestMod.ActivityName);
            SelectableLabelField(debugAndroidKeyLabel, FacebookAndroidUtil.DebugKeyHash);

        }
        EditorGUILayout.Space();
    }

    private void AboutGUI()
    {
        var versionAttribute = FBBuildVersionAttribute.GetVersionAttributeOfType(typeof(IFacebook));
        EditorGUILayout.HelpBox("About the Facebook SDK", MessageType.None);
        SelectableLabelField(sdkVersion, versionAttribute.Version);
        SelectableLabelField(buildVersion, versionAttribute.ToString());
        EditorGUILayout.Space();
    }

    private void SelectableLabelField(GUIContent label, string value)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.Width(180), GUILayout.Height(16));
        EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
        EditorGUILayout.EndHorizontal();
    }
}
