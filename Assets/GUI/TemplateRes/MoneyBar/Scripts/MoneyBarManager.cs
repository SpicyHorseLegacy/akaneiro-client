using UnityEngine;
using System.Collections;

public class MoneyBarManager : MonoBehaviour
{
    public static MoneyBarManager Instance;

    void Awake()
    {
        Instance = this;
    }
	//mm
	void Start (){
		SetKarmaVal(PlayerDataManager.Instance.GetKarmaVal());
		SetCrystalVal(PlayerDataManager.Instance.GetCrystalVal());	
	}
	//#mm
    #region Interface
    [SerializeField]
    private UILabel karmaVal;
    public void SetKarmaVal(int val)
    {
        karmaVal.text = val.ToString();
    }
    [SerializeField]
    private UILabel crystalVal;
    public void SetCrystalVal(int val)
    {
        crystalVal.text = val.ToString();
    }

    public delegate void Handle_AddKarmaDelegate();
    public event Handle_AddKarmaDelegate OnAddKarmaDelegate;
    private void _AddKarmaDelegate()
    {
        if (OnAddKarmaDelegate != null)
        {
            OnAddKarmaDelegate();
        }
    }

    public delegate void Handle_AddCrystalDelegate();
    public event Handle_AddCrystalDelegate OnAddCrystalDelegate;
    private void _AddCrystalDelegate()
    {
        if (OnAddCrystalDelegate != null)
        {
            OnAddCrystalDelegate();
        }
    }
    #endregion
	
	//mm
	public void DestroyMoney(){
		Destroy(gameObject);
	}
	//#mm
}
