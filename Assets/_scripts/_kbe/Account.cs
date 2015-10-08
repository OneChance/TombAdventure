namespace KBEngine
{
	using UnityEngine; 
	using System; 
	using System.Collections; 
	using System.Collections.Generic;
	using System.Linq;
	
	public class Account : KBEngine.GameObject
	{

		public static bool isInit = false;
		public Dictionary<UInt64, Dictionary<string, object>> roles = new Dictionary<UInt64, Dictionary<string, object>> ();

		public Account ()
		{
		}
		
		public override void __init__ ()
		{
			//gdata交出控制权后，会重新加载初始化方法，此处不再请求角色列表
			if (!isInit) {
				Event.fireOut ("onLoginSuccessfully", new object[] {
					KBEngineApp.app.entity_uuid,
					id,
					this
				});
				baseCall ("reqRoleList");     
				isInit = true;
			}

		}

		public void onReqRoleList (Dictionary<string, object> roleList)
		{

			roles.Clear ();

			List<object> listinfos = (List<object>)roleList ["values"];

			for (int i = 0; i < listinfos.Count; i++) {
				Dictionary<string, object> info = (Dictionary<string, object>)listinfos [i];
				roles.Add ((UInt64)info ["dbid"], info);
			}

			// ui event
			Dictionary<UInt64, Dictionary<string, object>> rList = new Dictionary<ulong, Dictionary<string, object>> (roles);
			KBEngine.Event.fireOut ("onReqRoleList", new object[] { rList });

			if (listinfos.Count == 0)
				return;
		}

		public void reqCreateRole (string name, string pro)
		{
			baseCall ("reqCreateRole", name, pro);
		}

		public void onCreateRoleResult (Dictionary<string, object> role)
		{
			KBEngine.Event.fireOut ("onCreateRoleResult", role);
		}

		/*
			level:如果出售雇佣兵，该变量为雇佣兵等级，依次计算价格
			dbid:与服务器对应的背包格id
		*/
		public void TradeItem (int iid, int tradeType, int num, int dbid, int level)
		{
			baseCall ("tradeItem", iid, tradeType, num, dbid, level);
		}

		public void onTradeOver (Dictionary<string, object> role, string msg)
		{
			KBEngine.Event.fireOut ("onTradeOver", role, msg);
		}

		/*
			opertype  0：装备    1：卸下
		 */
		public void EquipOper (int dbid, int operType)
		{
			baseCall ("equipOper", dbid, operType);
		}

		public void onEquipOperOver (Dictionary<string, object> role, string msg)
		{
			KBEngine.Event.fireOut ("onEquipOperOver", role, msg);
		}

		public void UseItem (string fromTag, string toTag, int dbid)
		{
			baseCall ("useItem", fromTag, toTag, dbid);
		}
		
		public void onUseItemOver (Dictionary<string, object> role, string msg)
		{
			KBEngine.Event.fireOut ("onUseItemOver", role, msg);
		}

		public void AssistOper (int bg_dbid, int ass_dbid)
		{
			baseCall ("assistOper", bg_dbid, ass_dbid);
		}
		
		public void onAssistOperOver (Dictionary<string, object> role, string msg)
		{
			KBEngine.Event.fireOut ("onAssistOperOver", role, msg);
		}

		public void RecordSceneToServer (int tomb_id, int floor, Dictionary<string, object> floorInfo)
		{
			baseCall ("recordScene", tomb_id, floor, floorInfo);
		}

		public void StartDig (Dictionary<string, object> digInfo)
		{
			baseCall ("startDig", digInfo);
		}

		public void onDigUpdated (Dictionary<string, object> digInfo, Dictionary<string, object> role, string msg)
		{
			KBEngine.Event.fireOut ("onDigUpdated", digInfo, role, msg);
		}

		public void PlayerMove (int dir)
		{
			baseCall ("playerMove", dir);
		}

		public void onPlayerMove (Dictionary<string, object> role)
		{
			KBEngine.Event.fireOut ("onPlayerMove", role);
		}

		public void getSceneData (string key)
		{
			baseCall ("getSceneData", key);
		}
		
		public void OnGetSceneData (List<object> enemyTypeList)
		{
			KBEngine.Event.fireOut ("OnGetSceneData", enemyTypeList);
		}

		public void getBattleData (int enemyid, int enemy_dbid)
		{
			baseCall ("getBattleData", enemyid, enemy_dbid);
		}
		
		public void OnGetBattleData (List<object> enemyList)
		{
			KBEngine.Event.fireOut ("OnGetBattleData", enemyList);
		}

		public void OnOpExe (List<object> toBos, List<object> bag, int itemid)
		{
			KBEngine.Event.fireOut ("OnOpExe", toBos, bag, itemid);
		}

		public void addOp (string from_tag, string to_tag, int itemid)
		{
			baseCall ("addOp", from_tag, to_tag, itemid);
		}
		
		public void onAddOp (int isBattleStart)
		{
			KBEngine.Event.fireOut ("onAddOp", isBattleStart);
		}

		public void OnBattleAnim (int itemid, string from)
		{
			KBEngine.Event.fireOut ("OnBattleAnim", itemid, from);
		}

		public void undoOp ()
		{
			baseCall ("undoOp");
		}
		
		public void onUndoOp (string from_tag)
		{
			KBEngine.Event.fireOut ("onUndoOp", from_tag);
		}

		public void battleOver (string battle_res, Dictionary<string,object> playerInfo, List<object> assistList, int opCount, List<object> bag, int enemy_dbid)
		{
			KBEngine.Event.fireOut ("battleOver", battle_res, playerInfo, assistList, opCount, bag, enemy_dbid);
		}

		public void enterTomb (int tombid)
		{
			baseCall ("enterTomb", tombid);
		}
		
		public void onEnterTomb (int tombid)
		{
			KBEngine.Event.fireOut ("onEnterTomb", tombid);
		}

		public void toPreFloor ()
		{
			baseCall ("toPreFloor");
		}

		public void toNextFloor ()
		{
			baseCall ("toNextFloor");
		}

		public void queryOtherPlayer (string name, List<object> proList, int page)
		{
			baseCall ("queryOtherPlayer", name, proList, page);
		}

		public void onQueryOtherPlayer (List<object> playerInfos, int maxPage, int currentPage)
		{
			KBEngine.Event.fireOut ("onQueryOtherPlayer", playerInfos, maxPage, currentPage);
		}

		public void invitePlayer (int dbid)
		{
			baseCall ("invitePlayer", dbid);
		}

		public void onInvitePlayer (string msg, List<object> playerList,int leaderFlag)
		{
			KBEngine.Event.fireOut ("onInvitePlayer", msg, playerList,leaderFlag);
		}

		public void onInvited (Dictionary<string,object> playerInfo)
		{
			KBEngine.Event.fireOut ("onInvited", playerInfo);
		}

		public void inviteResponse (int agreeFlag, int toPlayer)
		{
			baseCall ("inviteResponse", agreeFlag, toPlayer);
		}

		public void offLineNoti (int playerId)
		{
			KBEngine.Event.fireOut ("offLineNoti", playerId);
		}

		public void onLineNoti (int playerId)
		{
			KBEngine.Event.fireOut ("onLineNoti", playerId);
		}

		public void LeaveTeam (int playerId,int type)
		{
			baseCall ("LeaveTeam", playerId,type);
		}

		public void RecordSceneLevel (int level)
		{
			baseCall ("RecordSceneLevel", level);
		}

		public void LeaderChange (int leaderFlag)
		{
			KBEngine.Event.fireOut ("LeaderChange",leaderFlag);
		}

		public void PlayerLeave (int playerId,int type)
		{
			KBEngine.Event.fireOut ("PlayerLeave", playerId,type);
		}
	}
} 
