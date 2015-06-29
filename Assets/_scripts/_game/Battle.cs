using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Battle : MonoBehaviour
{
	public GameObject[] enemyPos;
	public GameObject[] characterPos;
	private GlobalData gData;
	private Sprite enemySprite;
	private List<Character> characterList;
	private int enemyAttack;
	private int enemyHealth;
	private List<GameObject> waitForAttack;
	private List<GameObject> focusList;

	void Awake ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();

		string enemyName = gData.currentEnemy;

		if (enemyName.Contains ("(Clone)")) {
			enemyName = enemyName.Replace ("(Clone)", "");
		}

		object obj = Resources.Load (enemyName, typeof(GameObject));
		GameObject enmey = obj as GameObject;
		enemySprite = enmey.GetComponent<SpriteRenderer> ().sprite;

		enemyAttack = enmey.GetComponent<Enemy> ().attack;
		enemyHealth = enmey.GetComponent<Enemy> ().health;

		characterList = gData.characterList;

		waitForAttack = new List<GameObject> ();
		focusList = new List<GameObject> ();
	}

	// Use this for initialization
	void Start ()
	{
		int enemyNum = Random.Range (1, 4);
		//init
		for (int i=0; i<enemyNum; i++) {
			enemyPos [i].SetActive (true);
			enemyPos [i].GetComponent<Image> ().sprite = enemySprite;

			enemyPos [i].transform.FindChild ("Health").GetComponent<Text> ().text = enemyHealth.ToString ();
		}
		for (int i=0; i<characterList.Count; i++) {
			characterPos [i].SetActive (true);
			Character character = characterList [i];

			object obj = Resources.Load (character.PrefabName, typeof(GameObject));
			GameObject c = obj as GameObject;
			characterPos [i].GetComponent<Image> ().sprite = c.GetComponent<SpriteRenderer> ().sprite;
			characterPos [i].transform.FindChild ("Health").GetComponent<Text> ().text = character.Health.ToString ();
			characterPos [i].transform.FindChild ("Name").GetComponent<Text> ().text = character.CharacterName;

			waitForAttack.Add (characterPos [i]);
		}

		focusList.Add (waitForAttack [0]);
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i=0; i<focusList.Count; i++) {
			Focus (focusList [i]);
		}
	}

	void Act (UI_Battle.Op op)
	{
		if (op == UI_Battle.Op.ATTACK) {
			RecoverFocusList ();
			focusList.Add (enemyPos [0]);
		}
	}

	void ChooseEnemy (GameObject enemy)
	{
		//if use the item which can attack all of the object,ignore thie method 
		//if condition
		RecoverFocusList ();
		focusList.Add (enemy);
	}

	void Focus (GameObject go)
	{
		float lerp = Mathf.PingPong (Time.time, 0.5f) * 2f;  

		Color c = go.GetComponent<Image> ().color;
		Color fromC = new Color (c.r, c.g, c.b, 1f);
		Color toC = new Color (c.r, c.g, c.b, 0.3f);

		go.GetComponent<Image> ().color = Color.Lerp (fromC, toC, lerp);
	}

	void RecoverFocusList ()
	{
		for (int i=0; i<focusList.Count; i++) {
			Color c = focusList [i].GetComponent<Image> ().color;
			focusList [i].GetComponent<Image> ().color =  new Color (c.r, c.g, c.b, 1f);
		}
		focusList.Clear ();
	}
}
