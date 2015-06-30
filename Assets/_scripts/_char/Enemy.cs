using UnityEngine;
using System.Collections;

public class Enemy : BattleObj
{
	public Enemy (int health, int attack, int def, int dodge, string objName, string prefabName)
	{
		this.Health = health;
		this.Attack = attack;
		this.Def = def;
		this.Dodge = dodge;
		this.ObjName = objName;
		this.PrefabName = prefabName;
	}

	public Enemy (Enemy e)
	{
		this.Health = e.Health;
		this.Attack = e.Attack;
		this.Def = e.Def;
		this.Dodge = e.Dodge;
		this.ObjName = e.ObjName;
		this.PrefabName = e.PrefabName;
	}
}
