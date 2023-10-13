using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//just drop this script to empty game object on first scene you game start at, this all what you have to do
//no coding is required 
//then you can view logs , warnings , errors and exceptions inside your game
//just draw circle on your game to view all logs
public class InGameLog : MonoBehaviour {
	
	
	class Log
	{
		public int 	   count = 1;	
		public LogType logType ;
		public string  condition ;
		public string  stacktrace ;
	}
	//contains all uncollapsed log
	List<Log> logs = new List<Log>();
	//contains all collapsed logs
	List<Log> collapsedLogs = new List<Log>();
	//contain logs which should only appear to user , for example if you switch off show logs + switch off show warnings
	//and your mode is collapse,then this list will contains only collapsed errors
	List<Log> currentLog = new List<Log>();
	
	//used to check if the new coming logs is already exist or new one
	Dictionary<string , Log> logsDic = new Dictionary<string , Log>();
	
	[HideInInspector]
	//show hide In Game Logs
	public bool show = false ;
	//collapse logs
	bool collapse ;
	//to deside if you want to clean logs for new loaded scene
	bool clearOnNewSceneLoaded ;
	//show or hide logs
	bool showLog = true ;
	//show or hide warnings
	bool showWarning = true ;
	//show or hide errors
	bool showError = true ;
	
	//total number of logs
	int numOfLogs=0;
	//total number of warnings
	int numOfLogsWarning=0;
	//total number of errors
	int numOfLogsError=0;
	//total number of collapsed logs
	int numOfCollapsedLogs=0 ;
	//total number of collapsed warnings
	int numOfCollapsedLogsWarning =0;
	//total number of collapsed errors
	int numOfCollapsedLogsError =0;
	
	//maximum number of allowed logs to view
	public int maxAllowedLog = 1000 ;
	
	//fram rate per seconds
	int fps ;
	
	//show fps even if in game logs is hidden
	bool alwaysShowFps;
	
	//used to check if you have In Game Logs multiple time in different scene
	//only one should work and other should be deleted
	static bool created = false ;
	
	
	// gui
	Texture2D 	logIcon ;
	Texture2D 	warningIcon ;
	Texture2D 	errorIcon ;
	GUIContent 	logContent ;
	GUIContent 	warningContent ;
	GUIContent 	errorContent ;
	GUIStyle   	barStyle ;
	GUIStyle   	buttonActiveStyle ;
	
	GUIStyle   	backStyle ;
	GUIStyle   	evenLogStyle ;
	GUIStyle   	oddLogStyle ;
	GUIStyle   	logButtonStyle ;
	GUIStyle   	selectedLogStyle ;
	GUIStyle   	stackLabelStyle ;
	GUIStyle   	scrollerStyle ;
	GUISkin 	inGameLogsScrollerSkin ;
	void Awake()
	{
		//initialize gui and styles for gui porpose
		logIcon 	= getImage("log_icon" ,32,32);
		warningIcon = getImage("warning_icon" ,32,32);
		errorIcon 	= getImage("error_icon" ,32,32);
		logContent 	= new GUIContent("",logIcon,"show or hide logs");
		warningContent = new GUIContent("",warningIcon,"show or hide warnings");
		errorContent = new GUIContent("",errorIcon,"show or hide errors");
		barStyle = new GUIStyle();
		barStyle.border = new RectOffset(1,1,1,1);
		barStyle.normal.background = Resources.Load("bar",typeof(Texture2D)) as Texture2D ;
		barStyle.active.background = Resources.Load("button_active",typeof(Texture2D)) as Texture2D ;
		barStyle.alignment = TextAnchor.MiddleCenter ;
		barStyle.margin = new RectOffset(1,1,1,1);
		barStyle.padding = new RectOffset(4,4,4,4);
		barStyle.wordWrap = true ;
		barStyle.clipping = TextClipping.Clip;
		
		buttonActiveStyle = new GUIStyle();
		buttonActiveStyle.border = new RectOffset(1,1,1,1);
		buttonActiveStyle.normal.background = getImage("button_active" ,32,32);
		buttonActiveStyle.alignment = TextAnchor.MiddleCenter ;
		buttonActiveStyle.margin = new RectOffset(1,1,1,1);
		buttonActiveStyle.padding = new RectOffset(4,4,4,4);
		
		backStyle = new GUIStyle();
		backStyle.normal.background = getImage("even_log" ,16,16);
		
		evenLogStyle = new GUIStyle();
		evenLogStyle.wordWrap = true;
		
		oddLogStyle = new GUIStyle();
		oddLogStyle.normal.background = getImage("odd_log" ,16,16);
		oddLogStyle.wordWrap = true ;
		
		logButtonStyle = new GUIStyle();
		logButtonStyle.wordWrap = true;
		
		selectedLogStyle = new GUIStyle();
		selectedLogStyle.normal.background = getImage("selected" ,16,16);
		selectedLogStyle.normal.textColor = Color.white ;
		selectedLogStyle.wordWrap = true;
		
		stackLabelStyle = new GUIStyle();
		stackLabelStyle.wordWrap = true ;
		
		scrollerStyle = new GUIStyle(); 
		scrollerStyle.normal.background =  getImage("bar" ,32,32);
		
		inGameLogsScrollerSkin = Resources.Load("InGameLogsScrollerSkin",typeof(GUISkin)) as GUISkin ;
		if( !created )
		{
			DontDestroyOnLoad( gameObject );
			Application.RegisterLogCallback (new Application.LogCallback (CaptureLog));
			created = true ;
		}
		else 
		{
			Debug.LogWarning("tow manager is exists delete the second");
			DestroyImmediate( gameObject );
		}
	}
	
