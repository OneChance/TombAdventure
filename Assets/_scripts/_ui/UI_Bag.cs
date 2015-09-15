using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class UI_Bag : MonoBehaviour
{

	private GameObject avatar;
	private GameObject head;
	private GameObject hand;
	private GameObject clothes;
	private GameObject foot;
	private GameObject ass_1;
	private GameObject ass_2;
	private GameObject ass_3;
	private List<GameObject> assList;
	private List<GameObject> focusList;
	private GameObject uiCharP;
	private GameObject uiEquip;
	private List<Character> cList;
	private List<Equipment> eList;
	public GameObject bag;
	public GameObject itemInfo;
	public GameObject charInfo;
	private GlobalData gData;
	private GameObject charNote;
	private bool charInfoInit = false;
	
	void Start ()
	{
		avatar = charInfo.transform.FindChild ("Avatar").gameObject;
		head = charInfo.transform.FindChild ("Head").gameObject;
		hand = charInfo.transform.FindChild ("Hand").gameObject;
		clothes = charInfo.transform.FindChild ("Clothes").gameObject;
		foot = charInfo.transform.FindChild ("Foot").gameObject;
		ass_1 = charInfo.transform.FindChild ("Ass_1").gameObject;
		ass_2 = charInfo.transform.FindChild ("Ass_2").gameObject;
		ass_3 = charInfo.transform.FindChild ("Ass_3").gameObject;

		charNote = charInfo.transform.FindChild ("CharNote").gameObject;

		
		assList = new List<GameObject> ();
		assList.Add (ass_1);
		assList.Add (ass_2);
		assList.Add (ass_3);
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		focusList = new List<GameObject> ();
		
		uiCharP = Resources.Load ("UIChar", typeof(GameObject)) as GameObject;
		uiEquip = Resources.Load ("UIEquip", typeof(GameObject)) as GameObject;
		
		cList = gData.characterList;
		eList = cList [0].EquipList;
	}

	public void ClearFocus ()
	{
		GameUtil.UnFocus (focusList);
	}

	void Update ()
	{
		if (focusList != null) {
			for (int i=0; i<focusList.Count; i++) {
				GameUtil.Focus (focusList [i]);
			}
		}
	}

	public void UpdateUIInfo ()
	{

		cList = gData.characterList;

		if (charInfo.activeInHierarchy) {

			charNote.GetComponent<Text> ().text = cList [0].CharInfo;

			for (int i=1; i<=assList.Count; i++) {
				
				GameObject ass = assList [i - 1];

				if (i < cList.Count) {
					ass.transform.FindChild ("UIChar(Clone)").GetComponent<Image> ().sprite = Resources.Load<Sprite> (cList [i].PrefabName);			
					ass.transform.FindChild ("UIChar(Clone)").GetComponent<UI_Player> ().c = cList [i];
				} else {
					ass.transform.FindChild ("UIChar(Clone)").GetComponent<Image> ().sprite = Resources.Load<Sprite> ("_images/_game/no_ass");
					ass.transform.FindChild ("UIChar(Clone)").GetComponent<UI_Player> ().c = null;
				}
			}
		}
	}

	public GameObject getEquipPosGo (int pos)
	{
		
		GameObject eP = null;		
		
		switch (pos) {
		case (int)Equipment.EquipPos.BODY:
			eP = clothes;
			break;
		case  (int)Equipment.EquipPos.HAND:
			eP = hand;
			break;
		case  (int)Equipment.EquipPos.HEAD:
			eP = head;
			break;
		case  (int)Equipment.EquipPos.FOOT:
			eP = foot;
			break;
		}	
		
		return eP;
	}
	
	public void DestroyIfNotNull (Transform trans)
	{
		if (trans != null) {
			Destroy (trans.gameObject);
		}
	}

	public void UpdateUIEquip ()
	{
		//先清除所有装备
		DestroyIfNotNull (clothes.transform.FindChild ("UIEquip(Clone)"));
		DestroyIfNotNull (hand.transform.FindChild ("UIEquip(Clone)"));
		DestroyIfNotNull (head.transform.FindChild ("UIEquip(Clone)"));
		DestroyIfNotNull (foot.transform.FindChild ("UIEquip(Clone)"));


		for (int i=0; i<eList.Count; i++) {			
			GameObject eP = getEquipPosGo (eList [i].ep);
			GameObject uiEquipO = Instantiate (uiEquip, new Vector3 (eP.transform.position.x, eP.transform.position.y, 0), Quaternion.identity) as GameObject;
			uiEquipO.GetComponent<Image> ().sprite = Resources.Load<Sprite> (eList [i].prefabName);
			uiEquipO.GetComponent<UI_Equip> ().e = eList [i];
			uiEquipO.transform.SetParent (eP.transform);
		}

	}

	public void ShowCharInfo ()
	{
		if (!charInfoInit) {

			charInfoInit = true;

			cList = gData.characterList;

			//生成玩家
			GameObject uiCharAvatar = Instantiate (uiCharP, new Vector3 (avatar.transform.position.x, avatar.transform.position.y, 0), Quaternion.identity) as GameObject;
			uiCharAvatar.GetComponent<Image> ().sprite = Resources.Load<Sprite> (cList [0].PrefabName);
			uiCharAvatar.GetComponent<UI_Player> ().c = cList [0];
			uiCharAvatar.transform.SetParent (avatar.transform);
			
			//加载装备
			UpdateUIEquip ();
			
			//助手角色信息
			for (int i=1; i<=assList.Count; i++) {
				GameObject ass = assList [i - 1];
				GameObject uiCharAss = Instantiate (uiCharP, new Vector3 (ass.transform.position.x, ass.transform.position.y, 0), Quaternion.identity) as GameObject;
				uiCharAss.transform.SetParent (ass.transform);
			}
		}

		UpdateUIInfo ();
	}

	public void CloseWindow (Button button)
	{
		button.transform.parent.gameObject.SetActive (false);
	}

	public void Equip ()
	{
		charInfo.SetActive (!charInfo.activeInHierarchy);
		if (charInfo.activeInHierarchy) {
			ShowCharInfo ();
		}
	}

	void closeBag ()
	{
		itemInfo.SetActive (false);
		bag.SetActive (false);	
	}

	public void Item ()
	{
		itemInfo.SetActive (false);
		bag.SetActive (!bag.activeInHierarchy);
		Character currentC = gData.characterList [0];
		if (bag.activeInHierarchy)
			bag.SendMessage ("InitBag", currentC);
	}

	//更新玩家数据
	public void refreshCharacterData (Dictionary<string, object> role)
	{
		gData.characterList = DataHelper.GetCharacterFromServer (role, gData.siList);
		cList = gData.characterList;
		eList = cList [0].EquipList;
	}

	//交易 服务器callback
	public void TradeOver (Dictionary<string, object> role, string msg)
	{
		if (!msg.Equals ("ok")) {
			ShowHint.Hint (StringCollection.stringDict_CN [msg]);
		} else {
			//更新背包
			refreshCharacterData (role);
			ItemUseComplete ();
		}
	}

	//装备操作 服务器callback
	public void onEquipOperOver (Dictionary<string, object> role, string msg)
	{
		if (!msg.Equals ("ok")) {
			ShowHint.Hint (StringCollection.stringDict_CN [msg]);
		} else {
			refreshCharacterData (role);
			UpdateUIEquip ();
			ItemUseComplete ();
		}
	}

	//道具使用 服务器callback
	public void onUseItemOver (Dictionary<string, object> role, string msg)
	{
		if (!msg.Equals ("ok")) {
			ShowHint.Hint (StringCollection.stringDict_CN [msg]);
		} else {
			refreshCharacterData (role);
			ItemUseComplete ();
		}
	}

	//装备雇佣兵 服务器callback
	public void onAssistOperOver (Dictionary<string, object> role, string msg)
	{
		if (!msg.Equals ("ok")) {
			ShowHint.Hint (StringCollection.stringDict_CN [msg]);
		} else {
			refreshCharacterData (role);
			ItemUseComplete ();
		}
	}

	//玩家移动 callback
	public void OnPlayerMove (Dictionary<string, object> role)
	{
		refreshCharacterData (role);
		UpdateUIInfo ();
	}

	public void UseItem (Button button)
	{
		Baggrid bg = gData.currentItem;
		Item item = bg.Item;
		item.useable = true;

		//如果是商店模式，购买售出逻辑
		if (gData.isShop) {
			InputField tradeNum = itemInfo.transform.FindChild ("TradeNum").GetComponent<InputField> ();
		
			if (tradeNum.text.Equals ("")) {
				ShowHint.Hint (StringCollection.NOTRADENUM);
				return;
			}

			//获得道具ID
			string iid = item.prefabName.Split (new char[]{'/'}) [2].Split (new char[]{'_'}) [1];

			if (button.transform.FindChild ("Text").GetComponent<Text> ().text.Equals (StringCollection.BUY)) {
				gData.account.TradeItem (int.Parse (iid), 0, int.Parse (tradeNum.text), 0, bg.level);
			} else {
				gData.account.TradeItem (int.Parse (iid), 1, int.Parse (tradeNum.text), bg.dbid, bg.level);
			}

		} else {
			//打开人物属性面板
			charInfo.SetActive (true);
			ShowCharInfo ();
			//获取道具信息

			if (item.ct == (int)global::Item.CommonType.CONSUME) {
				//如果是群体道具，直接应用，否则等待点击玩家后使用

				Debug.Log ("sdfasdfafasdf  " + item.rt);

				if (item.rt == (int)global::Item.RangeType.MULTI) {
					gData.account.UseItem (cList [0].ObjName, "ALLTEAM", bg.dbid);
				} else {
					//人物格闪动
					GameUtil.UnFocus (focusList);
					focusList.Add (avatar.transform.FindChild ("UIChar(Clone)").gameObject);
					for (int i=1; i<cList.Count; i++) {
						GameObject ass = assList [i - 1];
						focusList.Add (ass.transform.FindChild ("UIChar(Clone)").gameObject);
					}
				}
				
			} else if (item.ct == (int)global::Item.CommonType.EQUIPMENT) {
				if (button.transform.FindChild ("Text").GetComponent<Text> ().text.Equals (StringCollection.NOEQUIP)) {
					//装备卸下
					gData.account.EquipOper (bg.Item.dbid, 1);
				} else { 
					//装备穿戴
					gData.account.EquipOper (bg.dbid, 0);
				}
			} else if (item.ct == (int)global::Item.CommonType.MERCENARY) {
				if (button.transform.FindChild ("Text").GetComponent<Text> ().text.Equals (StringCollection.LEAVETEAM)) {//离队
					gData.account.AssistOper (0, ((Mercenary)item).c.dbid);
				} else {
					//助手格闪动
					GameUtil.UnFocus (focusList);
					for (int i=0; i<assList.Count; i++) {
						focusList.Add (assList [i]);
					}
				}
			}
			itemInfo.SetActive (false);
		}
	}
	
	public void ItemUseComplete ()
	{
		GameUtil.UnFocus (focusList);
		//通知UI更新
		if (charInfo.activeInHierarchy) {
			UpdateUIInfo ();
		}
		itemInfo.SetActive (false);
		gData.currentItem = null;
		if (bag.activeInHierarchy) {
			bag.SendMessage ("InitBag", cList [0]);
		}
	}

	public void closeAllBag ()
	{
		charInfo.SetActive (false);
		closeBag ();
	}

	public void InputTradeNum (InputField input)
	{
		if (gData.currentItem.Num > 0 && !input.text.Equals ("")) {
			input.text = Mathf.Min (gData.currentItem.Num, int.Parse (input.text)).ToString ();
		}
	}
}
