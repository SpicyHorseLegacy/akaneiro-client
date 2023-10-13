using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollViewManager : MonoBehaviour
{
    public enum Direction
    {
        ADD = 0,
        SUBTRACT = 1,
    }

    void Start()
    {
        InitScrollView();
    }

    void Update()
    {
        Move();
    }

    #region interface
    [SerializeField]
    private UIGrid itemsRoot;
    private Transform curRoot;
    public void UpdateScrollView()
    {
        if (curRoot != null)
            curRoot.GetComponent<UIGrid>().Reposition();
    }

    [SerializeField]
    private Transform itemsRootParent;

    public List<Transform> GetItemList()
    {
        return itemList;
    }
    #endregion

    #region local

    public void InitScrollView()
    {
        SetShowArrow(isShowArrow);
        SetShowScrollBar(isShowScrollBar);
        //CleanList();
        UpdateScrollView();
    }

    [SerializeField]
    private float speed;
    private bool isMove = false;
    public void Move()
    {
        if (isMove)
        {
            switch (direction)
            {
                case Direction.SUBTRACT:
                    scrollBar.scrollValue -= Time.deltaTime * speed;
                    if (scrollBar.scrollValue < 0f)
                    {
                        scrollBar.scrollValue = 0f;
                    }
                    break;
                case Direction.ADD:
                    scrollBar.scrollValue += Time.deltaTime * speed;
                    if (scrollBar.scrollValue > 1f)
                    {
                        scrollBar.scrollValue = 1f;
                    }
                    break;
            }

        }

    }

    [SerializeField]
    private bool isShowArrow;
    [SerializeField]
    private NGUIButton addButton;
    [SerializeField]
    private NGUIButton subtractButton;

    private void SetShowArrow(bool isShow)
    {
        if (addButton == null || subtractButton == null)
        {
            return;
        }
        NGUITools.SetActive(addButton.gameObject, isShow);
        NGUITools.SetActive(subtractButton.gameObject, isShow);
    }

    [SerializeField]
    private bool isShowScrollBar;
    [SerializeField]
    private UIScrollBar scrollBar;

    private void SetShowScrollBar(bool isShow)
    {
        if (scrollBar == null)
            return;

        NGUITools.SetActive(scrollBar.gameObject, isShow);
    }

    private Direction direction;
    private void OnPressAddArrowDelegate()
    {
        /* TODO : Slide materials list
         * 
        direction = Direction.ADD;
        isMove = true;*/
    }

    private void OnPressSubtractArrowDelegate()
    {
        /* TODO : Slide materials list
         * 
        direction = Direction.SUBTRACT;
        isMove = true;*/
    }

    private void OnReleaseAddArrowDelegate()
    {
        isMove = false;
    }

    private void OnReleaseSubtractArrowDelegate()
    {
        isMove = false;
    }

    private void CreateItemRoot()
    {
        Debug.Log("CreateItemRoot");
        GameObject obj = (GameObject)Instantiate(itemsRoot.gameObject);
        obj.transform.parent = itemsRootParent;
        obj.transform.localPosition = new Vector3(10f, 0f, 0f);
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<UIGrid>().cellWidth = 52f;
        obj.GetComponent<UIGrid>().cellHeight = 50f;
        curRoot = obj.transform;
    }

    private List<Transform> itemList = new List<Transform>();

    public void CleanList()
    {
        if (curRoot != null)
            Destroy(curRoot.gameObject);

        itemList.Clear();

        if (curRoot == null)
            CreateItemRoot();
    }
    public void AddElement(Transform obj)
    {
        if (curRoot == null)
            CreateItemRoot();

        obj.parent = curRoot.transform;
        obj.localPosition = Vector3.zero;
        obj.localScale = new Vector3(0.6f, 0.6f, 1f);
        itemList.Add(obj);
        UpdateScrollView();
    }

    public void DelElement(Transform obj)
    {
        itemList.Remove(obj);

        if (obj != null)
            Destroy(obj.gameObject);

        UpdateScrollView();
    }
    #endregion
}
