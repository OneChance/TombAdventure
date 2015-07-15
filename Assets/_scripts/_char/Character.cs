using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character:BattleObj
{

	private List<Baggrid> bgList;
	private bool isOnLinePlayer;
	private int stamina;
	private int maxStamina;
	public Pro Pro;

	public Character (int health,int maxHealth,int attack, int def, int dodge, string objName,bool isOnLinePlayer,int stamina,int  maxStamina,Pro pro)
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
