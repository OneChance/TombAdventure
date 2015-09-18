using UnityEngine;
using System.Collections;

public class Action
{
	public Baggrid bg;
	public UI_Battle.Op op;
	public int itemid; //行为也被定义为一种道具

	public Action (UI_Battle.Op op, Baggrid bg)
	{
		this.op = op;
		this.bg = bg;
	}
}