	void OnEnable()
	{
		if( logs.Count == 0 )//if recompile while in play mode
			clear(); 
	}
	void OnDisable()
	{ 
		
	}
	
	void Start () {
		
		//load user config 
		show 		= (PlayerPrefs.GetInt( "InGameLogs_show" )==1)?true:false;
		collapse 	= (PlayerPrefs.GetInt( "InGameLogs_collapse" )==1)?true:false;
		clearOnNewSceneLoaded = (PlayerPrefs.GetInt( "InGameLogs_clearOnNewSceneLoaded" )==1)?true:false;
		showLog 	= (PlayerPrefs.GetInt( "InGameLogs_showLog" ,1) ==1)?true:false;
		showWarning = (PlayerPrefs.GetInt( "InGameLogs_showWarning" ,1) ==1)?true:false;
		showError 	= (PlayerPrefs.GetInt( "InGameLogs_showError" ,1) ==1)?true:false;
		alwaysShowFps = (PlayerPrefs.GetInt( "InGameLogs_alwaysShowFps" ) ==1)?true:false;
	}
	
	//clear all logs
	void clear()
	{
		logs.Clear();
		collapsedLogs.Clear();
		currentLog.Clear();
		logsDic.Clear();
		selectedIndex = -1;
		numOfLogs = 0;
		numOfLogsWarning = 0;
		numOfLogsError = 0;
		numOfCollapsedLogs = 0;
		numOfCollapsedLogsWarning = 0;
		numOfCollapsedLogsError = 0;
	}
	
	Rect	logsRect  ;
	Rect 	stackRect  ;
	Vector2 scrollPosition;
	Vector2 scrollPosition2;
	int 	selectedIndex = -1;
	Log     selectedLog ;
	float 	oldDrag ;
	float 	oldDrag2 ;
	int 	startIndex;
	
	//try to make texture GUI type 
	Texture2D getImage(string path , int width , int height )
	{
		Texture2D texture =  ( Texture2D  )Resources.Load( path , typeof(Texture2D  ));
		return texture ;
		/*Texture2D guiTexture = new Texture2D( width, height, TextureFormat.ATC_RGBA8, false);
		byte[] data = texture.EncodeToPNG();
		guiTexture.LoadImage( data );
		return guiTexture;*/
	}
	
	
	//calculate what is the currentLog : collapsed or not , hide or view warnings ......
	void calculateCurrentLog()
	{
		currentLog.Clear();
		if( collapse )
		{
			for( int i = 0 ; i < collapsedLogs.Count ; i++ )
			{
				Log log = collapsedLogs[i];
				if( log.logType == LogType.Log && !showLog )
					continue;
				if( log.logType == LogType.Warning && !showWarning )
					continue;
				if( log.logType == LogType.Error && !showError )
					continue;
				if( log.logType == LogType.Exception && !showError )
					continue;
				
				currentLog.Add( log );
			}
		}
		else 
		{
			for( int i = 0 ; i < logs.Count ; i++ )
			{
				Log log = logs[i];
				if( log.logType == LogType.Log && !showLog )
					continue;
				if( log.logType == LogType.Warning && !showWarning )
					continue;
				if( log.logType == LogType.Error && !showError )
					continue;
				if( log.logType == LogType.Exception && !showError )
					continue;
				
				currentLog.Add( log );
			}
		}
	}
	
