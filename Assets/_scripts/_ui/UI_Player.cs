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

			if (gData.currentItem.Item.ct == (int)Item.CommonType.CONSUME) {

				gData.account.UseItem (gData.characterList [0].ObjName, c.dbid.ToString (), gData.currentItem.dbid);

			} else if (gData.currentItem.Item.ct == (int)Item.CommonType.MERCENARY) {
				if (gameObject.transform.parent.name.Contains ("Ass")) {
					//更换助手,如果点击的位置上有助手,替换,否则直接参战
					if (GetComponent<Image> ().sprite.name.Equals ("no_ass")) {
						gData.account.AssistOper (gData.currentItem.dbid, 0);			
					} else {
						gData.account.AssistOper (gData.currentItem.dbid, c.dbid);
					}
				}
			}
		} else {
			if (gameObject.transform.parent.name.Contains ("Ass") && GetComponent<UI_Player> ().c != null) {
				Character c = GetComponent<UI_Player> ().c;
				Mercenary m = new Mercenary (c);

				m.assPos = int.Parse (transform.parent.name.Split (new char[]{'_'}) [1]);
				itemInfo = GameObject.FindGameObjectWithTag ("UI").transform.FindChild ("ItemInfo").gameObject;
				itemInfo.SetActive (true);
				itemInfo.transform.FindChild ("Pic").GetComponent<Image> ().sprite = Resources.Load <Sprite> (m.prefabName);

				Transform button = itemInfo.transform.FindChild ("UseButton");

				string cNote = "";

				if (m.c.IsOnLinePlayer) {
					button.gameObject.SetActive (false);
				} else {
					cNote = gData.siList [m.c.iid].note;
					button.FindChild ("Text").GetComponent<Text> ().text = StringCollection.LEAVETEAM;
				}

				itemInfo.transform.FindChild ("Note").GetComponent<Text> ().text = m.note + cNote;

				gData.currentItem = new Baggrid (m, 1, 0);
				gData.currentItem.Item.useable = false;
			}
		}
	}
}
