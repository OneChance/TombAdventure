using UnityEngine;
using System.Collections;

public class ElementData{
	public Vector3 pos;
	public string objName;
	public Vector3 eulerAngles;
	public int order;

	public ElementData (Vector3 pos, string objName,Vector3 eulerAngles,int order)
	{
		this.pos = pos;
		this.objName = objName;
		this.eulerAngles = eulerAngles;
		this.order = order;
	}
}
