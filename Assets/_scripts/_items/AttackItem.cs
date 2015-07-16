using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackItem : Item
{

	public AttackItem ()
	{
		this.name = "AttackItem";
		this.rt = RangeType.SINGLE;
		this.ct = CommonType.CONSUME;
		this.ot = ObjType.Enemy;
	}

	public  override void doSth <T>(T from, List<T> to)
	{
		int attack = from.Attack;
		
		for (int i=0; i<to.Count; i++) {
			int def = to [i].Def;
			to [i].Health = to [i].Health - Mathf.Max (attack - def, 0);
		}
	}
}
