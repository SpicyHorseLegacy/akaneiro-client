using UnityEngine;
using System.Collections;

public class UI_Stash_TabsManager : MonoBehaviour {

    [SerializeField] UI_Stash_Tab_Control[] Tabs = new UI_Stash_Tab_Control[0];
    [SerializeField] GameObject[] Tabs_BG = new GameObject[0];
    [SerializeField] GameObject Tab_Cover;

    void Awake()
    {
        for (int i = 0; i < Tabs.Length; i++)
        {
            Tabs[i].Init(this, i);
        }
    }

    public void UpdateTabsInfo(UI_TypeDefine.UI_Stash_Tab_data _data)
    {
        int _maxidx = Tabs.Length;
        if (_maxidx > _data.MaxIDX)
            _maxidx = _data.MaxIDX;

        for (int i = 0; i < _maxidx; i++ )
        {
            if (i >= _data.BoughtIDX)
            {
                Tabs[i].Reset();
                continue; ;
            }
            bool _isOpen = false;
            if (i == _data.CurIDX)
            {
                _isOpen = true;
                Tab_Cover.transform.position = Tabs_BG[i].transform.position;
                Tab_Cover.GetComponentInChildren<UISprite>().panel.Refresh();
            }
            Tabs[i].SetBoxState(_isOpen);
            Tabs[i].transform.localScale = new Vector3(51, 55, 1) * (_isOpen ? 1.1f : 1f);
        }
    }

    public void TabBeClicked(int _i)
    {
        StashManager.Instance.StashTabDelegate(_i);
    }
}
