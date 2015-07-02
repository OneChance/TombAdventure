using UnityEngine;
using System.Collections;

public class Baggrid{
	private Item item;
	private int num;

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
