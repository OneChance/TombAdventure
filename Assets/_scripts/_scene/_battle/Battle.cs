using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Battle : MonoBehaviour
{
	public int minEnemyNum = 1;
	public int maxEnemyNum = 2;
	public GameObject[] enemyPos;
	public GameObject[] characterPos;
	public GameObject actionButton;
	public GlobalData gData;
	private Sprite enemySprite;
	public List<Character> characterList;
	public List<GameObject> waitForAttack;
	private List<GameObject> focusList;
	private List<BattleOp> opList;
	private Enemy currentEnemy;
	private Action currentAct;
	private bool newTurnInit = false;
	private List<Baggrid> enemyAttackTypeList;
	private bool battleStart = false;
	private bool battleIng = false;
	private bool dead;
	private bool victory;
	private GameObject canvas;
	private int battleExp = 0;

	void Awake ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		canvas = GameObject.FindGameObjectWithTag ("UI");
		currentEnemy = gData.currentEnemy;
		enemySprite = Resources.Load <Sprite> ("_images/_game/" + currentEnemy.PrefabName);

		characterList = gData.characterList;

		waitForAttack = new List<GameObject> ();
		focusList = new List<GameObject> ();
		opList = new List<BattleOp> ();
		enemyAttackTypeList = new List<Baggrid> ();

		//敌人攻击类型列表，此处只添加额普通单体攻击
		enemyAttackTypeList.Add (new Baggrid (new AttackItem (), 1,-1));

		//初始化按钮文本
		Transform buttons = canvas.transform.FindChild ("Button").transform;
		buttons.FindChild ("Attack_B").FindChild ("Text").GetComponent<Text> ().text = StringCollection.ATTACK;
		buttons.FindChild ("Item_B").FindChild ("Text").GetComponent<Text> ().text = StringCollection.ITEM;
		buttons.FindChild ("Wait_B").FindChild ("Text").GetComponent<Text> ().text = StringCollection.WAIT;
		buttons.FindChild ("Ok_B").FindChild ("Text").GetComponent<Text> ().text = StringCollection.CONFIRM;
		buttons.FindChild ("Undo_B").FindChild ("Text").GetComponent<Text> ().text = StringCollection.CANCEL;
	}
	
	void Start ()
	{
		int enemyNum = Random.Range (minEnemyNum, maxEnemyNum + 1);

		battleExp = enemyNum * currentEnemy.exp;

		//初始化敌人和玩家
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
			characterPos [i].GetComponent<Image> ().sprite = Resources.Load<Sprite> (character.PrefabName);
			characterPos [i].transform.FindChild ("Health").GetComponent<Text> ().text = character.Health.ToString ();
			characterPos [i].transform.FindChild ("Name").GetComponent<Text> ().text = character.ObjName;
			characterPos [i].GetComponent<PosChar> ().battleObj = character;
		}
	}

	void Update ()
	{
		//如果战斗未开始且战斗指令数量已经等于玩家数量了，即战斗开始
		if (opList.Count == characterList.Count && !battleStart) {
			battleStart = true;
		}


		//所有指令执行完毕
		if (opList.Count == 0 && !newTurnInit) {
			//battle not over
			NewTurn ();
		}

		for (int i=0; i<focusList.Count; i++) {
			GameUtil.Focus (focusList [i]);
		}

		if (battleStart && !battleIng) {

			for (int i=0; i<enemyPos.Length; i++) {
				if (enemyPos [i].activeInHierarchy && enemyPos [i].GetComponent<PosChar> ().battleObj.Health > 0) {
					EnemyAttack (enemyPos [i]);
				}
			}

			StartCoroutine (BattleProcess ());
		}
	}

	IEnumerator BattleProcess ()
	{

		//战斗开始时,隐藏按钮
		actionButton.SetActive (false);			

		battleIng = true;

		for (int i=0; i<opList.Count; i++) {

			// this 2s for simulate battle animation
			yield return new WaitForSeconds (2.0f);

			if (opList [i].From.GetComponent<PosChar> ().battleObj.Health > 0) {

				Baggrid bg = opList [i].Bg;
				
				List<BattleObj> toList = new List<BattleObj> ();

				//获取位置上挂载的battleObj
				for (int j=0; j<opList[i].To.Count; j++) {
					//如果选择的目标已经给上一个玩家打死,不添加
					if (opList [i].To [j].GetComponent<PosChar> ().battleObj.Health > 0) {
						toList.Add (opList [i].To [j].GetComponent<PosChar> ().battleObj);
					}
				}

				if (toList.Count > 0) {
					bg.Item.doSth (opList [i].From.GetComponent<PosChar> ().battleObj, toList);	
				}
			

				bg.Num = bg.Num - 1;
				
				UpdateUI ();
			}

			opList.Remove (opList [i]);
			i--;
		}

		dead = Dead ();
		victory = Victory ();

		if (victory) {
			gData.victory = true;
			Application.LoadLevel ("main");
		} else if (dead) {
			gData.victory = false;
			Application.LoadLevel ("main");
		}

		battleIng = false; // this turn is over
		newTurnInit = false;//tell to init a new turn
				
		//联机模式下,如果玩家死了,就不再显示按钮了(如果战斗过程中被人复活,则再显示)
		actionButton.SetActive (true);	
	}

	void NewTurn ()
	{
		for (int i=0; i<characterList.Count; i++) {
			Character character = characterList [i];

			//如果是联机模式,攻击列表中只有玩家自己
			if (character.Health > 0 && (i == 0 || !character.IsOnLinePlayer)) {
				waitForAttack.Add (characterPos [i]);
			}
		}

		focusList.Add (waitForAttack [0]);

		newTurnInit = true;
		battleStart = false;
	}

	void UpdateUI ()
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

	//玩家动作,根据道具类型，设置聚焦列表
	void Act (Action act)
	{
		currentAct = act;
		RecoverFocusList ();

		//等待道具无聚焦列表
		if (act.Op != UI_Battle.Op.WAIT) {
			//判断道具属性，使用于敌方还是友方，单体还是多体
			if (act.Bg.Item.ot == (int)Item.ObjType.Enemy) {
				if (act.Bg.Item.rt == (int)Item.RangeType.SINGLE) {
					
					//如果是单体，选择第一个活着的敌人
					for (int i=0; i<enemyPos.Length; i++) {
						if (enemyPos [i].activeInHierarchy && enemyPos [i].GetComponent<PosChar> ().battleObj.Health > 0) {
							focusList.Add (enemyPos [i]);
							break;
						}
					}
					
				} else {
					for (int i=0; i<enemyPos.Length; i++) {
						if (enemyPos [i].activeInHierarchy && enemyPos [i].GetComponent<PosChar> ().battleObj.Health > 0) {
							focusList.Add (enemyPos [i]);
						}
					}
				}
			} else {
				if (act.Bg.Item.rt == (int)Item.RangeType.SINGLE) {
					focusList.Add (characterPos [0]);
				} else {
					for (int i=0; i<characterPos.Length; i++) {
						if (characterPos [i].activeInHierarchy) {
							focusList.Add (characterPos [i]);
						}
					}
				}
			}
		}
	}

	void ChooseTarget (GameObject target)
	{
		//if there is no act choose,you can not choose target
		if (currentAct == null) {
			return;
		} else {
			Item item = currentAct.Bg.Item;

			//if choose the target can not be apply the item,return
			if (target.name.Contains ("EPos")) {
				if (item.ot == (int)Item.ObjType.Friend) {
					ShowHint.Hint (StringCollection.ITEMTOFRIEND);
					return;
				} else if (target.GetComponent<PosChar> ().battleObj.Health <= 0) {
					ShowHint.Hint (StringCollection.INVALIDTARGET);
					return;
				}
			} else {
				if (item.ot == (int)Item.ObjType.Enemy) {
					ShowHint.Hint (StringCollection.ITEMTOENEMY);
					return;
				}
			}
			//if use the item which can attack all of the object,do not change the focusList
			if (item.rt == (int)Item.RangeType.MULTI) {
				return;
			} else {
				RecoverFocusList ();
				focusList.Add (target);
			}
		}
	}

	/*
	 *  清除焦点列表 
	 */
	void RecoverFocusList ()
	{
		for (int i=0; i<focusList.Count; i++) {
			Color c = focusList [i].GetComponent<Image> ().color;
			focusList [i].GetComponent<Image> ().color = new Color (c.r, c.g, c.b, 1f);
		}
		focusList.Clear ();
	}
	/*
	 *  添加战斗指令
	 */
	void AddOp ()
	{

		if (currentAct == null) {
			//no act,refocus
			RecoverFocusList ();
			focusList.Add (waitForAttack [0]);
			return;
		}

		GameObject from = waitForAttack [0];

		List<GameObject> to = new List<GameObject> (); 
		
		for (int i=0; i<focusList.Count; i++) {
			to.Add (focusList [i]);
		}
		
		BattleOp bo = new BattleOp (from, to, currentAct.Bg);
		opList.Add (bo);
		RecoverFocusList ();
		waitForAttack.Remove (waitForAttack [0]);

		if (waitForAttack.Count > 0) {
			focusList.Add (waitForAttack [0]);
		}

		currentAct = null;
	}



	//enemy attack ai in battle
	void EnemyAttack (GameObject from)
	{
		//choose a type of attack in a random way
		Baggrid bg = enemyAttackTypeList [Random.Range (0, enemyAttackTypeList.Count)];
		List<GameObject> attackList = new List<GameObject> ();

		if (bg.Item.rt == (int)Item.RangeType.SINGLE) {
			//choose a player with the min health
			GameObject minHealthChar = characterPos [0];

			for (int i=1; i<characterList.Count; i++) {
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

		BattleOp bo = new BattleOp (from, attackList, bg);
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

	bool Victory ()
	{
		bool victory = true;
		for (int i=0; i<enemyPos.Length; i++) {
			if (enemyPos [i].activeInHierarchy) {
				if (enemyPos [i].GetComponent<PosChar> ().battleObj.Health > 0) {
					victory = false;
					break;
				}
			}
		}


		if (victory) {
			//获得经验
			for (int i=0; i<characterList.Count; i++) {
				characterList [i].AddExp (battleExp);
			}
		}
		return victory;
	}

	bool Dead ()
	{
		bool dead = true;
		for (int i=0; i<characterPos.Length; i++) {
			if (characterPos [i].activeInHierarchy) {
				if (characterPos [i].GetComponent<PosChar> ().battleObj.Health > 0) {
					dead = false;
					break;
				}
			}
		}
		return dead;
	}
}
