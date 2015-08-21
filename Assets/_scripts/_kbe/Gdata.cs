﻿namespace KBEngine
{
	using UnityEngine; 
	using System; 
	using System.Collections; 
	using System.Collections.Generic;
	using System.Linq;
	
	public class Gdata : KBEngine.GameObject 
	{
		
		public Dictionary<UInt64, Dictionary<string, object>> items = new Dictionary<UInt64, Dictionary<string, object>>();
		
		public Gdata()
		{
		}
		
		public override void __init__()
		{
			Debug.Log("gdata run ..............................................");
			baseCall("reqItemList",0);     
		}
		
		public void onReqItemList(Dictionary<string, object> itemList)
		{
			
			items.Clear();
			
			List<object> listinfos = (List<object>)itemList["values"];
			
			for (int i = 0; i < listinfos.Count; i++)
			{
				Dictionary<string, object> info = (Dictionary<string, object>)listinfos[i];
				items.Add((UInt64)info["dbid"], info);
			}
			
			// ui event
			Dictionary<UInt64, Dictionary<string, object>> iList = new Dictionary<ulong, Dictionary<string, object>>(items);
			KBEngine.Event.fireOut("onReqItemList", new object[] { iList });
			
			if (listinfos.Count == 0)
				return;
		}
	}
} 