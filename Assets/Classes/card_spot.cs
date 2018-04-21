using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class card_spot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	virtual public void arrange_cards(){

	}

	virtual public bool play_card(GameObject the_card){
		return true;
	}
}
