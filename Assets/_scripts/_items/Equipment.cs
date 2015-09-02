using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Equipment : Item
{
	public int strength;
	public int archeology;
	public int def;
	public int dodge;
	public int ep;
	public int eLevel;
	public int health;
	public int stamina;

	public Equipment (int strength, int archeology, int def, int dodge, int ep, string itemId, string name, int eLevel, int price,int health,int stamina):base(itemId,name)
	{
		this.ct = (int)global::Item.CommonType.EQUIPMENT;
		this.strength = strength;
		this.archeology = archeology;
		this.def = def;
		this.dodge = dodge;
		this.ep = ep;
		this.eLevel = eLevel;
		this.price = price;
		this.health = health;
		this.stamina = stamina;

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
		if (health > 0) {
			this.note = this.note + StringCollection.HEALTH + "+" + health + "\n";
		}
		if (stamina > 0) {
			this.note = this.note + StringCollection.STAMINA + "+" + stamina + "\n";
		}
	}

	public enum EquipPos
	{
		HEAD=1,
		BODY=2,
		FOOT=3,
		HAND=4,
		ALL=5
	}
}
