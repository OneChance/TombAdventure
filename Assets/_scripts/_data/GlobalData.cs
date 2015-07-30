﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalData : MonoBehaviour
{
	public int currentFloor = 1;
	public Enemy currentEnemy;
	public List<Character> characterList;
	public bool victory = true;
	public string currentEnemyName;
	public Vector3 playerPos; //record the pos of player when battle start
	public Baggrid currentItem;
	public Vector3 preDigPos;
	public bool isPlayer; //ture为玩家联机模式，false为单机佣兵模式
	public bool isShop = false;
	public Tomb currentTomb;

	public void InitPlayerInfo(){
		//从服务器端获取玩家数据初始化
		isPlayer = false; //初始的时候总是佣兵模式，玩家在线后与其他玩家组队，才会变成联机模式
		characterList = new List<Character> ();

		Equipment e = new Equipment(1,2,0,0,Equipment.EquipPos.BODY,"2","学者的思考",1,200);
		List<Equipment> eList = new List<Equipment>();
		eList.Add(e);
		
		Character c = new Character (2000,30,100, 50, 0, 0, "zhouhui", true,200,200,ProFactory.getPro("Geomancer","1"),1,0,eList,-1);
		
		HealthItem item = new HealthItem (Item.RangeType.SINGLE, 10, "1", "单体治疗药剂",50);
		List<Baggrid> bgList = new List<Baggrid> ();
		Baggrid bg = new Baggrid (item, 2);
		bgList.Add (bg);
		
//		Equipment e2 = new Equipment(2,3,Equipment.EquipPos.BODY,"3","学者的幻想",1,500);
//		Baggrid bg2 = new Baggrid (e2, 1);
//		bgList.Add (bg2);
		
		c.BgList = bgList;
		
		characterList.Add (c);
		
		Character c2 = new Character (0,40, 100,50, 0, 0, "unity", false,100,100,ProFactory.getPro("Settler","1"),1,0,null,-1);
		characterList.Add (c2);
	}
}
