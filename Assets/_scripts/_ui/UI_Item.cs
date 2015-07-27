﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour {
	
	private GameObject itemInfo;
	private GlobalData gData;
	private Baggrid bg;
	public bool fromShop = false;
	private InputField tradeNum;

	void Awake ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(delegate() {
			this.OnClick(); 
		});
	}
		
	public void OnClick(){
		itemInfo = GameObject.FindGameObjectWithTag("UI").transform.FindChild("ItemInfo").gameObject;
		itemInfo.SetActive (true);
		gData.currentItem = bg;
		itemInfo.transform.FindChild("Pic").GetComponent<Image>().sprite = Resources.Load <Sprite>("_images/_ui/"+bg.Item.prefabName);
		itemInfo.transform.FindChild ("ItemName").GetComponent<Text> ().text = bg.Item.name;
		itemInfo.transform.FindChild ("Note").GetComponent<Text> ().text = bg.Item.note;

		Text buttonText = itemInfo.transform.FindChild("UseButton").FindChild("Text").GetComponent<Text>();

		tradeNum = itemInfo.transform.FindChild("TradeNum").GetComponent<InputField>();

		if(tradeNum){
			tradeNum.text = "";
		}

		if(gData.isShop){
			itemInfo.transform.FindChild("TradeNum").gameObject.SetActive(true);
			if(fromShop){
				buttonText.text = StringCollection.BUY;
			}else{
				buttonText.text = StringCollection.SELL;
				tradeNum.text = bg.Num.ToString(); //默认最大数量
			}
		}else{
			itemInfo.transform.FindChild("TradeNum").gameObject.SetActive(false);
			if(bg.Item.ct==Item.CommonType.CONSUME){
				buttonText.text = StringCollection.ITEMUSE;
			}else if(bg.Item.ct==Item.CommonType.EQUIPMENT){
				buttonText.text = StringCollection.ITEMEQUIP;
			}
		}
	}

	public Baggrid Bg {
		get {
			return this.bg;
		}
		set {
			bg = value;
		}
	}

}
