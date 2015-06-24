using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public GameObject follow;

	// Update is called once per frame
	void Update () {
		Vector3 followPos = follow.transform.position;
		transform.position = new Vector3(followPos.x,followPos.y,transform.position.z);
	}
}
