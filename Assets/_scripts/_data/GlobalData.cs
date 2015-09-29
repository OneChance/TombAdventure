using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KBEngine;
using UnityEngine.UI;

public class GlobalData : MonoBehaviour
{
	public int currentFloor = 1;
	public Enemy currentEnemy;
	public List<Character> characterList;
	public bool victory = true;
	public string currentEnemyName;
	public Vector3 playerPos; //record the pos of player when battle start
	public Baggrid currentItem;
	public Vector3 preDigPos;
	public bool isPlayer; //ture为玩家联机模式，false为单机佣兵模式
	public bool isShop = false;
	public Tomb currentTomb;
	public Dictionary<int, ServerItemData> siList;
	public List<int> itemShopConten;
	public List<int> assistShopContent;
	public List<int> equipShopContent;
	public Account account;
	public List<Tomb> tombs;
	public int enemyNeedRemove = 0;
	public List<Dictionary<string,object>> invitedList;
	private UnityEngine.GameObject invites;
	private UnityEngine.GameObject invitesShow;
	private int currentPage = 1;
	private UnityEngine.GameObject rowPrefab;
	private UnityEngine.GameObject[] rows;

	void Start ()
	{
		invitedList = new List<Dictionary<string, object>> ();
	}

	void OnLevelWasLoaded (int level)
	{
		currentPage = 1;	
	}
	
	void Update ()
	{
		if (Application.loadedLevelName.Equals ("city") || Application.loadedLevelName.Equals ("main")) {
			if (invitedList.Count > 0) {

				if (invites == null) {
					UnityEngine.GameObject canvasGO = UnityEngine.GameObject.FindGameObjectWithTag ("UI");
					UnityEngine.GameObject invitesPrefab = Resources.Load ("Invites", typeof(UnityEngine.GameObject)) as UnityEngine.GameObject;
					invites = Instantiate (invitesPrefab, canvasGO.transform.position, Quaternion.identity) as UnityEngine.GameObject;
					invites.transform.SetParent (canvasGO.transform);
					invites.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (-53, 25, 0);
					invites.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
					invites.GetComponent<Button> ().onClick.AddListener (delegate() {
						Debug.Log ("123");
						ShowInvitedList ();
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

	public void ClearQueryList ()
	{
		if (rows != null) {
			for (int i=0; i<rows.Length; i++) {
				rows [i].SetActive (false);
			}
		}
	}

	public void QueryInvited (int page)
	{
		ClearQueryList ();
		string name = invitesShow.transform.FindChild ("Query_Name").FindChild ("Text").GetComponent<Text> ().text;
		Query (page, name, DataHelper.getProListByChooseFromPanel (invitesShow));
	}

	public void Query (int page, string name, List<object> proList)
	{

		//filter by condition
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
		currentPage = page;

		invitesShow.transform.FindChild ("Page").FindChild ("Text").GetComponent<Text> ().text = currentPage.ToString () + "/" + maxPage;
	
		for (int i=0; i<5; i++) {
			int index = (currentPage - 1) * 5 + i;
			if (index < filterdList.Count) {

				Dictionary<string, object> info = (Dictionary<string, object>)filterdList [index];

				if (rowPrefab == null) {
					rowPrefab = Resources.Load ("PlayerInfoRow_Invite", typeof(UnityEngine.GameObject)) as UnityEngine.GameObject;
				}

				UnityEngine.GameObject infoRow = rows [i];

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

				
				inButton.GetComponent<Button> ().onClick.AddListener (delegate() {
					inviteRes (1, inButton.GetComponent<PlayerId> ().dbid);
				});

				rejectButton.GetComponent<Button> ().onClick.AddListener (delegate() {
					inviteRes (0, inButton.GetComponent<PlayerId> ().dbid);
				});

			} else {
				break;
			}
		}
	}

	public void inviteRes (int flag, int dbid)
	{
		this.account.inviteResponse (flag, dbid);

		for (int i=0; i<invitedList.Count; i++) {
			if (invitedList [i] ["attack"].ToString ().Equals (dbid.ToString ())) {
				invitedList.Remove (invitedList [i]);
				break;
			}
		}

		if (invitedList.Count == 0) {
			invitesShow.SetActive (false);
			invites.SetActive (false);
		} else {
			QueryInvited (currentPage);
		}
	}

	public void ShowInvitedList ()
	{
		if (invitesShow == null) {
			UnityEngine.GameObject canvasGO = UnityEngine.GameObject.FindGameObjectWithTag ("UI");
			UnityEngine.GameObject invitesListPrefab = Resources.Load ("InvitedList", typeof(UnityEngine.GameObject)) as UnityEngine.GameObject;
			invitesShow = Instantiate (invitesListPrefab, canvasGO.transform.position, Quaternion.identity) as UnityEngine.GameObject;
			invitesShow.transform.SetParent (canvasGO.transform);
			invitesShow.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
			DataHelper.FillPlayerInfoRowTitle (invitesShow);
			Button queryButton = invitesShow.transform.FindChild ("Query").GetComponent<Button> ();
			Button preButton = invitesShow.transform.FindChild ("Pre").GetComponent<Button> ();
			Button nextButton = invitesShow.transform.FindChild ("Next").GetComponent<Button> ();

			queryButton.onClick.AddListener (delegate() {
				QueryInvited (1);
			});

			preButton.onClick.AddListener (delegate() {
				PrePage ();
			});

			nextButton.onClick.AddListener (delegate() {
				nextPage ();
			});

			rows = UnityEngine.GameObject.FindGameObjectsWithTag ("PlayerInfoRow_Invite");
		}
		
		invitesShow.SetActive (!invitesShow.activeInHierarchy);

		if (invitesShow.activeInHierarchy) {
			QueryInvited (1);
		}
	}

	public void PrePage ()
	{
		currentPage = currentPage - 1;
		QueryInvited (currentPage);
	}
	
	public void nextPage ()
	{
		currentPage = currentPage + 1;
		QueryInvited (currentPage);
	}

	public void OffLineNoti (int playerId)
	{
		for (int i=1; i<characterList.Count; i++) {
			if (characterList [i].playerId == playerId) {
				ShowHint.Hint ("[" + characterList [i].ObjName + "]" + StringCollection.stringDict_CN ["OFFLINE"]);
				characterList [i].onLine = false;
				break;
			}
		}

		//更新UI
		UnityEngine.GameObject gameController = UnityEngine.GameObject.FindGameObjectWithTag ("GameController");
		if (gameController != null && gameController.GetComponent<UI_Bag> () != null) {
			gameController.GetComponent<UI_Bag> ().ShowCharInfo ();
		}
	}

	public void OnLineNoti (int playerId)
	{
		for (int i=1; i<characterList.Count; i++) {
			if (characterList [i].playerId == playerId) {
				ShowHint.Hint ("[" + characterList [i].ObjName + "]" + StringCollection.stringDict_CN ["ONLINE"]);
				characterList [i].onLine = true;
				break;
			}
		}
		
		//更新UI
		UnityEngine.GameObject gameController = UnityEngine.GameObject.FindGameObjectWithTag ("GameController");
		if (gameController != null && gameController.GetComponent<UI_Bag> () != null) {
			gameController.GetComponent<UI_Bag> ().ShowCharInfo ();
		}
	}
}