	void OnGUI()
	{
		if( !show )
		{
			if( alwaysShowFps )
			{
				GUILayout.Label( "fps = " + fps );
			}
			
			return ;
		}
		logsRect.x = 0f ;
		logsRect.y = 0f ;
		logsRect.width = Screen.width ;
		logsRect.height = Screen.height * 0.75f ;
		
		stackRect.x = 0f ;
		stackRect.y = Screen.height * 0.75f ;
		stackRect.width = Screen.width ;
		stackRect.height = Screen.height * 0.25f ;
		
		GUI.skin = inGameLogsScrollerSkin ;
		GUILayout.Space(10f);
		
		GUILayout.BeginArea( logsRect , backStyle );
		GUILayout.BeginHorizontal( barStyle );
		
		if( GUILayout.Button( "Clear" , barStyle , GUILayout.Height(50)))
		{
			clear();
		}
		if( GUILayout.Button( "Collapse" , (collapse)?buttonActiveStyle:barStyle, GUILayout.Height(50)))
		{
			collapse = !collapse ;
			calculateCurrentLog();
		}
		if( GUILayout.Button( "Clear On New\nScene Loaded" , (clearOnNewSceneLoaded)?buttonActiveStyle:barStyle, GUILayout.Height(50)))
		{
			clearOnNewSceneLoaded = !clearOnNewSceneLoaded ;
		}
		if( GUILayout.Button( "Always\nShow Fps" , (alwaysShowFps)?buttonActiveStyle:barStyle,GUILayout.Height(50)))
		{
			alwaysShowFps = !alwaysShowFps ;
		}
		
		GUILayout.FlexibleSpace();
		
		GUILayout.Label(  fps.ToString() ,barStyle );
		string logsText = " " ;
		if( collapse ){
			if( numOfCollapsedLogs>=maxAllowedLog ) logsText+= "+";
			logsText+= numOfCollapsedLogs ;
		}
		else {
			if( numOfLogs>=maxAllowedLog ) logsText+= "+";
			logsText+= numOfLogs ;
		}
		string logsWarningText = " " ;
		if( collapse ){
			if( numOfCollapsedLogsWarning>=maxAllowedLog ) logsWarningText+= "+";
			logsWarningText+= numOfCollapsedLogsWarning ;
		}
		else {
			if( numOfLogsWarning>=maxAllowedLog ) logsWarningText+= "+";
			logsWarningText+= numOfLogsWarning ;
		}
		string logsErrorText = " " ;
		if( collapse ){
			if( numOfCollapsedLogsError>=maxAllowedLog ) logsErrorText+= "+";
			logsErrorText+= numOfCollapsedLogsError ;
		}
		else {
			if( numOfLogsError>=maxAllowedLog ) logsErrorText+= "+";
			logsErrorText+= numOfLogsError ;
		}
		logContent.text = logsText ;
		if( GUILayout.Button( logContent ,(showLog)?buttonActiveStyle:barStyle, GUILayout.Height(50)))
		{
			showLog = !showLog ;
			calculateCurrentLog();
		}
		warningContent.text = logsWarningText ;
		if( GUILayout.Button( warningContent,(showWarning)?buttonActiveStyle:barStyle, GUILayout.Height(50)))
		{
			showWarning = !showWarning ;
			calculateCurrentLog();
		}
		errorContent.text = logsErrorText ;
		if( GUILayout.Button( errorContent ,(showError)?buttonActiveStyle:barStyle, GUILayout.Height(50)))
		{
			showError = !showError ;
			calculateCurrentLog();
		}
		
		
		if( GUILayout.Button( "  Hide  " ,barStyle, GUILayout.Height(50)))
		{
			show = false;
		}
		
		
		GUILayout.EndHorizontal();
		setStartPos();
		float drag = getDrag(); 
		
		if( drag != 0 && logsRect.Contains( new Vector2(startPos.x , Screen.height- startPos.y) ) )
		{
			scrollPosition.y += (drag - oldDrag) ;
		}
		scrollPosition = GUILayout.BeginScrollView( scrollPosition   );
	
		oldDrag = drag ;
		
		
		int totalVisibleCount = (int)(Screen.height * 0.75f / 30) ;
		int totalCount = getTotalCount() ;
		/*if( totalCount < 100 )
			inGameLogsScrollerSkin.verticalScrollbarThumb.fixedHeight = 0;
		else 
			inGameLogsScrollerSkin.verticalScrollbarThumb.fixedHeight = 64;*/
		
		totalVisibleCount = Mathf.Min( totalVisibleCount , totalCount - startIndex );
		int index = 0 ;
		int beforeHeight = startIndex*30 ;
		selectedIndex = Mathf.Clamp( selectedIndex , -1 , totalCount -1);
		if( beforeHeight > 0 )
		{
			//fill invisible gap befor scroller to make proper scroller pos
			GUILayout.BeginHorizontal(  GUILayout.Height( beforeHeight ) );
			GUILayout.Label(" ");
			GUILayout.EndHorizontal();
		}
		
		int endIndex = startIndex + totalVisibleCount ;
		endIndex = Mathf.Clamp( endIndex , 0 ,totalCount );
		for( int i = startIndex ; (startIndex + index) < endIndex ; i++ )
		{
			
			if( i >= currentLog.Count )
				break;
			Log log = currentLog[i];
			
			if( log.logType == LogType.Log && !showLog )
				continue;
			if( log.logType == LogType.Warning && !showWarning )
				continue;
			if( log.logType == LogType.Error && !showError )
				continue;
			if( log.logType == LogType.Exception && !showError )
				continue;
			
			if( index >= totalVisibleCount )
			{
				break;
			}
			
			GUIContent content = null;
			if( log.logType == LogType.Log )
				content = logContent ;
			else if( log.logType == LogType.Warning )
				content = warningContent ;
			else 
				content = errorContent ;
			content.text = log.condition ;
			
			
			
			if( selectedIndex == (startIndex + index) )
			{
				selectedLog = log ;
				GUILayout.BeginHorizontal( selectedLogStyle   );
				GUILayout.Label( content ,selectedLogStyle   );
				//GUILayout.FlexibleSpace();
				if( collapse )
					GUILayout.Label( log.count.ToString() ,barStyle ,GUILayout.Width(50));
				//else 
				//	GUILayout.Label( (startIndex + index).ToString() , barStyle ,GUILayout.Width(50));
				GUILayout.EndHorizontal();
			}
			else 
			{
				GUILayout.BeginHorizontal( ((startIndex +index)%2 == 0 )? evenLogStyle : oddLogStyle  , GUILayout.Height(30 ) );
				if( GUILayout.Button( content ,logButtonStyle ) )
				{
					selectedIndex = startIndex + index ;
				}
				//GUILayout.FlexibleSpace();
				if( collapse )
					GUILayout.Label( log.count.ToString() , barStyle ,GUILayout.Width(50));
				//else 
				//	GUILayout.Label( (startIndex+index).ToString() , barStyle ,GUILayout.Width(50));
				GUILayout.EndHorizontal();
			}
			
			
			index++;
		}
		
		int afterHeight = (totalCount - ( startIndex + totalVisibleCount ))*30 ;
		if( afterHeight > 0 )
		{
			//fill invisible gap after scroller to make proper scroller pos
			GUILayout.BeginHorizontal(  GUILayout.Height(afterHeight ) );
			GUILayout.Label(" ");
			GUILayout.EndHorizontal();
		}
		
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		GUILayout.BeginArea( stackRect , backStyle );
		if( selectedIndex != -1 )
		{
			float drag2 = getDrag(); 
			if( drag2 != 0 && stackRect.Contains( new Vector2(startPos.x , Screen.height- startPos.y) ) )
			{
				scrollPosition2.y += drag2 - oldDrag2 ;
			}
			oldDrag2 = drag2 ;
			scrollPosition2 = GUILayout.BeginScrollView( scrollPosition2  );
			
			GUILayout.BeginHorizontal( ); 
			if( selectedLog != null )
			{
				string stacktrace = ( string.IsNullOrEmpty(selectedLog.stacktrace))?"stacktrace is empty":selectedLog.stacktrace;
				GUILayout.Label(  stacktrace , stackLabelStyle );
			}
			GUILayout.EndHorizontal();
			GUILayout.EndScrollView();
		}
		GUILayout.EndArea();
		
	}
	
