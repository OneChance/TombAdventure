using UnityEngine;
using System.Collections;
using KBEngine;
using System.Collections.Generic;
using System;

public class Kbe : MonoBehaviour
{

	public int ui_state = 0;
	UI_Login loginUI;

	// Use this for initialization
	void Start ()
	{

		loginUI = UnityEngine.GameObject.FindGameObjectWithTag ("GameController").GetComponent<UI_Login> ();

		installEvents ();
	}

	void installEvents ()
	{
		// common
		KBEngine.Event.registerOut ("onDisableConnect", this, "onDisableConnect");
		KBEngine.Event.registerOut ("onConnectStatus", this, "onConnectStatus");
		KBEngine.Event.registerOut ("onKicked", this, "onKicked");

		// login
		KBEngine.Event.registerOut ("onLoginFailed", this, "onLoginFailed");
		KBEngine.Event.registerOut ("onLoginGatewayFailed", this, "onLoginGatewayFailed");
		KBEngine.Event.registerOut ("onLoginSuccessfully", this, "onLoginSuccessfully");
		KBEngine.Event.registerOut ("login_baseapp", this, "login_baseapp");
		KBEngine.Event.registerOut ("Loginapp_importClientMessages", this, "Loginapp_importClientMessages");
		KBEngine.Event.registerOut ("onReqRoleList", this, "onReqRoleList");
		KBEngine.Event.registerOut ("onReqItemList", this, "onReqItemList");
		KBEngine.Event.registerOut ("onCreateRoleResult", this, "onCreateRoleResult");

		//trade
		KBEngine.Event.registerOut ("onTradeOver", this, "onTradeOver");
		//equip
		KBEngine.Event.registerOut ("onEquipOperOver", this, "onEquipOperOver");
		//item
		KBEngine.Event.registerOut ("onUseItemOver", this, "onUseItemOver");
		//assist
		KBEngine.Event.registerOut ("onAssistOperOver", this, "onAssistOperOver");
		//dig upadte
		KBEngine.Event.registerOut ("onDigUpdated", this, "onDigUpdated");
		//player move upadte
		KBEngine.Event.registerOut ("onPlayerMove", this, "onPlayerMove");
		//get scene data
		KBEngine.Event.registerOut ("OnGetSceneData", this, "OnGetSceneData");

		KBEngine.Event.registerOut ("OnGetBattleData", this, "OnGetBattleData");

		KBEngine.Event.registerOut ("onAddOp", this, "onAddOp");

		KBEngine.Event.registerOut ("OnBattleAnim", this, "OnBattleAnim");

		KBEngine.Event.registerOut ("onUndoOp", this, "onUndoOp");

		KBEngine.Event.registerOut ("battleOver", this, "battleOver");
	}

	void OnDestroy ()
	{
		KBEngine.Event.deregisterOut (this);
	}

	public void onConnectStatus (bool success)
	{
		if (!success)
			ShowHint.Hint ("connect(" + KBEngineApp.app.getInitArgs ().ip + ":" + KBEngineApp.app.getInitArgs ().port + ") is error! (连接错误)");
		else
			Debug.Log ("connect successfully, please wait...(连接成功，请等候...)");
	}

	public void onDisableConnect ()
	{
	}

	public void onKicked (UInt16 failedcode)
	{
		ShowHint.Hint (KBEngineApp.app.serverErr (failedcode));
		Application.LoadLevel ("login");
		ui_state = 0;
	}

	public void onLoginFailed (UInt16 failedcode)
	{
		//"login is failed(登陆失败), err=" + 
		if (failedcode == 20) {
			//login is failed(登陆失败), err=" + KBEngineApp.app.serverErr (failedcode) + ", " + 
			ShowHint.Hint (System.Text.Encoding.ASCII.GetString (KBEngineApp.app.serverdatas ()));
		} else {
			//"login is failed(登陆失败), err=" + 
			ShowHint.Hint (KBEngineApp.app.serverErr (failedcode));
		}
	}

	public void onLoginGatewayFailed (UInt16 failedcode)
	{
		ShowHint.Hint ("loginGateway is failed(登陆网关失败), err=" + KBEngineApp.app.serverErr (failedcode));
	}

	public void login_baseapp ()
	{
		Debug.Log ("connect to loginGateway, please wait...(连接到网关， 请稍后...)");
	}

	public void onLoginSuccessfully (UInt64 rndUUID, Int32 eid, Account accountEntity)
	{
		Debug.Log ("login is successfully!(登陆成功!)");
		ui_state = 1;
	}

