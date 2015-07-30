using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Player : MonoBehaviour
{

	public Character c;
	private GlobalData gData;
	private UI_Bag uiBag;
	private GameObject itemInfo;

	void Awake ()
	{
		gData = GameObject.FindGameObjectWithTag ("GlobalData").GetComponent<GlobalData> ();
		uiBag = GameObject.FindGameObjectWithTag ("GameController").GetComponent<UI_Bag> ();
		Button btn = gameObject.GetComponent<Button> ();
		btn.onClick.AddListener (delegate() {
			this.OnClick (); 
		});
	}
		
	public void OnClick ()
	{
		if (gData.currentItem != null && gData.currentItem.Item.useable) {

			if (gData.currentItem.Item.ct == Item.CommonType.CONSUME) {
				//对当前角色使用道具,此处From参数没有意义
				List<BattleObj> toList = new List<BattleObj> ();
				toList.Add (c);
				gData.currentItem.Item.doSth (new BattleObj (), toList);
				//用完减少数量并移除
				gData.currentItem.Num--;
				uiBag.SendMessage ("ItemUseComplete");
			} else if (gData.currentItem.Item.ct == Item.CommonType.MERCENARY) {
				if (gameObject.transform.parent.name.Contains ("Ass")) {
					//更换助手
					Mercenary m = (Mercenary)(gData.currentItem.Item);
					//如果点击的位置上有助手,替换,否则直接参战
					if (GetComponent<Image> ().sprite.name.Equals ("no_ass")) {
						gData.characterList.Add (m.c);	
						gData.currentItem.Num--;				
					} else {
						Character posC = gData.characterList [int.Parse (transform.parent.name.Split (new char[]{'_'}) [1])];

						gData.characterList [int.Parse (transform.parent.name.Split (new char[]{'_'}) [1])] = m.c;

						List<Baggrid> bgList = gData.characterList [0].BgList;

						for (int i=0; i<bgList.Count; i++) {
							if (bgList [i].Item.name.Equals (m.name)) {
								bgList [i].Item = new Mercenary (posC);
								break;
							}
						}
					}
					uiBag.SendMessage ("ItemUseComplete");
				}
			}
		} else {
			if (gameObject.transform.parent.name.Contains ("Ass") && GetComponent<UI_Player>().c !=null) {
				Character c = GetComponent<UI_Player> ().c;
				Mercenary m = new Mercenary (c);
				m.assPos = int.Parse (transform.parent.name.Split (new char[]{'_'}) [1]);
				itemInfo = GameObject.FindGameObjectWithTag ("UI").transform.FindChild ("ItemInfo").gameObject;
				itemInfo.SetActive (true);
				itemInfo.transform.FindChild ("Pic").GetComponent<Image> ().sprite = Resources.Load <Sprite> (m.prefabName);
				itemInfo.transform.FindChild ("Note").GetComponent<Text> ().text = m.note;
				Text buttonText = itemInfo.transform.FindChild ("UseButton").FindChild ("Text").GetComponent<Text> ();
				buttonText.text = StringCollection.LEAVETEAM;
				gData.currentItem = new Baggrid (m, 1);
			}
		}
	}
}
