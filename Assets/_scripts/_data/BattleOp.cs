using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleOp
{
	private GameObject from;
	private List<GameObject> to;
	private Baggrid bg;

	public BattleOp (GameObject from, List<GameObject> to, Baggrid bg)
	{
		this.from = from;
		this.to = to;
		this.bg = bg;
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

	public Baggrid Bg {
		get {
			return this.bg;
		}
		set {
			bg = value;
		}
	}
}
