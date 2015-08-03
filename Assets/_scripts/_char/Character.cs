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

	public Character ()
	{

	}
	
	public Character (int money, int health, int maxHealth, int def, int dodge, string objName, bool isOnLinePlayer, int stamina, int  maxStamina, Pro pro, int level, int exp, List<Equipment> eList, int logId)
	{
		this.money = money;
		this.Health = health;
		this.MaxHealth = maxHealth;
		this.Def = def;
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

		//玩家的属性->基本属性+装备属性
		AddExp (0);
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
			if (equipList != null) {
				for (int i=0; i<equipList.Count; i++) {
					this.StrengthAdd += equipList [i].strength;
					this.ArcheologyAdd += equipList [i].archeology;
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

	public int[] LevelUp (int exp, int level)
	{
		int nextExp = level * LEVELEXPADD;

		if (exp >= nextExp) {
			level++;
			return LevelUp (exp - nextExp, level);
		} else {
			return new int[2]{exp,level};
		}
	}

	public void AddExp (int addExp)
	{
		this.exp += addExp;

		int[] explevel = LevelUp (this.exp, this.level);

		this.exp = explevel [0];
		this.level = explevel [1];

		this.MaxHealth = this.level * this.Pro.healthAdd;
		this.maxStamina = this.level * this.Pro.staminaAdd;

		this.strengthBase = (int)(this.level * this.Pro.strengthFactor * (isOnLinePlayer ? 1 : 0.5f));
		this.archeologyBase = (int)(this.level * this.Pro.archeologyFactor * (isOnLinePlayer ? 1 : 0.5f));
		this.defBase = (int)(this.level * this.Pro.defFactor * (isOnLinePlayer ? 1 : 0.5f));
		this.dodgeBase = (int)((this.level * 0.2) * this.Pro.dodgeFactor * (isOnLinePlayer ? 1 : 0.5f)); //每五级升级一次躲闪
		
		this.strength = this.strengthBase + this.strengthAdd;
		this.archeology = this.archeologyBase + this.archeologyAdd;
		this.def = this.defBase + this.defAdd;
		this.dodge = this.dodgeBase + this.dodgeAdd;

	
		if (StringCollection.GEOMANCER.Equals (Pro.proname)) {
			this.Attack = strength;
			digPower = strength;
		} else if (StringCollection.SETTLER.Equals (Pro.proname)) {
			this.Attack = strength * 2;
			digPower = strength * 4;
		} else if (StringCollection.EXORCIST.Equals (Pro.proname)) {
			this.Attack = strength * 4;
			digPower = strength;
		} else if (StringCollection.DOCTOR.Equals (Pro.proname)) {
			this.Attack = strength;
			digPower = strength;
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
				StringCollection.DODGE + ":" + this.dodge + "\n";

			return this.charInfo;
		}
		set {
			charInfo = value;
		}
	}

}
