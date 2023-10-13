using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System;
using System.Linq;
	
public sealed class facebookObject : MonoBehaviour
{
	public Texture2D profilePic;
	public Texture2D tempProfilePic ;
	public bool haveTheProfilePicture = false ;
	public string lastPurchasedItem;
		
    #region FB.Init() example

    private bool isInit = false;

    private void CallFBInit()
    {
        FB.Init(OnInitComplete, OnHideUnity);
    }

    private void OnInitComplete()
    {
        Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
		//FB.Canvas.SetAspectRatio(1280,720);
		//FB.Canvas.SetResolution(1280,720,false);
        isInit = true;
		if (FB.IsLoggedIn == false){
			CallFBLogin();	
		}else if (FB.IsLoggedIn == true){
			CallFBLogin();	
		}
    }

    private void OnHideUnity(bool isGameShown)
    {
        Debug.Log("Is game showing? " + isGameShown);
    }

    #endregion

    #region FB.Login() example

    private void CallFBLogin()
    {
        FB.Login("email,publish_actions", Callback);
		Debug.Log("logged innnnnn");
		//
		//login data to the server autoLogin
		//
		string url = "https://facebook.angry-red.com/message.php";
		//Platform.Instance.platformType.Set(EUserType.eUserType_Facebook);
		Platform.Instance.platformType.Set(1);
		//Debug.Log(Platform.Instance.platformType.Get());
		WWWForm form = new WWWForm();
		form.AddField("uid", FB.UserId);
		form.AddField("access_token",FB.AccessToken);
		if (FB.IsLoggedIn == true){
			form.AddField("message", "success");
		}else if (FB.IsLoggedIn == false){
			form.AddField("message", "fail");
		}
		form.AddField("version",WebLoginCtrl.globalVersion);
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
		
    }
	IEnumerator WaitForRequest(WWW www){
		yield return www;
		if(www.error == null){
			//Debug.Log("WWW Ok!: " + www.data);
			lastPurchasedItem += www.data;			
		} else {
			//Debug.Log("WWW Error: "+ www.error);
			//lastPurchasedItem += www.error;
		}
	} 

    #endregion

    #region FB.PublishInstall() example

    private void CallFBPublishInstall()
    {
        FB.PublishInstall(PublishComplete);
    }

    private void PublishComplete(FBResult result)
    {
        Debug.Log("publish response: " + result.Text);
    }

    #endregion

    #region FB.AppRequest() Friend Selector

    public string FriendSelectorTitle = "I am Playing, Akaniero. And you should do dude";
    public string FriendSelectorMessage = "Derp";
    public string FriendSelectorFilters = "[\"all\",\"app_users\",\"app_non_users\"]";
    public string FriendSelectorData = "{}";
    public string FriendSelectorExcludeIds = "";
    public string FriendSelectorMax = "";

    private void CallAppRequestAsFriendSelector()
    {
        // If there's a Max Recipients specified, include it
        int? maxRecipients = null;
        if (FriendSelectorMax != "")
        {
            try
            {
                maxRecipients = Int32.Parse(FriendSelectorMax);
            }
            catch (Exception e)
            {
                status = e.Message;
            }
        }

        // include the exclude ids
        string[] excludeIds = (FriendSelectorExcludeIds == "") ? null : FriendSelectorExcludeIds.Split(',');

        FB.AppRequest(
            message: FriendSelectorMessage,
            filters: FriendSelectorFilters,
            excludeIds: excludeIds,
            maxRecipients: maxRecipients,
            data: FriendSelectorData,
            title: FriendSelectorTitle,
            callback: Callback
        );
    }
    #endregion

    #region FB.AppRequest() Direct Request

    public string DirectRequestTitle = "";
    public string DirectRequestMessage = "Herp";
    private string DirectRequestTo = "";

    private void CallAppRequestAsDirectRequest()
    {
        if (DirectRequestTo == "")
        {
            throw new ArgumentException("\"To Comma Ids\" must be specificed", "to");
        }
        FB.AppRequest(
            message: DirectRequestMessage,
            to: DirectRequestTo.Split(','),
            title: DirectRequestTitle,
            callback: Callback
        );
    }

    #endregion

    #region FB.Feed() example

