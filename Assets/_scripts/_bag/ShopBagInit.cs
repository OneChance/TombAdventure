using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopBagInit : MonoBehaviour
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

	public void InitBag (string shopType)
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

		List<Baggrid> bgList = new List<Baggrid> ();

		//获得商店物品列表
		switch (shopType) {
		case "item":
			HealthItem item = new HealthItem (Item.RangeType.SINGLE, 10, "1", "单体治疗药剂", 50);
			Baggrid bg = new Baggrid (item, 0);
			bgList.Add (bg);

			Equipment e2 = new Equipment (2, 3, 0, 0, Equipment.EquipPos.BODY, "3", "学者的幻想", 1, 500);
			Baggrid bg2 = new Baggrid (e2, 0);
			bgList.Add (bg2);

			break;
		case "mercenary":

			Character c = new Character (0, 500, 500, 50, 0, 0, "潘子", false, 500, 500, ProFactory.getPro ("Settler", "100"), 1, 0, null, -1);
			Mercenary m = new Mercenary (c);

			Baggrid bg3 = new Baggrid (m, 0);
			bgList.Add (bg3);

			break;
		}
			
		for (int i=0; i<bgList.Count; i++) {
			Baggrid bg = bgList [i];

			GameObject itemO = Instantiate (itemPrefab, new Vector3 (grids [i].transform.position.x, grids [i].transform.position.y, 0), Quaternion.identity) as GameObject;
			itemO.GetComponent<Image> ().sprite = Resources.Load <Sprite> (bg.Item.prefabName);
			itemO.GetComponent<UI_Item> ().Bg = bg;
			itemO.GetComponent<UI_Item> ().fromShop = true;
			
			string num = bg.Num.ToString ();
			
			if (num.Equals ("0")) {
				num = "";
			}
			
			itemO.transform.FindChild ("Num").GetComponent<Text> ().text = num;
			itemO.transform.SetParent (grids [i].transform);
		}
	}	
}
