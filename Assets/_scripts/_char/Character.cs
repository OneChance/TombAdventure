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
	public int stamina;
	public int maxStamina;
	public Pro Pro;
	public int level;
	public int exp;
	public int nextLevelExp;
	public int strength;
	public int archeology;
	public int def;
	public int strengthBase;
	public int archeologyBase;
	public int defBase;
	public int dodgeBase;
	private int strengthAdd;
	private int archeologyAdd;
	private int defAdd;
	private int dodgeAdd;
	private static int LEVELEXPADD = 50;
	private string charInfo;
	public int digPower;
	public string mnote;
	public int dbid = 0;
	public int iid = 0;

	public Character ()
	{

	}
	
	public Character (int money, int health, int maxHealth, int strength,int archeology, int def, int dodge, string objName, bool isOnLinePlayer, int stamina, int  maxStamina, Pro pro, int level, int exp, List<Equipment> eList, int logId)
	{
		this.money = money;
		this.Health = health;
		this.MaxHealth = maxHealth;
		this.strength = strength;
		this.archeology = archeology;
		this.def = def;
		this.dodge = dodge;
		this.ObjName = objName;
		this.PrefabName = "_images/_game/" + pro.prefabname;
		this.isOnLinePlayer = isOnLinePlayer;
		this.stamina = stamina;
		this.maxStamina = maxStamina;
		this.Pro = pro;
		this.level = level;
		this.exp = exp;
		this.nextLevelExp = level * LEVELEXPADD;
		this.logId = logId;
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
		}
	}

	public int StrengthAdd {
		get {
			return this.strengthAdd;
		}
		set {
			strengthAdd = value;
			strength = strengthBase + strengthAdd;
		}
	}

	public int ArcheologyAdd {
		get {
			return this.archeologyAdd;
		}
		set {
			archeologyAdd = value;
			archeology = archeologyBase + archeologyAdd;
		}
	}

	public int DefAdd {
		get {
			return this.defAdd;
		}
		set {
			defAdd = value;
			def = defBase + defAdd;
		}
	}

	public int DodgeAdd {
		get {
			return this.dodgeAdd;
		}
		set {
			dodgeAdd = value;
			dodge = dodgeBase + dodgeAdd;
		}
	}

	public string CharInfo {
		get {

			this.charInfo = 
				StringCollection.NAME + ":" + this.ObjName + "\n" + 
				StringCollection.PRO + ":" + this.Pro.proname + "[" + this.level + "]" + "\n" + 
				StringCollection.EXP + ":" + this.exp + "/" + this.nextLevelExp + "\n" + 
				StringCollection.HEALTH + ":" + this.Health + "/" + this.MaxHealth + "\n" + 
				StringCollection.STAMINA + ":" + this.stamina + "/" + this.maxStamina + "\n" + 
				StringCollection.STRENGTH + ":" + this.strength + "\n" + 
				StringCollection.ARCHEOLOGY + ":" + this.archeology + "\n" + 
				StringCollection.ATTACK + ":" + this.Attack + "\n" + 
				StringCollection.DEF + ":" + this.def + "\n" + 
				StringCollection.DODGE + ":" + this.dodge + "\n\n" + mnote;


			return this.charInfo;
		}
		set {
			charInfo = value;
		}
	}

}
