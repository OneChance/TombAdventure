using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleOp
{
	private GameObject from;
	private List<GameObject> to;
	private Item item;

	public BattleOp (GameObject from, List<GameObject> to, Item item)
	{
		this.from = from;
		this.to = to;
		this.item = item;
	}
	
	public GameObject From {
		get {
			return this.from;
		}
		set {
			from = value;
		}
	}

	public List<GameObject> To {
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
