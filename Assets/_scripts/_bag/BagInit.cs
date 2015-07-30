using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BagInit : MonoBehaviour
{

	public int border = 9;
	public int gridRow = 4;
	public int gridCol = 4;
	public int gridSize = 40;
	private float firstX;
	private float firstY;
	public GameObject grid;
	public List<GameObject> grids;
	private GameObject itemPrefab;
	public bool init;

	void Awake ()
	{
		firstX = transform.position.x - border * 0.5f - gridSize - border - gridSize * 0.5f;
		firstY = transform.position.y + border * 0.5f + gridSize + border + gridSize * 0.5f;
		init = false;
		itemPrefab = Resources.Load ("item", typeof(GameObject)) as GameObject;
	}

	public void InitBag (Character c)
	{

		if (!init) {
			//init all grid
			for (int i=0; i<gridRow; i++) {
				for (int j=0; j<gridCol; j++) {
					GameObject gridO = Instantiate (grid, new Vector3 (firstX + j * (border + gridSize), firstY - i * (border + gridSize), 0), Quaternion.identity) as GameObject;
					gridO.transform.SetParent (transform);
					grids.Add (gridO);
				}
			}

			init = true;
		}

		for (int i=0; i<grids.Count; i++) {
			if (grids [i].transform.childCount > 0)
				Destroy (grids [i].transform.GetChild (0).gameObject);
		}

		List<Baggrid> bgList = c.BgList;

		if (Application.loadedLevelName.Equals ("city")) {
			transform.FindChild ("Money").FindChild ("Text").GetComponent<Text> ().text = c.money.ToString ();
		}

		for (int i=0; i<bgList.Count; i++) {
			Baggrid bg = bgList [i];

			//战斗场景不能使用装备
			if (Application.loadedLevelName.Equals ("battle") && bg.Item.ct == Item.CommonType.EQUIPMENT) {
				continue;
			}

			if (bg.Num > 0) {
				GameObject itemO = Instantiate (itemPrefab, new Vector3 (grids [i].transform.position.x, grids [i].transform.position.y, 0), Quaternion.identity) as GameObject;
				itemO.GetComponent<Image> ().sprite = Resources.Load <Sprite> (bg.Item.prefabName);
				itemO.GetComponent<UI_Item> ().Bg = bg;

				string num = bg.Num.ToString ();

				if (bg.Item.ct == Item.CommonType.EQUIPMENT || bg.Item.ct == Item.CommonType.MERCENARY) {
					itemO.transform.FindChild ("Num").gameObject.SetActive (false);
				} else {
					itemO.transform.FindChild ("Num").GetComponent<Text> ().text = num;
				}			

				itemO.transform.SetParent (grids [i].transform);
			} else {
				bgList.Remove (bg);
				i--;
			}
		}
	}	
}