	List<Vector2> gestureDetector = new List<Vector2>();
	Vector2 gestureSum = Vector2.zero ;
	float gestureLength = 0;
	bool isGestureDone()
	{
		if( Application.platform == RuntimePlatform.Android || 
			Application.platform == RuntimePlatform.IPhonePlayer )
		{
			if( Input.touches.Length != 1 )
			{
				gestureDetector.Clear();
			}
			else 
			{
				if( Input.touches[0].phase == TouchPhase.Canceled || Input.touches[0].phase == TouchPhase.Ended )
					gestureDetector.Clear();
				else if( Input.touches[0].phase == TouchPhase.Moved )
				{
					Vector2 p = Input.touches[0].position;
					if( gestureDetector.Count == 0 ||  (p - gestureDetector[ gestureDetector.Count-1]).magnitude > 10 )
						gestureDetector.Add( p ) ;
				}
			}
		}
		else 
		{
			/*if( Input.GetMouseButtonUp(0) )
			{
				gestureDetector.Clear();
			}
			else 
			{
				if( Input.GetMouseButton(0))
				{
					Vector2 p = new Vector2( Input.mousePosition.x , Input.mousePosition.y );
					if( gestureDetector.Count == 0 ||  (p - gestureDetector[ gestureDetector.Count-1]).magnitude > 10 )
						gestureDetector.Add( p );
				}
			}*/
			if( Input.GetKeyUp(KeyCode.L) )
			{
				//gestureDetector.Clear();
				this.show=false;
			}
			else 
			{
				if( Input.GetKeyDown(KeyCode.L) )
				{
					this.show=true;
				}

			}
		}
		
		if( gestureDetector.Count < 10 )
			return false;
		
		gestureSum = Vector2.zero ;
		gestureLength  = 0 ;
		Vector2 prevDelta = Vector2.zero ;
		for( int i = 0 ; i < gestureDetector.Count - 2 ; i++ )
		{
			
			Vector2 delta = gestureDetector[i+1] - gestureDetector[i] ;
			float deltaLength = delta.magnitude ;
			gestureSum += delta  ; 
			gestureLength += deltaLength ; 
			
			float dot =  Vector2.Dot( delta , prevDelta ) ;
			if( dot < 0f )
			{
				gestureDetector.Clear();
				return false;
			}
			
			prevDelta = delta ;
		}
		
		int gestureBase = (Screen.width + Screen.height ) / 4 ;
		
		if( gestureLength > gestureBase && gestureSum.magnitude < gestureBase/2 )
		{
			gestureDetector.Clear();
			return true ;
		}
		
		return false ;
	}

	
	int getTotalCount()
	{
		int count = 0 ;
		if( collapse )
		{
			if( showLog ) 		count += numOfCollapsedLogs ;
			if( showWarning ) 	count += numOfCollapsedLogsWarning ;
			if( showError ) 	count += numOfCollapsedLogsError ;
		}
		else 
		{
			if( showLog ) 		count += numOfLogs ;
			if( showWarning ) 	count += numOfLogsWarning ;
			if( showError ) 	count += numOfLogsError ;
		}
		return count ;
	}
	
