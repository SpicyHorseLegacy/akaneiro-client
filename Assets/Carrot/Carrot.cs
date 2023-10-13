#region License
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
#endregion

#region References
using System;
using CarrotInc.MiniJSON;
using System.Net;
using UnityEngine;
using System.Security;
using System.Collections;
using System.Net.Security;
using CarrotInc.Amazon.Util;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
#endregion

/// <summary>
/// A MonoBehaviour which can be attached to a Unity GameObject to
/// provide access to Carrot functionality.
/// </summary>
public partial class Carrot : MonoBehaviour
{
    /// <summary>
    /// Gets the <see cref="Carrot"/> singleton.
    /// </summary>
    /// <value> The <see cref="Carrot"/> singleton.</value>
    public static Carrot Instance
    {
        get
        {
            if(mInstance == null)
            {
                mInstance = FindObjectOfType(typeof(Carrot)) as Carrot;

                if(mInstance == null)
                {
                    GameObject carrotGameObject = GameObject.Find("CarrotGameObject");
                    if(carrotGameObject == null)
                    {
                        carrotGameObject = new GameObject("CarrotGameObject");
                        carrotGameObject.AddComponent("Carrot");
                    }
                    mInstance = carrotGameObject.GetComponent<Carrot>();
                }

                TextAsset carrotJson = Resources.Load("carrot") as TextAsset;
                if(carrotJson == null)
                {
                    throw new NullReferenceException("Carrot text asset not found. Use the configuration tool in the 'Edit/Carrot' menu to generate it.");
                }
                else
                {
                    Dictionary<string, object> carrotConfig = null;
                    carrotConfig = Json.Deserialize(carrotJson.text) as Dictionary<string, object>;
                    mInstance.mFacebookAppId = carrotConfig["carrotAppId"] as string;
                    mInstance.mCarrotAppSecret = carrotConfig["carrotAppSecret"] as string;
                    mInstance.mBundleVersion = carrotConfig["appBundleVersion"] as string;

                    if(string.IsNullOrEmpty(mInstance.mFacebookAppId))
                    {
                        throw new ArgumentException("Carrot App Id has not been configured. Use the configuration tool in the 'Edit/Carrot' menu to assign your Carrot App Id and Secret.");
                    }
                }
            }
            return mInstance;
        }
    }

    /// <summary>
    /// Represents a Carrot authentication status for a user.
    /// </summary>
    public enum AuthStatus : int
    {
        /// <summary>The current user has not yet authorized the app, or has deauthorized the app.</summary>
        NotAuthorized = -1,

        /// <summary>The current authentication status has not been determined.</summary>
        Undetermined = 0,

        /// <summary>The current user has not granted the 'publish_actions' permission, or has removed the permission.</summary>
        ReadOnly = 1,

        /// <summary>The current user has granted all needed permissions and Carrot will send events to the Carrot server.</summary>
        Ready = 2
    }

    /// <summary>
    /// Responses to Carrot requests.
    /// </summary>
    public enum Response
    {
        /// <summary>Successful.</summary>
        OK,

        /// <summary>User has not authorized 'publish_actions', read only.</summary>
        ReadOnly,

        /// <summary>Service tier exceeded, not posted.</summary>
        UserLimitHit,

        /// <summary>Authentication error, app secret incorrect.</summary>
        BadAppSecret,

        /// <summary>Resource not found.</summary>
        NotFound,

        /// <summary>User is not authorized for Facebook App.</summary>
        NotAuthorized,

        /// <summary>Dynamic OG object not created due to parameter error.</summary>
        ParameterError,

        /// <summary>Network error.</summary>
        NetworkError,

        /// <summary>Undetermined error.</summary>
        UnknownError,
    }

    /// <summary>Carrot SDK version.</summary>
    public static readonly string SDKVersion = "1.1.0";

    /// <summary>
    /// Carrot debug users which can be assigned to UserId in order to simulate
    /// different cases for use.
    /// </summary>
    public class DebugUser
    {
        /// <summary>A user which never exists.</summary>
        public static readonly string NoSuchUser = "nosuchuser";

        /// <summary>A user which has not authorized the 'publish_actions' permission.</summary>
        public static readonly string ReadOnlyUser = "nopublishactions";

        /// <summary>A user which deauthorized the Facebook application.</summary>
        public static readonly string DeauthorizedUser = "deauthorized";
    }

