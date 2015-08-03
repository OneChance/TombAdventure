using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

	public GameObject getEquipPosGo (Equipment.EquipPos pos)
	{
		
		GameObject eP = null;		
		
		switch (pos) {
		case Equipment.EquipPos.BODY:
			eP = clothes;
			break;
		case Equipment.EquipPos.HAND:
			eP = hand;
			break;
		case Equipment.EquipPos.HEAD:
			eP = head;
			break;
		case Equipment.EquipPos.FOOT:
			eP = foot;
			break;
		}	
		
		return eP;
	}
	
	public void UpdateUIEquip (Equipment.EquipPos pos)
	{
		
		bool haveUpdate = false;
		
		for (int i=0; i<eList.Count; i++) {
			
			GameObject eP = getEquipPosGo (eList [i].ep);
			
			if (pos == Equipment.EquipPos.ALL || pos == eList [i].ep) {
				
				haveUpdate = true;
				
				//检测该位置有没有装备,如果有先删除
				if (eP.transform.FindChild ("UIEquip(Clone)") != null) {
					Destroy (eP.transform.FindChild ("UIEquip(Clone)").gameObject);
				}
				
				GameObject uiEquipO = Instantiate (uiEquip, new Vector3 (eP.transform.position.x, eP.transform.position.y, 0), Quaternion.identity) as GameObject;
				uiEquipO.GetComponent<Image> ().sprite = Resources.Load<Sprite> (eList [i].prefabName);
				uiEquipO.GetComponent<UI_Equip> ().e = eList [i];
				uiEquipO.transform.SetParent (eP.transform);
			}
		}
		
		//如果要更新的位置，已经没有装备，移除
		if (!haveUpdate && pos != Equipment.EquipPos.ALL) {
			Destroy (getEquipPosGo (pos).transform.FindChild ("UIEquip(Clone)").gameObject);
		}
	}

	public void InitCharInfo ()
	{
		if (!charInfoInit) {

			charInfoInit = true;

			//生成玩家
			GameObject uiCharAvatar = Instantiate (uiCharP, new Vector3 (avatar.transform.position.x, avatar.transform.position.y, 0), Quaternion.identity) as GameObject;
			uiCharAvatar.GetComponent<Image> ().sprite = Resources.Load<Sprite> (cList [0].PrefabName);
			uiCharAvatar.GetComponent<UI_Player> ().c = cList [0];
			uiCharAvatar.transform.SetParent (avatar.transform);
			
			//加载装备
			UpdateUIEquip (Equipment.EquipPos.ALL);
			
			//助手角色信息
			for (int i=1; i<=assList.Count; i++) {
				GameObject ass = assList [i - 1];
				GameObject uiCharAss = Instantiate (uiCharP, new Vector3 (ass.transform.position.x, ass.transform.position.y, 0), Quaternion.identity) as GameObject;
				uiCharAss.transform.SetParent (ass.transform);
			}
			
			UpdateUIInfo ();
		}
	}

	public void CloseWindow (Button button)
	{
		button.transform.parent.gameObject.SetActive (false);
	}

	public void Equip ()
	{
		charInfo.SetActive (!charInfo.activeInHierarchy);
		if (charInfo.activeInHierarchy) {
			InitCharInfo ();
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
	
	public void UseItem (Button button)
	{
		Baggrid bg = gData.currentItem;
		Item item = bg.Item;
		item.useable = true;

		List<Baggrid> bgList = gData.characterList [0].BgList;

		//如果是商店模式，购买售出逻辑
		if (gData.isShop) {
			InputField tradeNum = itemInfo.transform.FindChild ("TradeNum").GetComponent<InputField> ();
		
			if (tradeNum.text.Equals ("")) {
				ShowHint.Hint (StringCollection.NOTRADENUM);
				return;
			}

			int tradeMoney = int.Parse (tradeNum.text) * item.price;

			if (button.transform.FindChild ("Text").GetComponent<Text> ().text.Equals (StringCollection.BUY)) {

				if (tradeMoney > gData.characterList [0].money) {
					ShowHint.Hint (StringCollection.NOTENOUGHMONEY);
					return;
				}

				gData.characterList [0].money -= tradeMoney;
				BagUtil.AddItem (bgList, new Baggrid (item, int.Parse (tradeNum.text)));
			} else {
				gData.characterList [0].money += (int)(tradeMoney * 0.5); //卖出只有买入价格的二分之一
				bg.Num -= int.Parse (tradeNum.text);
			}

			ItemUseComplete ();
		} else {
			//打开人物属性面板
			charInfo.SetActive (true);
			InitCharInfo ();
			//获取道具信息

			if (item.ct == global::Item.CommonType.CONSUME) {
				//人物格闪动
				GameUtil.UnFocus (focusList);
				focusList.Add (avatar.transform.FindChild ("UIChar(Clone)").gameObject);
				for (int i=1; i<cList.Count; i++) {
					GameObject ass = assList [i - 1];
					focusList.Add (ass.transform.FindChild ("UIChar(Clone)").gameObject);
				}
				
				//如果是群体道具，直接应用，否则等待点击玩家后使用
				if (item.rt == global::Item.RangeType.MULTI) {
					bg.Item.doSth (cList [0], cList);
					//用完减少数量
					gData.currentItem.Num--;
					ItemUseComplete ();
				}
				
			} else if (item.ct == global::Item.CommonType.EQUIPMENT) {
				if (button.transform.FindChild ("Text").GetComponent<Text> ().text.Equals (StringCollection.NOEQUIP)) {//装备卸下
					//从装备中移除
					for (int i=0; i<eList.Count; i++) {
						if (eList [i].ep == ((Equipment)item).ep) {
							eList.Remove (eList [i]);
							break;
						}
					}
					//添加到背包
					BagUtil.AddItem (bgList, bg);

				} else { //装备穿戴
					bool haveE = false;
					Equipment oldE = null;
					
					for (int i=0; i<eList.Count; i++) {
						if (eList [i].ep == ((Equipment)item).ep) {
							oldE = eList [i];
							eList [i] = (Equipment)item; //替换装备
							haveE = true;
							break;
						}
					}
					
					if (!haveE) {
						eList.Add ((Equipment)item);
						//从背包移除
						for (int i=0; i<bgList.Count; i++) {
							if (bgList [i].Item.name.Equals (item.name)) {
								bgList.Remove (bgList [i]);
							}
						}
					} else {
						//替换背包里的装备，找到同名装备替换，即使有多个，因为背包会自动排序，随便替换一个也没有问题
						for (int i=0; i<bgList.Count; i++) {
							if (bgList [i].Item.name.Equals (item.name)) {
								bgList [i].Item = oldE;
								break;
							}
						}
					}
				}
				
				UpdateUIEquip (((Equipment)item).ep);
				cList [0].EquipList = eList;
				ItemUseComplete ();
			} else if (item.ct == global::Item.CommonType.MERCENARY) {
				if (button.transform.FindChild ("Text").GetComponent<Text> ().text.Equals (StringCollection.LEAVETEAM)) {//离队

					//从队中移除
					Mercenary m = (Mercenary)item;
					if (m.assPos > 0) {
						cList.Remove (cList [m.assPos]);
					}
					
					//添加到背包
					BagUtil.AddItem (bgList, bg);
					ItemUseComplete ();
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
		gData.currentItem.Item.useable = false;
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
