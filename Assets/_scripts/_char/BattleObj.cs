using UnityEngine;
using System.Collections;

public class BattleObj
{

	private string objName;
	private string prefabName;
	private int health = 100;
	private int def = 0;
	private int attack = 5;
	private float dodge = 0.0f;

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

	public float Dodge {
		get {
			return this.dodge;
		}
		set {
			dodge = value;
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
