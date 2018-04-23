using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class draft_spawner : MonoBehaviour {

	public float time_between_cards = 0.4f;
	private float time_till_card = 0;

	public GameObject basic_card_prefab;
	private card_library spawnable_cards;

	private Camera draft_cam;
	private bool drafting = true;

	private List<GameObject> spawned_cards = new List<GameObject>();
	private List<CardTumbleData> tumble_data = new List<CardTumbleData>();

	public float draft_time = 10;
	public float after_draft_time = 5;
	public Text time_text;

	public draft_deck drafted_pile;


	// Use this for initialization
	void Start () {
		spawnable_cards = gameObject.GetComponent<card_library>();
		draft_cam = Camera.main;

		time_text.text = draft_time.ToString("D#0") + " Seconds Remaining!";
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (drafting)
		{
			draft_time -= Time.deltaTime;
		}
		else
		{
			after_draft_time -= Time.deltaTime;
			time_text.text = "Last Chance! Starting match in " + after_draft_time.ToString("#0");
			if (after_draft_time <= 0)
			{
				GameObject.FindWithTag("Not_Destroyed").GetComponent<deck_holder>().the_deck = drafted_pile.the_deck; //Store the drafted deck in an object that isn't destroyed when we change scenes.
				SceneManager.LoadScene("Main_Board");
			}
		}

		if (draft_time <= 0 && drafting)
		{
			drafting = false;
			time_text.text = "Last Chance! Starting match in " + after_draft_time.ToString("#0");
		}
		else if (drafting)
		{
			time_text.text = draft_time.ToString("#0.0") + " Seconds Remaining!";
		}
			

		time_till_card -= Time.deltaTime;
		if (time_till_card <= 0 && drafting)
		{
			time_till_card = time_between_cards;
			GameObject new_card = Instantiate(basic_card_prefab);
			Card card_data = new_card.GetComponent<Card>();
			card_data.draggable = true;
			card_data.current_state = Card.card_states.Draft;
			card_data.assign_type(spawnable_cards.master_card_list[Random.Range(0, spawnable_cards.master_card_list.Count)]);

			CardTumbleData td = new CardTumbleData();
			td.rotationOffset = Random.Range(0,360);
			td.speed = -0.1f / Random.Range(1.5f, 3.5f);

			spawned_cards.Add(new_card);
			tumble_data.Add(td);

			float random_y = Random.Range(-2.0f,4.0f);
			new_card.transform.SetPositionAndRotation(new Vector3(draft_cam.aspect * draft_cam.orthographicSize + 2.0f, random_y, 0.0f), Quaternion.identity);

		}

		for(int i = 0; i < spawned_cards.Count && i < tumble_data.Count; i++){
			spawned_cards[i].transform.SetPositionAndRotation(spawned_cards[i].transform.position + new Vector3(tumble_data[i].speed, 0.0f, 0.0f), Quaternion.AngleAxis(tumble_data[i].rotationOffset - Time.time * 30, Vector3.back));
		}
	}

	public struct CardTumbleData{
		public float rotationOffset;
		public float speed;
	}
}
