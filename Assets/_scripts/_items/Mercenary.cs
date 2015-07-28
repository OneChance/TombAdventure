using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mercenary : Item
{
	public Character c;

	public Mercenary (Character  c)
	{
		this.ct = global::Item.CommonType.MERCENARY;
		this.c = c;
		this.prefabName = c.PrefabName;
		this.name = c.ObjName + "[" + c.Pro.proname + "]";
		this.note = StringCollection.STRENGTH + ":" + c.strengthBase + "\n" + 
			StringCollection.STAMINA + ":" + c.Stamina + "\n" +
			StringCollection.ARCHEOLOGY + ":" + c.archeologyBase + "\n";
	}

	public  override void doSth <T> (T from, List<T> to)
	{
		
	}
}
