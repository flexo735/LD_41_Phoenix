using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu_controller : MonoBehaviour {
	public Camera menuCamera;
	public GameObject basic_card_prefab;
	public card_library library;
	int cardTimer = 0;
	List<GameObject> cards;

	public void onPlayClick(){
		SceneManager.LoadScene("Main_Board");
	}
	public void onQuitClick(){
		Application.Quit();
		UnityEditor.EditorApplication.isPlaying = false;
	}

	public void Start(){
		cards = new List<GameObject>();
	}


	// Spawn some cards and make 'em spin!!
	public void FixedUpdate(){
		cardTimer--;
		if(cardTimer < 0){
			cardTimer = 300;
			
			GameObject new_card = Instantiate(basic_card_prefab);
			Card card_object = new_card.GetComponent<Card>();
			card_object.draggable = true;
			card_object.current_state = Card.card_states.Hand;
			card_object.assign_type(library.master_card_list[Random.Range(0, library.master_card_list.Count)]);
			new_card.transform.SetPositionAndRotation(new Vector3(menuCamera.aspect * menuCamera.orthographicSize + 1.5f, -1.0f, 0.0f), Quaternion.identity);
			cards.Add(new_card);
		}
		for(int i = 0; i < cards.Count; i++){
			cards[i].transform.SetPositionAndRotation(cards[i].transform.position + new Vector3(-0.01f, 0.0f, 0.0f), Quaternion.AngleAxis(i * 60 - Time.time * 3, Vector3.back));
		}
		if(cards.Count >= 6)
		{
			if(-cards[5].transform.position.x > menuCamera.aspect * menuCamera.orthographicSize + 1)
			{
				GameObject.Destroy(cards[0], 0.01f);
				GameObject.Destroy(cards[1], 0.01f);
				GameObject.Destroy(cards[2], 0.01f);
				GameObject.Destroy(cards[3], 0.01f);
				GameObject.Destroy(cards[4], 0.01f);
				GameObject.Destroy(cards[5], 0.01f);
				cards.Remove(cards[0]);
				cards.Remove(cards[0]);
				cards.Remove(cards[0]);
				cards.Remove(cards[0]);
				cards.Remove(cards[0]);
				cards.Remove(cards[0]);
			}
		}
	}
}
