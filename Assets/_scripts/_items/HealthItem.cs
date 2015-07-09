﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthItem : Item
{
	private int heal;

	public HealthItem (RangeType range,int heal,string itemId,string name)
	{
		this.name = name;
		this.Rt = range;
		this.ct = CommonType.CONSUME;
		this.heal = heal;
		this.prefabName = "item_"+itemId;
		this.ot = ObjType.Friend;
		this.ut = UseType.BATTLE;
		this.note = "恢复"+this.targetNote+"友军"+this.heal+"点生命力";
	}

	public override void doSth (BattleObj from, List<BattleObj> to)
	{
		for (int i=0; i<to.Count; i++) {
			to [i].Health = Mathf.Min(to [i].Health + heal,to[i].MaxHealth);
		}
	}
}
