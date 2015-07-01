using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Item
{

	public enum rangeType
	{
		SINGLE,
		MULTI
	}

	public string name;
	public rangeType rt;

	public abstract void doSth (BattleObj from, List<BattleObj> to); 
}
