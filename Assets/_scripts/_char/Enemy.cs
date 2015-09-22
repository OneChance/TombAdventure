using UnityEngine;
using System.Collections;

public class Enemy : BattleObj
{
	public int enemyid;

	public Enemy(){

	}

	public Enemy(Enemy e){
		this.PrefabName = e.PrefabName;
		this.enemyid = e.enemyid;
	}
}
