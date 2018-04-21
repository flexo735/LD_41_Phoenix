using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]

public class Card : MonoBehaviour {


	//Key card stats//
	public int attack_power;
	public int defence_power;
	public int health;
	public float attack_time;
	public float defence_time;
	public float casting_time;

	public card_types type;

	public SpriteRenderer card_art;
	public SpriteRenderer type_art;

	public Canvas textCanvas;

	public Text name_text;
	public Text attack_power_text;
	public Text defence_power_text;
	public Text health_text;
	public Text attack_time_text;
	public Text defence_time_text;
	public Text casting_time_text;
	public Text flavour_text_text;

	public enum card_states {Hand,Waiting,Attacking,Cooldown};
	[SerializeField]
	public card_states current_state;

	public card_spot held_in;

	//Making card draggable//
	private Vector3 offset;




	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void assign_type(raw_card_stats this_card_type)
	{
		this.attack_power = this_card_type.attack_power;
		this.defence_power = this_card_type.defence_power;
		this.health = this_card_type.health;
		this.attack_time = this_card_type.attack_time;
		this.defence_time = this_card_type.defence_time;
		this.casting_time = this_card_type.casting_time;

		card_art.sprite = this_card_type.card_art;
		type_art.sprite = this_card_type.type_art;

		type = this_card_type.type;

		this.attack_power_text.text = "" + this.attack_power;
		this.defence_power_text.text = "" + this.defence_power;
		this.health_text.text = "" + this.health;
		this.attack_time_text.text = "" + this.attack_time;
		this.defence_time_text.text = "" + this.defence_time;
		this.casting_time_text.text = "" + this.casting_time; 

		this.name_text.text = this_card_type.name;
		name = this_card_type.name + "_card";
		this.flavour_text_text.text = this_card_type.flavour_text;
	}

	void OnMouseEnter(){
		transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
		GetComponent<SpriteRenderer>().sortingOrder = 3;
		card_art.sortingOrder = 3;
		type_art.sortingOrder = 4;
		textCanvas.sortingOrder = 4;
	}

	void OnMouseExit(){
		transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		GetComponent<SpriteRenderer>().sortingOrder = 1;
		card_art.sortingOrder = 1;
		type_art.sortingOrder = 2;
		textCanvas.sortingOrder = 2;
	}

	void OnMouseDown() 
	{
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
	}

	void OnMouseDrag()
	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		transform.position = curPosition;
	}

	void OnMouseUp()
	{
		if (current_state == card_states.Hand){
			//We check to see if we've been dropped on a combat hotspot.
			bool found_dropspot = false;
			BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
			Collider2D[] items_overlapped = Physics2D.OverlapAreaAll(collider.bounds.min, collider.bounds.max);
			foreach (Collider2D hit in items_overlapped)
			{
				if (hit.gameObject.GetComponent<combat_spot>() != null && hit.gameObject.GetComponent<combat_spot>().currently_holding == null)
				{
					//TODO: play the card to the combat spot, remove it from the player hand.
					held_in.play_card(this.gameObject);
					held_in = hit.gameObject.GetComponent<combat_spot>();
					hit.gameObject.GetComponent<combat_spot>().currently_holding = this.gameObject;
					current_state = card_states.Cooldown;
					found_dropspot = true;
					held_in.arrange_cards();
					break;
				}
			}
			if (!found_dropspot)
			{
				held_in.arrange_cards();
			}
			//TODO: Check to see if we're in the play area of the map. In that case play the card, remove it from your hand and and assign it to a different holder to arrange.
		}

		else if (current_state == card_states.Waiting){
			//TODO: Check to see if we're being assigned to another creature or face. If so, start attacking, defending, ect.
		}
		else{
			held_in.arrange_cards();
		}
	}
}
