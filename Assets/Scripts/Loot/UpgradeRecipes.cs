using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradeRecipes : MonoBehaviour
{
	public static UpgradeRecipes Instance;
	private List<UpgradeRecipe> recipes = new List<UpgradeRecipe>();
	private Dictionary<int, Dictionary<int, UpgradeRecipe>> recipeDict = new Dictionary<int, Dictionary<int, UpgradeRecipe>>();
	public enum UpgradeType{NONE, ELEM, ENCHANT, GEM, LEVELUP};
	
	public UpgradeRecipe GetRecipe(int attrID, int type)
	{
		if(!recipeDict.ContainsKey(type) || !recipeDict[type].ContainsKey(attrID))
			return null;
		else
			return recipeDict[type][attrID];
	}
	
	void Awake() 
	{
		Instance = this;
	}
	
	void InitRecipesFromFile()
	{
		string _fileName = LocalizeManage.Instance.GetLangPath("UpgradeRecipes.Recipes");
		TextAsset textAsset = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = textAsset.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i)
		{
			string[] vals = itemRowsList[i].Split(new char[] { '	', '	' });
			
			UpgradeRecipe recipe = new UpgradeRecipe();
			recipe.ID = int.Parse(vals[0]);
			recipe.type = int.Parse(vals[1]);
			recipe.attrID = int.Parse(vals[2]);
			recipe.name = vals[3];
			recipe.mat1 = int.Parse(vals[4]);
			recipe.mat1Count = int.Parse(vals[5]);
			recipe.mat2 = int.Parse(vals[6]);
			recipe.mat2Count = int.Parse(vals[7]);
			recipe.mat3 = int.Parse(vals[8]);
			recipe.mat3Count = int.Parse(vals[9]);
			recipe.mat4 = int.Parse(vals[10]);
			recipe.mat4Count = int.Parse(vals[11]);
			recipe.ksPrize = int.Parse(vals[12]);
			recipe.ksChance = int.Parse(vals[13]);
			recipe.crPrize = int.Parse(vals[14]);
			recipe.crChance = int.Parse(vals[15]);
			
			if(!recipeDict.ContainsKey(recipe.type))
				recipeDict.Add(recipe.type, new Dictionary<int, UpgradeRecipe>());
			
			if( recipeDict[recipe.type].ContainsKey(recipe.attrID) )
				Debug.LogError("Duplicated attr " + recipe.attrID + " type " + recipe.type);
			else
				recipeDict[recipe.type].Add(recipe.attrID, recipe);
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		InitRecipesFromFile();
	}
}

public class UpgradeRecipe
{
	public int ID = 0;
	public int type = 0;
	public int attrID = 0;
	public string name = "";
	public int mat1 = 0;
	public int mat1Count = 0;
	public int mat2 = 0;
	public int mat2Count = 0;
	public int mat3 = 0;
	public int mat3Count = 0;
	public int mat4 = 0;
	public int mat4Count = 0;
	public int ksPrize = 0;
	public int ksChance = 0;
	public int crPrize = 0;
	public int crChance = 0;
}
