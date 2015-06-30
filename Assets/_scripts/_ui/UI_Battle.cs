using UnityEngine;
using System.Collections;

public class UI_Battle : MonoBehaviour
{

	private Battle battle;

	void Awake ()
	{
		battle = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Battle> ();
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

	}

	public void WaitClick ()
	{

	}

	public void AttackClick ()
	{
		battle.SendMessage ("Act", Op.ATTACK);
	}

	public void OkClick ()
	{
		battle.SendMessage ("AddOp", Op.ATTACK);
	}

	public void ProClick ()
	{
		
	}

	public void PosClick (GameObject clickGo)
	{
		battle.SendMessage ("ChooseTarget", clickGo);
	}
}
