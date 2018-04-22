using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player_Hand {
	public float startDelay = 7.0f;
	public float decisionDelay = 1.0f;
	public float decisionUncertainty = 1.0f;
	public float decisionTimer = 0.0f;
	float timeUntilNextDecision = 0.0f;

	public List<combat_spot> player_Attack_Spots;
	//public List<combat_spot> player_Defend_Spots;
	public List<combat_spot> AI_Attack_Spots;
	//public List<combat_spot> AI_Defend_Spots;

	public Player_Hand player_Hand;

	// when the AI decides to draw a card
	public void AIDraw(){
		draw_card(true);

		decisionTimer = 4.0f;
	}

	// when the AI decides to play a card in attack position
	private void AIPlayAttacker(int laneNum, Card card){
		
		card.play_card_to_combat_spot(AI_Attack_Spots[laneNum]);

		decisionTimer = card.casting_time;
	}
	// when the AI decides to play a card in defence position
	/*private void AIPlayDefender(int laneNum, Card card){
		
		card.play_card_to_combat_spot(AI_Defend_Spots[laneNum]);

		decisionTimer = card.casting_time;
	}*/

	// when the AI attacks with a card
	private void AIAttack(Card AICard, Card enemyCard){
		// todo: the attack
		if(enemyCard == null){
			// hit face
			AICard.start_attack(player_Hand);
		}
	}
	// when the AI defends with a card
	private void AIDefend(Card AICard, Card enemyCard){
		// todo: the defence
		//AICard.
		AICard.defend_against_attack(enemyCard);
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
		EnemyDefending,
		FriendDefending,
		Clash // one side attacking, the other is defending
	}
	struct laneContents {
		public LaneState state;
		public float timeUntilCast;
		public float timeUntilAttack;
		public float timeUntilDefence;
		public int attackPower;
		public int defencePower;
		public int health;
	}

	

	private AIDecision makeDecision(){
		if(current_hand.Count == 0){
			return AIDecision.draw;
		}
		float[,] confidence = new float[AI_Attack_Spots.Count, player_Attack_Spots.Count];
		int bestDefenceCard = -1;
		int bestDefenceTarget = -1;
		int bestAttackCard = -1;
		int bestAttackTarget = -1;
		int bestAttackPower = -1;
		float bestDefenceConfidence = -10.0f;
		float bestAttackConfidence = -10.0f;
		bool canAttack = false;
		bool canDefend = false;
		
		
		// check AI defence cards versus player's attack cards
		for(int d = 0; d < AI_Attack_Spots.Count; d++){
			if(!AI_Attack_Spots[d].currently_holding)
			{
				return AIDecision.defend1 + d;
			}
			Card AICard = AI_Attack_Spots[d].currently_holding.GetComponent<Card>();
			if(AICard.current_state != Card.card_states.Waiting)
			{
				for(int a = 0; a < player_Attack_Spots.Count; a++)
					confidence[d,a] = -10.0f;
				continue;
			}

			for(int a = 0; a < player_Attack_Spots.Count; a++){
				if(!player_Attack_Spots[a].currently_holding)
				{
					confidence[d,a] = -10.0f;
					continue;
				}
				Card enemyCard = player_Attack_Spots[a].currently_holding.GetComponent<Card>();
				if(enemyCard.current_state != Card.card_states.Attacking){
					confidence[d,a] = -10.0f;
					continue;
				}

				canDefend = true;

				if(enemyCard.attack_power != 0.0f)
					confidence[d,a]  = 0.1f * Mathf.Floor(AICard.health / enemyCard.attack_power);
				else
					confidence[d,a]  = 0.1f * AICard.health;

				if(AICard.defence_power != 0.0f)
					confidence[d,a] -= 0.1f * Mathf.Floor(enemyCard.health / AICard.defence_power);
				else
					confidence[d,a] -= 1.0f;


				if(enemyCard.current_state == Card.card_states.Attacking && AICard.defence_time < enemyCard.time_left)
				{
					confidence[d,a] += 1.0f;
				}
				if(confidence[d,a] > bestDefenceConfidence){
					bestDefenceCard = d;
					bestDefenceTarget = a;
					bestDefenceConfidence = confidence[d,a];
				}
			}
		}

		confidence = new float[AI_Attack_Spots.Count, player_Attack_Spots.Count];
		// check AI attack cards versus player's defence cards
		for(int a = 0; a < AI_Attack_Spots.Count; a++){
			if(!AI_Attack_Spots[a].currently_holding)
			{
				for(int d = 0; d < player_Attack_Spots.Count; d++)
					confidence[d,a] = -10.0f;
				return AIDecision.attack1 + a;
			}
			Card AICard = AI_Attack_Spots[a].currently_holding.GetComponent<Card>();
			if(AICard.current_state != Card.card_states.Waiting){
				for(int d = 0; d < player_Attack_Spots.Count; d++)
					confidence[d,a] = -10.0f;
				continue;
			}
			canAttack = true;
			if(bestAttackConfidence < -5.0f && AICard.attack_power > bestAttackPower){
				bestAttackCard = a;
				bestAttackPower = AICard.attack_power;
			}
			for(int d = 0; d < player_Attack_Spots.Count; d++){
				if(!player_Attack_Spots[d].currently_holding)
				{
					confidence[d,a] = -10.0f;
					continue;
				}
				Card enemyCard = player_Attack_Spots[d].currently_holding.GetComponent<Card>();


				if(enemyCard.defence_power != 0.0f)
					confidence[d,a]  = 0.1f * Mathf.Floor(AICard.health / enemyCard.defence_power);
				else
					confidence[d,a]  = 0.1f * AICard.health;

				if(AICard.attack_power != 0.0f)
					confidence[d,a] -= 0.1f * Mathf.Floor(enemyCard.health / AICard.attack_power);
				else
					confidence[d,a] -= 1.0f;


				if(enemyCard.current_state == Card.card_states.Attacking && AICard.attack_time < enemyCard.time_left)
				{
					confidence[d,a] += 1.0f;
				}
				if(confidence[d,a] > bestAttackConfidence){
					bestAttackCard = a;
					bestAttackTarget = d;
					bestAttackConfidence = confidence[d,a];
				}
			}
		}


		if(bestAttackConfidence >= bestDefenceConfidence && canAttack){
			//if(bestAttackTarget == -1){
				AIAttack(AI_Attack_Spots[bestAttackCard].currently_holding.GetComponent<Card>(), null);
			//}
			/*else
			{
				AIAttack(AI_Attack_Spots[bestAttackCard].currently_holding.GetComponent<Card>(), 
					player_Attack_Spots[bestAttackTarget].currently_holding.GetComponent<Card>());
					Debug.LogError("No, you need to fix this Tim");
			}*/
		}else if(canDefend){
			AIDefend(AI_Attack_Spots[bestDefenceCard].currently_holding.GetComponent<Card>(), 
				player_Attack_Spots[bestDefenceTarget].currently_holding.GetComponent<Card>());
		}

		/*List<laneContents> lanes = new List<laneContents>();
		lanes.Add(checkLane(0));
		lanes.Add(checkLane(1));
		lanes.Add(checkLane(2));
		lanes.Add(checkLane(3));
		lanes.Add(checkLane(4));

		int highestAttack = 0;
		int lowestDefence = 200;
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
			if(lanes[i].state == LaneState.Empty || (lanes[i].state == LaneState.EnemyDefending && lanes[i].defencePower < lowestDefence)){
				attackLane = i;
				lowestDefence = lanes[i].defencePower;
			}
		}
		if(highestAttack > 0 && AI_Spots[defendLane].currently_holding == null){
			return AIDecision.defend1 + defendLane;
		}
		if(current_hand.Count > 1 && AI_Spots[attackLane].currently_holding == null && lowestDefence < 5){
			return AIDecision.attack1 + attackLane;
		}
		if(current_hand.Count < 5){
			return AIDecision.draw;
		}
		if(AI_Spots[attackLane].currently_holding == null){
			return AIDecision.attack1 + attackLane;
		}
		if(current_hand.Count < 6){
			return AIDecision.draw;
		}*/
		return AIDecision.none;
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


	/*private int FindBestAttack(laneContents lane){
		float[] confidence = new float[current_hand.Count];
		int bestCard = 0;
		float bestConfidence = -10.0f;
		if(lane.state == LaneState.Empty)
		{
			for(int i = 0; i < current_hand.Count; i++){
				Card card = current_hand[i].GetComponent<Card>();
				confidence[i]  = 0.1f * card.health;
				confidence[i] += 0.1f * card.attack_power;
				confidence[i] += 0.2f * (card.casting_time + card.attack_time);
				if(confidence[i] > bestConfidence)
				{
					bestCard = i;
					bestConfidence = confidence[i];
				}
			}
		}
		else
		{

			for(int i = 0; i < current_hand.Count; i++){
				Card card = current_hand[i].GetComponent<Card>();
				
				
				if(lane.defencePower != 0.0f)
					confidence[i]  = 0.1f * Mathf.Floor(card.health / lane.defencePower);
				else
					confidence[i] = -0.1f * card.health;

				if(card.attack_power != 0.0f)
					confidence[i] -= 0.1f * Mathf.Floor(lane.health / card.attack_power);
				else
					confidence[i] -= 0.1f * lane.health;


				if(card.casting_time + card.attack_time < lane.timeUntilCast + lane.timeUntilDefence)
				{
					confidence[i] += 0.2f;
				}
				if(confidence[i] > bestConfidence){
					bestCard = i;
					bestConfidence = confidence[i];
				}
			}
		}
		return bestCard;
	}*/

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
				AIPlayAttacker((int)choice - 6, current_hand
					[0]
					.GetComponent<Card>());
			}
			else if((int)choice >= (int)AIDecision.attack1)
			{
				AIPlayAttacker((int)choice - 1, current_hand
					[0]
					.GetComponent<Card>());
			}

			timeUntilNextDecision = Time.time + decisionTimer + decisionDelay + Random.Range(0.0f, decisionUncertainty);
			decisionTimer = 0.0f;
		}
	}
}
