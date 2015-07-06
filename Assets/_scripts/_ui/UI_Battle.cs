using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class UI_Battle : MonoBehaviour
{

	private Battle battle;
	private GlobalData gData;
	public GameObject bag;

	void Awake ()
	{
		battle = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Battle> ();
		gData = battle.gData;
	}

	public enum Op
	{
		NOACT,
		ATTACK,
		ITEM,
		WAIT,
		PRO
	}

	public void UseItem(Baggrid bg){
		bag.SetActive (false);
		Action act = new Action (Op.ITEM,bg);
		battle.SendMessage ("Act", act);
	}

	public void ItemClick ()
	{
		bag.SetActive (!bag.activeInHierarchy);
		Character currentC = (Character)battle.waitForAttack [0].GetComponent<PosChar> ().battleObj;
		if(bag.activeInHierarchy)
			bag.SendMessage ("InitBag",currentC);
	}

	public void WaitClick ()
	{
		bag.SetActive (false);
	}

	public void AttackClick ()
	{
		bag.SetActive (false);
		Action act = new Action (Op.ATTACK,new Baggrid (new AttackItem (), 1));
		battle.SendMessage ("Act", act);
	}

	public void OkClick ()
	{
		battle.SendMessage ("AddOp");
	}

	public void ProClick ()
	{
		bag.SetActive (false);
	}

	public void PosClick (GameObject clickGo)
	{
		battle.SendMessage ("ChooseTarget", clickGo);
	}

	public void UndoClick ()
	{
		battle.SendMessage ("Undo");
	}
}
