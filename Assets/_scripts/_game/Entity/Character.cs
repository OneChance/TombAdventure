using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character
{

	private int attack;
	private int health;
	private string characterName;
	private string prefabName;
	private List<Item> itemList;

	public Character (int attack, int health,string characterName,string prefabName)
	{
		this.attack = attack;
		this.health = health;
		this.characterName = characterName;
		this.prefabName = prefabName;
	}

	public string CharacterName {
		get {
			return this.characterName;
		}
		set {
			characterName = value;
		}
	}

	public string PrefabName {
		get {
			return this.prefabName;
		}
		set {
			prefabName = value;
		}
	}

	public int Attack {
		get {
			return this.attack;
		}
		set {
			attack = value;
		}
	}

	public int Health {
		get {
			return this.health;
		}
		set {
			health = value;
		}
	}

	public List<Item> ItemList {
		get {
			return this.itemList;
		}
		set {
			itemList = value;
		}
	}
}
