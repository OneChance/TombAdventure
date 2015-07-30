using UnityEngine;
using System.Collections;

public class Enemy : BattleObj
{

	public int exp;
	public int moneyRangeMin;
	public int moneyRangeMax;

	public Enemy (int health, int attack, int def, int dodge, string objName, string prefabName, int exp)
	{
		this.Health = health;
		this.Attack = attack;
		this.Def = def;
		this.dodge = dodge;
		this.ObjName = objName;
		this.PrefabName = prefabName;
		this.exp = exp;
	}

	public Enemy (Enemy e)
	{
		this.Health = e.Health;
		this.Attack = e.Attack;
		this.Def = e.Def;
		this.dodge = e.dodge;
		this.ObjName = e.ObjName;
		this.PrefabName = e.PrefabName;
	}
}
