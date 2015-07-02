using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BagInit : MonoBehaviour {

	public int border = 9;
	public int gridRow = 4;
	public int gridCol = 4;
	public int gridSize = 40;
	private float firstX;
	private float firstY;
	public GameObject grid;

	void Awake(){
		firstX = transform.position.x - border * 0.5f - gridSize- border - gridSize * 0.5f;
		firstY = transform.position.y + border * 0.5f + gridSize + border + gridSize * 0.5f;
	}

	void InitBag(Character c){

		//init all grid
		for (int i=0; i<gridRow; i++) {
			for (int j=0; j<gridCol; j++) {
				GameObject gridO = Instantiate (grid, new Vector3(firstX + j*(border+gridSize),firstY-i*(border+gridSize),0), Quaternion.identity) as GameObject;
				gridO.transform.SetParent(transform);
			}
		}

		List<Item> itemList = c.ItemList;
		for (int i=0; i<itemList.Count; i++) {
			Debug.Log(itemList[i]);
		}
	}	
}