	public void Loginapp_importClientMessages ()
	{
		Debug.Log ("Loginapp_importClientMessages ...");
	}

	public void onReqRoleList (Dictionary<UInt64, Dictionary<string, object>> roleList)
	{
		loginUI.roleList = roleList;
	}

	/*请求道具信息callback*/
	public void onReqItemList (Dictionary<UInt64, Dictionary<string, object>> itemList, List<object> itemshop, List<object> assistshop, List<object> equipshop, List<object> tombs)
	{
		loginUI.ItemDown (itemList, itemshop, assistshop, equipshop, tombs);
	}

	/*创建角色callback*/
	public void onCreateRoleResult (Dictionary<string, object> role)
	{
		UI_Create createUI = UnityEngine.GameObject.FindGameObjectWithTag ("UI").GetComponent<UI_Create> ();
		createUI.OnCreateRole (role);
	}

	/*交易callback*/
	public void onTradeOver (Dictionary<string, object> role, string msg)
	{
		UI_Bag bagUI = UnityEngine.GameObject.FindGameObjectWithTag ("GameController").GetComponent<UI_Bag> ();
		bagUI.TradeOver (role, msg);
	}

	/*装备操作callback*/
	public void onEquipOperOver (Dictionary<string, object> role, string msg)
	{
		UI_Bag bagUI = UnityEngine.GameObject.FindGameObjectWithTag ("GameController").GetComponent<UI_Bag> ();
		bagUI.onEquipOperOver (role, msg);
	}

	/*使用道具callback*/
	public void onUseItemOver (Dictionary<string, object> role, string msg)
	{
		UI_Bag bagUI = UnityEngine.GameObject.FindGameObjectWithTag ("GameController").GetComponent<UI_Bag> ();
		bagUI.onUseItemOver (role, msg);
	}

	/*装备雇佣兵 callback*/
	public void onAssistOperOver (Dictionary<string, object> role, string msg)
	{
		UI_Bag bagUI = UnityEngine.GameObject.FindGameObjectWithTag ("GameController").GetComponent<UI_Bag> ();
		bagUI.onAssistOperOver (role, msg);
	}

	/*Dig信息update callback*/
	public void onDigUpdated (Dictionary<string, object> digInfo, Dictionary<string, object> role, string msg)
	{
		SceneGen sceneGen = UnityEngine.GameObject.FindGameObjectWithTag ("GameController").GetComponent<SceneGen> ();
		sceneGen.OnDigUpdated (digInfo, role, msg);
	}

	/*玩家移动 callback*/
	public void onPlayerMove (Dictionary<string, object> role)
	{
		UI_Bag bagUI = UnityEngine.GameObject.FindGameObjectWithTag ("GameController").GetComponent<UI_Bag> ();
		//可能在快速移动的某个时间点，碰到了敌人，导致场景跳转，那么此时就找不到bagUI了
		if (bagUI != null) {
			bagUI.OnPlayerMove (role);
		}
	}
	
	public void OnGetSceneData (List<object> enemyTypeList)
	{
		SceneGen sceneGen = UnityEngine.GameObject.FindGameObjectWithTag ("GameController").GetComponent<SceneGen> ();
		sceneGen.OnGetSceneData (enemyTypeList);
	}

	public void OnGetBattleData (Dictionary<string, object> role, List<object> bag)
	{
		Battle battle = UnityEngine.GameObject.FindGameObjectWithTag ("GameController").GetComponent<Battle> ();
		battle.OnGetBattleData (role,bag);
	}

	public void onAddOp (int opCount)
	{
		Battle battle = UnityEngine.GameObject.FindGameObjectWithTag ("GameController").GetComponent<Battle> ();
		battle.OnAddOp (opCount);
	}

	public void OnBattleAnim (int itemid)
	{
		Battle battle = UnityEngine.GameObject.FindGameObjectWithTag ("GameController").GetComponent<Battle> ();
		battle.OnBattleAnim (itemid);
	}

	public void onUndoOp (string from_tag)
	{
		Battle battle = UnityEngine.GameObject.FindGameObjectWithTag ("GameController").GetComponent<Battle> ();
		battle.OnUndoOp (from_tag);
	}

	public void battleOver (string battle_res, Dictionary<string,object> playerInfo, List<object> assistList)
	{
		Battle battle = UnityEngine.GameObject.FindGameObjectWithTag ("GameController").GetComponent<Battle> ();
		battle.BattleOver (battle_res, playerInfo, assistList);
	}
}
