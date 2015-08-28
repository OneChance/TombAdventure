using UnityEngine;
using System.Collections;

public class ItemFactory
{

	public enum DetailCommonType
	{
		EQUIPMENT=1,
		MERCENARY=2,
		HEALTH=3,
		STAMINA=4,
		ATTACK=5
	}

	public static Item getItemFromSID (ServerItemData sid)
	{
		switch (sid.commontype) {
		case (int)DetailCommonType.HEALTH:
			return new  HealthItem (sid.rangetype, sid.attack, sid.prefabname, sid.note, sid.price);
		case (int)DetailCommonType.MERCENARY:
			Character c = new Character (0,sid.health, sid.health, sid.strength,sid.archeology,sid.def,sid.dodge, sid.name, false,sid.stamina, sid.stamina, ProFactory.getPro (sid.pro, sid.prefabname), 1, 0, null, -1);
			c.Health = c.MaxHealth;
			c.stamina = c.maxStamina;
			c.mnote = sid.note;
			Mercenary m = new Mercenary (c);
			return m;
		default:
			return null;
		}
	}

}
