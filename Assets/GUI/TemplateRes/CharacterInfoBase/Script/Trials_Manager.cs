using UnityEngine;
using System.Collections;

public class Trials_Manager : MonoBehaviour {

    public static Trials_Manager Instance;

    void Awake()
    {
        Instance = this;
        GUIManager.Instance.AddTemplateInitEnd();
    }

    #region Interface

    public class TrialsData
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

	[SerializeField]  private UILabel Kills_All_Value;
	[SerializeField]  private UILabel Kills_Wolves_Value;
	[SerializeField]  private UILabel Kills_Imps_Value;
	[SerializeField]  private UILabel Kills_Werewolves_Value;
	[SerializeField]  private UILabel Kills_Trees_Value;
	[SerializeField]  private UILabel Kills_Bugs_Value;
	[SerializeField]  private UILabel Kills_Undead_Value;
	[SerializeField]  private UILabel Kills_Oni_Value;
	[SerializeField]  private UILabel BossKills_OniLord_Value;
	[SerializeField]  private UILabel BossKills_Moroko_Value;
	[SerializeField]  private UILabel BossKills_Gashadokuro_Value;
	[SerializeField]  private UILabel BossKills_Bloodfang_Value;
	[SerializeField]  private UILabel BossKills_WinterBanshee_Value;
	[SerializeField]  private UILabel BossKills_Algernon_Value;
	[SerializeField]  private UILabel BossKills_Futakuchi_Value;
	[SerializeField]  private UILabel BossKills_Spider_Value;
	[SerializeField]  private UILabel Trials_Attacks_Value;
	[SerializeField]  private UILabel Trials_HPLost_Value;
	[SerializeField]  private UILabel Trials_MPUsed_Value;
	[SerializeField]  private UILabel Trials_Food_Value;
	[SerializeField]  private UILabel Trials_Drinks_Value;
	[SerializeField]  private UILabel Trials_Deaths_Value;
	[SerializeField]  private UILabel Trials_Revives_Value;
	[SerializeField]  private UILabel Trials_KarmaOwned_Value;
	[SerializeField]  private UILabel Trials_KarmaSpent_Value;
	[SerializeField]  private UILabel Trials_CrystalsOwned_Value;
	[SerializeField]  private UILabel Trials_CrystalsSpent_Value;
	[SerializeField]  private UILabel Trials_AllySummoned_Value;
	[SerializeField]  private UILabel Trials_PetSummoned_Value;
	[SerializeField]  private UILabel Trials_ItemSold_Value;

    public void UpdateChaInfo(TrialsData _playerdata)
    {
		Kills_All_Value.text = _playerdata.BaseStat_POW;
		Kills_Wolves_Value.text = _playerdata.BaseStat_DEF;
		Kills_Imps_Value.text = _playerdata.BaseStat_SKL;
		Kills_Werewolves_Value.text = _playerdata.BaseStat_HP;
		Kills_Trees_Value.text = _playerdata.BaseStat_HPRegen;
		Kills_Bugs_Value.text = _playerdata.BaseStat_MP;
		Kills_Undead_Value.text = _playerdata.BaseStat_MPRegen;
		Kills_Oni_Value.text = _playerdata.BaseStat_MPRegen;
//
		BossKills_OniLord_Value.text = _playerdata.Bonuses_RDPS;
		BossKills_Moroko_Value.text = _playerdata.Bonuses_LDPS;
		BossKills_Gashadokuro_Value.text = _playerdata.Bonuses_CriChance;
		BossKills_Bloodfang_Value.text = _playerdata.Bonuses_CriDMG;
		BossKills_WinterBanshee_Value.text = _playerdata.Bonuses_AttackSpeed;
		BossKills_Algernon_Value.text = _playerdata.Bonuses_MoveSpeed;
		BossKills_Futakuchi_Value.text = _playerdata.Bonuses_DMGReduction;
		BossKills_Spider_Value.text = _playerdata.BOnuses_TotalArmor;
//
		Trials_Attacks_Value.text = _playerdata.DMGBonus_Fire;
		Trials_HPLost_Value.text = _playerdata.DMGBonus_Frost;
		Trials_MPUsed_Value.text = _playerdata.DMGBonus_Blast;
		Trials_Food_Value.text = _playerdata.DMGBonus_Storm;
		Trials_Drinks_Value.text = _playerdata.Resistance_Fire;
		Trials_Deaths_Value.text = _playerdata.Resistance_Frost;
		Trials_Revives_Value.text = _playerdata.Resistance_Blast;
		Trials_KarmaOwned_Value.text = _playerdata.Resistance_Storm;
		Trials_KarmaSpent_Value.text = _playerdata.Resistance_Fire;
		Trials_CrystalsOwned_Value.text = _playerdata.Resistance_Frost;
		Trials_CrystalsSpent_Value.text = _playerdata.Resistance_Blast;
		Trials_AllySummoned_Value.text = _playerdata.Resistance_Storm;
		Trials_PetSummoned_Value.text = _playerdata.Resistance_Blast;
		Trials_ItemSold_Value.text = _playerdata.Resistance_Storm;
    }

    #endregion
}
