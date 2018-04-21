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
	public card_states current_state;

	public Player_Hand held_in;

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
			held_in.arrange_cards();
			//TODO: Check to see if we're in the play area of the map. In that case play the card, remove it from your hand and and assign it to a different holder to arrange.
		}

		else if (current_state == card_states.Waiting){
			//TODO: Check to see if we're being assigned to another creature or face. If so, start attacking, defending, ect.
		}
	}
}
