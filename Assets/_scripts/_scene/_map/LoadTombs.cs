using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoadTombs : MonoBehaviour
{

	public List<Tomb> tombs;
	private GlobalData gData;

	void Start ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();


	}
}
