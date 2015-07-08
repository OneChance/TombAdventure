using UnityEngine;
using System.Collections;

public class ElementData{
	private Vector3 pos;
	private string objName;
	private Vector3 eulerAngles;
	private int order;

	public ElementData (Vector3 pos, string objName,Vector3 eulerAngles,int order)
	{
		this.pos = pos;
		this.objName = objName;
		this.eulerAngles = eulerAngles;
		this.order = order;
	}

	public int Order {
		get {
			return this.order;
		}
		set {
			order = value;
		}
	}

	public Vector3 EulerAngles {
		get {
			return this.eulerAngles;
		}
		set {
			eulerAngles = value;
		}
	}

	public Vector3 Pos {
		get {
			return this.pos;
		}
		set {
			pos = value;
		}
	}

	public string ObjName {
		get {
			return this.objName;
		}
		set {
			objName = value;
		}
	}

}
