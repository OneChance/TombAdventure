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
}
