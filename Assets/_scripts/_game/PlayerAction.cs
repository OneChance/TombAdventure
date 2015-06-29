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
		Character c = new Character(5,100,"zhouhui",gameObject.name);
		characterList.Add(c);
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Enemy") {
			gData.currentEnemy = coll.gameObject.name;
			gData.characterList = characterList;
			DontDestroyOnLoad (gData);
			Application.LoadLevel ("battle");
		}
	}
}
