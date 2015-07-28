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
			if(Vector3.Distance(player.position,transform.position)>GetComponent<SpriteRenderer>().bounds.size.x * 0.25){
					sceneGen.SendMessage("ToPreFloor");
			}
		}
	}
}
