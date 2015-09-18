using UnityEngine;
using System.Collections;

public class BattleObj
{

	private string objName;
	private string prefabName;
	private int health = 100;
	private int maxHealth = 100;
	private int def = 0;
	private int attack = 5;
	public int dodge = 0;
	public int dbid;//战斗中与服务器对象的对应关系

	public int MaxHealth {
		get {
			return this.maxHealth;
		}
		set {
			maxHealth = value;
		}
	}

	public int Health {
		get {
			return this.health;
		}
		set {
			health = value;
		}
	}

	public int Def {
		get {
			return this.def;
		}
		set {
			def = value;
		}
	}

	public int Attack {
		get {
			return this.attack;
		}
		set {
			attack = value;
		}
	}

	public string PrefabName {
		get {
			return this.prefabName;
		}
		set {
			prefabName = value;
		}
	}

	public string ObjName {
		get {
			return this.objName;
		}
		set {
			objName = value;
		}
	}
}
