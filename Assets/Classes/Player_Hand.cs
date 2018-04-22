using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Hand : card_spot {

	public GameObject basic_card_prefab;

	public List <GameObject> current_hand = new List<GameObject>();

	public float xoffset_per_card = 0.5f;
	public int starting_hand_size = 6;

	protected card_library our_cards;

	public int health_value = 50;
	public GameObject health_node;

	public bool is_person = true; //Set to false if this is the AI and not the person.

	public Text health_text;

	public bool locked_out = false;
	private float lockout_time;

	public Radial_Countdown_Timer mouse_timer;



	// Use this for initialization
	void Start () 
	{
		health_text.text = "Health: " + health_value.ToString();

		our_cards = gameObject.GetComponent<card_library>();

		for (int counter = 0; counter < starting_hand_size; counter++){
			draw_card(false);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		lockout_time -= Time.deltaTime;
		if (locked_out && lockout_time <=0)
		{
			locked_out = false;
		}
	}

	public override bool play_card(GameObject the_card)
	{
		if (!locked_out)
		{
			set_lockout(the_card.GetComponent<Card>().casting_time);
			current_hand.Remove(the_card);
			arrange_cards();
			return true;
		}
		else
		{
			arrange_cards();
			return false;
		}

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

	public void draw_card(bool lock_out_after)
	{
		if (!locked_out)
		{
			GameObject new_card = Instantiate(basic_card_prefab);
			Card card_object = new_card.GetComponent<Card>();
			card_object.current_state = Card.card_states.Hand;
			card_object.held_in = this;
			card_object.assign_type(our_cards.master_card_list[Random.Range(0,our_cards.master_card_list.Count)]); //Assign the card a random type for now
			current_hand.Add(new_card);
			card_object.draggable = is_person;
			card_object.controlling_player = this;
			arrange_cards();

			if (lock_out_after)
			{
				set_lockout(4.0f);
			}
		}
	}

	public void take_damage(int amount)
	{
		health_value -= amount;
		health_text.text = "Health: " + health_value.ToString();
	}

	public void set_lockout(float time)
	{
		locked_out = true;
		lockout_time = time;
		if (mouse_timer != null)
		{
			mouse_timer.start_countdown(time);
		}
	}
}