    public string FeedToId = "";
    public string FeedLink = "";
    public string FeedLinkName = "";
    public string FeedLinkCaption = "";
    public string FeedLinkDescription = "";
    public string FeedPicture = "";
    public string FeedMediaSource = "";
    public string FeedActionName = "";
    public string FeedActionLink = "";
    public string FeedReference = "";
    public bool IncludeFeedProperties = false;
    private Dictionary<string, string[]> FeedProperties = new Dictionary<string, string[]>();

    private void CallFBFeed()
    {
        Dictionary<string, string[]> feedProperties = null;
        if (IncludeFeedProperties)
        {
            feedProperties = FeedProperties;
        }
        FB.Feed(
            toId: FeedToId,
            link: FeedLink,
            linkName: /*FeedLinkName*/"Akaniero Demon Hunters",
            linkCaption: /*FeedLinkCaption*/"I am playing Akaniero now, Come and Join",
            linkDescription: /*FeedLinkDescription*/"Akaniero, Akaniero,Akaniero,Akaniero,Akaniero,Akaniero,Akaniero,",
            picture: /*FeedPicture*/ "https://facebook.bigheadbash.com/client/webClientAKATest/playingAkaneiro.jpg",
            mediaSource: FeedMediaSource,
            actionName: FeedActionName,
            actionLink: FeedActionLink,
            reference: FeedReference,
            properties: feedProperties,
            callback: Callback
        );
    }

    #endregion

    #region FB.Canvas.Pay() example

    public string PayProduct = "";

    private void CallFBPay()
    {
        FB.Canvas.Pay(PayProduct);
    }

    #endregion

    #region FB.API() example

    public string ApiQuery = "";

    private void CallFBAPI()
    {
        FB.API(ApiQuery, Facebook.HttpMethod.GET, Callback);
    }

    #endregion

    #region FB.GetDeepLink() example

    private void CallFBGetDeepLink()
    {
        FB.GetDeepLink(Callback);
    }

    #endregion

    #region FB.AppEvent.LogEvent example

    public float PlayerLevel = 1.0f;

    public void CallAppEventLogEvent()
    {
        var parameters = new Dictionary<string, object>();
        parameters[Facebook.FBAppEventParameterName.Level] = "Player Level";
        FB.AppEvents.LogEvent(Facebook.FBAppEventName.AchievedLevel, PlayerLevel, parameters);
        PlayerLevel++;
    }

    #endregion

    #region GUI

    private string status = "Ready";

    private string lastResponse = "";
    public GUIStyle textStyle = new GUIStyle();
    private Texture2D lastResponseTexture;

    private Vector2 scrollPosition = Vector2.zero;
#if UNITY_IOS || UNITY_ANDROID
    int buttonHeight = 60;
    int mainWindowWidth = Screen.width - 30;
    int mainWindowFullWidth = Screen.width;
#else
    int buttonHeight = 24;
    int mainWindowWidth = 500;
    int mainWindowFullWidth = 530;
#endif

    private int TextWindowHeight
    {
        get
        {
#if UNITY_IOS || UNITY_ANDROID
            return IsHorizontalLayout() ? Screen.height : 85;
#else
        return Screen.height;
#endif

        }
    }

