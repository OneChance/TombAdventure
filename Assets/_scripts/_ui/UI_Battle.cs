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

	public void ItemClick ()
	{
		bag.SetActive (true);
		Character currentC = (Character)battle.waitForAttack [0].GetComponent<PosChar> ().battleObj;
		bag.SendMessage ("InitBag",currentC);
	}

	public void WaitClick ()
	{
		bag.SetActive (false);
	}

	public void AttackClick ()
	{
		bag.SetActive (false);
		battle.SendMessage ("Act", Op.ATTACK);
	}

	public void OkClick ()
	{
		battle.SendMessage ("AddOp", Op.ATTACK);
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
