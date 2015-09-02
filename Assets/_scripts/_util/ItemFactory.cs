using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public static Item getItemFromSID (ServerItemData sid, Dictionary<string, object> bg_server)
	{
		switch (sid.commontype) {
		case (int)DetailCommonType.HEALTH:
			return new  HealthItem (sid.rangetype, sid.attack, sid.prefabname, sid.note, sid.price);
		case (int)DetailCommonType.MERCENARY:


			int health = sid.health;
			int maxhealth = sid.health;
			int strength = sid.strength;
			int archeology = sid.archeology;
			int def = sid.def;
			int dodge = sid.dodge;
			int stamina = sid.stamina;
			int maxstamina = sid.stamina;
			int level = 1;
			int exp = 0;

			if (bg_server != null) {
				health = int.Parse (bg_server ["health"].ToString ());
				maxhealth = int.Parse (bg_server ["maxhealth"].ToString ());
				strength = int.Parse (bg_server ["strength"].ToString ());
				archeology = int.Parse (bg_server ["archeology"].ToString ());
				def = int.Parse (bg_server ["def"].ToString ());
				dodge = int.Parse (bg_server ["dodge"].ToString ());
				stamina = int.Parse (bg_server ["stamina"].ToString ());
				maxstamina = int.Parse (bg_server ["maxstamina"].ToString ());
				level = int.Parse (bg_server ["level"].ToString ());
				exp = int.Parse (bg_server ["exp"].ToString ());
			}



			Character c = new Character (0, health, maxhealth, strength, archeology, def, dodge, sid.name, false, stamina, maxstamina, ProFactory.getPro (sid.pro, sid.prefabname), level, exp, null, -1);

			c.mnote = sid.note;
			Mercenary m = new Mercenary (c);
			return m;
		case (int)DetailCommonType.EQUIPMENT:
			Equipment e = new Equipment (sid.strength, sid.archeology, sid.def, sid.dodge, sid.epos, sid.prefabname, sid.name, sid.level, sid.price, sid.health, sid.stamina);
			return e;
		default:
			return null;
		}
	}

}
