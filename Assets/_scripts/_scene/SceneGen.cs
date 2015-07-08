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
	private List<Vector3> ablePos;
	private List<Vector3> addedPos;
	private float blockX;
	private float blockY;
	private List<GameObject> blockList;
	private List<GameObject> itemList;
	private List<GameObject> enemyList;

	private List<ElementData> itemData;
	private List<ElementData> blockData;
	private List<ElementData> enemyData;

	private Vector3 genPos;
	private float border = 2.5f;
	private GlobalData gData;
	private SceneInfo currentSceneInfo;

	void Start ()
	{
		currentSceneInfo = new SceneInfo ();

		ablePos = new List<Vector3> ();
		addedPos = new List<Vector3> ();

		blockList = new List<GameObject> ();
		itemList = new List<GameObject> ();
		enemyList = new List<GameObject> ();

		blockData = new List<ElementData> ();
		itemData = new List<ElementData> ();
		enemyData = new List<ElementData> ();

		genPos = new Vector3 (0, 0, 0);
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		int currentFloor = gData.currentFloor;
		int scenesNum = gData.scenes.Count;
		
		if (scenesNum >= currentFloor + 1) {
			GenerateSceneFromSceneInfo (gData.scenes [currentFloor]);
		} else {
			GenerateSceneRandom ();
		}
	}


	void GenerateSceneFromSceneInfo (SceneInfo sceneInfo)
	{
		currentSceneInfo = sceneInfo;

		blockData = currentSceneInfo.BlockData;
		itemData = currentSceneInfo.ItemData;
		enemyData = currentSceneInfo.EnemyData;

		for (int i=0; i<blockData.Count; i++) {
			string prefabName = blockData[i].ObjName.Replace("(Clone)","");
			object obj = Resources.Load (prefabName, typeof(GameObject));
			GameObject blockObj = obj as GameObject;
			GameObject block = Instantiate (blockObj, blockData[i].Pos, Quaternion.identity) as GameObject;
			block.transform.eulerAngles = blockData[i].EulerAngles;
			block.GetComponent<SpriteRenderer>().sortingOrder = blockData[i].Order;
			block.transform.parent = ground;
		}

		for (int i=0; i<itemData.Count; i++) {
			string prefabName = itemData[i].ObjName.Replace("(Clone)","");
			object obj = Resources.Load (prefabName, typeof(GameObject));
			GameObject itemObj = obj as GameObject;
			GameObject item = Instantiate (itemObj, itemData[i].Pos, Quaternion.identity) as GameObject;
			item.GetComponent<SpriteRenderer>().sortingOrder = itemData[i].Order;
			item.transform.parent = groundItem;
		}

		for (int i=0; i<enemyData.Count; i++) {
			if(!gData.victory || !gData.currentEnemyName.Equals(enemyData[i].ObjName)){
				string prefabName = enemyData[i].ObjName.Replace("@"+i,"").Replace("(Clone)","");
				object obj = Resources.Load (prefabName, typeof(GameObject));
				GameObject enemyObj = obj as GameObject;
				GameObject enemy = Instantiate (enemyObj, enemyData[i].Pos, Quaternion.identity) as GameObject;
				enemy.GetComponent<SpriteRenderer>().sortingOrder = itemData[i].Order;
				enemy.transform.parent = enemys;
			}else if(gData.victory && gData.currentEnemyName.Equals(enemyData[i].ObjName)){
				enemyData.Remove(enemyData[i]);
				i--;
			}
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

			//generate ground item
			int itemNum = Random.Range (minItemNum, maxItemNum);

			for (int j=0; j<itemNum; j++) {
				GameObject item = itemPrefabs [Random.Range (0, itemPrefabs.Length)];
				Vector3 itemPos = getRandomPos (blockPos, item);
				GameObject itemO = Instantiate (item, itemPos, Quaternion.identity) as GameObject;
				itemO.GetComponent<SpriteRenderer> ().sortingOrder = 5;
				itemO.transform.parent = groundItem;
				itemList.Add(itemO);
			}

			//generate enemy
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

	//record scene info
	public void RecScene(){
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
		gData.scenes[gData.currentFloor] = currentSceneInfo;
	
	}

	private Vector3 getRandomPos (Vector3 blockPos, GameObject element)
	{
		Vector3 randomPos = getValidPos (blockPos, element);
		while (randomPos==new Vector3(-999,-999,-999)) {
			randomPos = getValidPos (blockPos, element);
		}
		return randomPos;
	}

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

	//find out if blockList contains the block pos passed in,and return the direction of the pos
	private Direction haveBlock (PosInfo pi)
	{
		for (int i=0; i<blockList.Count; i++) {
			if (blockList [i].transform.position == pi.Pos) {
				return pi.Direction;
			}
		}
		return Direction.Error;
	}

	private void GenerateGround ()
	{
		for (int i=0; i<maxBlockNum; i++) {
			GenerateBlock (genPos);
		}
	}

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
