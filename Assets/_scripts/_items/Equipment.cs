using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Equipment : Item
{
	public int strength;
	public int archeology;
	public int def;
	public int dodge;
	public EquipPos ep;
	public int eLevel;

	public Equipment (int strength, int archeology, int def, int dodge, EquipPos ep, string itemId, string name, int eLevel, int price):base(itemId,name)
	{
		this.ct = global::Item.CommonType.EQUIPMENT;
		this.strength = strength;
		this.archeology = archeology;
		this.def = def;
		this.dodge = dodge;
		this.ep = ep;
		this.eLevel = eLevel;
		this.price = price;

		if (strength > 0) {
			this.note = this.note + StringCollection.STRENGTH + "+" + strength + "\n";
		}
		if (archeology > 0) {
			this.note = this.note + StringCollection.ARCHEOLOGY + "+" + archeology + "\n";
		}
		if (def > 0) {
			this.note = this.note + StringCollection.DEF + "+" + def + "\n";
		}
		if (dodge > 0) {
			this.note = this.note + StringCollection.DODGE + "+" + dodge + "\n";
		}
	}

	public enum EquipPos
	{
		HEAD,
		BODY,
		FOOT,
		HAND,
		ALL
	}

	public static EquipPos getPosByIndex (int pos)
	{
		switch (pos) {
		case 1:
			return EquipPos.HEAD;
		case 2:
			return EquipPos.BODY;
		case 3:
			return EquipPos.FOOT;
		case 4:
			return EquipPos.HAND;
		default:
			return EquipPos.ALL;
		}
	}

	public  override void doSth <T> (T from, List<T> to)
	{
		
	}
}
