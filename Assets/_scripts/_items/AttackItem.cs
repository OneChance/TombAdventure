using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackItem : Item
{

	public AttackItem ()
	{
		this.name = "AttackItem";
		this.rt = rangeType.MULTI;
	}

	public override void doSth (BattleObj from, List<BattleObj> to)
	{
		int attack = from.Attack;

		for (int i=0; i<to.Count; i++) {
			int def = to [i].Def;
			to [i].Health = to [i].Health - Mathf.Max (attack - def, 0);
		}
	}
}
