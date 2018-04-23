using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu_controller : MonoBehaviour {
	public Camera menuCamera;
	public GameObject basic_card_prefab;
	public card_library library;
	int cardTimer = 0;
	List<spinner> cards;

	public void onPlayClick(){
		SceneManager.LoadScene("Draft_Screen");
	}
	public void onHowClick(){
		SceneManager.LoadScene("Tutorial_Board");
	}
	public void onQuitClick(){
		Application.Quit();
	}

	public void Start(){
		cards = new List<spinner>();
	}

	private struct spinner{
		public GameObject card;
		public float rotationOffset;
		public float speed;
	}

	// Spawn some cards and make 'em spin!!
	public void FixedUpdate(){
		cardTimer--;
		if(cardTimer < 0){
			cardTimer = 300;
			
			spinner new_card = new spinner();
			new_card.card = Instantiate(basic_card_prefab);
			Card card_object = new_card.card.GetComponent<Card>();
			card_object.draggable = true;
			card_object.current_state = Card.card_states.Draft;
			card_object.assign_type(library.master_card_list[Random.Range(0, library.master_card_list.Count)]);
			new_card.card.transform.SetPositionAndRotation(new Vector3(menuCamera.aspect * menuCamera.orthographicSize + 1.5f, 0.0f, 0.0f), Quaternion.identity);
			new_card.rotationOffset = Random.Range(0.0f, 360.0f);
			new_card.speed = -0.01f;
			cards.Add(new_card);

			new_card = new spinner();
			new_card.card = Instantiate(basic_card_prefab);
			card_object = new_card.card.GetComponent<Card>();
			card_object.draggable = true;
			card_object.current_state = Card.card_states.Draft;
			card_object.assign_type(library.master_card_list[Random.Range(0, library.master_card_list.Count)]);
			new_card.card.transform.SetPositionAndRotation(new Vector3(-menuCamera.aspect * menuCamera.orthographicSize - 1.5f, 3.0f, 0.0f), Quaternion.identity);
			new_card.rotationOffset = Random.Range(0.0f, 360.0f);
			new_card.speed = 0.01f;
			cards.Add(new_card);

			new_card = new spinner();
			new_card.card = Instantiate(basic_card_prefab);
			card_object = new_card.card.GetComponent<Card>();
			card_object.draggable = true;
			card_object.current_state = Card.card_states.Draft;
			card_object.assign_type(library.master_card_list[Random.Range(0, library.master_card_list.Count)]);
			new_card.card.transform.SetPositionAndRotation(new Vector3(-menuCamera.aspect * menuCamera.orthographicSize - 1.5f, -3.0f, 0.0f), Quaternion.identity);
			new_card.rotationOffset = Random.Range(0.0f, 360.0f);
			new_card.speed = 0.01f;
			cards.Add(new_card);
		}

		for(int i = 0; i < cards.Count; i++){
			cards[i].card.transform.SetPositionAndRotation(cards[i].card.transform.position + new Vector3(cards[i].speed, 0.0f, 0.0f), Quaternion.AngleAxis(cards[i].rotationOffset - Time.time * 3, Vector3.back));
			if(Mathf.Abs(cards[i].card.transform.position.x) > menuCamera.aspect * menuCamera.orthographicSize + 3.0f)
			{
				GameObject.Destroy(cards[i].card);
				cards.RemoveAt(i);
				i--;
			}
		}
	}
}
