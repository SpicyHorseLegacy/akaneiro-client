using Facebook;
using UnityEngine;
using System.Diagnostics;
using System.Text;
using System.Collections;

namespace UnityEditor.FacebookEditor
{
    public class FacebookAndroidUtil
    {
        private static string debugKeyHash;

        public static string DebugKeyHash {
            get
            {
                if (debugKeyHash == null)
                {
                    var debugKeyStore = (Application.platform == RuntimePlatform.WindowsEditor) ? @"%HOMEPATH%\.android\debug.keystore" : @"~/.android/debug.keystore";
                    debugKeyHash = GetKeyHash("androiddebugkey", debugKeyStore, "android");
                }
                return debugKeyHash;
            }
        }

        private static string GetKeyHash(string alias, string keyStore, string password)
        {
            var proc = new Process();
            var arguments = @"""keytool -storepass {0} -keypass {1} -exportcert -alias {2} -keystore {3} | openssl sha1 -binary | openssl base64""";
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                proc.StartInfo.FileName = "cmd";
                arguments = @"/C " + arguments;
            }
            else
            {
                proc.StartInfo.FileName = "bash";
                arguments = @"-c " + arguments;
            }
            proc.StartInfo.Arguments = string.Format(arguments, password, password, alias, keyStore);
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            var keyHash = new StringBuilder();
            while (!proc.HasExited)
            {
                keyHash.Append(proc.StandardOutput.ReadToEnd());
            }
            return keyHash.ToString().TrimEnd('\n');
        }

        public static bool HasAndroidSDK()
        {
            return EditorPrefs.HasKey("AndroidSdkRoot") && System.IO.Directory.Exists(EditorPrefs.GetString("AndroidSdkRoot"));
        }
    }
}