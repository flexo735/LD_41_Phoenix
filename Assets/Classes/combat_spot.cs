using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combat_spot : card_spot 
{
	public GameObject currently_holding = null;
	public bool player_spot = true; //Set false for AI controlled.
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public override void arrange_cards()
	{
		if (currently_holding != null)
		{
			currently_holding.transform.position = this.transform.position;
		}
	}

	public override void play_card(GameObject the_card)
	{
		//TODO: Do something when this card is played
	}
}
