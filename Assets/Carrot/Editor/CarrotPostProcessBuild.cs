/* Carrot -- Copyright (C) 2012 GoCarrot Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEditor.Callbacks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

public static class CarrotPostProcessBuild
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        // Check for official Unity SDK presence
        try
        {
            Assembly assembly = Assembly.Load("IFacebook");
            Type type = Type.GetType("Facebook.FBBuildVersionAttribute,IFacebook");

            FieldInfo sdkVersionField = type.GetField("SDKVersion");
            string sdkVersion = sdkVersionField.GetValue(null).ToString();

            object[] buildVersionAttributes = assembly.GetCustomAttributes(type, false);
            string buildVersion = "N/A";
            foreach(object attribute in buildVersionAttributes)
            {
                buildVersion = attribute.ToString();
            }
            Debug.Log("Carrot detected Unity Facebook SDK Version: " + sdkVersion + " build: " + buildVersion);

            // If needed apply fix-up to iOS code
            if(target == BuildTarget.iPhone)
            {
                string fullPath = Path.Combine(Application.dataPath, "Facebook/Editor/iOS/FbUnityInterface.mm");
                string data = Load(fullPath);

                string hash = null;
                using(var cryptoProvider = new SHA1CryptoServiceProvider())
                {
                    byte[] bytes = new byte[data.Length * sizeof(char)];
                    System.Buffer.BlockCopy(data.ToCharArray(), 0, bytes, 0, bytes.Length);
                    hash = BitConverter.ToString(cryptoProvider.ComputeHash(bytes)).Replace("-", String.Empty);
                }

                // Build 130827.e8d7fe03ac79388
                if(string.Compare(hash, "D36DE7E5867FE51D68DBF53FED8C4B01FEF1F19E", true) == 0)
                {
                    data = data.Replace("if (self.session == nil || self.session.state != FBSessionStateCreated) ", "");
                    data = data.Replace("defaultAudience:FBSessionDefaultAudienceNone", "defaultAudience:FBSessionDefaultAudienceFriends");
                    Save(fullPath, data);
                }
            }
        }
        finally {}
    }

    static string Load(string fullPath)
    {
        string data;
        FileInfo projectFileInfo = new FileInfo(fullPath);
        StreamReader fs = projectFileInfo.OpenText();
        data = fs.ReadToEnd();
        fs.Close();
        return data;
    }

    static void Save(string fullPath, string data)
    {
        System.IO.StreamWriter writer = new System.IO.StreamWriter(fullPath, false);
        writer.Write(data);
        writer.Close();
    }
}
