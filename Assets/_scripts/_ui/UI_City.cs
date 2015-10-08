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
	private UnityEngine.GameObject[] rows;
	private int playerPage = 1;
	public List<Dictionary<string,object>> invitedList;
	private GameObject invites;
	public GameObject inviteFinder;
	private int invitePage = 1;
	private GameObject inviteRowPrefab;
	private GameObject[] inviteRows;
	private bool inviteFinderInit = false;

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
		invitedList = new List<Dictionary<string, object>> ();
		invitePage = 1;
	}

	void Update ()
	{
		if (invitedList.Count > 0) {
			
			if (invites == null) {
				UnityEngine.GameObject canvasGO = UnityEngine.GameObject.FindGameObjectWithTag ("UI");
				UnityEngine.GameObject invitesPrefab = Resources.Load ("Invites", typeof(UnityEngine.GameObject)) as UnityEngine.GameObject;
				invites = Instantiate (invitesPrefab, canvasGO.transform.position, Quaternion.identity) as UnityEngine.GameObject;
				invites.transform.SetParent (canvasGO.transform);
				invites.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (-53, 25, 0);
				invites.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
				invites.GetComponent<Button> ().onClick.AddListener (delegate() {
					ShowInviteFinder ();
				});
			} else {
				invites.SetActive (true);
			}
			
			invites.transform.FindChild ("Text").GetComponent<Text> ().text = invitedList.Count.ToString ();
			
			GameUtil.Focus (invites);
			
		} else {
			if (invites != null) {
				invites.SetActive (false);
			}
		}
	}

	public void ShowInviteFinder ()
	{
		inviteFinder.SetActive (!inviteFinder.activeInHierarchy);
	
		if (inviteFinder.activeInHierarchy) {

			if (!inviteFinderInit) {
				DataHelper.FillPlayerInfoRowTitle (inviteFinder);
				Button queryButton = inviteFinder.transform.FindChild ("Query").GetComponent<Button> ();
				Button preButton = inviteFinder.transform.FindChild ("Pre").GetComponent<Button> ();
				Button nextButton = inviteFinder.transform.FindChild ("Next").GetComponent<Button> ();
				
				queryButton.onClick.AddListener (delegate() {
					invitePage = 1;
					QueryInvited ();
				});
				
				preButton.onClick.AddListener (delegate() {
					InvitePrePage ();
				});
				
				nextButton.onClick.AddListener (delegate() {
					InviteNextPage ();
				});

				inviteRows = UnityEngine.GameObject.FindGameObjectsWithTag ("PlayerInfoRow_Invite");
			}

			invitePage = 1;
			QueryInvited ();
		}
	}

	public void InvitePrePage ()
	{
		invitePage = invitePage - 1;
		QueryInvited ();
	}
	
	public void InviteNextPage ()
	{
		invitePage = invitePage + 1;
		QueryInvited ();
	}

	public void QueryInvited ()
	{
		ClearInviteList ();
		string name = inviteFinder.transform.FindChild ("Query_Name").FindChild ("Text").GetComponent<Text> ().text;
		Query (invitePage, name, DataHelper.getProListByChooseFromPanel (inviteFinder));
	}

	public void Query (int page, string name, List<object> proList)
	{
		List<Dictionary<string, object>> filterdList = new List<Dictionary<string, object>> ();
		
		for (int i=0; i<invitedList.Count; i++) {
			Dictionary<string, object> info = invitedList [i];
			bool name_add = false;
			bool pro_add = false;
			
			if (info ["name"].ToString ().Contains (name)) {
				name_add = true;
			}
			
			for (int j=0; j<proList.Count; j++) {
				if (proList [j].ToString ().Equals (info ["pro"].ToString ())) {
					pro_add = true;
					break;
				}
			}
			
			if (name_add && pro_add) {
				filterdList.Add (invitedList [i]);
			}
		}
		
		int invitesNum = filterdList.Count;
		int maxPage = (int)(invitesNum / 5);
		if (invitesNum % 5 > 0) {
			maxPage++;
		}
		page = Mathf.Min (maxPage, page);
		page = Mathf.Max (1, page);
		invitePage = page;
		
		inviteFinder.transform.FindChild ("Page").FindChild ("Text").GetComponent<Text> ().text = invitePage.ToString () + "/" + maxPage;
		
		for (int i=0; i<5; i++) {
			int index = (invitePage - 1) * 5 + i;
			if (index < filterdList.Count) {
				
				Dictionary<string, object> info = (Dictionary<string, object>)filterdList [index];
				
				if (rowPrefab == null) {
					rowPrefab = Resources.Load ("PlayerInfoRow_Invite", typeof(UnityEngine.GameObject)) as UnityEngine.GameObject;
				}
				
				UnityEngine.GameObject infoRow = inviteRows [i];
				
				infoRow.SetActive (true);
				
				Transform it = infoRow.transform;
				it.FindChild ("Name").FindChild ("Text").GetComponent<Text> ().text = info ["name"].ToString ();
				it.FindChild ("Pro").FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN [info ["pro"].ToString ()];
				it.FindChild ("Level").FindChild ("Text").GetComponent<Text> ().text = info ["level"].ToString ();
				
				Transform inButton = it.FindChild ("Response").FindChild ("In");
				inButton.FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN ["INVITED_IN"];
				inButton.GetComponent<PlayerId> ().dbid = int.Parse (info ["attack"].ToString ());
				
				Transform rejectButton = it.FindChild ("Response").FindChild ("Reject");
				rejectButton.FindChild ("Text").GetComponent<Text> ().text = StringCollection.stringDict_CN ["INVITED_REJECT"];
				rejectButton.GetComponent<PlayerId> ().dbid = int.Parse (info ["attack"].ToString ());
				
				inButton.GetComponent<Button> ().onClick.RemoveAllListeners();
				inButton.GetComponent<Button> ().onClick.AddListener (delegate() {
					inviteRes (1, inButton.GetComponent<PlayerId> ().dbid);
				});

				rejectButton.GetComponent<Button> ().onClick.RemoveAllListeners();
				rejectButton.GetComponent<Button> ().onClick.AddListener (delegate() {
					inviteRes (0, inButton.GetComponent<PlayerId> ().dbid);
				});
				
			} else {
				break;
			}
		}
	}

	public void OnInvited (Dictionary<string,object> playerInfo)
	{
		bool haveSamePlayer = false;
		
		for (int i=0; i<invitedList.Count; i++) {
			if (invitedList [i] ["attack"].ToString ().Equals (playerInfo ["attack"].ToString ())) {
				haveSamePlayer = true;
				break;
			}
		}
		
		if (!haveSamePlayer) {
			invitedList.Add (playerInfo);
		}
	}
	
	public void ClearInviteList ()
	{
		if (inviteRows != null) {
			for (int i=0; i<inviteRows.Length; i++) {
				inviteRows [i].SetActive (false);
			}
		}
	}

	public void inviteRes (int flag, int dbid)
	{
		gData.account.inviteResponse (flag, dbid);
		
		for (int i=0; i<invitedList.Count; i++) {
			if (invitedList [i] ["attack"].ToString ().Equals (dbid.ToString ())) {
				invitedList.Remove (invitedList [i]);
				break;
			}
		}
		
		if (invitedList.Count == 0) {
			inviteFinder.SetActive (false);
			invites.SetActive (false);
		} else {
			QueryInvited ();
		}
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

		playerPage = currentPage;

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
		for (int i=0; i<invitedList.Count; i++) {
			if (invitedList [i] ["attack"].ToString ().Equals (dbid.ToString ())) {
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

	public void QueryPlayer ()
	{
		ClearQueryList ();
		string name = playerFinder.transform.FindChild ("Query_Name").FindChild ("Text").GetComponent<Text> ().text;
		gData.account.queryOtherPlayer (name, getProListByChoose (), playerPage);
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
		playerPage = playerPage - 1;
		QueryPlayer ();
	}

	public void nextPage ()
	{
		playerPage = playerPage + 1;
		QueryPlayer ();
	}

	public void InvitePlayer (PlayerId playerId)
	{
		gData.account.invitePlayer (playerId.dbid);
	}

	//组队请求应答
	public void OnInvitePlayer (string msg, List<object> assistList, int leaderFlag)
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
				QueryPlayer ();
			}

			ShowHint.Hint (showMsg);
		} else {
			//更新雇佣兵列表
			//清空雇佣兵
			while (gData.characterList.Count>1) {
				gData.characterList.RemoveAt (1);
			}
		
			if (leaderFlag == 2) {
				ShowHint.Hint (StringCollection.stringDict_CN ["PLAYERIN"]);
			} else	 if (assistList.Count > (gData.characterList.Count - 1)) {
				Dictionary<string, object> assistInfo = (Dictionary<string, object>)assistList [assistList.Count - 1];
				ShowHint.Hint ("[" + assistInfo ["playername"] + "]" + StringCollection.stringDict_CN ["PLAYERIN"]);
			}

			DataHelper.getAssistsFromAssistList (assistList, gData.siList, gData.characterList);

			if (leaderFlag == 1) {
				gData.characterList [0].isLeader = true;
			}

			//更新UI
			SendMessage("UpdateUIInfo");
		}
	}

}
