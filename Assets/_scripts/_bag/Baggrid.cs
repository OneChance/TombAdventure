using UnityEngine;
using System.Collections;

public class Baggrid{
	public int dbid;
	private Item item;
	private int num;
	public int level = 1;//如果背包里是助手道具，有级别属性

	public Baggrid (Item item, int num,int dbid)
	{
		this.item = item;
		this.num = num;
		this.dbid = dbid;
	}

	public Item Item {
		get {
			return this.item;
		}
		set {
			item = value;
		}
	}

	public int Num {
		get {
			return this.num;
		}
		set {
			num = value;
		}
	}
}
