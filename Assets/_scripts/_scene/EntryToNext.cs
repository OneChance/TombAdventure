using UnityEngine;
using System.Collections;

public class EntryToNext : MonoBehaviour {

	private SceneGen sceneGen;
	private Transform player;

	void Awake(){
		sceneGen = GameObject.FindGameObjectWithTag ("GameController").GetComponent<SceneGen>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag.Equals("Player")&&GetComponent<SpriteRenderer>().sprite.name.Equals("dig_3")){
			if(player.position != transform.position){
					//回上一层
					sceneGen.SendMessage("ToNextFloor",transform.position);
			}
		}
	}
}
