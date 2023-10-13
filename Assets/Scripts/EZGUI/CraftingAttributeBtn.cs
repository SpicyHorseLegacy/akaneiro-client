using UnityEngine;
using System.Collections;

public class CraftingAttributeBtn : MonoBehaviour 
{
	[SerializeField]
	UIButton icon;
	[SerializeField]
	SpriteText attributeName;
	[SerializeField]
	SpriteText attributeInfo;
	[SerializeField]
	AutoSpriteControlBase myBtn;
	
	public int AttrID = 0;
	public int ItemType = 0;
	public UpgradeRecipes.UpgradeType UpgradeType;
	public int recipeTypeID = -1;
	public int IconIndex = 0;
	public string AttrDesc = "";
	public string AttrName = "";
	public int Level = 0;
	
	public void Setup(int attrID, int itemType, UpgradeRecipes.UpgradeType upgradeType)
	{
		hideAll(false);
		
		AttrID = attrID;
		ItemType = itemType;
		UpgradeType = upgradeType;
		getRecipeInfos(attrID, itemType, upgradeType);
		
		attributeName.Text = AttrName;
		attributeInfo.Text = AttrDesc;
		
		icon.SetUVs(new Rect(0,0,1,1));
		
		Debug.Log("attrID " + attrID + " itemType " + itemType + " upgradeType " + upgradeType + " IconIndex " + (IconIndex -1));
		
		switch(upgradeType) {
		case UpgradeRecipes.UpgradeType.ELEM:
			icon.SetTexture(_UI_CS_ElementsInfo.Instance.EleIcon[IconIndex-1]);
			break;
		case UpgradeRecipes.UpgradeType.ENCHANT:
			icon.SetTexture(_UI_CS_ElementsInfo.Instance.EncIcon[IconIndex-1]);
			break;
		case UpgradeRecipes.UpgradeType.GEM:
			icon.SetTexture(_UI_CS_ElementsInfo.Instance.GemIcon[IconIndex-1]);
			break;
		case UpgradeRecipes.UpgradeType.LEVELUP:
			break;
		}
	}
	
	void getRecipeInfos(int attrID, int itemType, UpgradeRecipes.UpgradeType upgradeType)
	{
		Level = attrID % 100;
		
		if(itemType == 7 || itemType == 8)
		{
			if(upgradeType == UpgradeRecipes.UpgradeType.ELEM)
			{
				foreach(var encInfo in ItemDeployInfo.Instance._SItemEleInfoWeaponAttrAttrList)
				{
					if(encInfo.id == attrID)
					{
						AttrName = encInfo.eleNameLv;
						IconIndex = encInfo.eleIconIdx;
						AttrDesc = encInfo.eleDesc1 + encInfo.eleDesc2;
					}
				}
				
				recipeTypeID = 1;
			}
			else if(upgradeType == UpgradeRecipes.UpgradeType.ENCHANT)
			{
				foreach(var encInfo in ItemDeployInfo.Instance._SItemEncInfoWeaponAttrAttrList)
				{
					if(encInfo.id == attrID)
					{
						AttrName = encInfo.encNameLv;
						IconIndex = encInfo.encIconIdx;
						AttrDesc = encInfo.encDesc1 + encInfo.encDesc2;
					}
				}
				
				recipeTypeID = 2;
			}
			else if(upgradeType == UpgradeRecipes.UpgradeType.GEM)
			{
				foreach(var encInfo in ItemDeployInfo.Instance._SItemGemInfoList)
				{
					if(encInfo.id == attrID)
					{
						AttrName = encInfo.gemeNameLv;
						IconIndex = encInfo.gemIconIdx;
						AttrDesc = encInfo.gemDesc1 + encInfo.gemDesc2;
					}
				}
				
				recipeTypeID = 7;
			}
		}
		else if(itemType == 2 || itemType == 5)
		{
			if(upgradeType == UpgradeRecipes.UpgradeType.ELEM)
			{
				foreach(var encInfo in ItemDeployInfo.Instance._SItemEleInfoAccessoriesAttrAttrList)
				{
					if(encInfo.id == attrID)
					{
						AttrName = encInfo.eleNameLv;
						IconIndex = encInfo.eleIconIdx;
						AttrDesc = encInfo.eleDesc1 + encInfo.eleDesc2;
					}
				}
				
				recipeTypeID = 5;
			}
			else if(upgradeType == UpgradeRecipes.UpgradeType.ENCHANT)
			{
				foreach(var encInfo in ItemDeployInfo.Instance._SItemEncInfoAccessoriesAttrAttrList)
				{
					if(encInfo.id == attrID)
					{
						AttrName = encInfo.encNameLv;
						IconIndex = encInfo.encIconIdx;
						AttrDesc = encInfo.encDesc1 + encInfo.encDesc2;
					}
				}
				
				recipeTypeID = 6;
			}
			else if(upgradeType == UpgradeRecipes.UpgradeType.GEM)
			{
				foreach(var encInfo in ItemDeployInfo.Instance._SItemGemInfoList)
				{
					if(encInfo.id == attrID)
					{
						AttrName = encInfo.gemeNameLv;
						IconIndex = encInfo.gemIconIdx;
						AttrDesc = encInfo.gemDesc1 + encInfo.gemDesc2;
					}
				}
				
				recipeTypeID = 7;
			}
		}
		else
		{
			if(upgradeType == UpgradeRecipes.UpgradeType.ELEM)
			{
				foreach(var encInfo in ItemDeployInfo.Instance._SItemEleInfoArmorAttrList)
				{
					if(encInfo.id == attrID)
					{
						AttrName = encInfo.eleNameLv;
						IconIndex = encInfo.eleIconIdx;
						AttrDesc = encInfo.eleDesc1 + encInfo.eleDesc2;
					}
				}
				
				recipeTypeID = 3;
			}
			else if(upgradeType == UpgradeRecipes.UpgradeType.ENCHANT)
			{
				foreach(var encInfo in ItemDeployInfo.Instance._SItemEncInfoArmorAttrList)
				{
					if(encInfo.id == attrID)
					{
						AttrName = encInfo.encNameLv;
						IconIndex = encInfo.encIconIdx;
						AttrDesc = encInfo.encDesc1 + encInfo.encDesc2;
					}
				}
				
				recipeTypeID = 4;
			}
			else if(upgradeType == UpgradeRecipes.UpgradeType.GEM)
			{
				foreach(var encInfo in ItemDeployInfo.Instance._SItemGemInfoList)
				{
					if(encInfo.id == attrID)
					{
						AttrName = encInfo.gemeNameLv;
						IconIndex = encInfo.gemIconIdx;
						AttrDesc = encInfo.gemDesc1 + encInfo.gemDesc2;
					}
				}
				
				recipeTypeID = 7;
			}
		}
		
		Debug.Log("Setup recipe " + attrID + " item " + itemType + " upgrade " + upgradeType);
	}
	
	public void Hide()
	{
		hideAll(true);
	}
	
	private void hideAll(bool hide)
	{
		icon.Hide(hide);
		myBtn.Hide(hide);
		attributeName.Hide(hide);
		attributeInfo.Hide(hide);
	}
}
