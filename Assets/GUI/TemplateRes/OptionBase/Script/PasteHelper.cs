using UnityEngine;

using System.Collections;

public class PasteHelper : MonoBehaviour
{
    TextEditor mTe = null;

    public delegate void OnPaste(string pastedString);
    public static event OnPaste Paste;

    void Awake()
    {
        mTe = new TextEditor();
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.control)
        {
            if (e.keyCode == KeyCode.V)
            {
                // Paste
                mTe.content.text = "";
                mTe.Paste();

                if (Paste != null)
                    Paste(mTe.content.text);
            }
        }
    }
}