    /// <summary>
    /// Return the string value of an <see cref="AuthStatus"/> value.
    /// </summary>
    /// <returns>The string description of an <see cref="AuthStatus"/>.</returns>
    public static string authStatusString(AuthStatus authStatus)
    {
        switch(authStatus)
        {
            case Carrot.AuthStatus.NotAuthorized: return "Carrot user has not authorized the application.";
            case Carrot.AuthStatus.Undetermined: return "Carrot user status is undetermined.";
            case Carrot.AuthStatus.ReadOnly: return "Carrot user has not allowed the 'publish_actions' permission.";
            case Carrot.AuthStatus.Ready: return "Carrot user is authorized.";
            default: return "Invalid Carrot AuthStatus.";
        }
    }

    /// <summary>
    /// The delegate type for the <see cref="AuthenticationStatusChanged"/> event.
    /// </summary>
    /// <param name="sender">The object which dispatched the <see cref="AuthenticationStatusChanged"/> event.</param>
    /// <param name="status">The new authentication status.</param>
    public delegate void AuthenticationStatusChangedHandler(object sender, AuthStatus status);

    /// <summary>
    /// An event which will notify listeners when the authentication status for the Carrot user has changed.
    /// </summary>
    public static event AuthenticationStatusChangedHandler AuthenticationStatusChanged;

    /// <summary>
    /// The callback delegate type for Carrot requests.
    /// </summary>
    public delegate void CarrotRequestResponse(Response response, string errorText);

    /// <summary>
    /// Check the authentication status of the current Carrot user.
    /// </summary>
    /// <value>The <see cref="GoCarrotInc.Carrot.AuthStatus"/> of the current Carrot user.</value>
    public AuthStatus Status
    {
        get
        {
            return mAuthStatus;
        }
        private set
        {
            if(mAuthStatus != value)
            {
                mAuthStatus = value;
                if(AuthenticationStatusChanged != null)
                {
                    AuthenticationStatusChanged(this, mAuthStatus);
                }

                foreach(CarrotCache.CachedRequest request in mCarrotCache.RequestsInCache(mAuthStatus))
                {
                    StartCoroutine(signedRequestCoroutine(request, cachedRequestHandler(request, null)));
                }
            }
        }
    }

    /// <summary>
    /// The user id for the current Carrot user.
    /// </summary>
    /// <value>The user id of the current Carrot user.</value>
    public string UserId
    {
        get
        {
            return mUserId;
        }
        set
        {
            if(mUserId != value)
            {
                mUserId = value;
                this.Status = AuthStatus.Undetermined;
            }
        }
    }

    /// <summary>
    /// An app-specified tag for associating metrics with A/B testing groups or other purposes.
    /// </summary>
    /// <value>The assigned tag.</value>
    public string Tag
    {
        get;
        set;
    }

    public DateTime InstallDate
    {
        get;
        private set;
    }

    /// <summary>
    /// Validate a Facebook user to allow posting of Carrot events.
    /// </summary>
    /// <remarks>
    /// This method will trigger notification of authentication status using the <see cref="AuthenticationStatusChanged"/> event.
    /// </remarks>
    /// <param name="accessTokenOrFacebookId">Facebook user access token or Facebook User Id.</param>
    public void validateUser(string accessTokenOrFacebookId)
    {
        StartCoroutine(validateUserCoroutine(accessTokenOrFacebookId));
    }

    /// <summary>
    /// Post an achievement to Carrot.
    /// </summary>
    /// <param name="achievementId">Carrot achievement id.</param>
    /// <param name="callback">Optional <see cref="CarrotRequestResponse"/> which will be used to deliver the reply.</param>
    public void postAchievement(string achievementId, CarrotRequestResponse callback = null)
    {
        if(string.IsNullOrEmpty(achievementId))
        {
            throw new ArgumentNullException("achievementId must not be null or empty string.", "achievementId");
        }

        StartCoroutine(cachedRequestCoroutine(ServiceType.Post, "/me/achievements.json", new Dictionary<string, object>() {
                {"achievement_id", achievementId}
        }, callback));
    }

