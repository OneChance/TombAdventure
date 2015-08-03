using UnityEngine;
using System.Collections;

public class GlobalDataManager : MonoBehaviour
{

	
	public GameObject gDataPrefab;
	public static bool have = false;
	private GameObject gData;
	
	// Use this for initialization	
	void Start ()
	{	
		if (!have) {		
			gData = Instantiate (gDataPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
			have = true;		
			DontDestroyOnLoad (gData);
		}	
	}
}
