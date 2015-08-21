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

	}
} 
