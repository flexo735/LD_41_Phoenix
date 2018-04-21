using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hand : card_spot {

	public GameObject basic_card_prefab;

	public List <GameObject> current_hand = new List<GameObject>();

	public float xoffset_per_card = 0.5f;
	public int starting_hand_size = 6;

	private card_library our_cards;

	// Use this for initialization
	void Start () 
	{
		our_cards = gameObject.GetComponent<card_library>();

		for (int counter = 0; counter < starting_hand_size; counter++){
			GameObject new_card = Instantiate(basic_card_prefab);
			Card card_object = new_card.GetComponent<Card>();
			card_object.current_state = Card.card_states.Hand;
			card_object.held_in = this;
			card_object.assign_type(our_cards.master_card_list[Random.Range(0,our_cards.master_card_list.Count)]); //Assign the card a random type for now
			current_hand.Add(new_card);
		}
		arrange_cards();
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
