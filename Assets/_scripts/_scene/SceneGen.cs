using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public GameObject[] enemyPrefabs;
	public Transform ground;
	public Transform groundItem;
	public Transform player;
	public Transform enemys;
	public Transform digs;
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
	private float digSide;
	private float playerSide;
	private List<Character> cList;
	private GameObject currentDigging;
	private bool digging;
	private float digTimer;
	private UI_Input uiInput;

	void Start ()
	{

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
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		int currentFloor = gData.currentFloor;
		int scenesNum = gData.scenes.Count;

		digPrefab =  Resources.Load ("Dig", typeof(GameObject)) as GameObject;
		digSide = digPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
		playerSide = player.GetComponent<SpriteRenderer>().bounds.size.x;
		uiInput  = GameObject.FindGameObjectWithTag ("GameController").GetComponent<UI_Input>();
		
		if (scenesNum >= currentFloor) {
			GenerateSceneFromSceneInfo (gData.scenes [currentFloor-1]);
		} else {
			GenerateSceneRandom ();
		}
	}


	public GameObject getDig(Vector3 playerPos){

		for(int i=0;i<digList.Count;i++){
			float dX = Mathf.Abs(playerPos.x - digList[i].transform.position.x);
			float dY = Mathf.Abs(playerPos.y - digList[i].transform.position.y);

			//如果所在位置有坑，返回这个坑
			if(dX<digSide*0.5+playerSide*0.5 && dY<digSide*0.5+playerSide*0.5){
				return digList[i];
			}
			//如果所在位置限制范围内有坑，返回空
			if(dX<digSide && dY<digSide){
				return null;
			}
		}

		//如果位置不违反规则，返回新的坑对象
		GameObject dig = Instantiate (digPrefab, player.position, Quaternion.identity) as GameObject;
		dig.transform.SetParent(digs);
		//生成挖掘点信息
		DigInfo di = dig.GetComponent<DigInfo>();
		di.currentDeep = 0;
		di.texType = 0;
		//根据当前层数,随机生成深度
		di.deep = Random.Range(gData.currentFloor*10,gData.currentFloor*20);
		digList.Add(dig);
		return dig;
	}

	public void DigInMap(List<Character> cList){

		this.cList = cList;

		//在当前位置开始挖掘
		GameObject dig = getDig(player.transform.position);

		if(dig==null){
			Debug.Log("此处貌似有坚硬的石块，换个地方试试吧");
			uiInput.SendMessage("DigStop");
		}else{
			//开始挖掘
			currentDigging = dig;
			digging = true;
		}
	}

	public void Digging(){
		int sumStrength = 0;
		for(int i=0;i<cList.Count;i++){
			if(cList[i].Stamina>0){
				sumStrength+=cList[i].strength;
			}
		}
		//一点力量挖掘一个深度
		DigInfo di = currentDigging.GetComponent<DigInfo>();
		di.currentDeep += sumStrength;
		if(di.currentDeep>(di.texType+1)*(di.deep*0.3333)){
			di.texType++;
			currentDigging.GetComponent<SpriteRenderer>().sprite = Resources.Load <Sprite>("_images/_game/dig_"+Mathf.Min(2,di.texType));
		}

		//一次挖掘扣除3点体能
		for(int i=0;i<cList.Count;i++){
			if(cList[i].Stamina>0){
				cList[i].Stamina = Mathf.Max(0,cList[i].Stamina - 3);
			}
		}

		if(di.currentDeep>=di.deep){
			digging = false;
			Debug.Log("挖到底了.............");
			uiInput.SendMessage("DigStop");
		}
	}

	//UI点击停止挖掘，传递给这个函数
	public void StopDigInMap(){
		digging = false;
	}

	void Update(){

		digTimer+=Time.deltaTime;

		if(digging && digTimer>3){
			Digging();
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

		for (int i=0; i<blockData.Count; i++) {
			string prefabName = blockData[i].objName.Replace("(Clone)","");
			object obj = Resources.Load (prefabName, typeof(GameObject));
			GameObject blockObj = obj as GameObject;
			GameObject block = Instantiate (blockObj, blockData[i].pos, Quaternion.identity) as GameObject;
			block.transform.eulerAngles = blockData[i].eulerAngles;
			block.GetComponent<SpriteRenderer>().sortingOrder = blockData[i].order;
			block.transform.parent = ground;
		}


		for (int i=0; i<itemData.Count; i++) {
			string prefabName = itemData[i].objName.Replace("(Clone)","");
			object obj = Resources.Load (prefabName, typeof(GameObject));
			GameObject itemObj = obj as GameObject;
			GameObject item = Instantiate (itemObj, itemData[i].pos, Quaternion.identity) as GameObject;
			item.GetComponent<SpriteRenderer>().sortingOrder = itemData[i].order;
			item.transform.parent = groundItem;
		}

		//如果是从战斗场景回来,如果胜利,则移除敌人
		ElementData enemyNeedToRemove = null; 

		for (int i=0; i<enemyData.Count; i++) {
			if(!gData.victory || !gData.currentEnemyName.Equals(enemyData[i].objName)){
				string prefabName = enemyData[i].objName.Replace("(Clone)","").Split(new char[]{'@'})[0];
				object obj = Resources.Load (prefabName, typeof(GameObject));
				GameObject enemyObj = obj as GameObject;
				GameObject enemy = Instantiate (enemyObj, enemyData[i].pos, Quaternion.identity) as GameObject;
				enemy.name = enemyData[i].objName;
				enemy.GetComponent<SpriteRenderer>().sortingOrder = itemData[i].order;
				enemy.transform.parent = enemys;
			}else if(gData.victory && gData.currentEnemyName.Equals(enemyData[i].objName)){
				enemyNeedToRemove = enemyData[i];
			}
		}

		if(enemyNeedToRemove!=null){
			enemyData.Remove(enemyNeedToRemove);
		}

		//加载挖掘点		
		for (int i=0; i<digData.Count; i++) {
			GameObject dig = Instantiate (digPrefab, digData[i].pos, Quaternion.identity) as GameObject;
			dig.GetComponent<SpriteRenderer>().sortingOrder = itemData[i].order;
			DigInfo di = dig.GetComponent<DigInfo>();
			di.deep = ((DigData)digData[i]).deep;
			di.currentDeep = ((DigData)digData[i]).currentDeep;
			di.texType =  ((DigData)digData[i]).texType;
			//更换贴图
			dig.GetComponent<SpriteRenderer>().sprite = Resources.Load <Sprite>("_images/_game/dig_"+di.texType);
			dig.transform.parent = digs;
			//为了用于可挖掘位置的判断
			digList.Add(dig);
		}
	}

	public void GenerateSceneRandom ()
	{
		GenerateGround ();
		ReplaceTex ();
		GenerateElements ();
		gData.scenes.Add (new SceneInfo());
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
				itemO.GetComponent<SpriteRenderer> ().sortingOrder = 5;
				itemO.transform.parent = groundItem;
				itemList.Add(itemO);
			}

			//生成敌人
			int start = level * typesOfEnemyInOneGroup;
			int end = start + typesOfEnemyInOneGroup;

			int enemyNum = Random.Range (minEnemyNum, maxEnemyNum);

			for (int j=0; j<enemyNum; j++) {
				GameObject enemy = enemyPrefabs [Random.Range (start, end)];
				Vector3 enemyPos = getRandomPos (blockPos, enemy);
				GameObject enemyO = Instantiate (enemy, enemyPos, Quaternion.identity) as GameObject;
				enemyO.name = enemyO.name + "@"+num;
				enemyO.GetComponent<SpriteRenderer> ().sortingOrder = 5;
				enemyO.transform.parent = enemys;
				enemyList.Add(enemyO);
				num++;
			}
		}
	}

	//记录场景信息
	public void RecScene(){

		//场景切换之间不会有变动的数据，只在首次保存层时记录
		if (currentSceneInfo.BlockData == null || currentSceneInfo.BlockData.Count == 0) {
			for (int i=0; i<blockList.Count; i++) {
				GameObject blockO = blockList[i];
				ElementData ed = new ElementData(blockO.transform.position,blockO.name,blockO.transform.eulerAngles,blockO.GetComponent<SpriteRenderer>().sortingOrder);
				blockData.Add(ed);
			}
			currentSceneInfo.BlockData = blockData;

			for (int i=0; i<itemList.Count; i++) {
				GameObject itemO = itemList[i];
				ElementData ed = new ElementData(itemO.transform.position,itemO.name,itemO.transform.eulerAngles,itemO.GetComponent<SpriteRenderer>().sortingOrder);
				itemData.Add(ed);
			}
			currentSceneInfo.ItemData = itemData;


			for (int i=0; i<enemyList.Count; i++) {
				GameObject enemyO = enemyList[i];
				ElementData ed = new ElementData(enemyO.transform.position,enemyO.name,enemyO.transform.eulerAngles,enemyO.GetComponent<SpriteRenderer>().sortingOrder);
				enemyData.Add(ed);
			}
			currentSceneInfo.EnemyData = enemyData;
		}

		//挖掘点数据在从主场景切换到战斗场景的过程中,贴图会发生变动，所以每次记录场景时都要更新
		//先清除原数据(由于digList和digData之间没有关联，所以不方便在更新dig贴图时，同时更新digData的数据，这里在保存场景时全部重新保存)
		digData.Clear();
		for (int i=0; i<digList.Count; i++) {
			GameObject digO = digList[i];
			AddNewDigData(digO);
		}

		Debug.Log(digData.Count);

		currentSceneInfo.DigData = digData;
		
		gData.scenes[gData.currentFloor-1] = currentSceneInfo;
	}

	void AddNewDigData(GameObject digO){
		DigInfo di = digO.GetComponent<DigInfo>();
		DigData dd = new DigData(digO.transform.position,digO.name,digO.transform.eulerAngles,digO.GetComponent<SpriteRenderer>().sortingOrder,di.deep,di.currentDeep,di.texType);
		digData.Add(dd);
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

		float pX = Random.Range (blockPos.x - blockX / 2 + border + elementWidth / 2, blockPos.x + blockX / 2 - border - elementWidth / 2);
		float pY = Random.Range (blockPos.y - blockY / 2 + border + elementHeight / 2, blockPos.y + blockY / 2 - border - elementHeight / 2);

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
			blockList[i] = block;
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

		if (blockX == 0 || blockY == 0) {
			blockX = block.GetComponent<SpriteRenderer> ().bounds.size.x * 15 / 16;
			blockY = block.GetComponent<SpriteRenderer> ().bounds.size.y * 15 / 16;
		}

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
