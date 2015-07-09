using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character:BattleObj
{

	private List<Baggrid> bgList;
	private bool isOnLinePlayer;
	private int stamina;

	public Character (int health,int attack, int def, int dodge, string objName, string prefabName,bool isOnLinePlayer,int stamina)
	{
		this.Health = health;
		this.MaxHealth = health;
		this.Attack = attack;
		this.Def = def;
		this.Dodge = dodge;
		this.ObjName = objName;
		this.PrefabName = prefabName;
		this.isOnLinePlayer = isOnLinePlayer;
		this.stamina = stamina;
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
