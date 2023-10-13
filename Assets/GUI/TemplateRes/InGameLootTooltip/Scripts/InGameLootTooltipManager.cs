using UnityEngine;
using System;
using System.Collections.Generic;
using System.Timers;

#region Extensions...
public static class UnityFix
{
    public static bool Intersect(this Rect rectA, Rect rectB)
    {
        // Source code extracted from Systen.Drawing.dll
        return ((((rectB.x < (rectA.x + rectA.width)) && (rectA.x < (rectB.x + rectB.width))) && (rectB.y < (rectA.y + rectA.height))) && (rectA.y < (rectB.y + rectB.height)));
    }

    public static Rect[] SplitInFourRects(this Rect rect)
    {
        List<Rect> rects = new List<Rect>(4);
        rects.Add(new Rect(rect.x, rect.y, rect.width / 2f, rect.height / 2f));
        rects.Add(new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height / 2f));
        rects.Add(new Rect(rect.x + rect.width / 2f, rect.y + rect.height / 2f, rect.width / 2f, rect.height / 2f));
        rects.Add(new Rect(rect.x, rect.y + rect.height / 2f, rect.width / 2f, rect.height / 2f));

        return rects.ToArray();
    }
}
#endregion

public class InGameLootTooltipManager : MonoBehaviour
{
    public static List<InGameLootTooltipManager> _lootsTooltipList = new List<InGameLootTooltipManager>();

    public static InGameLootTooltipManager LastInstance
    {
        get { return (_lootsTooltipList != null && _lootsTooltipList.Count > 0) ? _lootsTooltipList[_lootsTooltipList.Count - 1] : null; }
    }

    private enum Direction
    {
        NONE = 0x000,
        NORTH_WEST = 0x001,
        NORTH_EAST = 0x002,
        SOUTH_EAST = 0x004,
        SOUTH_WEST = 0x008,
        WEST = 0x009,
        NORTH = 0x003,
        EAST = 0x006,
        SOUTH = 0x00c,
        RANDOM = 0x00f
    }

    private const int m_defaultWidthScreen = 1280;
    private const int m_defaultHeightScreen = 720;

    public UISprite m_background;
    public UILabel m_label;

    public float m_step = 5f;

    private Rect m_screenRectangle;

    private Vector3 m_offset = Vector3.zero;

    private WeakReference m_lootTransform;

    private bool m_isQuitting = false;

    public Rect ScreenRectangle
    {
        get { return m_screenRectangle; }
    }

    public Vector2 ScreenPosition
    {
        get { return new Vector2(m_screenRectangle.x, m_screenRectangle.y); }
    }

    public Vector2 ScreenSize
    {
        get { return new Vector2(m_screenRectangle.width, m_screenRectangle.height); }
    }

    public bool IsAlive
    {
        get { return !(m_lootTransform == null || (m_lootTransform.Target as Transform) == null); }
    }

    void Start()
    {
        _lootsTooltipList.Add(this);
        this.GetComponent<UITemplate>().name += "_" + _lootsTooltipList.Count;
        this.GetComponent<UITemplate>().templateName += "_" + _lootsTooltipList.Count;

        if (this.transform.parent != null)
            this.transform.parent = this.transform.parent.parent;

        if (GameConfig.IsAutoFadeLootNames)
        {
            AutoFade();
        }
    }

    void LateUpdate()
    {
		Profiler.BeginSample("LateUpdate(): Organize/PickedUp");
        if (m_lootTransform != null && m_lootTransform.Target != null) {
            Organize();
		} else if (m_lootTransform != null && m_lootTransform.Target == null) {
            PickedUp();
		}
		Profiler.EndSample();
		
		Profiler.BeginSample("LateUpdate(): Fade");
        if (GameConfig.IsAutoFadeLootNames) {
            AutoFade();
		} else {
            CancelFade();
		}
		Profiler.EndSample();
    }

    void OnApplicationQuit()
    {
        m_isQuitting = true;
    }

