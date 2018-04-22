using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class draft_deck : MonoBehaviour {

	public Text deck_size;
	public card_library the_deck;

	// Use this for initialization
	void Start () {
		the_deck = gameObject.GetComponent<card_library>();
		deck_size.text = the_deck.master_card_list.Count.ToString() + " Cards";
	}

	void FixedUpdate(){
		deck_size.text = the_deck.master_card_list.Count.ToString() + " Cards";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
