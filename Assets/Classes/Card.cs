using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Card : MonoBehaviour {


	//Key card stats//
	public int attack_power;
	public int defence_power;
	public int health;
	public float attack_time;
	public float defence_time;
	public float casting_time;

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
