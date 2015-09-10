using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInfo
{
	public List<ElementData> blockData;
	public List<ElementData> itemData;
	public List<ElementData> enemyData;
	public List<ElementData> digData;
	public ElementData nextEntry;//如果当前层时墓穴，此变量指定棺材位置，否则指定下层入口位置
	public ElementData preEntry;
	public Vector3 digToNextPos;//挖掘的通往下一层的坑位置
	public bool isTomb = false;

	public List<ElementData> EnemyData {
		get {
			return this.enemyData;
		}
		set {
			enemyData = value;
		}
	}
	
	public List<ElementData> BlockData {
		get {
			return this.blockData;
		}
		set {
			blockData = value;
		}
	}
	
	public List<ElementData> ItemData {
		get {
			return this.itemData;
		}
		set {
			itemData = value;
		}
	}

	public List<ElementData> DigData {
		get {
			return this.digData;
		}
		set {
			digData = value;
		}
	}
}
