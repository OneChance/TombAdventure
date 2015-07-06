using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character:BattleObj
{

	private List<Baggrid> bgList;

	public Character (int health,int attack, int def, int dodge, string objName, string prefabName)
	{
		this.Health = health;
		this.MaxHealth = health;
		this.Attack = attack;
		this.Def = def;
		this.Dodge = dodge;
		this.ObjName = objName;
		this.PrefabName = prefabName;
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