    /// <summary>
    /// Post a high score to Carrot.
    /// </summary>
    /// <param name="score">Score.</param>
    /// <param name="callback">Optional <see cref="CarrotRequestResponse"/> which will be used to deliver the reply.</param>
    public void postHighScore(uint score, CarrotRequestResponse callback = null)
    {
        StartCoroutine(cachedRequestCoroutine(ServiceType.Post, "/me/scores.json", new Dictionary<string, object>() {
                {"value", score}
        }, callback));
    }

    /// <summary>
    /// Sends an Open Graph action which will use an existing object.
    /// </summary>
    /// <param name="actionId">Carrot action id.</param>
    /// <param name="objectInstanceId">Carrot object instance id.</param>
    /// <param name="callback">Optional <see cref="CarrotRequestResponse"/> which will be used to deliver the reply.</param>
    public void postAction(string actionId, string objectInstanceId, CarrotRequestResponse callback = null)
    {
        postAction(actionId, null, objectInstanceId, callback);
    }

    /// <summary>
    /// Sends an Open Graph action which will use an existing object.
    /// </summary>
    /// <param name="actionId">Carrot action id.</param>
    /// <param name="actionProperties">Parameters to be submitted with the action.</param>
    /// <param name="objectInstanceId">Carrot object instance id.</param>
    /// <param name="callback">Optional <see cref="CarrotRequestResponse"/> which will be used to deliver the reply.</param>
    public void postAction(string actionId, IDictionary actionProperties, string objectInstanceId,
                           CarrotRequestResponse callback = null)
    {
        if(string.IsNullOrEmpty(objectInstanceId))
        {
            throw new ArgumentNullException("objectInstanceId must not be null or empty string.", "objectInstanceId");
        }

        if(string.IsNullOrEmpty(actionId))
        {
            throw new ArgumentNullException("actionId must not be null or empty string.", "actionId");
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>() {
            {"action_id", actionId},
            {"action_properties", actionProperties == null ? new Dictionary<string, object>() : actionProperties},
            {"object_properties", new Dictionary<string, object>()}
        };
        if(objectInstanceId != null) parameters["object_instance_id"] = objectInstanceId;

        StartCoroutine(cachedRequestCoroutine(ServiceType.Post, "/me/actions.json", parameters, callback));
    }

    /// <summary>
    /// Sends an Open Graph action which will create a new object.
    /// </summary>
    /// <param name="actionId">Carrot action id.</param>
    /// <param name="viralObject">A <see cref="ViralObject"/> describing the object to be created.</param>
    /// <param name="callback">Optional <see cref="CarrotRequestResponse"/> which will be used to deliver the reply.</param>
    public void postAction(string actionId, ViralObject viralObject,
                           CarrotRequestResponse callback = null)
    {
        postAction(actionId, null, viralObject, callback);
    }

    /// <summary>
    /// Sends an Open Graph action which will create a new object.
    /// </summary>
    /// <param name="actionId">Carrot action id.</param>
    /// <param name="actionProperties">Parameters to be submitted with the action.</param>
    /// <param name="viralObject">A <see cref="ViralObject"/> describing the object to be created.</param>
    /// <param name="callback">Optional <see cref="CarrotRequestResponse"/> which will be used to deliver the reply.</param>
    public void postAction(string actionId, IDictionary actionProperties,
                           ViralObject viralObject,
                           CarrotRequestResponse callback = null)
    {
        if(string.IsNullOrEmpty(actionId))
        {
            throw new ArgumentNullException("actionId must not be null or empty string.", "actionId");
        }

        if(viralObject == null)
        {
            throw new ArgumentNullException("viralObject must not be null.", "viralObject");
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>() {
            {"action_id", actionId},
            {"action_properties", actionProperties == null ? new Dictionary<string, object>() : actionProperties},
            {"object_properties", viralObject.toDictionary()}
        };
        StartCoroutine(cachedRequestCoroutine(ServiceType.Post, "/me/actions.json", parameters, callback));
    }

    /// <summary>
    /// Post a 'Like' action that likes the Game's Facebook Page.
    /// </summary>
    /// <param name="callback">Optional <see cref="CarrotRequestResponse"/> which will be used to deliver the reply.</param>
    public void likeGame(CarrotRequestResponse callback = null)
    {
        StartCoroutine(cachedRequestCoroutine(ServiceType.Post, "/me/like.json", new Dictionary<string, object>() {
            {"object", "game"}
        }, callback));
    }

    /// <summary>
    /// Post a 'Like' action that likes the Publisher's Facebook Page.
    /// </summary>
    /// <param name="callback">Optional <see cref="CarrotRequestResponse"/> which will be used to deliver the reply.</param>
    public void likePublisher(CarrotRequestResponse callback = null)
    {
        StartCoroutine(cachedRequestCoroutine(ServiceType.Post, "/me/like.json", new Dictionary<string, object>() {
            {"object", "publisher"}
        }, callback));
    }

    /// <summary>
    /// Post a 'Like' action that likes an achievement.
    /// </summary>
    /// <param name="achievementId">The achievement identifier.</param>
    /// <param name="callback">Optional <see cref="CarrotRequestResponse"/> which will be used to deliver the reply.</param>
    public void likeAchievement(string achievementId, CarrotRequestResponse callback = null)
    {
        if(string.IsNullOrEmpty(achievementId))
        {
            throw new ArgumentNullException("achievementId must not be null or empty string.", "achievementId");
        }

        StartCoroutine(cachedRequestCoroutine(ServiceType.Post, "/me/like.json", new Dictionary<string, object>() {
            {"object", "achievement:" + achievementId}
        }, callback));
    }

    /// <summary>
    /// Post a 'Like' action that likes an Open Graph object.
    /// </summary>
    /// <param name="objectId">The instance id of the Carrot object.</param>
    /// <param name="callback">Optional <see cref="CarrotRequestResponse"/> which will be used to deliver the reply.</param>
    public void likeObject(string objectId, CarrotRequestResponse callback = null)
    {
        if(string.IsNullOrEmpty(objectId))
        {
            throw new ArgumentNullException("objectId must not be null or empty string.", "objectId");
        }

        StartCoroutine(cachedRequestCoroutine(ServiceType.Post, "/me/like.json", new Dictionary<string, object>() {
            {"object", "object:" + objectId}
        }, callback));
    }

    /// <summary>
    /// Inform Carrot about a purchase of premium currency for metrics tracking.
    /// </summary>
    /// <param name="amount">The amount of real money spent.</param>
    /// <param name="currency">The type of real money spent (eg. USD).</param>
    /// <param name="callback">Optional <see cref="CarrotRequestResponse"/> which will be used to deliver the reply.</param>
    public void postPremiumCurrencyPurchase(float amount, string currency, CarrotRequestResponse callback = null)
    {
        StartCoroutine(cachedRequestCoroutine(ServiceType.Metrics, "/purchase.json", new Dictionary<string, object>() {
            {"amount", amount},
            {"currency", currency}
        }, callback));
    }

    #region Internal
    /// @cond hide_from_doxygen
    Carrot()
    {
        mCarrotCache = new CarrotCache();
        this.InstallDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(mCarrotCache.InstallDate);
    }

    private CarrotRequestResponse cachedRequestHandler(CarrotCache.CachedRequest cachedRequest,
                                                       CarrotRequestResponse callback)
    {
        return (Response ret, string errorText) => {
                switch(ret)
                {
                    case Response.OK:
                    case Response.NotFound:
                    case Response.ParameterError:
                        cachedRequest.RemoveFromCache();
                        break;

                    default:
                        cachedRequest.AddRetryInCache();
                        break;
                }
                if(callback != null) callback(ret, errorText);
        };
    }
    /// @endcond
    #endregion

    #region Metrics
    /// @cond hide_from_doxygen
    private IEnumerator sendInstallMetricIfNeeded()
    {
        if(!mCarrotCache.InstallMetricSent)
        {
            yield return StartCoroutine(cachedRequestCoroutine(ServiceType.Metrics, "/install.json", new Dictionary<string, object>() {
                    {"install_date", mCarrotCache.InstallDate}
            }, (Response response, string errorText) => {
                if(response == Response.OK)
                {
                    mCarrotCache.markInstallMetricSent();
                }
            }));
        }
        yield return null;
    }

    private IEnumerator sendAppOpenedEvent()
    {
        yield return StartCoroutine(cachedRequestCoroutine(ServiceType.Metrics, "/app_opened.json", new Dictionary<string, object>() {}, null));
    }
    /// @endcond
    #endregion

    #region MonoBehaviour
    /// @cond hide_from_doxygen
    void Start()
    {
        DontDestroyOnLoad(this);
        StartCoroutine(servicesDiscoveryCoroutine());

#if UNITY_IPHONE || UNITY_ANDROID
        StartCoroutine(sendInstallMetricIfNeeded());
        mSessionStartTime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
#else
        StartCoroutine(sendAppOpenedEvent());
#endif
    }

#if UNITY_IPHONE || UNITY_ANDROID
    void OnApplicationPause(bool isPaused)
    {
        if(isPaused)
        {
            long sessionEndTime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            StartCoroutine(cachedRequestCoroutine(ServiceType.Metrics, "/session.json", new Dictionary<string, object>() {
                    {"start_time", mSessionStartTime},
                    {"end_time", sessionEndTime}
            }, null));
        }
        else
        {
            mSessionStartTime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
    }
#endif

    void OnApplicationQuit()
    {
        mCarrotCache.Dispose();
        Destroy(this);
    }
    /// @endcond
    #endregion

    #region Service Type
    /// @cond hide_from_doxygen
    public enum ServiceType : int
    {
        Auth    = -2,
        Metrics = -1,
        Post    = 2
    }

    private string hostForServiceType(ServiceType type)
    {
        switch(type)
        {
            case ServiceType.Auth: return mAuthHostname;
            case ServiceType.Metrics: return mMetricsHostname;
            case ServiceType.Post: return mPostHostname;
        }
        return null;
    }
    /// @endcond
    #endregion

    #region Carrot request coroutines
    /// @cond hide_from_doxygen
    private void addCommonPayloadFields(UnityEngine.WWWForm payload, Dictionary<string, object> urlParams)
    {
        payload.AddField("app_version", mBundleVersion);
        if(urlParams != null) urlParams.Add("app_version", mBundleVersion);
        if(!string.IsNullOrEmpty(this.Tag))
        {
            payload.AddField("tag", this.Tag);
            if(urlParams != null) urlParams.Add("tag", this.Tag);
        }
    }

    private IEnumerator servicesDiscoveryCoroutine()
    {
        if(string.IsNullOrEmpty(mFacebookAppId))
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(servicesDiscoveryCoroutine());
        }
        else
        {
            string urlString = String.Format("http://{0}/services.json?sdk_version={1}&sdk_platform={2}&game_id={3}&app_version={4}",
                mServicesDiscoveryHost,
                UnityEngine.WWW.EscapeURL(Carrot.SDKVersion),
                UnityEngine.WWW.EscapeURL(SystemInfo.operatingSystem.Replace(" ", "_").ToLower()),
                UnityEngine.WWW.EscapeURL(mFacebookAppId),
                UnityEngine.WWW.EscapeURL(mBundleVersion));

            UnityEngine.WWW request = new UnityEngine.WWW(urlString);
            yield return request;

            if(request.error == null)
            {
                Dictionary<string, object> reply = Json.Deserialize(request.text) as Dictionary<string, object>;
                mPostHostname = reply["post"] as string;
                mAuthHostname = reply["auth"] as string;
                mMetricsHostname = reply["metrics"] as string;

                if(!string.IsNullOrEmpty(mAccessTokenOrFacebookId))
                {
                    validateUser(mAccessTokenOrFacebookId);
                }
                else
                {
                    foreach(CarrotCache.CachedRequest crequest in mCarrotCache.RequestsInCache(mAuthStatus))
                    {
                        StartCoroutine(signedRequestCoroutine(crequest, cachedRequestHandler(crequest, null)));
                    }
                }
            }
            else
            {
                // Log error and retry in 10 seconds
                Debug.Log(request.error);
                yield return new WaitForSeconds(10);
                StartCoroutine(servicesDiscoveryCoroutine());
            }
        }
    }

    private IEnumerator validateUserCoroutine(string accessTokenOrFacebookId)
    {
        AuthStatus ret = AuthStatus.Undetermined;
        string hostname = hostForServiceType(ServiceType.Auth);
        mAccessTokenOrFacebookId = accessTokenOrFacebookId;

        if(string.IsNullOrEmpty(hostname))
        {
            return false;
        }

        if(string.IsNullOrEmpty(mUserId))
        {
            throw new NullReferenceException("UserId is empty. Assign a UserId before calling validateUser");
        }

        ServicePointManager.ServerCertificateValidationCallback = CarrotCertValidator;

        UnityEngine.WWWForm payload = new UnityEngine.WWWForm();
        payload.AddField("access_token", mAccessTokenOrFacebookId);
        payload.AddField("api_key", mUserId);
        addCommonPayloadFields(payload, null);

        UnityEngine.WWW request = new UnityEngine.WWW(String.Format("https://{0}/games/{1}/users.json", hostname, mFacebookAppId), payload);
        yield return request;

        int statusCode = 0;
        if(request.error != null)
        {
            Match match = Regex.Match(request.error, "^([0-9]+)");
            if(match.Success)
            {
                statusCode = int.Parse(match.Value);
            }
            else
            {
                Debug.Log(request.error);
            }
        }
        else
        {
            // TODO: Change if JSON updates to include code
            // Dictionary<string, object> reply = Json.Deserialize(request.text) as Dictionary<string, object>;
            // statusCode = (int)((long)reply["code"]);
            statusCode = 200;
        }

        switch(statusCode)
        {
            case 201:
            case 200: // Successful
                ret = AuthStatus.Ready;
                break;

            case 401: // User has not authorized 'publish_actions', read only
                ret = AuthStatus.ReadOnly;
                break;

            case 404:
            case 405: // User is not authorized for Facebook App
            case 422: // User was not created
                ret = AuthStatus.NotAuthorized;
                break;
        }
        this.Status = ret;

        yield return ret;
    }

    private IEnumerator cachedRequestCoroutine(ServiceType serviceType,
                                               string endpoint,
                                               Dictionary<string, object> parameters,
                                               CarrotRequestResponse callback = null)
    {
        CarrotCache.CachedRequest cachedRequest = mCarrotCache.CacheRequest(serviceType, endpoint, parameters);
        if((int)serviceType <= (int)mAuthStatus)
        {
            yield return StartCoroutine(signedRequestCoroutine(cachedRequest, cachedRequestHandler(cachedRequest, callback)));
        }
        else
        {
            if(callback != null) callback(Response.OK, authStatusString(mAuthStatus));
            yield return null;
        }
    }

    public static string signParams(string hostname, string endpoint, string secret, Dictionary<string, object> urlParams)
    {
        // Build sorted list of key-value pairs
        string[] keys = new string[urlParams.Keys.Count];
        urlParams.Keys.CopyTo(keys, 0);
        Array.Sort(keys);
        List<string> kvList = new List<string>();
        foreach(string key in keys)
        {
            string asStr;
            if((asStr = urlParams[key] as string) != null)
            {
                kvList.Add(String.Format("{0}={1}", key, asStr));
            }
            else
            {
                kvList.Add(String.Format("{0}={1}", key,
                    Json.Serialize(urlParams[key])));
            }
        }
        string payload = String.Join("&", kvList.ToArray());
        string signString = String.Format("{0}\n{1}\n{2}\n{3}", "POST", hostname.Split(new char[]{':'})[0], endpoint, payload);
        string sig = AWSSDKUtils.HMACSign(signString, secret, KeyedHashAlgorithm.Create("HMACSHA256"));
        return sig;
    }

    private IEnumerator signedRequestCoroutine(CarrotCache.CachedRequest cachedRequest,
                                               CarrotRequestResponse callback = null)
    {
        Response ret = Response.UnknownError;
        string errorText = null;
        string hostname = hostForServiceType(cachedRequest.ServiceType);

        if(string.IsNullOrEmpty(hostname))
        {
            if(callback != null) callback(Response.OK, "");
            return false;
        }

        if(string.IsNullOrEmpty(mUserId))
        {
            throw new NullReferenceException("UserId is empty. Assign a UserId before using Carrot.");
        }

        ServicePointManager.ServerCertificateValidationCallback = CarrotCertValidator;

        Dictionary<string, object> urlParams = new Dictionary<string, object> {
            {"api_key", mUserId},
            {"game_id", mFacebookAppId},
            {"request_date", cachedRequest.RequestDate},
            {"request_id", cachedRequest.RequestId}
        };
        Dictionary<string, object> parameters = cachedRequest.Parameters;

        // If this has an attached image, bytes will be placed here.
        byte[] imageBytes = null;

        if(parameters != null)
        {
            // Check for image on dynamic objects
            if(parameters.ContainsKey("object_properties"))
            {
                IDictionary objectProperties = parameters["object_properties"] as IDictionary;
                object image = objectProperties["image"];
                Texture2D imageTex2D;
                if((imageTex2D = image as Texture2D) != null)
                {
                    imageBytes = imageTex2D.EncodeToPNG();
                    using(SHA256 sha256 = SHA256Managed.Create())
                    {
                        objectProperties["image_sha"] = System.Text.Encoding.UTF8.GetString(sha256.ComputeHash(imageBytes));
                    }
                }
                else if(image is string)
                {
                    objectProperties["image_url"] = image;
                }
                objectProperties.Remove("image");
            }

            // Merge params
            foreach(KeyValuePair<string, object> entry in parameters)
            {
                urlParams[entry.Key] = entry.Value;
            }
        }

        UnityEngine.WWWForm formPayload = new UnityEngine.WWWForm();
        addCommonPayloadFields(formPayload, urlParams);

        string[] keys = new string[urlParams.Keys.Count];
        urlParams.Keys.CopyTo(keys, 0);
        foreach(string key in keys)
        {
            string asStr;
            if((asStr = urlParams[key] as string) != null)
            {
                formPayload.AddField(key, asStr);
            }
            else
            {
                formPayload.AddField(key,
                    Json.Serialize(urlParams[key]));
            }
        }

        string sig = signParams(hostname, cachedRequest.Endpoint, mCarrotAppSecret, urlParams);
        formPayload.AddField("sig", sig);

        // Attach image
        if(imageBytes != null)
        {
            formPayload.AddBinaryData("image_bytes", imageBytes);
        }

        UnityEngine.WWW request = new UnityEngine.WWW(String.Format("https://{0}{1}", hostname, cachedRequest.Endpoint), formPayload);
        yield return request;

        int statusCode = 0;
        if(request.error != null)
        {
            Match match = Regex.Match(request.error, "^([0-9]+)");
            if(match.Success)
            {
                statusCode = int.Parse(match.Value);
            }
            else
            {
                errorText = request.error;
                Debug.Log(request.error);
            }
        }
        else
        {
            Dictionary<string, object> reply = Json.Deserialize(request.text) as Dictionary<string, object>;
            statusCode = (int)((long)reply["code"]);
        }

        switch(statusCode)
        {
            case 201:
            case 200: // Successful
                ret = Response.OK;
                if(cachedRequest.ServiceType != ServiceType.Metrics) this.Status = AuthStatus.Ready;
                break;

            case 401: // User has not authorized 'publish_actions', read only
                ret = Response.ReadOnly;
                if(cachedRequest.ServiceType != ServiceType.Metrics) this.Status = AuthStatus.ReadOnly;
                break;

            case 402: // Service tier exceeded, not posted
                ret = Response.UserLimitHit;
                if(cachedRequest.ServiceType != ServiceType.Metrics) this.Status = AuthStatus.Ready;
                break;

            case 403: // Authentication error, app secret incorrect
                ret = Response.BadAppSecret;
                if(cachedRequest.ServiceType != ServiceType.Metrics) this.Status = AuthStatus.Ready;
                break;

            case 404: // Resource not found
                ret = Response.NotFound;
                if(cachedRequest.ServiceType != ServiceType.Metrics) this.Status = AuthStatus.Ready;
                break;

            case 405: // User is not authorized for Facebook App
                ret = Response.NotAuthorized;
                if(cachedRequest.ServiceType != ServiceType.Metrics) this.Status = AuthStatus.NotAuthorized;
                break;

            case 424: // Dynamic OG object not created due to parameter error
                ret = Response.ParameterError;
                if(cachedRequest.ServiceType != ServiceType.Metrics) this.Status = AuthStatus.Ready;
                break;
        }
        if(callback != null) callback(ret, errorText);
    }
    /// @endcond
    #endregion

    #region SSL Cert Validator
    /// @cond hide_from_doxygen
    private static bool CarrotCertValidator(object sender, X509Certificate certificate,
                                            X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        // This is not ideal
        return true;
    }
    /// @endcond
    #endregion

    #region Member Variables
    private static Carrot mInstance = null;
    private AuthStatus mAuthStatus;
    private string mUserId;
    private string mServicesDiscoveryHost = "services.gocarrot.com";
    private string mPostHostname;
    private string mAuthHostname;
    private string mMetricsHostname;
    private string mFacebookAppId;
    private string mCarrotAppSecret;
    private string mBundleVersion;
    private string mAccessTokenOrFacebookId;
    private CarrotCache mCarrotCache;
    private long mSessionStartTime;
    #endregion
}
