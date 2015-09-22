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
	private Sprite attack_s;
	private Sprite heal_s;
	private Dictionary<int,Sprite> spriteDic;
	private int animScope = 5;
	private Dictionary<string,GameObject> posDic;
	private bool backToMain = false;

	IEnumerator PlayActAnim (int itemid, GameObject fromBO)
	{  
		if (spriteDic.ContainsKey (gData.siList [itemid].commontype)) {
			Sprite s = fromBO.GetComponent<Image> ().sprite;
			
			fromBO.GetComponent<Image> ().sprite = spriteDic [gData.siList [itemid].commontype];
			
			yield return new WaitForSeconds (0.5f);
			
			fromBO.GetComponent<Image> ().sprite = s;
		}
	}

	public void OnBattleAnim (int itemid, string from)
	{

		GameObject fromBO = null;

		if (int.Parse (from) < 5) {
			for (int i=0; i<characterPos.Length; i++) {
				if (characterPos [i].GetComponent<PosChar> ().battleObj.dbid == int.Parse (from)) {
					fromBO = characterPos [i];
					break;
				}
			}
		} else {
			for (int i=0; i<enemyPos.Length; i++) {
				if (enemyPos [i].GetComponent<PosChar> ().battleObj.dbid == int.Parse (from)) {
					fromBO = enemyPos [i];
					break;
				}
			}
		}

		if (fromBO != null) {
			//根据ITEMID决定播放动画
			StartCoroutine (PlayActAnim (itemid, fromBO));  
		}
	}

	public void OnOpExe (List<object> toBos, List<object> bag, int itemid)
	{

		for (int i=0; i<toBos.Count; i++) {
			Dictionary<string,object> boInfo = (Dictionary<string,object>)toBos [i];
			UpdateUIWithTarget (boInfo ["dbid"].ToString (), boInfo ["health"].ToString (), itemid);
		}

		if (bag.Count > 0) {		
			//更新背包
			DataHelper.UpdateBag (gData.characterList [0], bag, gData.siList);
		}
	}

	public void OnGetBattleData (List<object> enemyList)
	{
		for (int i=0; i<enemyList.Count; i++) {		
			Dictionary<string, object> enemyInfo = (Dictionary<string, object>)enemyList [i];
			enemyPos [i].SetActive (true);
			enemyPos [i].GetComponent<Image> ().sprite = enemySprite;
			Enemy enemy = new Enemy ();
			enemy.dbid = int.Parse (enemyInfo ["dbid"].ToString ());
			enemy.Health = int.Parse (enemyInfo ["health"].ToString ());
			enemyPos [i].GetComponent<PosChar> ().battleObj = enemy;
		}

		UpdateUIAll ();
		NewTurn ();
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
		posDic = new Dictionary<string, GameObject> ();

		enemySprite = Resources.Load <Sprite> ("_images/_game/" + currentEnemy.PrefabName);

		spriteDic = new Dictionary<int, Sprite> ();
		attack_s = Resources.Load<Sprite> ("_images/_game/act_5");
		spriteDic.Add (5, attack_s);
		heal_s = Resources.Load<Sprite> ("_images/_game/act_3");
		spriteDic.Add (3, heal_s);


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

		gData.account.getBattleData (currentEnemy.enemyid,currentEnemy.dbid);
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

	void UpdateUIAll ()
	{
		for (int i=0; i<enemyPos.Length; i++) {
			if (enemyPos [i].activeInHierarchy) {
				enemyPos [i].transform.FindChild ("Health").GetComponent<Text> ().text = enemyPos [i].GetComponent<PosChar> ().battleObj.Health.ToString ();
				posDic.Add (enemyPos [i].GetComponent<PosChar> ().battleObj.dbid.ToString (), enemyPos [i]);
			}
		}
		for (int i=0; i<characterPos.Length; i++) {
			if (characterPos [i].activeInHierarchy) {
				characterPos [i].transform.FindChild ("Health").GetComponent<Text> ().text = characterPos [i].GetComponent<PosChar> ().battleObj.Health.ToString ();
				posDic.Add (characterPos [i].GetComponent<PosChar> ().battleObj.dbid.ToString (), characterPos [i]);
			}
		}
	}

	void UpdateUIWithTarget (string to, string newHealth, int itemid)
	{

		int dir = 1;

		if (int.Parse (to) < 5) {
			dir = -1;
		}

		GameObject posGo = posDic [to];

		ServerItemData sid = gData.siList [itemid];

		posGo.GetComponent<PosChar> ().battleObj.Health = int.Parse (newHealth); 
		HealthChangeAnim (posGo, posGo.transform.FindChild ("Health").GetComponent<Text> ().text, newHealth, dir, sid);
		posGo.transform.FindChild ("Health").GetComponent<Text> ().text = newHealth;

		if (newHealth.Equals ("0")) {
			Color c = posGo.GetComponent<Image> ().color;
			posGo.GetComponent<Image> ().color = new Color (c.r, c.g, c.b, 0.1f);
		}
	}

	void HealthChangeAnim (GameObject go, string pre_health, string now_health, int dir, ServerItemData sid)
	{
		int pre_health_int = int.Parse (pre_health);
		int now_health_int = int.Parse (now_health);

		if (pre_health_int > now_health_int) {
			StartCoroutine (PlayAttackedAnim (go, dir));
		} else if (pre_health_int < now_health_int) {
			StartCoroutine (PlayHealedAnim (go));
		} else {
			if (sid.commontype == 3) {
				StartCoroutine (PlayHealedAnim (go));
			} else {
				StartCoroutine (PlayDefAnim (go, dir));
			}
		}
	}

	IEnumerator PlayAttackedAnim (GameObject go, int dir)
	{  
		go.transform.Translate (Vector2.left * animScope * dir);
		yield return new WaitForSeconds (0.1f);
		go.transform.Translate (Vector2.right * (animScope * 2) * dir);
		yield return new WaitForSeconds (0.1f);
		go.transform.Translate (Vector2.left * animScope * dir);
	}

	IEnumerator PlayDefAnim (GameObject go, int dir)
	{  
		go.transform.Translate (Vector2.left * animScope * dir);
		yield return new WaitForSeconds (0.1f);
		go.transform.Translate (Vector2.right * animScope * dir);
	}

	IEnumerator PlayHealedAnim (GameObject go)
	{  
		go.transform.localScale = new Vector3 (1.3f, 1.3f, 1f);
		yield return new WaitForSeconds (0.3f);
		go.transform.localScale = new Vector3 (1, 1, 1);
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

	public void OnAddOp (int isBattleStart)
	{
		RecoverFocusList ();
		waitForAttack.Remove (waitForAttack [0]);

		if (waitForAttack.Count > 0) {
			focusList.Add (waitForAttack [0]);
		}
		
		currentAct = null;

		if (isBattleStart == 1) {
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

	public void BattleOver (string battle_res, Dictionary<string,object> playerInfo, List<object> assistList, int battleOver,List<object> bag, int enemy_dbid)
	{
		if (battle_res.Equals ("loose")) {
			gData.victory = false;
			Application.LoadLevel ("main");
		} else if (battle_res.Equals ("goon")) {
			if (battleOver == 0) {  //如果战斗没有结束，当前指令集已执行完毕，开始新的一轮
				NewTurn ();
			}
		} else {

			string showMsg = "";

			string[] itemList = battle_res.Split (new char[]{'.'});
			for (int i=0; i<itemList.Length; i++) {
				if (!itemList [i].Equals ("")) {
					if (itemList [i].Split (new char[]{'@'}) [0].Equals ("exp")) {
						int itemNum = int.Parse (itemList [i].Split (new char[]{'@'}) [1]);			
						showMsg = showMsg + StringCollection.stringDict_CN ["exp"] + " : " + itemNum + "\n";
					} else {
						int itemId = int.Parse (itemList [i].Split (new char[]{'@'}) [0]);
						int itemNum = int.Parse (itemList [i].Split (new char[]{'@'}) [1]);					
						showMsg = showMsg + gData.siList [itemId].name + " * " + itemNum + "\n";
					}
				}
			}

			DataHelper.UpdatePlayerAttr (gData.characterList [0], playerInfo);

			for (int i=1; i<gData.characterList.Count; i++) {
				for (int j=0; j<assistList.Count; j++) {

					Dictionary<string,object> assitInfo = (Dictionary<string,object>)assistList [j];

					if (gData.characterList [i].dbid == int.Parse (assitInfo ["dbid"].ToString ())) {
						DataHelper.UpdatePlayerAttr (gData.characterList [i], assitInfo);
						break;
					}
				}
			}		

			//更新背包
			DataHelper.UpdateBag(gData.characterList[0],bag,gData.siList);

			//标记删除敌人
			gData.enemyNeedRemove = enemy_dbid;


			ShowHint.Hint (showMsg);
			backToMain = true;
		}
	}

	public void HintBattleGet ()
	{
		if (backToMain) {
			gData.victory = true;
			Application.LoadLevel ("main");	
		}
	}

}
