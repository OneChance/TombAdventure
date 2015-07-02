using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Battle : MonoBehaviour
{
	public int minEnemyNum = 2;
	public int maxEnemyNum = 3;
	public GameObject[] enemyPos;
	public GameObject[] characterPos;
	public GlobalData gData;
	private Sprite enemySprite;
	private List<Character> characterList;
	public List<GameObject> waitForAttack;
	private List<GameObject> focusList;
	private List<BattleOp> opList;
	private Enemy currentEnemy;
	private UI_Battle.Op currentOp;
	private bool newTurnInit = false;
	private List<Item> enemyAttackTypeList;
	private bool battleStart = false;
	private bool battleIng = false;

	void Awake ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();

		currentEnemy = gData.currentEnemy;

		object obj = Resources.Load (currentEnemy.PrefabName, typeof(GameObject));
		GameObject enmey = obj as GameObject;
		enemySprite = enmey.GetComponent<SpriteRenderer> ().sprite;

		characterList = gData.characterList;

		waitForAttack = new List<GameObject> ();
		focusList = new List<GameObject> ();
		opList = new List<BattleOp> ();
		enemyAttackTypeList = new List<Item> ();

		//init enemy attack type
		enemyAttackTypeList.Add (new AttackItem ());
	}

	// Use this for initialization
	void Start ()
	{
		int enemyNum = Random.Range (minEnemyNum, maxEnemyNum + 1);
		//init
		for (int i=0; i<enemyNum; i++) {
			enemyPos [i].SetActive (true);
			enemyPos [i].GetComponent<Image> ().sprite = enemySprite;
			enemyPos [i].transform.FindChild ("Health").GetComponent<Text> ().text = currentEnemy.Health.ToString ();
			//new instance for every enemy copy from currentEnemy
			Enemy enemy = new Enemy (currentEnemy);
			enemyPos [i].GetComponent<PosChar> ().battleObj = enemy;
		}
		for (int i=0; i<characterList.Count; i++) {
			characterPos [i].SetActive (true);
			Character character = characterList [i];

			object obj = Resources.Load (character.PrefabName, typeof(GameObject));
			GameObject c = obj as GameObject;
			characterPos [i].GetComponent<Image> ().sprite = c.GetComponent<SpriteRenderer> ().sprite;
			characterPos [i].transform.FindChild ("Health").GetComponent<Text> ().text = character.Health.ToString ();
			characterPos [i].transform.FindChild ("Name").GetComponent<Text> ().text = character.ObjName;
			characterPos [i].GetComponent<PosChar> ().battleObj = character;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (opList.Count == characterList.Count && !battleStart) {
			battleStart = true;
		}


		//all of the command execute complete
		if (opList.Count == 0 && !newTurnInit) {
			//battle not over
			NewTurn ();
		}

		for (int i=0; i<focusList.Count; i++) {
			Focus (focusList [i]);
		}

		if (battleStart && !battleIng) {

			//before start the battle,add the enmey's attack
			for (int i=0; i<enemyPos.Length; i++) {
				if (enemyPos [i].activeInHierarchy) {
					EnemyAttack (enemyPos [i]);
				}
			}

			StartCoroutine (BattleProcess ());

		}
	}

	IEnumerator BattleProcess ()
	{

		battleIng = true;

		for (int i=0; i<opList.Count; i++) {

			// this 2s for simulate battle animation
			yield return new WaitForSeconds (2.0f);
			
			Item item = opList [i].Item;
			
			List<BattleObj> toList = new List<BattleObj> ();
			
			//convert gameobject to battleobj
			for (int j=0; j<opList[i].To.Count; j++) {
				toList.Add (opList [i].To [j].GetComponent<PosChar> ().battleObj);
			}
			
			item.doSth (opList [i].From.GetComponent<PosChar> ().battleObj, toList);
			
			UpdateHealthUI ();
			
			opList.Remove (opList [i]);
			i--;
		}

		battleIng = false; // this turn is over
		newTurnInit = false;//tell to init a new turn
	}

	void NewTurn ()
	{

		for (int i=0; i<characterList.Count; i++) {
			Character character = characterList [i];
			if (character.Health > 0) {
				waitForAttack.Add (characterPos [i]);
			}
		}

		focusList.Add (waitForAttack [0]);

		newTurnInit = true;
		battleStart = false;
	}

	void UpdateHealthUI ()
	{
		for (int i=0; i<enemyPos.Length; i++) {
			if (enemyPos [i].activeInHierarchy) {
				enemyPos [i].transform.FindChild ("Health").GetComponent<Text> ().text = enemyPos [i].GetComponent<PosChar> ().battleObj.Health.ToString ();
			}
		}
		for (int i=0; i<characterPos.Length; i++) {
			if (characterPos [i].activeInHierarchy) {
				characterPos [i].transform.FindChild ("Health").GetComponent<Text> ().text = characterPos [i].GetComponent<PosChar> ().battleObj.Health.ToString ();
			}
		}
	}

	void Act (UI_Battle.Op op)
	{

		currentOp = op;

		if (op == UI_Battle.Op.ATTACK) {
			RecoverFocusList ();
			focusList.Add (enemyPos [0]);
		}
	}

	void ChooseTarget (GameObject enemy)
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
			focusList [i].GetComponent<Image> ().color = new Color (c.r, c.g, c.b, 1f);
		}
		focusList.Clear ();
	}

	Item chooseItem (UI_Battle.Op op)
	{
		if (op == UI_Battle.Op.ATTACK) {
			return new AttackItem ();
		}

		return null;
	}

	void AddOp ()
	{

		if (currentOp == UI_Battle.Op.NOACT) {

			//no act,refocus
			RecoverFocusList ();
			focusList.Add (waitForAttack [0]);

			return;
		}

		GameObject from = waitForAttack [0];
		List<GameObject> enemysAttacked = new List<GameObject> (); 

		for (int i=0; i<focusList.Count; i++) {
			enemysAttacked.Add (focusList [i]);
		}

		BattleOp bo = new BattleOp (from, enemysAttacked, chooseItem (currentOp));
		opList.Add (bo);

		RecoverFocusList ();
		waitForAttack.Remove (waitForAttack [0]);
		if (waitForAttack.Count > 0) {
			focusList.Add (waitForAttack [0]);
		}
	}



	//enemy attack ai in battle
	void EnemyAttack (GameObject from)
	{
		//choose a type of attack in a random way
		Item item = enemyAttackTypeList [Random.Range (0, enemyAttackTypeList.Count)];
		List<GameObject> attackList = new List<GameObject> ();

		if (item.rt == Item.RangeType.SINGLE) {
			//choose a player with the min health
			GameObject minHealthChar = characterPos [0];
			for (int i=1; i<characterPos.Length; i++) {

				int thisHealth = characterPos [i].GetComponent<PosChar> ().battleObj.Health;
				int currentHealth = minHealthChar.GetComponent<PosChar> ().battleObj.Health;

				if (characterPos [i].activeInHierarchy && thisHealth < currentHealth) {
					minHealthChar = characterPos [i];
				}
			}

			attackList.Add (minHealthChar);
		} else {

			for (int i=1; i<characterPos.Length; i++) {
				if (characterPos [i].activeInHierarchy) {
					attackList.Add (characterPos [i]);
				}
			}
		}

		BattleOp bo = new BattleOp (from, attackList, item);
		opList.Add (bo);
	}

	public void Undo ()
	{
		if (opList.Count > 0) {
			BattleOp op = opList [opList.Count - 1];
			RecoverFocusList ();
			focusList.Add (op.From);
			waitForAttack.Insert (0, op.From);
			opList.Remove (op);
		}
	}
}
