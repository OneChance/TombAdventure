using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Input : MonoBehaviour
{

	public Transform itemMenu;
	public Transform player;
	public float moveDistance = 0.5f;
	private bool menuUnfold = false;
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
		closeBag ();
		charInfo.SetActive (!charInfo.activeInHierarchy);

		List<Character> cList = player.GetComponent<PlayerAction> ().characterList;

		if (charInfo.activeInHierarchy) {
			name.GetComponent<Text>().text = cList[0].ObjName;
			proname.GetComponent<Text>().text = cList[0].Pro.proname;
			avatar.GetComponent<Image>().sprite = Resources.Load<Sprite>("_images/_game/"+cList[0].PrefabName);
			avatar.GetComponent<Image>().color = Color.white;

			health.GetComponent<Text>().text = cList[0].Health.ToString();
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
		charInfo.SetActive (false);
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
