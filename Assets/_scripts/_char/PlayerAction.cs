using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAction : MonoBehaviour
{

	private GlobalData gData;
	private List<Character> characterList;

	void Awake ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		characterList = new List<Character> ();
		Character c = new Character (100, 5, 0, 0, "zhouhui", gameObject.name);
		characterList.Add (c);
		Character c2 = new Character (100, 10, 0, 0, "unity", gameObject.name);
		characterList.Add (c2);
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Enemy") {
			gData.currentEnemy = coll.gameObject.GetComponent<EnemyAI> ().Enemy;
			gData.characterList = characterList;
			DontDestroyOnLoad (gData);
			Application.LoadLevel ("battle");
		}
	}
}
