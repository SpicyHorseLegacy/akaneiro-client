using UnityEngine;
using System.Collections;

public class BattleInfoPanel : MonoBehaviour {

    static public BattleInfoPanel Instance = null;

    bool bShowBattleInfo = false;

    public GUIStyle style;

    Rect windowRect = new Rect(Screen.width - 420, 20, 400, 400);
    string BattleInfo = "Welcome To BattleInfo Inspector";
    Vector2 scrollPosition = new Vector2(0, 11170);

    void Awake()
    {
        Instance = this;
    }

    void OnGUI()
    {
        if (Player.Instance)
        {
            if (bShowBattleInfo)
            {
                windowRect = GUI.Window(0, windowRect, ShowBattleInfo, "Battle Info");

                string attr = "";

                for (int i= 0; i< EAttributeType.ATTR_Max; i++)
                {
                    attr += new EAttributeType(i).GetString() + " : "+ Player.Instance.AttrMan.Attrs[i] + "\n";
                }

                GUI.Box(new Rect(Screen.width - 300, Screen.height - 500, 300, 250), attr);
            }
        }
    }

    void ShowBattleInfo(int windowID)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 20));

        scrollPosition = GUI.BeginScrollView(new Rect(10, 20, 380, 370), scrollPosition, new Rect(0, 0, 360, 11170), false, true);
        GUI.Box(new Rect(0, 0, 365, 11170), BattleInfo, style);
        GUI.EndScrollView();

        if (GUI.Button(new Rect(0, 20, 100, 50), "Clean"))
        {
            BattleInfo = "";
        }
    }

    int index = 0;
    public void ADD_Info(string info)
    {
        index ++;
        BattleInfo += "\n[" + index + "]" + info;
    }

    public void NotifyDebugInfo()
    {
        bShowBattleInfo = !bShowBattleInfo;
    }
}
