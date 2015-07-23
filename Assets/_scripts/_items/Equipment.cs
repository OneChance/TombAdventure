using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Equipment : Item
{
	public int strength;
	public int archeology;
	public EquipPos ep;
	public int eLevel;

	public Equipment(int strength,int archeology,EquipPos ep,string itemId,string name,int eLevel):base(itemId){
		this.ct = global::Item.CommonType.EQUIPMENT;
		this.strength = strength;
		this.archeology = archeology;
		this.ep = ep;
		this.eLevel = eLevel;
		if(strength>0){
			this.note = this.note +  StringCollection.STRENGTH + "+"+strength;
		}
		if(archeology>0){
			this.note = this.note + StringCollection.ARCHEOLOGY +"+"+archeology;
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
