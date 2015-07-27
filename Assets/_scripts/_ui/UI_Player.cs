using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Player : MonoBehaviour {

	public Character c;
	private GlobalData gData;
	private UI_Bag uiBag;

	void Awake ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		uiBag = GameObject.FindGameObjectWithTag ("GameController").GetComponent<UI_Bag> ();
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(delegate() {
			this.OnClick(); 
		});
	}
		
	public void OnClick(){
		if(gData.currentItem!=null){
			//对当前角色使用道具,此处From参数没有意义
			List<BattleObj> toList = new List<BattleObj>();
			toList.Add(c);
			gData.currentItem.Item.doSth(new BattleObj(),toList);
			//用完减少数量并移除
			gData.currentItem.Num--;
			uiBag.SendMessage("ItemUseComplete");
		}
	}
}
