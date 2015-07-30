using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mercenary : Item
{
	public Character c;
	public int assPos = -1;

	public Mercenary (Character  c)
	{
		this.ct = global::Item.CommonType.MERCENARY;
		this.c = c;
		this.prefabName = c.PrefabName;
		this.note = c.CharInfo;
		this.price = c.strength + c.archeology + (int)(c.Stamina * 0.5);
	}

	public  override void doSth <T> (T from, List<T> to)
	{
		
	}
}
