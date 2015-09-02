using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopBagInit : MonoBehaviour
{

	public int border = 12;
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


		GlobalData gData = UnityEngine.GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();

		List<Baggrid> bgList = new List<Baggrid> ();

		Dictionary<int, ServerItemData> siList = gData.siList;

		//获得商店物品列表
		List<int> shopConten = new List<int>();

		switch (shopType) {
		case "item":

			for(int i=0;i<gData.itemShopConten.Count;i++){
				shopConten.Add(gData.itemShopConten[i]);
			}

			for(int i=0;i<gData.equipShopContent.Count;i++){
				shopConten.Add(gData.equipShopContent[i]);
			}

			break;
		case "mercenary":
			shopConten = gData.assistShopContent;
			break;
		}

		if (shopConten != null && shopConten.Count > 0) {
			for (int i=0; i<shopConten.Count; i++) {
				ServerItemData sid = siList[shopConten[i]];
				Baggrid bg = new Baggrid (ItemFactory.getItemFromSID(sid,null), 0,-1);
				bgList.Add (bg);
			}
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