    void OnDestroy()
    {
        if (!m_isQuitting && m_lootTransform != null)
        {
            GameObject clone = Instantiate(this.gameObject, this.transform.position, this.transform.rotation) as GameObject;

            clone.transform.parent = this.transform.parent;
            clone.transform.localScale = Vector3.one;

            clone.GetComponent<UITemplate>().enabled = true;

            InGameLootTooltipManager tooltipClone = clone.GetComponent<InGameLootTooltipManager>();
            tooltipClone.m_lootTransform = new WeakReference(this.m_lootTransform.Target);
            tooltipClone.enabled = true;
            tooltipClone.m_background.enabled = true;
            tooltipClone.m_label.enabled = true;

            _lootsTooltipList.Remove(this);
            _lootsTooltipList.Add(tooltipClone);

            clone.SetActive(true);
        }
    }

    #region Fader...
    private Timer m_autoFader;
    private bool m_fade = false;
    private float m_fadeSpeed = 0.75f;

    public void AutoFade()
    {
        m_autoFader = new Timer(7500);
        m_autoFader.Elapsed += new ElapsedEventHandler(m_autoFader_Elapsed);
        m_autoFader.Start();
    }

    private void m_autoFader_Elapsed(object sender, ElapsedEventArgs e)
    {
        Fade();
        m_autoFader.Stop();
        m_autoFader = null;
    }

    public void CancelFade()
    {
        if (m_autoFader != null)
            m_autoFader.Stop();

        m_fade = false;

        m_background.color = new Color(m_background.color.r, m_background.color.g, m_background.color.b, 0.78f); // Reset alpha initial value 200
        m_label.color = new Color(m_label.color.r, m_label.color.g, m_label.color.b, 1f); // Reset alpha initial value 255
    }

    public void Fade()
    {
        m_fade = true;
    }
    #endregion

    public void Dropped(string lootName, Color lootColor, Transform lootTransform)
    {
        // Set loot name
        m_label.text = lootName;
        m_label.color = lootColor;

        // Resize background to actual size of the loot name
        m_background.transform.localScale = new Vector3(m_label.relativeSize.x * m_label.transform.localScale.x + 10, m_background.transform.localScale.y, m_background.transform.localScale.z);

        // Store item transform to update position of the tooltip after moved the character
        m_lootTransform = new WeakReference(lootTransform);

        // Init screen position of the tooltip
        Organize();
    }

    public void PickedUp()
    {
        _lootsTooltipList.Remove(this);
        m_lootTransform = null;

        GameObject.Destroy(this.gameObject);
    }

    public void Organize()
    {
        /* Don't display if we are not in game */
        if (GUIManager.IsInUIState("IngameScreen") == false)
        {
            Hide();
            return;
        }
        
        /* Look for intersecting others loot tips, to avoid overlapping */
        List<InGameLootTooltipManager> loots = new List<InGameLootTooltipManager>(_lootsTooltipList);
        bool noCollisionAtAll = true;
        foreach (InGameLootTooltipManager loot in loots)
            if (loot != this && loot.ScreenRectangle.Intersect(this.ScreenRectangle) && loot != null)
                loot.GiveRoom(this);

        if (noCollisionAtAll)
            Show();
        else
            Hide();
    }

