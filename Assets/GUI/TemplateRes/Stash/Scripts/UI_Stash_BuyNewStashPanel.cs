using UnityEngine;
using System.Collections;

public class UI_Stash_BuyNewStashPanel : MonoBehaviour {

    [SerializeField] GameObject PriceAndBTNGroup;
    [SerializeField] GameObject BTN_Ok;
    [SerializeField] UILabel Label_Title;
    [SerializeField] UILabel Label_Process;
    [SerializeField] UILabel Label_Price;

    public void Init(int _price)
    {
        PriceAndBTNGroup.SetActive(true);
        BTN_Ok.SetActive(false);
        Label_Title.text = "New Tab";
        Label_Process.text = LocalizeManage.Instance.GetDynamicText("ADDSLOTS");

        Label_Price.text = "" + _price;
    }

    void BTN_Buy_Clicked()
    {
        StashManager.Instance._CreateTabDelegate();

        PriceAndBTNGroup.SetActive(false);
        Label_Process.gameObject.SetActive(true);

        Label_Process.text = "Purchasing...";
        Label_Process.color = Color.white;
    }

    public void BuyStashSuccess()
    {
        BTN_Ok.SetActive(true);
        Label_Title.text = "Congratulations!";
        Label_Process.text = "You got one more stash box!!";
        Label_Process.color = Color.green;
    }

    public void BuyStashFailed()
    {
        StashManager.Instance._ClosePopUpDelegate();
    }
}
