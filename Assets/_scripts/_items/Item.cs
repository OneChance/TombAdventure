using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Item
{

	public enum RangeType
	{
		SINGLE,
		MULTI
	}

	public enum CommonType
	{
		EQUIPMENT,
		CONSUME
	}

	public enum ObjType
	{
		Friend,
		Enemy
	}

	public string name;
	public RangeType rt;
	public CommonType ct;
	public string prefabName; // relate with the item id of the server database
	public ObjType ot;
	public string note;
	 

	public abstract void doSth (BattleObj from, List<BattleObj> to); 
}
