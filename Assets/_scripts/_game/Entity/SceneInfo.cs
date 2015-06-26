using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInfo
{
	private List<GameObject> blockList;
	private List<GameObject> itemList;
	
	List<GameObject> BlockList {
		get {
			return this.blockList;
		}
		set {
			blockList = value;
		}
	}
	
	List<GameObject> ItemList {
		get {
			return this.itemList;
		}
		set {
			itemList = value;
		}
	}
}
