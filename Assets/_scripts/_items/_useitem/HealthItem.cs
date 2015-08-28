using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthItem : Item
{
	private int heal;

	public HealthItem (int range,int heal,string itemId,string name,int price):base(itemId,name)
	{
		this.Rt = range;
		this.ct = (int)CommonType.CONSUME;
		this.heal = heal;
		this.ot = (int)ObjType.Friend;
		this.ut = (int)UseType.BATTLE;
		this.price = price;
	}

	public  override void doSth <T>(T from, List<T> to)
	{
		for (int i=0; i<to.Count; i++) {
			to [i].Health = Mathf.Min(to [i].Health + heal,to[i].MaxHealth);
		}
	}
}
