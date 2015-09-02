using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mercenary : Item
{
	public Character c;
	public int assPos = -1;

	public Mercenary (Character  c)
	{
		this.ct = (int)global::Item.CommonType.MERCENARY;
		this.c = c;
		this.prefabName = c.PrefabName;
		this.note = c.CharInfo;
		this.price = 0;
	}

}
