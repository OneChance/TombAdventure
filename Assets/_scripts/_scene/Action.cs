using UnityEngine;
using System.Collections;

public class Action {
	private UI_Battle.Op op;
	private Baggrid bg;


	public Action (UI_Battle.Op op, Baggrid bg)
	{
		this.op = op;
		this.bg = bg;
	}
	
	public UI_Battle.Op Op {
		get {
			return this.op;
		}
		set {
			op = value;
		}
	}

	public Baggrid Bg {
		get {
			return this.bg;
		}
		set {
			bg = value;
		}
	}
}
