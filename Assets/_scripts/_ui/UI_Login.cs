using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Login : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Transform canvas = GameObject.FindGameObjectWithTag("UI").transform;
		canvas.FindChild("UsernameLable").GetComponent<Text>().text = StringCollection.USERNAME;
		canvas.FindChild("PasswordLable").GetComponent<Text>().text = StringCollection.PASSWORD;
		canvas.FindChild("Login").FindChild("Text").GetComponent<Text>().text = StringCollection.LOGIN;
		canvas.FindChild("Reg").FindChild("Text").GetComponent<Text>().text = StringCollection.REG;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Login(){

	}

	public void Reg(){

	}
}
