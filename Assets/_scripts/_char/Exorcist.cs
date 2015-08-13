using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Exorcist : Pro
{
	public Exorcist (string image_name)
	{
		this.proname = StringCollection.EXORCIST;
		this.prefabname = "Exorcist_" + image_name;
		this.strengthFactor = 6;
		this.archeologyFactor = 0;
		this.defFactor = 2;
		this.dodgeFactor = 2;
		this.staminaAdd = 40;
		this.healthAdd = 40;
        this.itemid = 3;
	}
}
