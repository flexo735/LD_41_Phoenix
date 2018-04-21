using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player_Hand {
	public float startDelay = 7.0f;
	public float decisionDelay = 1.0f;
	public float decisionUncertainty = 1.0f;
	public float decisionTimer = 0.0f;
	float timeUntilNextDecision = 0.0f;
	AIDecision choice = AIDecision.none;

	public List<combat_spot> player_Spots;
	public List<combat_spot> AI_Spots;

	// when the AI decides to draw a card
	public void AIDraw(){
		draw_card(true);

		// todo: set the correct cooldown from drawing a card
		decisionTimer = 1.0f;
	}

	// when the AI decides to play a card
	private void AIAttack(int laneNum, Card card){
		// todo: play the attacking card
		
		card.play_card_to_combat_spot(AI_Spots[laneNum]);

		decisionTimer = card.casting_time;
	}

	enum AIDecision{
		none = -1,
		draw = 0,
		attack1 = 1,
		attack2,
		attack3,
		attack4,
		attack5,
		defend1,
		defend2,
		defend3,
		defend4,
		defend5
	}

	public enum LaneState {
		Empty = 0,
		EnemyAttacking = 1,
		FriendAttacking,
		Clash // one side attacking, the other is defending
	}
	struct laneContents {
		public LaneState state;
		public float timeUntilCast;
		public float timeUntilAttack;
		public int attackPower;
		public int defencePower;
		public int health;
	}

	// get info from the lane
	laneContents checkLane(int laneNum){
		laneContents result = new laneContents();
		result.state = LaneState.Empty;
		result.timeUntilCast = 0.0f;
		result.timeUntilAttack = 0.0f;
		result.attackPower = 0;
		result.defencePower = 0;
		result.health = 0;

		if(player_Spots[laneNum].currently_holding != null)
		{
			if(AI_Spots[laneNum].currently_holding != null)
			{
				result.state = LaneState.Clash;
			}
			else
			{
				result.state = LaneState.EnemyAttacking;
			}

			Card enemyCard = player_Spots[laneNum].currently_holding.GetComponent<Card>();
			result.timeUntilCast = enemyCard.casting_time;
			result.timeUntilAttack = enemyCard.attack_time;
			result.attackPower = enemyCard.attack_power;
			result.defencePower = enemyCard.defence_power;
			result.health = enemyCard.health;
		}
		else if(AI_Spots[laneNum].currently_holding != null)
		{
			result.state = LaneState.FriendAttacking;
		}
		return result;
	}

	private AIDecision makeDecision(){
		if(current_hand.Count == 0){
			return AIDecision.draw;
		}
		List<laneContents> lanes = new List<laneContents>();
		lanes.Add(checkLane(0));
		lanes.Add(checkLane(1));
		lanes.Add(checkLane(2));
		lanes.Add(checkLane(3));
		lanes.Add(checkLane(4));

		int highestAttack = 0;
		int lowestDefense = 0;
		int defendLane = 0;
		int attackLane = 0;


		for(int i = 0; i < lanes.Count; i++)
		{
			// check attacking cards, then defending cards.
			if(lanes[i].state == LaneState.EnemyAttacking){
				if(lanes[i].attackPower > highestAttack){
					defendLane = i;
					highestAttack = lanes[i].attackPower;
				}
			}
			if(lanes[i].state == LaneState.Empty){
				attackLane = i;
			}
		}
		if(highestAttack > 0 && AI_Spots[defendLane].currently_holding == null){
			return AIDecision.defend1 + defendLane;
		}
		if(current_hand.Count < 5){
			return AIDecision.draw;
		}
		if(AI_Spots[attackLane].currently_holding == null){
			return AIDecision.attack1 + attackLane;
		}
		return AIDecision.draw;
	}

	void Start(){
		health_text.text = "Health: " + health_value.ToString();

		our_cards = gameObject.GetComponent<card_library>();

		for (int counter = 0; counter < starting_hand_size; counter++){
			draw_card(false);
		}

		timeUntilNextDecision = Time.time + startDelay;
		is_person = false;
	}

	void FixedUpdate(){
		// Make a decision every interval, with a bit of random delay so it seems more human
		if(Time.time > timeUntilNextDecision){
			AIDecision choice = makeDecision();
			Debug.Log("The AI decided to " + choice.ToString());
			if(choice == AIDecision.draw)
			{
				AIDraw();
			}
			else if((int)choice >= (int)AIDecision.defend1)
			{
				// todo: create logic for deciding which card to play
				AIAttack((int)choice - 6, current_hand[0].GetComponent<Card>());
			}
			else if((int)choice >= (int)AIDecision.attack1)
			{
				// todo: create logic for deciding which card to play
				AIAttack((int)choice - 1, current_hand[0].GetComponent<Card>());
			}

			timeUntilNextDecision = Time.time + decisionTimer + decisionDelay + Random.Range(0.0f, decisionUncertainty);
			decisionTimer = 0.0f;
		}
	}
}
