using UnityEngine;

public class DetectMouseHover : MonoBehaviour
{
    [SerializeField]
    public SelectBaseManager m_parent;

    void OnHover(bool isOver)
    {
        if (isOver)
            m_parent.m_mouseOverImportantThings = true;
        else
            m_parent.m_mouseOverImportantThings = false;
    }
}
