using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Doctor : Pro {
	public Doctor(string image_name){
		this.proname = StringCollection.DOCTOR;
		this.prefabname = "Doctor_"+image_name;
		this.strengthFactor = 2;
		this.archeologyFactor = 0;
		this.defFactor = 6;
		this.dodgeFactor = 1;
		this.staminaAdd = 20;
		this.healthAdd = 60;
        this.itemid = 4;
	}
}
