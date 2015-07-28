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
			if(Vector3.Distance(player.position,transform.position)>GetComponent<SpriteRenderer>().bounds.size.x * 0.25){
					sceneGen.SendMessage("ToNextFloor",transform.position);
			}
		}
	}
}
