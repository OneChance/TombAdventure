using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour
{

	private GlobalData gData;

	void Awake(){
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Enemy"){
			gData.currentEnemy = coll.gameObject.name;
			DontDestroyOnLoad(gData);
			Application.LoadLevel("battle");
		}
	}
}
