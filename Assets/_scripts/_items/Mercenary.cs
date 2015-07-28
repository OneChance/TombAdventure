using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mercenary : Item
{
	public Character c;

	public Mercenary(Character  c){
		this.ct = global::Item.CommonType.MERCENARY;
		this.c = c;
		this.prefabName = c.PrefabName;
	}

	public  override void doSth <T>(T from, List<T> to){
		
	}
}
