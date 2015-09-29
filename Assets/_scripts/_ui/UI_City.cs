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
	public GameObject inviteFinder;
	private bool finderInit = false;
	public GameObject rowPrefab;
	private UnityEngine.GameObject[] rows;
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
			gameObject.SendMessage ("ShowCharInfo");  //UI_Bag
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
				playerFinder.transform.FindChild ("NameLable").GetComponent<Text> ().text = StringCollection.NAME;
				playerFinder.transform.FindChild ("Query").FindChild ("Text").GetComponent<Text> ().text = StringCollection.QUERYPLAYER;
				Transform title = playerFinder.transform.FindChild ("Rows").FindChild ("Title");

				title.FindChild ("Name").FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Name"];
				title.FindChild ("Pro").FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Pro"];
				title.FindChild ("Level").FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Level"];

				playerFinder.transform.FindChild ("G_Choose").FindChild ("Label").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Geomancer"];
				playerFinder.transform.FindChild ("S_Choose").FindChild ("Label").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Settler"];
				playerFinder.transform.FindChild ("E_Choose").FindChild ("Label").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Exorcist"];
				playerFinder.transform.FindChild ("D_Choose").FindChild ("Label").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Doctor"];

				rows = UnityEngine.GameObject.FindGameObjectsWithTag ("PlayerInfoRow");

				finderInit = true;
			}
			ClearQueryList ();
			gData.account.queryOtherPlayer ("", getProListByChoose (), 1);
		}
	}

	public List<object> getProListByChoose ()
	{
		return DataHelper.getProListByChooseFromPanel (playerFinder);
	}

	public void OnQueryOtherPlayer (List<object> playerInfos, int maxPage, int currentPage)
	{

		this.page = currentPage;

		ClearQueryList ();

		if (playerInfos.Count > 0) {

			playerFinder.transform.FindChild ("NoPlayer").GetComponent<Text> ().text = "";

			for (int i=0; i<playerInfos.Count; i++) {

				GameObject infoRow = rows [i];

				infoRow.SetActive (true);

				Dictionary<string, object> info = (Dictionary<string, object>)playerInfos [i];

				Transform it = infoRow.transform;
				it.FindChild ("Name").FindChild ("Text").GetComponent<Text> ().text = info ["name"].ToString ();
				it.FindChild ("Pro").FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN [info ["pro"].ToString ()];
				it.FindChild ("Level").FindChild ("Text").GetComponent<Text> ().text = info ["level"].ToString ();
				it.FindChild ("Invite").FindChild ("Button").FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN ["Invite"];
				it.FindChild ("Invite").FindChild ("Button").GetComponent<PlayerId> ().dbid = int.Parse (info ["attack"].ToString ());

				Button btn = it.FindChild ("Invite").FindChild ("Button").GetComponent<Button> ();

				btn.onClick.AddListener (delegate() {
					//如果想邀请的玩家已近邀请了自己，就无法对他发起邀请
					if (!CheckInvite (btn.GetComponent<PlayerId> ().dbid)) {
						InvitePlayer (btn.GetComponent<PlayerId> ());
					}
				});

			}
		} else {
			playerFinder.transform.FindChild ("NoPlayer").GetComponent<Text> ().text = StringCollection.stringDict_CN ["NoPlayer"];
		}

		playerFinder.transform.FindChild ("Page").FindChild ("Text").GetComponent<Text> ().text = currentPage.ToString () + "/" + maxPage;
	}
	
	public bool CheckInvite (int dbid)
	{
		for (int i=0; i<gData.invitedList.Count; i++) {
			if (gData.invitedList [i] ["attack"].ToString ().Equals (dbid.ToString ())) {
				ShowHint.Hint (StringCollection.stringDict_CN ["INVITEDBYPLAYER"]);
				return true;
			}
		}
		return false;
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
		if (rows != null) {
			for (int i=0; i<rows.Length; i++) {
				rows [i].SetActive (false);
			}
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

	public void InvitePlayer (PlayerId playerId)
	{
		gData.account.invitePlayer (playerId.dbid);
	}

	//组队请求应答
	public void OnInvitePlayer (string msg, List<object> assistList)
	{
		string showMsg = "";

		if (!msg.Equals ("ok")) {

			showMsg = StringCollection.stringDict_CN [msg];

			if (msg.Equals ("INVITEREJECT")) {
				//获得拒绝的玩家信息
				Dictionary<string, object> info = (Dictionary<string, object>)assistList [0];
				showMsg = "[" + info ["playername"] + "]" + showMsg;
			}
			if (msg.Equals ("NOPLAYER")) {
				QueryPlayer (page);
			}

			ShowHint.Hint (showMsg);
		} else {
			//更新雇佣兵列表
			//清空雇佣兵
			while (gData.characterList.Count>1) {
				gData.characterList.RemoveAt (1);
			}

			DataHelper.getAssistsFromAssistList (assistList, gData.siList, gData.characterList);
		}
	}

}