    void Awake()
    {
		CallFBInit();
		status = "FB.Init() called with " + FB.AppId;
		
        textStyle.alignment = TextAnchor.UpperLeft;
        textStyle.wordWrap = true;
        textStyle.padding = new RectOffset(10, 10, 10, 10);
        textStyle.stretchHeight = true;
        textStyle.stretchWidth = false;

        FeedProperties.Add("key1", new[] { "valueString1" });
        FeedProperties.Add("key2", new[] { "valueString2", "http://www.facebook.com" });
    }
	#region calledMethods
	void postPicture(){
		StartCoroutine(TakeScreenshot());
	}
	void inviteFriends(){
		CallAppRequestAsFriendSelector();
	}
	void postScreenShotST(){
		CallFBFeed();
	}
	void buySomething(string productID){
		FB.Canvas.Pay(productID, "purchaseitem", 1, null, null, null, null, null, callback: Callback);
		lastPurchasedItem = productID.Substring(30);
	}
	#endregion
	/*
    void OnGUI()
    {
		
        if (IsHorizontalLayout())
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
        }
        GUILayout.Space(5);
        GUILayout.Box("Status: " + status, GUILayout.MinWidth(mainWindowWidth));

#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            scrollPosition.y += Input.GetTouch(0).deltaPosition.y;
        }
#endif

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MinWidth(mainWindowFullWidth));
        GUILayout.BeginVertical();
        GUI.enabled = !isInit;
        if (Button("FB.Init"))
        {
            CallFBInit();
            status = "FB.Init() called with " + FB.AppId;
        }

        GUI.enabled = isInit;
        if (Button("Login"))
        {
            CallFBLogin();
            status = "Login called";
        }

#if UNITY_IOS || UNITY_ANDROID
        if (Button("Publish Install"))
        {
            CallFBPublishInstall();
            status = "Install Published";
        }
#endif

        GUI.enabled = FB.IsLoggedIn;
        GUILayout.Space(10);
        LabelAndTextField("Title (optional): ", ref FriendSelectorTitle);
        LabelAndTextField("Message: ", ref FriendSelectorMessage);
        LabelAndTextField("Exclude Ids (optional): ", ref FriendSelectorExcludeIds);
        LabelAndTextField("Filters (optional): ", ref FriendSelectorFilters);
        LabelAndTextField("Max Recipients (optional): ", ref FriendSelectorMax);
        LabelAndTextField("Data (optional): ", ref FriendSelectorData);
        if (Button("Open Friend Selector"))
        {
            try
            {
                CallAppRequestAsFriendSelector();
                status = "Friend Selector called";
            }
            catch (Exception e)
            {
                status = e.Message;
            }
        }
        GUILayout.Space(10);
        LabelAndTextField("Title (optional): ", ref DirectRequestTitle);
        LabelAndTextField("Message: ", ref DirectRequestMessage);
        LabelAndTextField("To Comma Ids: ", ref DirectRequestTo);
        if (Button("Open Direct Request"))
        {
            try
            {
                CallAppRequestAsDirectRequest();
                status = "Direct Request called";
            }
            catch (Exception e)
            {
                status = e.Message;
            }
        }
        GUILayout.Space(10);
        LabelAndTextField("To Id (optional): ", ref FeedToId);
        LabelAndTextField("Link (optional): ", ref FeedLink);
        LabelAndTextField("Link Name (optional): ", ref FeedLinkName);
        LabelAndTextField("Link Desc (optional): ", ref FeedLinkDescription);
        LabelAndTextField("Link Caption (optional): ", ref FeedLinkCaption);
        LabelAndTextField("Picture (optional): ", ref FeedPicture);
        LabelAndTextField("Media Source (optional): ", ref FeedMediaSource);
        LabelAndTextField("Action Name (optional): ", ref FeedActionName);
        LabelAndTextField("Action Link (optional): ", ref FeedActionLink);
        LabelAndTextField("Reference (optional): ", ref FeedReference);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Properties (optional)", GUILayout.Width(150));
        IncludeFeedProperties = GUILayout.Toggle(IncludeFeedProperties, "Include");
        GUILayout.EndHorizontal();
        if (Button("Open Feed Dialog"))
        {
            try
            {
                CallFBFeed();
                status = "Feed dialog called";
            }
            catch (Exception e)
            {
                status = e.Message;
            }
        }
        GUILayout.Space(10);

#if UNITY_WEBPLAYER
        LabelAndTextField("Product: ", ref PayProduct);
        if (Button("Call Pay"))
        {
            CallFBPay();
        }
        GUILayout.Space(10);
#endif

        LabelAndTextField("API: ", ref ApiQuery);
        if (Button("Call API"))
        {
            status = "API called";
            CallFBAPI();
        }
        GUILayout.Space(10);
        if (Button("Take & upload screenshot"))
        {
            status = "Take screenshot";

            StartCoroutine(TakeScreenshot());
        }

        if (Button("Get Deep Link"))
        {
            CallFBGetDeepLink();
        }
#if UNITY_IOS || UNITY_ANDROID
        if (Button("Log FB App Event"))
        {
            status = "Logged FB.AppEvent";
            CallAppEventLogEvent();
        }
#endif

        GUILayout.Space(10);

        GUILayout.EndVertical();
        GUILayout.EndScrollView();

        if (IsHorizontalLayout())
        {
            GUILayout.EndVertical();
        }
        GUI.enabled = true;

        var textAreaSize = GUILayoutUtility.GetRect(640, TextWindowHeight);

        GUI.TextArea(
            textAreaSize,
            string.Format(
                " AppId: {0} \n Facebook Dll: {1} \n UserId: {2}\n IsLoggedIn: {3}\n AccessToken: {4}\n\n {5}",
                FB.AppId,
                (isInit) ? "Loaded Successfully" : "Not Loaded",
                FB.UserId,
                FB.IsLoggedIn,
                FB.AccessToken,
                lastResponse
            ), textStyle);

        if (lastResponseTexture != null)
        {
            GUI.Label(new Rect(textAreaSize.x + 5, textAreaSize.y + 200, lastResponseTexture.width, lastResponseTexture.height), lastResponseTexture);
        }

        if (IsHorizontalLayout())
        {
            GUILayout.EndHorizontal();
        }
    }
	 */
    void Callback(FBResult result)
    {
        lastResponseTexture = null;
        if (result.Error != null){
            lastResponse = "Error Response:\n" + result.Error;
			lastPurchasedItem = "++++++++++++";
		}else if (!ApiQuery.Contains("/picture")){
            lastResponse = "Success Response:\n" + result.Text;
			int val = 0;
			if (lastPurchasedItem == "KS_1.html"){
				lastPurchasedItem = "Karma Shard Pack 1";
			}else if (lastPurchasedItem == "KS_2.html"){
				lastPurchasedItem = "Karma Shard Pack 2";
			}else if (lastPurchasedItem == "KS_3.html"){
				lastPurchasedItem = "Karma Shard Pack 3";
			}else if (lastPurchasedItem == "KS_4.html"){
				lastPurchasedItem = "Karma Shard Pack 4";
			}else if (lastPurchasedItem == "KS_5.html"){
				lastPurchasedItem = "Karma Shard Pack 5";
			}else if (lastPurchasedItem == "KS_6.html"){
				lastPurchasedItem = "Karma Shard Pack 6";
			}else if (lastPurchasedItem == "KS_7.html"){
				lastPurchasedItem = "Karma Shard Pack 7";
			}else if (lastPurchasedItem == "CP_1.html"){
				lastPurchasedItem = "Crystals Pack 1";
			}else if (lastPurchasedItem == "CP_2.html"){
				lastPurchasedItem = "Crystals Pack 2";
			}else if (lastPurchasedItem == "CP_3.html"){
				lastPurchasedItem = "Crystals Pack 3";
			}else if (lastPurchasedItem == "CP_4.html"){
				lastPurchasedItem = "Crystals Pack 4";
			}else if (lastPurchasedItem == "CP_5.html"){
				lastPurchasedItem = "Crystals Pack 5";
			}else if (lastPurchasedItem == "CP_6.html"){
				lastPurchasedItem = "Crystals Pack 6";
			}else if (lastPurchasedItem == "CP_7.html"){
				lastPurchasedItem = "Crystals Pack 7";
			}else{
				lastPurchasedItem = "{{Nothing FOUND}}" ;
			}
		}else
        {
            lastResponseTexture = result.Texture;
            lastResponse = "Success Response:\n";
        }
    }
	//--a temp gui function
	void OnGUI (){
		GUI.Label (new Rect(10, 10, 600, 250), lastResponse+ "\n" + lastPurchasedItem);
		//GUI.TextArea (new Rect(10, 10, 600, 250), lastResponse+ "\n" + lastPurchasedItem);
	}
	//--end it
    private IEnumerator TakeScreenshot() 
    {
        yield return new WaitForEndOfFrame();

        var width = Screen.width;
        var height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        byte[] screenshot = tex.EncodeToPNG();

        var wwwForm = new WWWForm();
        wwwForm.AddBinaryData("image", screenshot, "AkanieroDemonHunters.png");
        wwwForm.AddField("message", "Woow. Look what I have achieved in Akaniero: Demon Hunters !");

        FB.API("me/photos", Facebook.HttpMethod.POST, Callback, wwwForm);
    }

    private bool Button(string label)
    {
        return GUILayout.Button(
          label, 
          GUILayout.MinHeight(buttonHeight), 
          GUILayout.MaxWidth(mainWindowWidth)
        );
    }

    private void LabelAndTextField(string label, ref string text)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, GUILayout.MaxWidth(150));
        text = GUILayout.TextField(text);
        GUILayout.EndHorizontal();
    }

    private bool IsHorizontalLayout()
    {
#if UNITY_IOS || UNITY_ANDROID
        return Screen.orientation == ScreenOrientation.Landscape;
#else
        return true;
#endif
    }

    #endregion
}