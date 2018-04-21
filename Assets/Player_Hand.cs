using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hand : MonoBehaviour {

	public GameObject basic_card_prefab;

	public List <GameObject> current_hand = new List<GameObject>();

	public float xoffset_per_card = 0.5f;
	public int starting_hand_size = 6;

	// Use this for initialization
	void Start () 
	{
		for (int counter = 0; counter < starting_hand_size; counter++){
			Vector3 card_position = this.transform.position;
			card_position.x += -((starting_hand_size/2f) * xoffset_per_card) + xoffset_per_card*counter;
			GameObject new_card = Instantiate(basic_card_prefab,card_position,this.transform.rotation);
			current_hand.Add(new_card);
		}
		//TODO: instantiate a bunch of cards at the start of the game
		//TODO: add a "Draw" method that draws cards from the side of the board instead of just instantiating them in your hand.
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
