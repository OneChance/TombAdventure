using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneGen: MonoBehaviour
{

	public GameObject groundBlock;
	public int maxBlockNum = 20;
	public int maxItemNum = 10;
	public int minItemNum = 3;
	public int level = 0;
	public int typesOfEnemyInOneGroup = 1;
	public int maxEnemyNum = 4;
	public int minEnemyNum = 2;
	public GameObject[] groundPrefabs;
	public GameObject[] itemPrefabs;
	public GameObject enemyPrefab;
	private List<Enemy> enemyTypes;
	public Transform ground;
	public Transform groundItem;
	public Transform player;
	public Transform enemys;
	public Transform digs;
	public int digDeepMin;
	public int digDeepMax;

	//边界墙的厚度
	public float wallSize = 1.5f;
	//下层入口的半径
	public float entryRadius = 3f;
	private List<Vector3> ablePos;
	private List<Vector3> addedPos;
	private float blockX;
	private float blockY;
	private List<GameObject> blockList;
	private List<GameObject> itemList;
	private List<GameObject> enemyList;
	private List<GameObject> digList;
	private List<ElementData> itemData;
	private List<ElementData> blockData;
	private List<ElementData> enemyData;
	private List<ElementData> digData;
	private Vector3 genPos;
	private float border = 2.5f;
	private GlobalData gData;
	private SceneInfo currentSceneInfo;
	private GameObject digPrefab;
	private GameObject preEntryPrefab;
	private GameObject coffinPrefab;
	private float digSide;
	private float playerSide;
	private List<Character> cList;
	private GameObject currentDigging;
	private bool digging;
	private float digTimer;
	private UI_Input uiInput;
	private Dictionary<string,List<FallItem>> fallList;

	void Start ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();

		if (!gData.victory) {

		}

		blockX = groundPrefabs [0].GetComponent<SpriteRenderer> ().bounds.size.x * 15 / 16;
		blockY = groundPrefabs [0].GetComponent<SpriteRenderer> ().bounds.size.y * 15 / 16;

		enemyTypes = new List<Enemy> ();
		currentSceneInfo = new SceneInfo ();
		ablePos = new List<Vector3> ();
		addedPos = new List<Vector3> ();
		blockList = new List<GameObject> ();
		itemList = new List<GameObject> ();
		enemyList = new List<GameObject> ();
		digList = new List<GameObject> ();
		blockData = new List<ElementData> ();
		itemData = new List<ElementData> ();
		enemyData = new List<ElementData> ();
		digData = new List<ElementData> ();
		genPos = new Vector3 (0, 0, 0);

		int currentFloor = gData.currentFloor;
		int scenesNum = gData.currentTomb.sceneList.Count;

		//根据KEY从服务器加载物品掉落列表
		//返回挖掘掉落,敌人掉落,棺材掉落列表(key: tombLevel_currentFloor) 
		fallList = getFallList (gData.currentTomb.tombLevel + "_" + gData.currentFloor);
		getEnemyTypes (gData.currentTomb.tombLevel + "_" + gData.currentFloor);

		digPrefab = Resources.Load ("Dig", typeof(GameObject)) as GameObject;
		digSide = digPrefab.GetComponent<SpriteRenderer> ().bounds.size.x;
		playerSide = player.GetComponent<SpriteRenderer> ().bounds.size.x;
		uiInput = GameObject.FindGameObjectWithTag ("GameController").GetComponent<UI_Input> ();

		preEntryPrefab = Resources.Load ("PreEntry", typeof(GameObject)) as GameObject;
		coffinPrefab = Resources.Load ("Coffin", typeof(GameObject)) as GameObject;

		//不论是新的场景或是原来保存的场景，通往上一层的入口的位置总是在玩家(0,0,0)的位置，所以这个数据不用保存到全局数据的场景数据里
		//在玩家位置生成上一层入口,由于该位置处不应该有敌人或场景物品，所以放在其他元素之前生成
		Instantiate (preEntryPrefab, player.position, Quaternion.identity);

		if (scenesNum >= currentFloor) {
			GenerateSceneFromSceneInfo (gData.currentTomb.sceneList [currentFloor - 1]);
		} else {
			//生成场景
			GenerateSceneRandom ();
		}


		//场景信息
		Transform sceneInfoUI = GameObject.FindGameObjectWithTag ("UI").transform.FindChild ("SceneInfo");
		sceneInfoUI.FindChild ("TombName").GetComponent<Text> ().text = gData.currentTomb.tombName;
		sceneInfoUI.FindChild ("FloorLable").GetComponent<Text> ().text = StringCollection.FLOOR;
		sceneInfoUI.FindChild ("Floor").GetComponent<Text> ().text = gData.currentFloor.ToString ();

		//自动上传当前记录
	}

	//根据场景key去服务获取当前场景敌人类别
	void getEnemyTypes (string key)
	{
		Enemy e1 = new Enemy (100, 20, 0, 0, StringCollection.E_1, "enemy_1", 300);
		Enemy e2 = new Enemy (30, 5, 0, 0, StringCollection.E_2, "enemy_2", 300);

		enemyTypes.Add (e1);
		enemyTypes.Add (e2);
	}

	Dictionary<string,List<FallItem>> getFallList (string key)
	{

		//此处请求服务器

		Dictionary<string,List<FallItem>> fallList = new Dictionary<string,List<FallItem>> ();
		List<FallItem> itemList = new List<FallItem> ();
		FallItem fi = new FallItem ();
		fi.item = new HealthItem ((int)Item.RangeType.SINGLE, 10, "1", "单体治疗药剂", 50);
		fi.minNum = 1;
		fi.maxNum = 3;
		fi.probability = 90;
		itemList.Add (fi);

		fallList.Add ("dig", itemList);
		fallList.Add ("enemy_1", itemList);
		fallList.Add ("enemy_2", itemList);

		return fallList;
	}

	public GameObject getDig (Vector3 playerPos)
	{

		for (int i=0; i<digList.Count; i++) {
			float dX = Mathf.Abs (playerPos.x - digList [i].transform.position.x);
			float dY = Mathf.Abs (playerPos.y - digList [i].transform.position.y);

			//如果所在位置有坑，返回这个坑
			if (dX < digSide * 0.5 + playerSide * 0.5 && dY < digSide * 0.5 + playerSide * 0.5) {
				return digList [i];
			}
			//如果所在位置限制范围内有坑，返回空
			if (dX < digSide && dY < digSide) {
				return null;
			}
		}

		//如果位置不违反规则，返回新的坑对象
		GameObject dig = Instantiate (digPrefab, player.position, Quaternion.identity) as GameObject;
		dig.transform.SetParent (digs);
		//生成挖掘点信息
		DigInfo di = dig.GetComponent<DigInfo> ();
		di.currentDeep = 0;
		di.texType = 0;
		//根据当前层数,随机生成深度
		di.deep = Random.Range (gData.currentFloor * digDeepMin, gData.currentFloor * digDeepMax);
		digList.Add (dig);
		return dig;
	}

	//挖掘
	public void DigInMap (List<Character> cList)
	{

		if (!checkBlockSide ()) {
			this.cList = cList;
			
			//在当前位置开始挖掘
			GameObject dig = getDig (player.transform.position);
			
			if (dig == null) {
				ShowHint.Hint (StringCollection.CANNOTDIG);
				uiInput.SendMessage ("DigStop");
			} else {
				//如果这个坑已经挖完
				DigInfo di = dig.GetComponent<DigInfo> ();
				if (di.currentDeep >= di.deep) {
					ShowHint.Hint (StringCollection.DIGOVER);
					uiInput.SendMessage ("DigStop");
				} else {
					//开始挖掘
					currentDigging = dig;
					digging = true;
				}
			}
		} else {
			ShowHint.Hint (StringCollection.ALONGWALL);
			uiInput.SendMessage ("DigStop");
		}
	}

	public void Digging ()
	{
		int sumDigPower = 0;

		for (int i=0; i<cList.Count; i++) {
			if (cList [i].Stamina > 0) {
				sumDigPower += cList [i].digPower;
			}
		}

		if (sumDigPower == 0) {
			ShowHint.Hint (StringCollection.NOSTAMINA);
			digging = false;
		} else {
			DigInfo di = currentDigging.GetComponent<DigInfo> ();
			di.currentDeep += sumDigPower;    //一点力量挖掘一个深度
			if (di.currentDeep > (di.texType + 1) * (di.deep * 0.3333)) {
				di.texType++;
				currentDigging.GetComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("_images/_game/dig_" + Mathf.Min (2, di.texType));
			}
			
			//一次挖掘扣除3点体能
			for (int i=0; i<cList.Count; i++) {
				if (cList [i].Stamina > 0) {
					cList [i].Stamina = Mathf.Max (0, cList [i].Stamina - 3);
				}
			}
			
			if (di.currentDeep >= di.deep) {

				digging = false;
				
				float digPosX = currentDigging.transform.position.x;
				float digPosY = currentDigging.transform.position.y;
				
				if (digPosX < (currentSceneInfo.nextEntry.pos.x + entryRadius) && digPosX > (currentSceneInfo.nextEntry.pos.x - entryRadius) &&
					digPosY < (currentSceneInfo.nextEntry.pos.y + entryRadius) && digPosY > (currentSceneInfo.nextEntry.pos.y - entryRadius)) {
					//挖掘地点在入口半径之内
					di.texType = 3;
					currentDigging.GetComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("_images/_game/dig_" + di.texType); //换成下层入口贴图
					currentSceneInfo.digToNextPos = player.position;//记录位置
					RecScene ();
					gData.currentFloor++;
					Application.LoadLevel ("main");
				} else {
					ItemFall ("dig");
				}
				uiInput.SendMessage ("DigStop");
			}

			//更新耐力变化
			uiInput.SendMessage ("UpdateUIInfo");
		}
	}

	void ItemFall (string key)
	{
		//掉落******************************************************************************************
		List<FallItem> itemList = fallList [key];		
		string itemGet = "";				
		for (int i=0; i<itemList.Count; i++) {
			if (GameUtil.RandomHappen (itemList [i].probability, 101)) {					
				int num = Random.Range (itemList [i].minNum, itemList [i].maxNum + 1);						
				itemGet = itemGet + " " + itemList [i].item.name + "x" + num;
				
				Baggrid baggrid = new Baggrid (itemList [i].item, num,-1);
				
				
				//加入背包(如果是雇佣兵模式，道具由玩家获得，如果是玩家最对模式，道具将roll获取)
				bool get = false;
				if (gData.isPlayer) {
					//弹出面板，roll
					//服务器返回队内其他玩家的roll值，判定
					//如果roll 到 get设置为true
				} else {
					get = true;
				}
				
				if (get) {
					BagUtil.AddItem (gData.characterList [0].BgList, baggrid);
				}
			}
		}				
		if (itemGet.Equals ("")) {
			ShowHint.Hint (StringCollection.DIGNOTHING);
		} else {
			ShowHint.Hint (StringCollection.ITEMGET + ":" + itemGet);
		}
		//***********************************************************************************************
	}

	//UI点击停止挖掘，传递给这个函数
	public void StopDigInMap ()
	{
		digging = false;
	}

	void Update ()
	{
		digTimer += Time.deltaTime;
		if (digging && digTimer > 3) {
			Digging ();
			digTimer = 0;
		}
	}


	//从保存的数据中加载场景
	void GenerateSceneFromSceneInfo (SceneInfo sceneInfo)
	{
		currentSceneInfo = sceneInfo;

		blockData = currentSceneInfo.BlockData;
		itemData = currentSceneInfo.ItemData;
		enemyData = currentSceneInfo.EnemyData;
		digData = currentSceneInfo.DigData;

		//修改玩家位置为保存的位置
		player.position = gData.playerPos;
		gData.playerPos = new Vector3 (0, 0, 0);

		for (int i=0; i<blockData.Count; i++) {
			string prefabName = blockData [i].objName.Replace ("(Clone)", "");
			object obj = Resources.Load (prefabName, typeof(GameObject));
			GameObject blockObj = obj as GameObject;
			GameObject block = Instantiate (blockObj, blockData [i].pos, Quaternion.identity) as GameObject;
			block.transform.eulerAngles = blockData [i].eulerAngles;
			block.GetComponent<SpriteRenderer> ().sortingOrder = blockData [i].order;
			block.transform.parent = ground;
		}


		for (int i=0; i<itemData.Count; i++) {
			string prefabName = itemData [i].objName.Replace ("(Clone)", "");
			object obj = Resources.Load (prefabName, typeof(GameObject));
			GameObject itemObj = obj as GameObject;
			GameObject item = Instantiate (itemObj, itemData [i].pos, Quaternion.identity) as GameObject;
			item.GetComponent<SpriteRenderer> ().sortingOrder = itemData [i].order;
			item.transform.parent = groundItem;
		}

		//如果是从战斗场景回来,如果胜利,则移除敌人
		ElementData enemyNeedToRemove = null; 

		for (int i=0; i<enemyData.Count; i++) {
			if (!gData.victory || !gData.currentEnemyName.Equals (enemyData [i].objName)) {
				GameObject enemy = Instantiate (enemyPrefab, enemyData [i].pos, Quaternion.identity) as GameObject;
				enemy.name = enemyData [i].objName;
				enemy.GetComponent<SpriteRenderer> ().sortingOrder = enemyData [i].order;
				enemy.GetComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("_images/_game/" + enemyData [i].objName.Split (new char[]{'@'}) [0]);
				enemy.transform.parent = enemys;
			} else if (gData.victory && gData.currentEnemyName.Equals (enemyData [i].objName)) {
				enemyNeedToRemove = enemyData [i];
			}
		}

		if (enemyNeedToRemove != null) {
			//敌人掉落
			ItemFall (enemyNeedToRemove.objName.Replace ("(Clone)", "").Split (new char[]{'@'}) [0]);
			enemyData.Remove (enemyNeedToRemove);
		}

		//加载挖掘点		
		for (int i=0; i<digData.Count; i++) {
			GameObject dig = Instantiate (digPrefab, digData [i].pos, Quaternion.identity) as GameObject;
			dig.GetComponent<SpriteRenderer> ().sortingOrder = itemData [i].order;
			DigInfo di = dig.GetComponent<DigInfo> ();
			di.deep = ((DigData)digData [i]).deep;
			di.currentDeep = ((DigData)digData [i]).currentDeep;
			di.texType = ((DigData)digData [i]).texType;
			//更换贴图
			dig.GetComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("_images/_game/dig_" + di.texType);
			dig.transform.parent = digs;
			//为了用于可挖掘位置的判断
			digList.Add (dig);
		}
	}

	//随机生成场景
	public void GenerateSceneRandom ()
	{
		//根据层数，随机判定当前层是否是墓穴
		if (Random.Range (1, 101) < Mathf.Min (9, gData.currentFloor) * 10) {
			currentSceneInfo.isTomb = true;
		}

		//生成地砖
		GenerateGround ();
		//替换，旋转材质
		ReplaceTex ();
		//如果当前层不是墓穴，生成通往下一层的入口，否则生成棺材位置
		GenerateNextEntryOrTomb ();
		if (currentSceneInfo.isTomb) {
			GeneraterTomb ();
		}
		//创建地图元素，场景，敌人
		GenerateElements ();
		//添加场景信息到全局数据对象
		gData.currentTomb.sceneList.Add (currentSceneInfo);
	}

	void GeneraterTomb ()
	{
		//根据墓穴等级，层数，决定棺材贴图名称
		string texName = "coffin_" + gData.currentTomb.tombLevel + "_" + ((int)((gData.currentFloor + 1) / 2));
		GameObject coffinO = Instantiate (coffinPrefab, currentSceneInfo.nextEntry.pos, Quaternion.identity) as GameObject;
		coffinO.GetComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("_images/_game/" + texName);
	}

	void GenerateNextEntryOrTomb ()
	{
		//随机获取一个砖块(除玩家所在砖块以外)
		GameObject block = blockList [Random.Range (1, blockList.Count)];
		float blockPosX = block.transform.position.x;
		float blockPosY = block.transform.position.y;
		//随机获取一个位置
		float pX = Random.Range (blockPosX - blockX * 0.5f + border + entryRadius, blockPosX + blockX * 0.5f - border - entryRadius);
		float pY = Random.Range (blockPosY - blockY * 0.5f + border + entryRadius, blockPosY + blockY * 0.5f - border - entryRadius); 
		ElementData nextEntry = new ElementData (new Vector3 (pX, pY, 0), "NextEntry", new Vector3 (0, 0, 0), 0);
		currentSceneInfo.nextEntry = nextEntry;
		//测试
//		if(gData.currentFloor==1){
//			player.position = nextEntry.pos;
//		}
	}

	private void GenerateElements ()
	{

		int num = 0; //the sequence of enemy

		for (int i=0; i<blockList.Count; i++) {
			GameObject block = blockList [i];			
			Vector3 blockPos = block.transform.position;

			//生成场景景物
			int itemNum = Random.Range (minItemNum, maxItemNum);

			for (int j=0; j<itemNum; j++) {
				GameObject item = itemPrefabs [Random.Range (0, itemPrefabs.Length)];
				Vector3 itemPos = getRandomPos (blockPos, item);
				GameObject itemO = Instantiate (item, itemPos, Quaternion.identity) as GameObject;
				itemO.transform.parent = groundItem;
				itemList.Add (itemO);
			}

			int enemyNum = Random.Range (minEnemyNum, maxEnemyNum);

			for (int j=0; j<enemyNum; j++) {
				Vector3 enemyPos = getRandomPos (blockPos, enemyPrefab);
				GameObject enemyO = Instantiate (enemyPrefab, enemyPos, Quaternion.identity) as GameObject;
				enemyO.GetComponent<SpriteRenderer> ().sortingOrder = 5;
				enemyO.transform.parent = enemys;
				Enemy e = enemyTypes [Random.Range (0, enemyTypes.Count)];
				enemyO.name = e.PrefabName + "@" + num;
				enemyO.GetComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("_images/_game/" + e.PrefabName);
				enemyO.GetComponent<EnemyAI> ().enemy = e;
				enemyList.Add (enemyO);
				num++;
			}
		}
	}

	//记录场景信息
	public void RecScene ()
	{

		//场景切换之间不会有变动的数据，只在首次保存层时记录
		if (currentSceneInfo.BlockData == null || currentSceneInfo.BlockData.Count == 0) {
			for (int i=0; i<blockList.Count; i++) {
				GameObject blockO = blockList [i];
				ElementData ed = new ElementData (blockO.transform.position, blockO.name, blockO.transform.eulerAngles, blockO.GetComponent<SpriteRenderer> ().sortingOrder);
				blockData.Add (ed);
			}
			currentSceneInfo.BlockData = blockData;

			for (int i=0; i<itemList.Count; i++) {
				GameObject itemO = itemList [i];
				ElementData ed = new ElementData (itemO.transform.position, itemO.name, itemO.transform.eulerAngles, itemO.GetComponent<SpriteRenderer> ().sortingOrder);
				itemData.Add (ed);
			}
			currentSceneInfo.ItemData = itemData;


			for (int i=0; i<enemyList.Count; i++) {
				GameObject enemyO = enemyList [i];
				ElementData ed = new ElementData (enemyO.transform.position, enemyO.name, enemyO.transform.eulerAngles, enemyO.GetComponent<SpriteRenderer> ().sortingOrder);
				enemyData.Add (ed);
			}
			currentSceneInfo.EnemyData = enemyData;
		}

		//挖掘点数据在从主场景切换到战斗场景的过程中,贴图会发生变动，所以每次记录场景时都要更新
		//先清除原数据(由于digList和digData之间没有关联，所以不方便在更新dig贴图时，同时更新digData的数据，这里在保存场景时全部重新保存)
		digData.Clear ();
		for (int i=0; i<digList.Count; i++) {
			GameObject digO = digList [i];
			AddNewDigData (digO);
		}

		currentSceneInfo.DigData = digData;

		gData.currentTomb.sceneList [gData.currentFloor - 1] = currentSceneInfo;
	}

	void AddNewDigData (GameObject digO)
	{
		DigInfo di = digO.GetComponent<DigInfo> ();
		DigData dd = new DigData (digO.transform.position, digO.name, digO.transform.eulerAngles, digO.GetComponent<SpriteRenderer> ().sortingOrder, di.deep, di.currentDeep, di.texType);
		digData.Add (dd);
	}

	private Vector3 getRandomPos (Vector3 blockPos, GameObject element)
	{
		Vector3 randomPos = getValidPos (blockPos, element);
		while (randomPos==new Vector3(-999,-999,-999)) {
			randomPos = getValidPos (blockPos, element);
		}
		return randomPos;
	}

	//获取有效的位置,用于生成场景元素
	private Vector3 getValidPos (Vector3 blockPos, GameObject element)
	{

		float elementWidth = element.GetComponent<SpriteRenderer> ().bounds.size.x;
		float elementHeight = element.GetComponent<SpriteRenderer> ().bounds.size.y;

		float pX = Random.Range (blockPos.x - blockX * 0.5f + border + elementWidth * 0.5f, blockPos.x + blockX * 0.5f - border - elementWidth * 0.5f);
		float pY = Random.Range (blockPos.y - blockY * 0.5f + border + elementHeight * 0.5f, blockPos.y + blockY * 0.5f - border - elementHeight * 0.5f);

		Vector3 pos = new Vector3 (pX, pY, blockPos.z);

		List<string> checkedTags = new List<string> ();
		checkedTags.Add ("Player");
		checkedTags.Add ("GroundItem");
		checkedTags.Add ("Enemy");

		return Zhstar_2D_Common.checkPosValid (pos, checkedTags, elementWidth, elementHeight);
	}
	
	//根据场景地砖的位置，替换贴图，旋转得到正确的位置
	private void ReplaceTex ()
	{

		int order = 0;

		for (int i=0; i<blockList.Count; i++) {
			GameObject block = blockList [i];

			Vector3 blockPos = block.transform.position;

			Destroy (block);

			List<PosInfo> round = GetRoundPos (blockPos);

			bool haveLeft = false;
			bool haveUp = false;
			bool haveRight = false;
			bool haveDown = false;

			for (int j=0; j<round.Count; j++) {
				switch (haveBlock (round [j])) {
				case Direction.Down:
					haveDown = true;
					break;
				case Direction.Up:
					haveUp = true;
					break;
				case Direction.Left:
					haveLeft = true;
					break;
				case Direction.Right:
					haveRight = true;
					break;
				default:
					break;				
				}
			}

			if (haveRight && !haveDown && !haveUp && !haveLeft) {
				block = Instantiate (groundPrefabs [0], block.transform.position, Quaternion.identity) as GameObject;
				order = 4;
			} else if (haveLeft && !haveDown && !haveUp && !haveRight) {
				block = Instantiate (groundPrefabs [0], block.transform.position, Quaternion.identity) as GameObject;
				block.transform.Rotate (new Vector3 (0, 0, 1), 180);
				order = 4;
			} else if (haveDown && !haveLeft && !haveUp && !haveRight) {
				block = Instantiate (groundPrefabs [0], block.transform.position, Quaternion.identity) as GameObject;
				block.transform.Rotate (new Vector3 (0, 0, 1), -90);
				order = 4;
			} else if (haveUp && !haveDown && !haveLeft && !haveRight) {
				block = Instantiate (groundPrefabs [0], block.transform.position, Quaternion.identity) as GameObject;
				block.transform.Rotate (new Vector3 (0, 0, 1), 90);
				order = 4;
			} else if (haveDown && haveRight && !haveUp && !haveLeft) {
				block = Instantiate (groundPrefabs [1], block.transform.position, Quaternion.identity) as GameObject;
				order = 2;
			} else if (haveUp && haveRight && !haveDown && !haveLeft) {
				block = Instantiate (groundPrefabs [1], block.transform.position, Quaternion.identity) as GameObject;
				block.transform.Rotate (new Vector3 (0, 0, 1), 90);
				order = 2;
			} else if (haveUp && haveLeft && !haveDown && !haveRight) {
				block = Instantiate (groundPrefabs [1], block.transform.position, Quaternion.identity) as GameObject;
				block.transform.Rotate (new Vector3 (0, 0, 1), 180);
				order = 2;
			} else if (haveDown && haveLeft && !haveUp && !haveRight) {
				block = Instantiate (groundPrefabs [1], block.transform.position, Quaternion.identity) as GameObject;
				block.transform.Rotate (new Vector3 (0, 0, 1), -90);
				order = 2;
			} else if (haveRight && haveLeft && !haveUp && !haveDown) {
				block = Instantiate (groundPrefabs [2], block.transform.position, Quaternion.identity) as GameObject;
				order = 3;
			} else if (haveUp && haveDown && !haveRight && !haveLeft) {
				block = Instantiate (groundPrefabs [2], block.transform.position, Quaternion.identity) as GameObject;
				block.transform.Rotate (new Vector3 (0, 0, 1), -90);
				order = 3;
			} else if (!haveUp && haveDown && haveRight && haveLeft) {
				block = Instantiate (groundPrefabs [3], block.transform.position, Quaternion.identity) as GameObject;
				block.transform.Rotate (new Vector3 (0, 0, 1), -90);
				order = 1;
			} else if (!haveLeft && haveDown && haveRight && haveUp) {
				block = Instantiate (groundPrefabs [3], block.transform.position, Quaternion.identity) as GameObject;
				order = 1;
			} else if (!haveRight && haveDown && haveLeft && haveUp) {
				block = Instantiate (groundPrefabs [3], block.transform.position, Quaternion.identity) as GameObject;
				block.transform.Rotate (new Vector3 (0, 0, 1), -180);
				order = 1;
			} else if (!haveDown && haveRight && haveLeft && haveUp) {
				block = Instantiate (groundPrefabs [3], block.transform.position, Quaternion.identity) as GameObject;
				block.transform.Rotate (new Vector3 (0, 0, 1), 90);
				order = 1;
			} else {
				block = Instantiate (groundPrefabs [4], block.transform.position, Quaternion.identity) as GameObject;
				order = 0;
			}
			 
			block.GetComponent<SpriteRenderer> ().sortingOrder = order;
			block.transform.parent = ground;
			blockList [i] = block;
		}
	}

	//判断该位置有没有砖块，如果有，返回这个位置的方位
	private Direction haveBlock (PosInfo pi)
	{
		for (int i=0; i<blockList.Count; i++) {
			if (blockList [i].transform.position == pi.Pos) {
				return pi.Direction;
			}
		}
		return Direction.Error;
	}

	//判断是否靠近了墙壁而无法挖掘
	private bool checkBlockSide ()
	{
		RaycastHit2D[] hitsUp = Physics2D.RaycastAll (player.position, Vector2.up);
		RaycastHit2D[] hitsLeft = Physics2D.RaycastAll (player.position, Vector2.left);
		RaycastHit2D[] hitsDown = Physics2D.RaycastAll (player.position, Vector2.down);
		RaycastHit2D[] hitsRight = Physics2D.RaycastAll (player.position, Vector2.right);

		if (getCollideBlock (hitsUp) != null && isBlockSide (getCollideBlock (hitsUp), Direction.Up)) {
			return true;
		} else if (getCollideBlock (hitsLeft) != null && isBlockSide (getCollideBlock (hitsLeft), Direction.Left)) {
			return true;
		} else if (getCollideBlock (hitsDown) != null && isBlockSide (getCollideBlock (hitsDown), Direction.Down)) {
			return true;
		} else if (getCollideBlock (hitsRight) != null && isBlockSide (getCollideBlock (hitsRight), Direction.Right)) {
			return true;
		}

		return false;
	}

	//获得射线碰到的砖块	
	private GameObject getCollideBlock (RaycastHit2D[] hits)
	{

		GameObject block = null;

		for (int i=0; i<hits.Length; i++) {
			if (hits [i].collider.gameObject.name.Contains ("ground-")) {
				block = hits [i].collider.gameObject;
				break;
			}
		}
	
		return block;
	}

	//检测玩家在dir方向上，与blockGo的边缘距离是否满足创建坑,wallSize是边界墙的厚度，根据图片内容，可在面板中调节
	private bool isBlockSide (GameObject blockGo, Direction dir)
	{

		Vector3 blockPos = blockGo.transform.position;
		Vector3 playerPos = player.position;
		float sideDis = 10;

		switch (dir) {
		case Direction.Up:
			sideDis = Mathf.Abs ((playerPos.y + playerSide * 0.5f) - (blockPos.y + blockY * 0.5f));
			break;
		case Direction.Left:
			sideDis = Mathf.Abs ((playerPos.x - playerSide * 0.5f) - (blockPos.x - blockX * 0.5f));
			break;
		case Direction.Down:
			sideDis = Mathf.Abs ((playerPos.y - playerSide * 0.5f) - (blockPos.y - blockY * 0.5f));
			break;
		case Direction.Right:
			sideDis = Mathf.Abs ((playerPos.x + playerSide * 0.5f) - (blockPos.x + blockX * 0.5f));
			break;
		}

		if (sideDis < wallSize) {
			return true;
		} else {
			return false;
		}
	}


	//生成地图
	private void GenerateGround ()
	{
		for (int i=0; i<maxBlockNum; i++) {
			GenerateBlock (genPos);
		}
	}

	//生成砖块
	private void GenerateBlock (Vector3 pos)
	{

		addedPos.Add (pos);

		GameObject block = Instantiate (groundBlock, pos, Quaternion.identity) as GameObject;

		blockList.Add (block);

		List<PosInfo> round = GetRoundPos (block.transform.position);
		addAblePos (round);

		genPos = ablePos [Random.Range (0, ablePos.Count)];
		ablePos.Remove (genPos);

	}

	//添加砖块到可使用位置列表，用于后续生成
	void addAblePos (List<PosInfo> newPosList)
	{
		for (int i=0; i<newPosList.Count; i++) {
			if (!ablePos.Contains (newPosList [i].Pos) && !addedPos.Contains (newPosList [i].Pos)) {
				ablePos.Add (newPosList [i].Pos);
			}
		}
	}

	List<PosInfo> GetRoundPos (Vector3 currentPos)
	{
		List	<PosInfo> round = new List<PosInfo> ();

		Vector3 left = new Vector3 (currentPos.x - blockX, currentPos.y, currentPos.z);
		Vector3 up = new Vector3 (currentPos.x, currentPos.y + blockY, currentPos.z);
		Vector3 right = new Vector3 (currentPos.x + blockX, currentPos.y, currentPos.z);
		Vector3 down = new Vector3 (currentPos.x, currentPos.y - blockY, currentPos.z);

		round.Add (new PosInfo (left, Direction.Left));
		round.Add (new PosInfo (up, Direction.Up));
		round.Add (new PosInfo (right, Direction.Right));
		round.Add (new PosInfo (down, Direction.Down));

		return round;
	}

	//根据探测等级返回提示消息
	public void getDetectorResult (int detectLevel)
	{

		Vector3 nextEntry = gData.currentTomb.sceneList [gData.currentFloor - 1].nextEntry.pos;
		
		float distance = Vector3.Distance (player.position, nextEntry);

		if (distance < entryRadius) {
			ShowHint.Hint (StringCollection.TRYTODIG);
		} else {
			if (detectLevel < 100) {
				//这个级别只提示模糊的信息
				if (distance > entryRadius * 4) {
					ShowHint.Hint (StringCollection.DISTANCEFAR);
				} else if (distance > entryRadius * 2) {
					ShowHint.Hint (StringCollection.DISTANCENEAR);
				} else if (distance > entryRadius) {
					ShowHint.Hint (StringCollection.DISTANCECLOSE);
				}
			} else if (detectLevel < 200) {
				//这个级别提示方位
				string x = "";
				string y = "";

				if (Mathf.Abs (nextEntry.x - player.position.x) > entryRadius) {
					if (nextEntry.x > player.position.x) {
						x = StringCollection.RIGHT;
					} else {
						x = StringCollection.LEFT;
					}
				}
				
				if (Mathf.Abs (nextEntry.y - player.position.y) > entryRadius) {
					if (nextEntry.y > player.position.y) {
						y = StringCollection.UP;
					} else {
						y = StringCollection.DOWN;
					}
				}
				
				ShowHint.Hint (StringCollection.POSHINT + x + y);
				
			} else {
				//这个级别提示具体距离
				string x = "";
				string y = "";
				
				if (distance > entryRadius) {
					if (nextEntry.x > player.position.x) {
						x = StringCollection.RIGHT;
					} else {
						x = StringCollection.LEFT;
					}

					int xStep = (int)(Mathf.Abs (nextEntry.x - player.position.x) / playerSide);

					if (xStep > 0) {
						x = x + xStep + StringCollection.STEP;
					}


					if (nextEntry.y > player.position.y) {
						y = StringCollection.UP;
					} else {
						y = StringCollection.DOWN;
					}

					int yStep = (int)(Mathf.Abs (nextEntry.y - player.position.y) / playerSide);

					if (yStep > 0) {
						y = y + yStep + StringCollection.STEP;
					}
				}

				ShowHint.Hint (StringCollection.POSHINT + x + y);
			}
		}
	}

	public void ToPreFloor ()
	{
		RecScene ();
		if (gData.currentFloor == 1) {
			//如果是第一层,回到城市
			Application.LoadLevel ("city");
		} else {
			gData.currentFloor--;
			Vector3 preFloorNextEntryPos = gData.currentTomb.sceneList [gData.currentFloor - 1].digToNextPos;
			gData.playerPos = preFloorNextEntryPos;
			Application.LoadLevel ("main");
		}
	}

	public void ToNextFloor (Vector3 pos)
	{
		gData.currentTomb.sceneList [gData.currentFloor - 1].digToNextPos = pos;
		RecScene ();
		gData.currentFloor++;
		Application.LoadLevel ("main");
	}
	
	public enum Direction
	{  
		Left,  
		Up,				
		Right,
		Down,
		Error
	}

	class PosInfo
	{
		private Vector3 pos;
		private Direction direction;

		public PosInfo (Vector3 pos, global::SceneGen.Direction direction)
		{
			this.pos = pos;
			this.direction = direction;
		}

		public Vector3 Pos {
			get {
				return this.pos;
			}
			set {
				pos = value;
			}
		}

		public Direction Direction {
			get {
				return this.direction;
			}
			set {
				direction = value;
			}
		}
	}
}
