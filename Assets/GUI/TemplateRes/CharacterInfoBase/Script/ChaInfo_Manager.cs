using UnityEngine;
using System.Collections;

public class ChaInfo_Manager : MonoBehaviour {

    public static ChaInfo_Manager Instance;

    void Awake()
    {
        Instance = this;
        GUIManager.Instance.AddTemplateInitEnd();
    }

    #region Interface

    public class ChaInfoData
    {
        public string BaseStat_POW;
        public string BaseStat_DEF;
        public string BaseStat_SKL;
        public string BaseStat_HP;
        public string BaseStat_HPRegen;
        public string BaseStat_MP;
		public string BaseStat_MPRegen;
        public string Bonuses_RDPS;
        public string Bonuses_LDPS;
        public string Bonuses_CriChance;
        public string Bonuses_CriDMG;
		public string Bonuses_AttackSpeed;
		public string Bonuses_MoveSpeed;
        public string Bonuses_DMGReduction;
        public string BOnuses_TotalArmor;
        public string DMGBonus_Fire;
        public string DMGBonus_Frost;
        public string DMGBonus_Blast;
        public string DMGBonus_Storm;
        public string Resistance_Fire;
        public string Resistance_Frost;
        public string Resistance_Blast;
        public string Resistance_Storm;
    }

    [SerializeField]  private UILabel BaseStat_POW_Value;
    [SerializeField]  private UILabel BaseStat_DEF_Value;
    [SerializeField]  private UILabel BaseStat_SKL_Value;
    [SerializeField]  private UILabel BaseStat_HP_Value;
	[SerializeField]  private UILabel BaseStat_HPRegen_Value;
    [SerializeField]  private UILabel BaseStat_MP_Value;
	[SerializeField]  private UILabel BaseStat_MPRegen_Value;
    [SerializeField]  private UILabel Bonuses_RDPS_Value;
    [SerializeField]  private UILabel Bonuses_LDPS_Value;
    [SerializeField]  private UILabel Bonuses_CriChance_Value;
    [SerializeField]  private UILabel Bonuses_CriDMG_Value;
	[SerializeField]  private UILabel Bonuese_AttackSpeed_Value;
	[SerializeField]  private UILabel Bonuses_MoveSpeed_Value;
    [SerializeField]  private UILabel Bonuses_DMGReduction_Value;
    [SerializeField]  private UILabel Bonuses_TotalAmor_Value;
    [SerializeField]  private UILabel DMGBonus_Fire_Value;
    [SerializeField]  private UILabel DMGBonus_Frost_Value;
    [SerializeField]  private UILabel DMGBonus_Blast_Value;
    [SerializeField]  private UILabel DMGBonus_Storm_Value;
    [SerializeField]  private UILabel Resistance_Fire_Value;
    [SerializeField]  private UILabel Resistance_Frost_Value;
    [SerializeField]  private UILabel Resistance_Blast_Value;
    [SerializeField]  private UILabel Resistance_Storm_Value;

    public void UpdateChaInfo(ChaInfoData _playerdata)
    {
        BaseStat_POW_Value.text = _playerdata.BaseStat_POW;
        BaseStat_DEF_Value.text = _playerdata.BaseStat_DEF;
        BaseStat_SKL_Value.text = _playerdata.BaseStat_SKL;
        BaseStat_HP_Value.text = _playerdata.BaseStat_HP;
		BaseStat_HPRegen_Value.text = _playerdata.BaseStat_HPRegen;
        BaseStat_MP_Value.text = _playerdata.BaseStat_MP;
		BaseStat_MPRegen_Value.text = _playerdata.BaseStat_MPRegen;

        Bonuses_RDPS_Value.text = _playerdata.Bonuses_RDPS;
        Bonuses_LDPS_Value.text = _playerdata.Bonuses_LDPS;
        Bonuses_CriChance_Value.text = _playerdata.Bonuses_CriChance;
        Bonuses_CriDMG_Value.text = _playerdata.Bonuses_CriDMG;
		Bonuese_AttackSpeed_Value.text = _playerdata.Bonuses_AttackSpeed;
		Bonuses_MoveSpeed_Value.text = _playerdata.Bonuses_MoveSpeed;
        Bonuses_DMGReduction_Value.text = _playerdata.Bonuses_DMGReduction;
        Bonuses_TotalAmor_Value.text = _playerdata.BOnuses_TotalArmor;

        DMGBonus_Fire_Value.text = _playerdata.DMGBonus_Fire;
        DMGBonus_Frost_Value.text = _playerdata.DMGBonus_Frost;
        DMGBonus_Blast_Value.text = _playerdata.DMGBonus_Blast;
        DMGBonus_Storm_Value.text = _playerdata.DMGBonus_Storm;

        Resistance_Fire_Value.text = _playerdata.Resistance_Fire;
        Resistance_Frost_Value.text = _playerdata.Resistance_Frost;
        Resistance_Blast_Value.text = _playerdata.Resistance_Blast;
        Resistance_Storm_Value.text = _playerdata.Resistance_Storm;
    }

    #endregion
}
