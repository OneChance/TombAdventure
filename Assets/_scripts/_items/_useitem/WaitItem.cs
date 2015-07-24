using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaitItem : Item
{

	public WaitItem ():base("noprefab")
	{
		this.name = "WaitItem";
	}

	public  override void doSth <T>(T from, List<T> to)
	{

	}
}
