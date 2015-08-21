using UnityEngine;
using System.Collections;
using KBEngine;
using System.Collections.Generic;
using System;

public class Kbe : MonoBehaviour {

    public int ui_state = 0;

    UI_Login loginUI;

	// Use this for initialization
	void Start () {

        loginUI = UnityEngine.GameObject.FindGameObjectWithTag("GameController").GetComponent<UI_Login>();

        installEvents();
	}

    void installEvents()
    {
        // common
        KBEngine.Event.registerOut("onDisableConnect", this, "onDisableConnect");
        KBEngine.Event.registerOut("onConnectStatus", this, "onConnectStatus");
        KBEngine.Event.registerOut("onKicked", this, "onKicked");

        // login
        KBEngine.Event.registerOut("onLoginFailed", this, "onLoginFailed");
        KBEngine.Event.registerOut("onLoginGatewayFailed", this, "onLoginGatewayFailed");
        KBEngine.Event.registerOut("onLoginSuccessfully", this, "onLoginSuccessfully");
        KBEngine.Event.registerOut("login_baseapp", this, "login_baseapp");
        KBEngine.Event.registerOut("Loginapp_importClientMessages", this, "Loginapp_importClientMessages");
        KBEngine.Event.registerOut("onReqRoleList", this, "onReqRoleList");
        KBEngine.Event.registerOut("onReqItemList", this, "onReqItemList");
    }

    void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }

    public void onConnectStatus(bool success)
    {
        if (!success)
            ShowHint.Hint("connect(" + KBEngineApp.app.getInitArgs().ip + ":" + KBEngineApp.app.getInitArgs().port + ") is error! (连接错误)");
        else
            Debug.Log("connect successfully, please wait...(连接成功，请等候...)");
    }

    public void onDisableConnect()
    {
    }

    public void onKicked(UInt16 failedcode)
    {
        ShowHint.Hint(KBEngineApp.app.serverErr(failedcode));
        Application.LoadLevel("login");
        ui_state = 0;
    }

    public void onLoginFailed(UInt16 failedcode)
    {
        if (failedcode == 20)
        {
            //login is failed(登陆失败), err=" + KBEngineApp.app.serverErr (failedcode) + ", " + 
            ShowHint.Hint(System.Text.Encoding.ASCII.GetString(KBEngineApp.app.serverdatas()));
        }
        else
        {
            //"login is failed(登陆失败), err=" + 
            ShowHint.Hint(KBEngineApp.app.serverErr(failedcode));
        }
    }

    public void onLoginGatewayFailed(UInt16 failedcode)
    {
        ShowHint.Hint("loginGateway is failed(登陆网关失败), err=" + KBEngineApp.app.serverErr(failedcode));
    }

    public void login_baseapp()
    {
        Debug.Log("connect to loginGateway, please wait...(连接到网关， 请稍后...)");
    }

    public void onLoginSuccessfully(UInt64 rndUUID, Int32 eid, Account accountEntity)
    {
        Debug.Log("login is successfully!(登陆成功!)");
        ui_state = 1;
    }

    public void Loginapp_importClientMessages()
    {
        Debug.Log("Loginapp_importClientMessages ...");
    }

    public void onReqRoleList(Dictionary<UInt64, Dictionary<string, object>> roleList)
    {
        loginUI.roleList = roleList;
    }

	public void onReqItemList(Dictionary<UInt64, Dictionary<string, object>> itemList) {
		loginUI.ItemDown(itemList);
    }
}
