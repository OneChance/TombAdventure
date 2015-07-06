using UnityEngine;
using System.Collections;

public class ElementData{
	private Vector3 pos;
	private string objName;

	public ElementData (Vector3 pos, string objName)
	{
		this.pos = pos;
		this.objName = objName;
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
