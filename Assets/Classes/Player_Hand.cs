using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hand : card_spot {

	public GameObject basic_card_prefab;

	public List <GameObject> current_hand = new List<GameObject>();

	public float xoffset_per_card = 0.5f;
	public int starting_hand_size = 6;

	// Use this for initialization
	void Start () 
	{
		for (int counter = 0; counter < starting_hand_size; counter++){
			GameObject new_card = Instantiate(basic_card_prefab);
			new_card.GetComponent<Card>().current_state = Card.card_states.Hand;
			new_card.GetComponent<Card>().held_in = this;
			current_hand.Add(new_card);
		}
		arrange_cards();
		//TODO: instantiate a bunch of cards at the start of the game
		//TODO: add a "Draw" method that draws cards from the side of the board instead of just instantiating them in your hand.
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public override void play_card(GameObject the_card)
	{
		current_hand.Remove(the_card);
		arrange_cards();
	}

	public override void arrange_cards() //Take all of the cards currently in our hand and arrange them.
	{
		int counter = 0;
		foreach (GameObject next_card in current_hand)
		{
			Vector3 card_position = this.transform.position;
			card_position.x += -((current_hand.Count/2f) * xoffset_per_card) + xoffset_per_card*counter;
			next_card.transform.position = card_position;
			counter++;
		}
	}


}
