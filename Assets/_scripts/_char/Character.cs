using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character:BattleObj
{

	private List<Baggrid> bgList;
	public  List<Equipment> equipList;
	private bool isOnLinePlayer;
	private int stamina;
	private int maxStamina;
	public Pro Pro;
	public int level;
	public int exp;
	public int nextLevelExp;
	public int strength;
	public int archeology;

	public Character (int health,int maxHealth,int attack, int def, int dodge, string objName,bool isOnLinePlayer,int stamina,int  maxStamina,Pro pro,int level,int exp,List<Equipment> eList)
	{
		this.Health = health;
		this.MaxHealth = maxHealth;
		this.Attack = attack;
		this.Def = def;
		this.Dodge = dodge;
		this.ObjName = objName;
		this.PrefabName = pro.prefabname;
		this.isOnLinePlayer = isOnLinePlayer;
		this.stamina = stamina;
		this.maxStamina = maxStamina;
		this.Pro = pro;
		this.level = level;
		this.exp = exp;
		this.nextLevelExp = level * 50;

		//玩家的属性->基本属性
		this.strength = this.level * this.Pro.strengthFactor;
		this.archeology = this.level * this.Pro.archeologyFactor;

		this.equipList = eList;

		//+装备属性
		if(equipList!=null){
			for(int i=0;i<equipList.Count;i++){
				this.strength+=equipList[i].strength;
				this.archeology+=equipList[i].archeology;
			}
		}
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


}
