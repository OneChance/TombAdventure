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
	private List<Baggrid> enemyAttackTypeList;
	private bool dead;
	private bool victory;
	private GameObject canvas;

	public void OnBattleAnim (int itemid)
	{
		Debug.Log ("item anim:" + itemid);
	}

	public void OnGetBattleData (Dictionary<string, object> role,List<object> bag)
	{

		int opCount = DataHelper.UpdatePlayerInfo_Battle (role, gData, enemyPos, enemySprite);

		if(bag.Count>0){		
			//更新背包
			DataHelper.UpdateBag (gData.characterList [0], bag,gData.siList);
		}

		UpdateUI ();

		if (opCount == 0) {	
			NewTurn ();
		} 
	}
	
	void Start ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		canvas = GameObject.FindGameObjectWithTag ("UI");

		currentEnemy = gData.currentEnemy;

		waitForAttack = new List<GameObject> ();
		focusList = new List<GameObject> ();
		opList = new List<BattleOp> ();
		enemyAttackTypeList = new List<Baggrid> ();

		enemySprite = Resources.Load <Sprite> ("_images/_game/" + currentEnemy.PrefabName);

		//初始化按钮文本
		Transform buttons = canvas.transform.FindChild ("Button").transform;
		buttons.FindChild ("Attack_B").FindChild ("Text").GetComponent<Text> ().text = StringCollection.ATTACK;
		buttons.FindChild ("Item_B").FindChild ("Text").GetComponent<Text> ().text = StringCollection.ITEM;
		buttons.FindChild ("Wait_B").FindChild ("Text").GetComponent<Text> ().text = StringCollection.WAIT;
		buttons.FindChild ("Ok_B").FindChild ("Text").GetComponent<Text> ().text = StringCollection.CONFIRM;
		buttons.FindChild ("Undo_B").FindChild ("Text").GetComponent<Text> ().text = StringCollection.CANCEL;

		characterList = gData.characterList;
		
		for (int i=0; i<characterList.Count; i++) {
			characterPos [i].SetActive (true);
			Character character = characterList [i];
			characterPos [i].GetComponent<Image> ().sprite = Resources.Load<Sprite> (character.PrefabName);
			characterPos [i].transform.FindChild ("Name").GetComponent<Text> ().text = character.ObjName;
			characterPos [i].GetComponent<PosChar> ().battleObj = character;
		}

		gData.account.getBattleData (currentEnemy.enemyid);
	}

	void Update ()
	{	
		for (int i=0; i<focusList.Count; i++) {
			GameUtil.Focus (focusList [i]);
		}
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

		//新一轮重新显示指令按钮(如果在组队模式下,玩家死亡但队友存活的情况下,隐藏玩家按钮)
		actionButton.SetActive (true);
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
		if (act.op != UI_Battle.Op.WAIT) {
			//判断道具属性，使用于敌方还是友方，单体还是多体
			if (act.bg.Item.ot == (int)Item.ObjType.Enemy) {
				if (act.bg.Item.rt == (int)Item.RangeType.SINGLE) {
					
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
				if (act.bg.Item.rt == (int)Item.RangeType.SINGLE) {
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
			Item item = currentAct.bg.Item;
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

		int from = waitForAttack [0].GetComponent<PosChar> ().battleObj.dbid;

		string to_tag = ""; 

		if (focusList.Count == 1) {
			to_tag = focusList [0].GetComponent<PosChar> ().battleObj.dbid.ToString ();
		} else  if (focusList.Count > 0) {
			int dbid = focusList [0].GetComponent<PosChar> ().battleObj.dbid;
			if (dbid > 4) {
				//所有敌人
				to_tag = "ALLENEMY";
			} else {
				to_tag = "ALLTEAM";
			}
		}

		int itemId = currentAct.bg.Item.dbid;

		if (itemId < 9000) {
			itemId = currentAct.bg.dbid;
		}
		gData.account.addOp (from.ToString (), to_tag, itemId);
	}

	public void OnAddOp (int opCount)
	{

		RecoverFocusList ();
		waitForAttack.Remove (waitForAttack [0]);
		
		if (waitForAttack.Count > 0) {
			focusList.Add (waitForAttack [0]);
		}
		
		currentAct = null;

		if (opCount == gData.characterList.Count) {
			actionButton.SetActive (false);			
		}
	}

	public void OnUndoOp (string from_tag)
	{

		if (!from_tag.Equals ("")) {
			RecoverFocusList ();
			GameObject fromObj = characterPos [int.Parse (from_tag) - 1];
			focusList.Add (fromObj);
			waitForAttack.Insert (0, fromObj);
		}
	}
	
	public void Undo ()
	{
		gData.account.undoOp ();
	}

	public void BattleOver (string battle_res, Dictionary<string,object> playerInfo, List<object> assistList)
	{
		if (battle_res.Equals ("win")) {

			//跟新属性
			Debug.Log (playerInfo ["exp"] + " :add exp");
			DataHelper.UpdatePlayerAttr (gData.characterList [0], playerInfo);
			Debug.Log (gData.characterList [0].exp + " :after upate");

			for (int i=1; i<gData.characterList.Count; i++) {
				for (int j=0; j<assistList.Count; j++) {

					Dictionary<string,object> assitInfo = (Dictionary<string,object>)assistList [j];

					if (gData.characterList [i].dbid == int.Parse (assitInfo ["dbid"].ToString ())) {
						DataHelper.UpdatePlayerAttr (gData.characterList [i], assitInfo);
						break;
					}
				}
			}

			gData.victory = true;
			Application.LoadLevel ("main");
		} else if (battle_res.Equals ("loose")) {
			gData.victory = false;
			Application.LoadLevel ("main");
		}
	}

}
