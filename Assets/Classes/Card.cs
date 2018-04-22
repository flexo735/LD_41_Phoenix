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
	public Text timer_text;

	public Image timer_bar;
	public Color start_timer_bar_colour;
	public Color end_timer_bar_colour;

	public enum card_states {Hand,Waiting,Attacking,Cooldown};
	[SerializeField]
	public card_states current_state;

	public card_spot held_in;

	//Making card draggable//
	private Vector3 offset;
	public bool draggable;

	//Card timing variables//
	public float time_left = 0;
	private float max_time_left; //Used to scale the timer bar relative to the size of the initial cooldown.

	//Attacking object holder//
	Player_Hand attack_target = null;
	public Player_Hand controlling_player;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (current_state == card_states.Cooldown)
		{
			time_left -= Time.deltaTime;
			if (time_left <= 0)
			{
				current_state = card_states.Waiting;
				timer_bar.enabled = false;
			}
		}

		else if (current_state == card_states.Attacking)
		{
			time_left -= Time.deltaTime;
			if (time_left <= 0)
			{
				current_state = card_states.Waiting;
				timer_bar.enabled = false;
				finish_attack(attack_target);
			}
		}

		if (current_state == card_states.Attacking || current_state == card_states.Cooldown)
		{
			//We want to update the thing.
			timer_bar.fillAmount = time_left/max_time_left;
			Color lerped_colour = Color.Lerp(end_timer_bar_colour,start_timer_bar_colour,time_left/max_time_left);
			timer_bar.color = lerped_colour;
		}


	}

	void FixedUpdate()
	{
		//We maintain the card state notice at the top, done in an update to play nice with other classes modifying it
		if (current_state == card_states.Cooldown)
		{
			timer_text.text = "Cooldown";
		}
		else if (current_state == card_states.Attacking)
		{
			timer_text.text = "Attacking!";
		}
		else if (current_state == card_states.Waiting)
		{
			timer_text.text = "Ready";
		}
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
		if (draggable && (current_state == card_states.Waiting || current_state == card_states.Hand))
		{
			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
		}
	}

	void OnMouseDrag()
	{
		if (draggable && (current_state == card_states.Waiting || current_state == card_states.Hand))
		{
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			transform.position = curPosition;
		}
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
				if (hit.gameObject.GetComponent<combat_spot>() != null && hit.gameObject.GetComponent<combat_spot>().currently_holding == null && hit.gameObject.GetComponent<combat_spot>().player_spot)
				{
					//TODO: play the card to the combat spot, remove it from the player hand.
					found_dropspot = true;
					play_card_to_combat_spot(hit.gameObject.GetComponent<combat_spot>());
					break;
				}
			}
			if (!found_dropspot)
			{
				held_in.arrange_cards();
			}
		}

		else if (current_state == card_states.Waiting){
			BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
			Collider2D[] items_overlapped = Physics2D.OverlapAreaAll(collider.bounds.min, collider.bounds.max);
			foreach (Collider2D hit in items_overlapped)
			{
				if (hit.tag == "Health_Node" && hit.transform.parent.gameObject.GetComponent<Player_Hand>() != null && !hit.transform.parent.gameObject.GetComponent<Player_Hand>().is_person)
				{
					start_attack(hit.transform.parent.gameObject.GetComponent<Player_Hand>());
					break;
				}

				else if (hit.gameObject.GetComponent<Card>() != null && hit.gameObject.GetComponent<Card>().controlling_player != this.controlling_player && hit.gameObject.GetComponent<Card>().current_state == card_states.Attacking)
				{
					//TODO: if we've been played on an attacking card we defend against it.
					defend_against_attack(hit.gameObject.GetComponent<Card>());
					break;
				}
			}

			held_in.arrange_cards(); // Always arrange yourself so you snap back to your attack point and begin attacking.
		}
		else{
			held_in.arrange_cards();
		}
	}

	public void play_card_to_combat_spot(combat_spot the_spot)
	{
		if (held_in.play_card(this.gameObject))
		{
			held_in = the_spot;
			the_spot.currently_holding = this.gameObject;
			current_state = card_states.Cooldown;
			time_left = casting_time;
			max_time_left = casting_time;
			held_in.arrange_cards();

			timer_bar.enabled = true;
		}
	}

	public void start_attack(Player_Hand the_target)
	{
		if (!controlling_player.locked_out)
		{
			current_state = card_states.Attacking;
			time_left = attack_time;
			max_time_left = attack_time;
			attack_target = the_target;

			timer_bar.enabled = true;
		}
	}

	void finish_attack(Player_Hand the_target)
	{
		the_target.take_damage(attack_power);
	}

	public void defend_against_attack(Card enemy_card)
	{
		Debug.Assert(enemy_card.current_state == card_states.Attacking);
		Debug.Assert(current_state == card_states.Waiting);
		enemy_card.health -= this.defence_power;
		this.health -= enemy_card.attack_power;

		enemy_card.health_text.text = "" + enemy_card.health;
		this.health_text.text = "" + this.health;
	
		if (enemy_card.health <= 0)
		{
			enemy_card.card_destroyed();
		}
		else
		{
			enemy_card.current_state = card_states.Cooldown; //If they aren't killed the attack is aborted and they are put into cooldown for the remaining attack time.
		}

		if (this.health <= 0)
		{
			this.card_destroyed();
		}
		else
		{
			time_left = defence_time;
			max_time_left = defence_time;
			current_state = card_states.Cooldown;

			timer_bar.enabled = true;
		}
	}


	void card_destroyed()
	{
		if (held_in is combat_spot)
		{
			held_in.GetComponent<combat_spot>().currently_holding = null;
		}
		GameObject.Destroy(gameObject);
	}
}
