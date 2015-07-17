using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Settler : Pro {
	public Settler(string image_name){
		this.proname = "拓荒者";
		this.prefabname = "Settler_"+image_name;
		this.strengthFactor = 2;
	}
}
