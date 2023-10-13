using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.FacebookEditor;
using UnityEditor.XCodeEditor;

namespace UnityEditor.FacebookEditor
{
    public static class XCodePostProcess
    {
        [PostProcessBuild(100)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            // If integrating with facebook on any platform, throw a warning if the app id is invalid
            if (!FBSettings.IsValidAppId)
            {
                Debug.LogWarning("You didn't specify a Facebook app ID.  Please add one using the Facebook menu in the main Unity editor.");
            }

            bool needsNewClassnames = IsVersion42OrLater();

            if (target == BuildTarget.iPhone)
            {
                UnityEditor.XCodeEditor.XCProject project = new UnityEditor.XCodeEditor.XCProject(path);

                // Find and run through all projmods files to patch the project

                string projModPath = System.IO.Path.Combine(Application.dataPath, "Facebook/Editor/iOS");
                var files = System.IO.Directory.GetFiles(projModPath, "*.projmods", System.IO.SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    project.ApplyMod(Application.dataPath, file);
                }
                project.Save();

                PlistMod.UpdatePlist(path, FBSettings.AppId);
                FixupFiles.FixSimulator(path);

                if (needsNewClassnames)
                    FixupFiles.AddVersionDefine(path);
            }

            if (target == BuildTarget.Android)
            {
                // The default Bundle Identifier for Unity does magical things that causes bad stuff to happen
                if (PlayerSettings.bundleIdentifier == "com.Company.ProductName")
                {
                    Debug.LogError("The default Unity Bundle Identifier (com.Company.ProductName) will not work correctly.");
                }
            }
        }

        private static bool IsVersion42OrLater()
        {
            string version = Application.unityVersion;
            string[] versionComponents = version.Split('.');

            int majorVersion = 0;
            int minorVersion = 0;

            try
            {
                if (versionComponents != null && versionComponents.Length > 0 && versionComponents[0] != null)
                    majorVersion = Convert.ToInt32(versionComponents[0]);
                if (versionComponents != null && versionComponents.Length > 1 && versionComponents[1] != null)
                    minorVersion = Convert.ToInt32(versionComponents[1]);
            }
            catch (System.Exception e)
            {
                FbDebug.Error("Error parsing Unity version number: " + e);
            }

            return (majorVersion > 4 || (majorVersion == 4 && minorVersion >= 2));
        }
    }
}
