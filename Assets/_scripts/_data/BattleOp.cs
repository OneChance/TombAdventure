using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleOp
{
	private BattleObj from;
	private List<BattleObj> to;
	private Item item;

	public BattleOp (BattleObj from, List<BattleObj> to, Item item)
	{
		this.from = from;
		this.to = to;
		this.item = item;
	}
	
	public BattleObj From {
		get {
			return this.from;
		}
		set {
			from = value;
		}
	}

	public List<BattleObj> To {
		get {
			return this.to;
		}
		set {
			to = value;
		}
	}

	public Item Item {
		get {
			return this.item;
		}
		set {
			item = value;
		}
	}
}
