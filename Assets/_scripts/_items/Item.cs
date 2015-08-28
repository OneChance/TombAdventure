using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Item
{

	public enum UseType
	{
		MAIN=1,
		BATTLE=2
	}

	public enum RangeType
	{
		SINGLE=1,
		MULTI=2
	}

	public enum CommonType
	{
		EQUIPMENT=1,
		MERCENARY=2,
		CONSUME=3
	}

	public enum ObjType
	{
		Friend=1,
		Enemy=2
	}

	public string name;
	public int rt;
	public int ct;
	public string prefabName; // relate with the item id of the server database
	public int ot;
	public string note;
	public string targetNote;
	public int ut;
	public int price;
	public bool useable = false;

	public Item ()
	{

	}

	public Item (string itemId,string itemname)
	{
		this.name = itemname;
		this.note = this.name + "\n\n";
		this.prefabName = "_images/_ui/item_" + itemId;
	}
	 
	public int Rt {
		get {
			return this.rt;
		}
		set {
			rt = value;
			if (value == (int)RangeType.SINGLE) {
				this.targetNote = "单个";
			} else {
				this.targetNote = "全部";
			}
		}
	}
	
	public abstract void doSth <T> (T from, List<T> to) where T:BattleObj; 
}
