using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character:BattleObj
{

	private List<Item> itemList;

	public Character (int health, int attack, int def, int dodge, string objName, string prefabName)
	{
		this.Health = health;
		this.Attack = attack;
		this.Def = def;
		this.Dodge = dodge;
		this.ObjName = objName;
		this.PrefabName = prefabName;
	}

	public List<Item> ItemList {
		get {
			return this.itemList;
		}
		set {
			itemList = value;
		}
	}


}