    private void Show()
    {
        if (m_background.gameObject.activeSelf == false)
        {
            m_background.gameObject.SetActive(true);
            m_label.gameObject.SetActive(true);
        }

        Transform lootTransform = m_lootTransform.Target as Transform;

        // Loot picked up
        if (lootTransform == null)
        {
            Hide();
            PickedUp();
            return;
        }
        // Loot too far to be seen
        else if (Vector3.Distance(lootTransform.position, Player.Instance.transform.position) > 10f) 
        {
            Hide();
            return;
        }

        // 8 points of the loot BB in 3D.
        Vector3 p1 = lootTransform.gameObject.renderer.bounds.min;
        Vector3 p2 = lootTransform.gameObject.renderer.bounds.max;
        Vector3 p3 = new Vector3(p1.x, p2.y, p2.z);
        Vector3 p4 = new Vector3(p1.x, p1.y, p2.z);
        Vector3 p5 = new Vector3(p2.x, p1.y, p1.z);
        Vector3 p6 = new Vector3(p2.x, p2.y, p1.z);
        Vector3 p7 = new Vector3(p1.x, p2.y, p1.z);
        Vector3 p8 = new Vector3(p2.x, p1.y, p2.z);

        // 8 points of the loot BB in the screen space.
        p1 = Camera.main.WorldToScreenPoint(p1);
        p2 = Camera.main.WorldToScreenPoint(p2);
        p3 = Camera.main.WorldToScreenPoint(p3);
        p4 = Camera.main.WorldToScreenPoint(p4);
        p5 = Camera.main.WorldToScreenPoint(p5);
        p6 = Camera.main.WorldToScreenPoint(p6);
        p7 = Camera.main.WorldToScreenPoint(p7);
        p8 = Camera.main.WorldToScreenPoint(p8);

        // Calculate the BB of the loot BB in screen space.
        Vector3 min = Vector3.Min(p1, Vector3.Min(p2, Vector3.Min(p3, Vector3.Min(p4, Vector3.Min(p5, Vector3.Min(p6, Vector3.Min(p7, p8)))))));
        Vector3 max = Vector3.Max(p1, Vector3.Max(p2, Vector3.Max(p3, Vector3.Max(p4, Vector3.Max(p5, Vector3.Max(p6, Vector3.Max(p7, p8)))))));

        // Update display rectangle
        m_screenRectangle = new Rect((min.x + (max.x - min.x) / 2f) - Screen.width / 2f, ((Screen.height - max.y - 20) - Screen.height / 2f) * -1, m_background.transform.localScale.x, m_background.transform.localScale.y);
        this.transform.localPosition = new Vector3(m_screenRectangle.x * (m_defaultWidthScreen / Mathf.Round(Camera.main.pixelWidth)) + m_offset.x, m_screenRectangle.y * (m_defaultHeightScreen / Mathf.Round(Camera.main.pixelHeight)) + m_offset.y, this.transform.localPosition.z);

        if (m_fade)
        {
            m_background.color = new Color(m_background.color.r, m_background.color.g, m_background.color.b, m_background.color.a - m_fadeSpeed * Time.deltaTime);
            m_label.color = new Color(m_label.color.r, m_label.color.g, m_label.color.b, m_label.color.a - m_fadeSpeed * Time.deltaTime);
        }
    }

    private void Hide()
    {
        if (m_background.gameObject.activeSelf == true)
        {
            m_background.gameObject.SetActive(false);
            m_label.gameObject.SetActive(false);
        }
    }

    private void GiveRoom(InGameLootTooltipManager to)
    {
        Rect[] subRects = to.ScreenRectangle.SplitInFourRects();
        Direction calculatedDirection = Direction.NONE;
        int index = 0;

        // Caculating direction to go to avoid overlap
        foreach (Rect subRect in subRects)
        {
            if (this.ScreenRectangle.Intersect(subRect))
            {
                calculatedDirection = calculatedDirection | ((Direction)(Mathf.Pow(2, index)));
            }
            index++;
        }

        // Move
        switch (calculatedDirection)
        {
            case Direction.NORTH_WEST:
                m_offset = new Vector3(-m_step, -m_step, 0);
                break;
            case Direction.NORTH_EAST:
                m_offset = new Vector3(m_step, -m_step, 0);
                break;
            case Direction.SOUTH_EAST:
                m_offset = new Vector3(m_step, m_step, 0);
                break;
            case Direction.SOUTH_WEST:
                m_offset = new Vector3(-m_step, m_step, 0);
                break;
            case Direction.WEST:
                m_offset = new Vector3(-m_step, 0, 0);
                break;
            case Direction.EAST:
                m_offset = new Vector3(m_step, 0, 0);
                break;
            case Direction.SOUTH:
                m_offset = new Vector3(0, m_step, 0);
                break;
            case Direction.NORTH:
            case Direction.RANDOM:
            default:
                m_offset = new Vector3(0, -m_step, 0);
                break;
        }
    }
}
