using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthItem : Item
{
	private int heal;

	public HealthItem (RangeType range,int heal,string itemId,string name,int price):base(itemId,name)
	{
		this.Rt = range;
		this.ct = CommonType.CONSUME;
		this.heal = heal;
		this.ot = ObjType.Friend;
		this.ut = UseType.BATTLE;
		this.price = price;
		this.note = this.note + "恢复"+this.targetNote+"友军"+this.heal+"点生命力";
	}

	public  override void doSth <T>(T from, List<T> to)
	{
		for (int i=0; i<to.Count; i++) {
			to [i].Health = Mathf.Min(to [i].Health + heal,to[i].MaxHealth);
		}
	}
}
