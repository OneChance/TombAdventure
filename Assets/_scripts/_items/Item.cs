using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Item
{
	public string name;

	string Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	public abstract void doSth(BattleObj from,List<BattleObj> to); 
}
