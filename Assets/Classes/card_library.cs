using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card_library : MonoBehaviour {

	public List<raw_card_stats> master_card_list = new List<raw_card_stats>();

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public enum card_types {Cold, Electric, Fire, Magic, Void, Water, Air};

[System.Serializable]
public class raw_card_stats{
	public string name;
	public string flavour_text;

	public int attack_power;
	public int defence_power;
	public int health;
	public float attack_time;
	public float defence_time;
	public float casting_time;

	public Sprite card_art;
	public card_types type;
	public Sprite type_art;

	raw_card_stats(string card_name, string card_flavour_text, int card_attack_power, int card_defence_power, int card_health, float card_attack_time, float card_defence_time, float card_casting_time, Sprite card_card_art, card_types card_type, Sprite card_type_art)
	{
		name = card_name;
		flavour_text = card_flavour_text;

		attack_power = card_attack_power;
		defence_power = card_defence_power;
		health = card_health;
		attack_time = card_attack_time;
		defence_time = card_defence_time;
		casting_time = card_casting_time;

		card_art = card_card_art;
		type = card_type;
		type_art = card_type_art;
	}
}
