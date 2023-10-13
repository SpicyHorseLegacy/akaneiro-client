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
using CarrotInc.MiniJSON;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.Threading;
using System.Net.Security;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class CarrotSettings : EditorWindow
{
    public static string CarrotAppId
    {
        get
        {
            LoadSettings();
            return mCarrotAppId;
        }
        private set
        {
            string appId = value.Trim();
            if(appId != mCarrotAppId)
            {
                mAppValid = false;
                mAppStatus = "";
                mCarrotAppId = appId;
                SaveSettings();
            }
        }
    }

    public static string CarrotAppSecret
    {
        get
        {
            LoadSettings();
            return mCarrotAppSecret;
        }
        private set
        {
            string appSecret = value.Trim();
            if(appSecret != mCarrotAppSecret)
            {
                mAppValid = false;
                mAppStatus = "";
                mCarrotAppSecret = appSecret;
                SaveSettings();
            }
        }
    }

    public static bool AppValid
    {
        get
        {
            LoadSettings();
            return mAppValid;
        }
        private set
        {
            if(value != mAppValid)
            {
                mAppValid = value;
                SaveSettings();
            }
        }
    }

    public static string AppStatus
    {
        get
        {
            LoadSettings();
            return mAppStatus;
        }
        private set
        {
            string appStatus = value.Trim();
            if(appStatus != mAppStatus)
            {
                mAppStatus = appStatus;
                SaveSettings();
            }
        }
    }

    [MenuItem("Edit/Carrot")]
    public static void ShowWindow()
    {
        LoadSettings();
        CarrotSettings settingsWindow = (CarrotSettings)GetWindow<CarrotSettings>(false, "Carrot Settings", false);
        settingsWindow.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        CarrotAppId = EditorGUILayout.TextField("Carrot App Id", mCarrotAppId);
        CarrotAppSecret = EditorGUILayout.TextField("Carrot App Secret", mCarrotAppSecret);

        if(AppValid)
        {
            EditorGUILayout.HelpBox(mAppStatus, MessageType.Info);
        }
        else if(!string.IsNullOrEmpty(mAppStatus))
        {
            EditorGUILayout.HelpBox(mAppStatus, MessageType.Error);
        }

        if(!AppValid)
        {
            if(GUILayout.Button("Validate Settings", GUILayout.Height(25)))
            {
                ValidateSettings();
            }
        }

        if(GUILayout.Button("Get a Carrot Account", GUILayout.Height(25)))
        {
            Application.OpenURL("https://gocarrot.com/developers/sign_up?referrer=unity");
        }
    }

    static void LoadSettings()
    {
        if(!mSettingsLoaded)
        {
            TextAsset carrotJson = Resources.Load("carrot") as TextAsset;
            if(carrotJson != null)
            {
                Dictionary<string, object> carrotConfig = null;
                carrotConfig = Json.Deserialize(carrotJson.text) as Dictionary<string, object>;
                mCarrotAppId = carrotConfig["carrotAppId"] as string;
                mCarrotAppSecret = carrotConfig["carrotAppSecret"] as string;
                mAppStatus = carrotConfig.ContainsKey("appStatus") ? carrotConfig["appStatus"] as string : "";
                mAppValid = carrotConfig.ContainsKey("appValid") ? (bool)carrotConfig["appValid"] : false;
            }
            mSettingsLoaded = true;
        }
    }

    static void SaveSettings()
    {
        Dictionary<string, object> carrotConfig = new Dictionary<string, object>();
        carrotConfig["carrotAppId"] = mCarrotAppId;
        carrotConfig["carrotAppSecret"] = mCarrotAppSecret;
        carrotConfig["appBundleVersion"] = PlayerSettings.bundleVersion.ToString();
        carrotConfig["appValid"] = mAppValid;
        carrotConfig["appStatus"] = mAppStatus;

        System.IO.Directory.CreateDirectory(Application.dataPath + "/Resources");
        File.WriteAllText(Application.dataPath + "/Resources/carrot.bytes", Json.Serialize(carrotConfig));
        AssetDatabase.Refresh();
    }

    static void ValidateSettings()
    {
        LoadSettings();
        string hostname = "gocarrot.com";
        string endpoint = String.Format("/games/{0}/validate_sig.json", mCarrotAppId);
        string versionString = PlayerSettings.bundleVersion.ToString();
        Dictionary<string, object> urlParams  = new Dictionary<string, object> {
            {"app_version", versionString},
            {"id", mCarrotAppId}
        };
        string sig = Carrot.signParams(hostname, endpoint, mCarrotAppSecret, urlParams);

        // Use System.Net.WebRequest due to crossdomain.xml bug in Unity Editor mode
        ServicePointManager.ServerCertificateValidationCallback = CarrotCertValidator;
        string postData = String.Format("app_version={0}&id={1}&sig={2}",
            WWW.EscapeURL(versionString),
            WWW.EscapeURL(mCarrotAppId),
            WWW.EscapeURL(sig));
        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        WebRequest request = WebRequest.Create(String.Format("https://{0}{1}", hostname, endpoint));
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = byteArray.Length;

        Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        try
        {
            using(WebResponse response = request.GetResponse())
            {
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();

                Dictionary<string, object> reply = null;
                reply = Json.Deserialize(responseFromServer) as Dictionary<string, object>;
                AppValid = true;
                AppStatus = "Settings valid for: " + reply["name"] as string;
            }
        }
        catch(WebException e)
        {
            HttpWebResponse response = (HttpWebResponse)e.Response;
            switch((int)response.StatusCode)
            {
                case 403:
                {
                    // Invalid signature
                    AppStatus = "Invalid Carrot App Secret";
                }
                break;

                case 404:
                {
                    // No such game id
                    AppStatus = "Invalid Carrot App Id";
                }
                break;

                default:
                {
                    // Unknown
                    AppStatus = "Unknown error during validation";
                }
                break;
            }
            AppValid = false;
        }
    }

    private static bool CarrotCertValidator(object sender, X509Certificate certificate,
                                            X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        // This is not ideal
        return true;
    }

    static string mCarrotAppId = "";
    static string mCarrotAppSecret = "";
    static bool mSettingsLoaded = false;
    static bool mAppValid = false;
    static string mAppStatus = "";
}
