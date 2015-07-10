using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Input : MonoBehaviour
{

	public Transform itemMenu;
	public Transform player;
	public float moveDistance = 0.5f;
	public GameObject bagContainer;
	public GameObject bag;
	public GameObject itemInfo;
	public GameObject charInfo;

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
	private List<GameObject> assList;

	void Awake(){
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
		assList = new List<GameObject> ();
		assList.Add (ass_1);
		assList.Add (ass_2);
		assList.Add (ass_3);
	}

	public void left ()
	{
		player.Translate (Vector2.left * moveDistance);
	}

	public void right ()
	{
		player.Translate (Vector2.right * moveDistance);
	}

	public void up ()
	{
		player.Translate (Vector2.up * moveDistance);
	}

	public void down ()
	{
		player.Translate (Vector2.down * moveDistance);
	}

	public void Equip ()
	{
		charInfo.SetActive (!charInfo.activeInHierarchy);

		List<Character> cList = player.GetComponent<PlayerAction> ().characterList;

		if (charInfo.activeInHierarchy) {
			//玩家角色信息
			name.GetComponent<Text>().text = cList[0].ObjName;
			proname.GetComponent<Text>().text = cList[0].Pro.proname;
			avatar.GetComponent<Image>().sprite = Resources.Load<Sprite>("_images/_game/"+cList[0].PrefabName);
			avatar.GetComponent<Image>().color = Color.white;
			health.GetComponent<Text>().text = cList[0].Health.ToString();
			stamina.GetComponent<Text>().text = cList[0].Stamina.ToString();
			//助手角色信息
			for(int i=1;i<cList.Count;i++){
				GameObject ass = assList[i-1];
				GameObject info = ass.transform.FindChild("Info").gameObject;
				info.SetActive(true);
				ass.GetComponent<Image>().sprite = Resources.Load<Sprite>("_images/_game/"+cList[i].PrefabName);
				ass.GetComponent<Image>().color = Color.white;
				info.transform.FindChild("H").GetComponent<Text>().text = cList[i].Health.ToString();
				info.transform.FindChild("S").GetComponent<Text>().text = cList[i].Stamina.ToString();
				info.transform.FindChild("Name").GetComponent<Text>().text = cList[i].ObjName;
				info.transform.FindChild("Pro").GetComponent<Text>().text = cList[i].Pro.proname;
			}
		}
	}


	void closeBag(){
		itemInfo.SetActive (false);
		bagContainer.SetActive (false);	
	}

	public void Dig ()
	{
		charInfo.SetActive (false);
		closeBag ();
	}

	public void Item ()
	{
		itemInfo.SetActive (false);
		bagContainer.SetActive (!bagContainer.activeInHierarchy);
		Character currentC = player.GetComponent<PlayerAction>().characterList[0];
		if(bagContainer.activeInHierarchy)
			bag.SendMessage ("InitBag",currentC);
	}

	public void UseItem(){
		Debug.Log ("use item");
	}
}