	//calculate  pos of first click on screen
	Vector2 startPos ;
	void setStartPos()
	{
		if( Application.platform == RuntimePlatform.Android || 
			Application.platform == RuntimePlatform.IPhonePlayer )
		{
			
			if( Input.touches.Length == 1 &&  Input.touches[0].phase == TouchPhase.Began )
			{
				startPos =  Input.touches[0].position ;
			}
		}
		else 
		{
			if( Input.GetMouseButtonDown(0) )
			{
				startPos =  new Vector2( Input.mousePosition.x , Input.mousePosition.y );
			}
		}
	}
	
	//calculate drag amount , this is used for scrolling
	float getDrag()
	{
		
		if( Application.platform == RuntimePlatform.Android || 
			Application.platform == RuntimePlatform.IPhonePlayer )
		{
			if( Input.touches.Length != 1 )
			{
				return 0 ;
			}
			return Input.touches[0].position.y - startPos.y ;
		}
		else 
		{
			if( Input.GetMouseButton(0) )
			{
				return Input.mousePosition.y - startPos.y ;
			}
			else 
			{
				return 0;
			}
		}
	}
	
	//calculate the start index of visible log
	void calculateStartIndex()
	{
		startIndex = (int)(scrollPosition.y /30)  ;
		startIndex = Mathf.Clamp( startIndex , 0 , getTotalCount() );
	}
	float elapsed ;
	int _fps = 0;
	void Update()
	{
		elapsed += Time.deltaTime ;
		_fps ++ ;
		if( elapsed >= 1f )
		{
			fps = _fps ;
			_fps = 0;
			elapsed =0f;
		}
		
		calculateStartIndex();
		if( !show && isGestureDone() )
		{
			show = true;
		}
		elapsed += Time.deltaTime ;
		if( elapsed > 1)
		{
			elapsed = 0;
			//be sure no body else take control of log 
			Application.RegisterLogCallback (new Application.LogCallback (CaptureLog));
		}
	}
	void CaptureLog (string condition, string stacktrace, LogType type)
	{
		bool newLogAdded = false ;
		Log log = null;
		bool skip = false ;
		if( logsDic.ContainsKey( condition ) )
		{
			logsDic[ condition ].count ++;
			log = logsDic[ condition ] ;
		}
		else 
		{
			
			if(type == LogType.Log )
			{
				if( numOfCollapsedLogs >= maxAllowedLog) skip = true ;
			}
			else if( type == LogType.Warning )
			{
				if( numOfCollapsedLogsWarning >= maxAllowedLog)  skip = true ;
			}
			else if( numOfCollapsedLogsError >= maxAllowedLog ) skip = true ;
			if( !skip )
			{
				if( type == LogType.Log )
					numOfCollapsedLogs++;
				else if( type == LogType.Warning )
					numOfCollapsedLogsWarning++;
				else 
					numOfCollapsedLogsError++;
				
				log = new Log(){ logType = type , condition = condition , stacktrace = stacktrace };
				collapsedLogs.Add( log );
				logsDic.Add( condition , log );
				
				
				if( collapse )
				{
					skip = false ;
					if( log.logType == LogType.Log && !showLog )
						skip = true;
					if( log.logType == LogType.Warning && !showWarning )
						skip = true;
					if( log.logType == LogType.Error && !showError )
						skip = true;
					if( log.logType == LogType.Exception && !showError )
						skip = true;
					if( !skip)
					{
						currentLog.Add( log );
						newLogAdded=true;
					}
				}
			}
		}
		
		skip = false;
		if(type == LogType.Log )
		{
			if( numOfLogs >= maxAllowedLog) skip = true ;
		}
		else if( type == LogType.Warning )
		{
			if( numOfLogsWarning >= maxAllowedLog)  skip = true ;
		}
		else if( numOfLogsError >= maxAllowedLog ) skip = true ;
		
		if( !skip )
		{
			if( type == LogType.Log )
				numOfLogs++;
			else if( type == LogType.Warning )
				numOfLogsWarning++;
			else 
				numOfLogsError++;
			
			logs.Add( log );
			if( !collapse )
			{
				skip = false ;
				if( log.logType == LogType.Log && !showLog )
					skip = true;
				if( log.logType == LogType.Warning && !showWarning )
					skip = true;
				if( log.logType == LogType.Error && !showError )
					skip = true;
				if( log.logType == LogType.Exception && !showError )
					skip = true;
				if( !skip)
				{
					currentLog.Add( log );
					newLogAdded=true;
				}
			}
		}
		
		if( newLogAdded )
		{
			calculateStartIndex();
			int totalCount = getTotalCount();
			int totalVisibleCount = (int)(Screen.height * 0.75f / 30) ;
			if( startIndex >= ( totalCount - totalVisibleCount ))
				scrollPosition.y += 30 ;
		}
	}
	
	//new scene is loaded
	void OnLevelWasLoaded()
	{
		if( clearOnNewSceneLoaded )
			clear();
	}
	
	//save user config
	void OnApplicationQuit()
	{
		PlayerPrefs.SetInt( "InGameLogs_show" ,(show==true)?1:0);
		PlayerPrefs.SetInt( "InGameLogs_collapse" ,(collapse==true)?1:0);
		PlayerPrefs.SetInt( "InGameLogs_clearOnNewSceneLoaded" ,(clearOnNewSceneLoaded==true)?1:0);
		PlayerPrefs.SetInt( "InGameLogs_showLog" ,(showLog==true)?1:0) ;
		PlayerPrefs.SetInt( "InGameLogs_showWarning" ,(showWarning==true)?1:0) ;
		PlayerPrefs.SetInt( "InGameLogs_showError" ,(showError==true)?1:0) ;
		PlayerPrefs.SetInt( "InGameLogs_alwaysShowFps" ,(alwaysShowFps==true)?1:0) ;
		
		PlayerPrefs.Save();
	}
}
