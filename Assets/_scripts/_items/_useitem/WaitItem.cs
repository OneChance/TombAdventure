using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaitItem : Item
{

	public WaitItem ():base("noprefab","noname")
	{
		this.name = "WaitItem";
	}

	public  override void doSth <T>(T from, List<T> to)
	{

	}
}
