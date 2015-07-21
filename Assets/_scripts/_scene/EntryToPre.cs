using UnityEngine;
using System.Collections;

public class EntryToPre : MonoBehaviour {

	private SceneGen sceneGen;
	private Transform player;

	void Awake(){
		sceneGen = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SceneGen>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}


	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag.Equals("Player")){
			if(player.position != transform.position){
					//回上一层
					sceneGen.SendMessage("ToPreFloor");
			}
		}
	}
}
