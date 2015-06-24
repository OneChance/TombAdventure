using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundGeneration : MonoBehaviour
{

	public GameObject groundBlock;
	public int maxBlockNum = 20;
	public int maxItemNum = 10;
	public int minItemNum = 3;

	public GameObject[] groundPrefabs;
	public Transform parent;
	private List<Vector3> ablePos;
	private List<Vector3> addedPos;
	private float blockX;
	private float blockY;
	private List<GameObject> blockList;
	private Vector3 genPos;

	void Awake ()
	{
		ablePos = new List<Vector3> ();
		addedPos = new List<Vector3> ();
		blockList = new List<GameObject> ();
		genPos = new Vector3 (0, 0, 0);
	}
	
	void Start ()
	{
		GenerateGround ();
		ReplaceTex ();
		GenerateGroundItem();
	}

	private void GenerateGroundItem(){
		for (int i=0; i<blockList.Count; i++) {
			GameObject block = blockList [i];			
			Vector3 blockPos = block.transform.position;


		}
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
			block.transform.parent = parent;
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

		genPos = ablePos [Random.Range (0, ablePos.Count - 1)];
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

		public PosInfo (Vector3 pos, global::GroundGeneration.Direction direction)
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
