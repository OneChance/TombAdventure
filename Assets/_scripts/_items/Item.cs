using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Item
{

	public enum UseType
	{
		MAIN,
		BATTLE
	}

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
	public string targetNote;
	public UseType ut;
	 
	public RangeType Rt {
		get {
			return this.rt;
		}
		set {
			rt = value;
			if(value==RangeType.SINGLE){
				this.targetNote = "单个";
			}else{
				this.targetNote = "全部";
			}
		}
	} 

	public abstract void doSth (BattleObj from, List<BattleObj> to); 
}
