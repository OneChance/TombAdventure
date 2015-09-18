using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Battle : MonoBehaviour
{

	private Battle battle;
	public GameObject bag;
	public GameObject itemInfo;

	void Awake ()
	{
		battle = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Battle> ();
	}

	public enum Op
	{
		NOACT=1,
		ATTACK=2,
		ITEM=3,
		WAIT=4,
		PRO=5
	}

	public void closeBag ()
	{
		if(itemInfo.activeInHierarchy){
			itemInfo.SetActive (false);
		}
		if(bag.activeInHierarchy){
			bag.SetActive (false);
		}
	}

	public void UseItem ()
	{
		closeBag ();
		Action act = new Action (Op.ITEM, battle.gData.currentItem);
		battle.gData.currentItem = null;
		battle.SendMessage ("Act", act);
	}

	public void ItemClick ()
	{
		itemInfo.SetActive (false);
		bag.SetActive (!bag.activeInHierarchy);
		//只获得第一个玩家的背包（不论是其他游戏玩家，还是NPC玩家，都没有背包）
		Character currentC = (Character)battle.characterList [0];
		if (bag.activeInHierarchy)
			bag.SendMessage ("InitBag", currentC);
	}

	public void WaitClick ()
	{
		closeBag ();
		Action act = new Action (Op.WAIT, new Baggrid (new WaitItem (), 1,-1));
		battle.SendMessage ("Act", act);
	}

	public void AttackClick ()
	{
		closeBag ();
		Action act = new Action (Op.ATTACK, new Baggrid (new AttackItem (), 1,-1));
		battle.SendMessage ("Act", act);
	}

	public void OkClick ()
	{
		battle.SendMessage ("AddOp");
	}

	public void PosClick (GameObject clickGo)
	{
		battle.SendMessage ("ChooseTarget", clickGo);
	}

	public void UndoClick ()
	{
		battle.SendMessage ("Undo");
	}

	public void CloseWindow (Button button)
	{
		button.transform.parent.gameObject.SetActive (false);
	}
}
