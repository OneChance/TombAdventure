using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Equipment : Item
{
	public int strength;
	public int intelligence;
	public EquipPos ep;

	public Equipment(int strength,int intelligence,EquipPos ep,string itemId,string name):base(itemId){
		this.ct = global::Item.CommonType.EQUIPMENT;
		this.strength = strength;
		this.intelligence = intelligence;
		this.ep = ep;
		if(strength>0){
			this.note = this.note + "力量+"+strength;
		}
		if(intelligence>0){
			this.note = this.note + "智力+"+strength;
		}
	}

	public enum EquipPos
	{
		HEAD,
		BODY,
		FOOT,
		HAND
	}

	public  override void doSth <T>(T from, List<T> to){
		
	}
}
