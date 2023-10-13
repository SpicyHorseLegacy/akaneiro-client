How to enabled advertiser ID for iOS:

1) Uncomment this section at the top of the GA_GenericInfo class (GameAnalytics > Plugins > Framework > Scripts):

	if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport ("__Internal")]
	private static extern string GetUserID ();
	#endif

2) Uncomment this section at the top of the GetUserUUID method of the GA_GenericInfo class (GameAnalytics > Plugins > Framework > Scripts):

	#if UNITY_IPHONE && !UNITY_EDITOR
	
	string uid = GetUserID();
	
	if (uid != null && uid != "")
		return uid;
	else
		return "";
	
	#endif

3) Move the GA_UserID.mm file from the GameAnalytics > Plugins > iOS folder to a new folder called Plugins > iOS in your Assets folder. This will cause Unity to include the file in your compiled XCode project when you build for iOS.

4) When you build your project for iOS, XCode will complain about some missing imports in the GA_UserID.mm file. To fix this go to the Build Phases tab of your Unity-iPhone project inside XCode, and fold out the Link Binary With Libraries section. Click the (+) button and add the AdSupport.framework.