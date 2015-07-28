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
	private GameObject name;
	private GameObject proname;
	private GameObject health;
	private GameObject stamina;
	private GameObject strength;
	private GameObject archeology;
	private List<GameObject> assList;
	private List<GameObject> focusList;
	private GameObject uiCharP;
	private GameObject uiEquip;
	private List<Character> cList;
	private List<Equipment> eList;
	private List<Baggrid> bgList;
	public GameObject bag;
	public GameObject itemInfo;
	public GameObject charInfo;
	private GlobalData gData;
	
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
		name = charInfo.transform.FindChild ("Name").gameObject;
		proname = charInfo.transform.FindChild ("Proname").gameObject;
		health = charInfo.transform.FindChild ("Health").gameObject;
		stamina = charInfo.transform.FindChild ("Stamina").gameObject;
		strength = charInfo.transform.FindChild ("Strength").gameObject;
		archeology = charInfo.transform.FindChild ("Archeology").gameObject;
		
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
		bgList = cList [0].BgList;
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
		//玩家角色信息
		stamina.GetComponent<Text> ().text = cList [0].Stamina.ToString ();
		health.GetComponent<Text> ().text = cList [0].Health.ToString ();
		strength.GetComponent<Text> ().text = cList [0].strength.ToString ();
		archeology.GetComponent<Text> ().text = cList [0].archeology.ToString ();
		//助手角色信息
		for (int i=1; i<cList.Count; i++) {
			GameObject ass = assList [i - 1];
			GameObject info = ass.transform.FindChild ("Info").gameObject;
			info.transform.FindChild ("S").GetComponent<Text> ().text = cList [i].Stamina.ToString ();
			info.transform.FindChild ("H").GetComponent<Text> ().text = cList [i].Health.ToString ();
			info.transform.FindChild ("Str").GetComponent<Text> ().text = cList [i].strength.ToString ();
			info.transform.FindChild ("Arc").GetComponent<Text> ().text = cList [i].archeology.ToString ();
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
		if (!haveUpdate) {
			Destroy (getEquipPosGo (pos).transform.FindChild ("UIEquip(Clone)").gameObject);
		}
	}

	public void InitCharInfo ()
	{
		if (name.GetComponent<Text> ().text.Equals ("")) {
			//玩家角色信息
			name.GetComponent<Text> ().text = cList [0].ObjName;
			proname.GetComponent<Text> ().text = cList [0].Pro.proname;
			
			//生成玩家
			GameObject uiCharAvatar = Instantiate (uiCharP, new Vector3 (avatar.transform.position.x, avatar.transform.position.y, 0), Quaternion.identity) as GameObject;
			uiCharAvatar.GetComponent<Image> ().sprite = Resources.Load<Sprite> (cList [0].PrefabName);
			uiCharAvatar.GetComponent<UI_Player> ().c = cList [0];
			uiCharAvatar.transform.SetParent (avatar.transform);
			
			charInfo.transform.FindChild ("StaminaLable").GetComponent<Text> ().text = StringCollection.STAMINA;
			charInfo.transform.FindChild ("StrengthLable").GetComponent<Text> ().text = StringCollection.STRENGTH;
			charInfo.transform.FindChild ("ArcheologyLable").GetComponent<Text> ().text = StringCollection.ARCHEOLOGY;
			charInfo.transform.FindChild ("HealthLable").GetComponent<Text> ().text = StringCollection.HEALTH;
			
			//加载装备
			UpdateUIEquip (Equipment.EquipPos.ALL);
			
			//助手角色信息
			for (int i=1; i<cList.Count; i++) {
				GameObject ass = assList [i - 1];
				GameObject info = ass.transform.FindChild ("Info").gameObject;
				info.SetActive (true);
				
				GameObject uiCharAss = Instantiate (uiCharP, new Vector3 (ass.transform.position.x, ass.transform.position.y, 0), Quaternion.identity) as GameObject;
				uiCharAss.GetComponent<Image> ().sprite = Resources.Load<Sprite> (cList [i].PrefabName);
				uiCharAss.transform.SetParent (ass.transform);
				uiCharAss.GetComponent<UI_Player> ().c = cList [i];
				info.transform.FindChild ("Name").GetComponent<Text> ().text = cList [i].ObjName;
				info.transform.FindChild ("Pro").GetComponent<Text> ().text = cList [i].Pro.proname;
				
				info.transform.FindChild ("SL").GetComponent<Text> ().text = StringCollection.STAMINA;
				info.transform.FindChild ("StrL").GetComponent<Text> ().text = StringCollection.STRENGTH;
				info.transform.FindChild ("ArcL").GetComponent<Text> ().text = StringCollection.ARCHEOLOGY;
				info.transform.FindChild ("HL").GetComponent<Text> ().text = StringCollection.HEALTH;
			}
			
			UpdateUIInfo ();
		}
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

		List<Baggrid> bgList = gData.characterList [0].BgList;

		//如果是商店模式，购买售出逻辑
		if (gData.isShop) {
			InputField tradeNum = itemInfo.transform.FindChild ("TradeNum").GetComponent<InputField> ();
		
			if (tradeNum.text.Equals ("")) {
				Debug.Log (StringCollection.NOTRADENUM);
				return;
			}

			int tradeMoney  = int.Parse (tradeNum.text) * item.price;

			if (button.transform.FindChild ("Text").GetComponent<Text> ().text.Equals (StringCollection.BUY)) {

				if(tradeMoney>gData.characterList [0].money){
					Debug.Log(StringCollection.NOTENOUGHMONEY);
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
					bgList.Add (bg);
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
							if (bgList [i].Item.name == item.name) {
								bgList [i].Item = oldE;
							}
						}
					}
				}
				
				UpdateUIEquip (((Equipment)item).ep);
				cList [0].EquipList = eList;
				ItemUseComplete ();
			}
			itemInfo.SetActive (false);
		}
	}
	
	public void ItemUseComplete ()
	{
		GameUtil.UnFocus (focusList);
		//通知UI更新
		UpdateUIInfo ();
		itemInfo.SetActive (false);
		gData.currentItem = null;
		bag.SendMessage ("InitBag", cList [0]);
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
