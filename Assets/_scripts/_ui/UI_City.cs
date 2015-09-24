using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_City : MonoBehaviour
{
	public GameObject shopBag;
	public GameObject playerBag;
	public GameObject itemInfo;
	public GameObject charInfo;
	private GlobalData gData;
	private Transform buttons;
	private GameObject exitButton;
	public GameObject playerFinder;
	private bool finderInit = false;
	public GameObject rowPrefab;
	public GameObject rows;
	private int page = 1;

	void Start ()
	{
		//初始化文本
		exitButton = GameObject.FindGameObjectWithTag ("UI").transform.FindChild ("Exit").gameObject;
		buttons = GameObject.FindGameObjectWithTag ("UI").transform.FindChild ("Buttons");
		buttons.FindChild ("Bag_B").FindChild ("Text").GetComponent<Text> ().text = StringCollection.BAG;
		buttons.FindChild ("Equip_B").FindChild ("Text").GetComponent<Text> ().text = StringCollection.EQUIPMENT;
		buttons.FindChild ("FindPlayer_B").FindChild ("Text").GetComponent<Text> ().text = StringCollection.FINDPLAYER;
		GameObject.FindGameObjectWithTag ("UI").transform.FindChild ("ShopBag").FindChild ("Leave").FindChild ("Text").GetComponent<Text> ().text = StringCollection.LEAVESHOP;
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
	}

	public void openShop (string shopType)
	{
		shopBag.SetActive (true);
		shopBag.SendMessage ("InitBag", shopType);
		itemInfo.SetActive (false);
		playerBag.SetActive (true);
		playerBag.SendMessage ("InitBag", gData.characterList [0]);
	
		//进入商店模式,隐藏其他按钮
		buttons.gameObject.SetActive (false);
		exitButton.SetActive (false);
		charInfo.SetActive (false);
		gData.isShop = true;
		itemInfo.transform.FindChild ("TradeNum").gameObject.SetActive (true);
	}

	public void PlayerBag ()
	{
		playerBag.SetActive (!playerBag.activeInHierarchy);
		if (playerBag.activeInHierarchy) {
			playerBag.SendMessage ("InitBag", gData.characterList [0]);
		} else {
			itemInfo.SetActive (false);
		}
	}

	public void PlayerEquip ()
	{
		charInfo.SetActive (!charInfo.activeInHierarchy);
		if (!charInfo.activeInHierarchy) {
			itemInfo.SetActive (false);
		} else {
			gameObject.SendMessage ("ShowCharInfo");
		}
	}

	public void leaveShop ()
	{
		shopBag.SetActive (false);
		playerBag.SetActive (false);
		itemInfo.SetActive (false);
		//离开商店模式,显示其他按钮
		buttons.gameObject.SetActive (true);
		exitButton.SetActive (true);
		itemInfo.transform.FindChild ("TradeNum").gameObject.SetActive (false);
		gData.isShop = false;
	}

	public void FindPlayer ()
	{

		playerFinder.SetActive (!playerFinder.activeInHierarchy);

		if (playerFinder.activeInHierarchy) {
			if (!finderInit) {
				
				playerFinder.transform.FindChild ("FindPlayer_NameLable").GetComponent<Text> ().text = StringCollection.NAME;
				playerFinder.transform.FindChild ("FindPlayer_Query").FindChild ("Text").GetComponent<Text> ().text = StringCollection.QUERYPLAYER;

				Transform title = playerFinder.transform.FindChild ("Rows").FindChild ("Title");

				title.FindChild ("Name").FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Name"];
				title.FindChild ("Pro").FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Pro"];
				title.FindChild ("Level").FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Level"];

				playerFinder.transform.FindChild ("G_Choose").FindChild ("Label").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Geomancer"];
				playerFinder.transform.FindChild ("S_Choose").FindChild ("Label").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Settler"];
				playerFinder.transform.FindChild ("E_Choose").FindChild ("Label").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Exorcist"];
				playerFinder.transform.FindChild ("D_Choose").FindChild ("Label").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Doctor"];

				finderInit = true;
			}

			gData.account.queryOtherPlayer ("", getProListByChoose (), 1);
		}
	}

	public List<object> getProListByChoose ()
	{
		List<object> proList = new List<object> ();

		if (playerFinder.transform.FindChild ("G_Choose").GetComponent<Toggle> ().isOn) {
			proList.Add ("Geomancer");
		}
		if (playerFinder.transform.FindChild ("S_Choose").GetComponent<Toggle> ().isOn) {
			proList.Add ("Settler");
		}
		if (playerFinder.transform.FindChild ("E_Choose").GetComponent<Toggle> ().isOn) {
			proList.Add ("Exorcist");
		}
		if (playerFinder.transform.FindChild ("D_Choose").GetComponent<Toggle> ().isOn) {
			proList.Add ("Doctor");
		}

		return proList;
	}

	public void OnQueryOtherPlayer (List<object> playerInfos, int maxPage, int currentPage)
	{

		this.page = currentPage;

		ClearQueryList ();

		if(playerInfos.Count>0){

			playerFinder.transform.FindChild ("NoPlayer").GetComponent<Text> ().text = "";

			for (int i=0; i<playerInfos.Count; i++) {
				Dictionary<string, object> info = (Dictionary<string, object>)playerInfos [i];
				GameObject infoRow = Instantiate (rowPrefab, rows.transform.position, Quaternion.identity) as GameObject;
				infoRow.transform.SetParent (rows.transform);
				infoRow.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (0, -1 * 42 * (i + 1), 0);
				infoRow.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
				
				Transform it = infoRow.transform;
				it.FindChild ("Name").FindChild ("Text").GetComponent<Text> ().text = info ["name"].ToString ();
				it.FindChild ("Pro").FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN [info ["pro"].ToString ()];
				it.FindChild ("Level").FindChild ("Text").GetComponent<Text> ().text = info ["level"].ToString ();
				it.FindChild ("Invite").FindChild ("Button").FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Invite"];
				
			}
		}else{
			playerFinder.transform.FindChild ("NoPlayer").GetComponent<Text> ().text = StringCollection.stringDict_CN ["NoPlayer"];
		}

		playerFinder.transform.FindChild ("Page").FindChild ("Text").GetComponent<Text> ().text = currentPage.ToString () + "/" + maxPage;
	}

	public void WorldMap ()
	{
		Application.LoadLevel ("map");
	}

	public void QueryPlayer (int page)
	{
		ClearQueryList ();
		string name = playerFinder.transform.FindChild ("Query_Name").FindChild ("Text").GetComponent<Text> ().text;
		gData.account.queryOtherPlayer (name, getProListByChoose (), page);
	}

	public void ClearQueryList ()
	{
		GameObject[] infoRows = GameObject.FindGameObjectsWithTag ("PlayerInfoRow");
		for (int i=0; i<infoRows.Length; i++) {
			Destroy (infoRows [i]);
		}
	}

	public void PrePage ()
	{
		page = page - 1;
		QueryPlayer (page);
	}

	public void nextPage ()
	{
		page = page + 1;
		QueryPlayer (page);
	}
}
