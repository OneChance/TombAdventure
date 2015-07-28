using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character:BattleObj
{
	public int money;
	private List<Baggrid> bgList;
	private List<Equipment> equipList;
	public int logId;
	private bool isOnLinePlayer;
	private int stamina;
	private int maxStamina;
	public Pro Pro;
	public int level;
	public int exp;
	public int nextLevelExp;
	public int strength;
	public int archeology;

	public int strengthBase;
	public int archeologyBase;

	private int strengthAdd;
	private int archeologyAdd;

	public Character (int money,int health,int maxHealth,int attack, int def, int dodge, string objName,bool isOnLinePlayer,int stamina,int  maxStamina,Pro pro,int level,int exp,List<Equipment> eList,int logId)
	{
		this.money = money;
		this.Health = health;
		this.MaxHealth = maxHealth;
		this.Attack = attack;
		this.Def = def;
		this.Dodge = dodge;
		this.ObjName = objName;
		this.PrefabName =  "_images/_game/"+pro.prefabname;
		this.isOnLinePlayer = isOnLinePlayer;
		this.stamina = stamina;
		this.maxStamina = maxStamina;
		this.Pro = pro;
		this.level = level;
		this.exp = exp;
		this.nextLevelExp = level * 50;
		this.logId = logId;

		//玩家的属性->基本属性
		this.strengthBase = this.level * this.Pro.strengthFactor;
		this.archeologyBase = this.level * this.Pro.archeologyFactor;

		this.strength = this.strengthBase  + this.strengthAdd;
		this.archeology = this.archeologyBase +this.archeologyAdd;

		this.EquipList = eList;
	}

	public int Stamina {
		get {
			return this.stamina;
		}
		set {
			stamina = value;
		}
	}

	public bool IsOnLinePlayer {
		get {
			return this.isOnLinePlayer;
		}
		set {
			isOnLinePlayer = value;
		}
	}

	public List<Baggrid> BgList {
		get {
			return this.bgList;
		}
		set {
			bgList = value;
		}
	}

	public List<Equipment> EquipList {
		get {
			return this.equipList;
		}
		set {
			equipList = value;
			this.StrengthAdd = 0;
			this.ArcheologyAdd = 0;
			//+装备属性
			if(equipList!=null){
				for(int i=0;i<equipList.Count;i++){
					this.StrengthAdd+=equipList[i].strength;
					this.ArcheologyAdd+=equipList[i].archeology;
				}
			}
		}
	}

	public int StrengthAdd {
		get {
			return this.strengthAdd;
		}
		set {
			strengthAdd = value;
			strength = strengthBase +strengthAdd;
		}
	}

	public int ArcheologyAdd {
		get {
			return this.archeologyAdd;
		}
		set {
			archeologyAdd = value;
			archeology = archeologyBase +archeologyAdd;
		}
	}

}
