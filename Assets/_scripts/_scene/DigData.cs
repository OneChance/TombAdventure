using UnityEngine;
using System.Collections;

public class DigData : ElementData {
	public int deep;
	public int currentDeep;
	public int texType;

	public DigData (Vector3 pos, string objName,Vector3 eulerAngles,int order,int deep,int currentDeep,int texType): base(pos,objName,eulerAngles,order)
	{
		this.deep = deep;
		this.currentDeep = currentDeep;
		this.texType = texType;
	}
}
