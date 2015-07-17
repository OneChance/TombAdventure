using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInfo
{
	private List<ElementData> blockData;
	private List<ElementData> itemData;
	private List<ElementData> enemyData;
	private List<ElementData> digData;

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
