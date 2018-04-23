using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class game_manager : MonoBehaviour {

	/*// singleton stuff
	private static game_manager instance;

	public static game_manager getInstance(){
		if(instance == null)
		{
			instance = new GameObject("Game_Manager").AddComponent<game_manager>();
			instance.inGame = false;
		}
		return instance;
	}*/
	// done singleton stuff

	public bool inGame = true;
	public Canvas winScreen;
	public Canvas loseScreen;
	public Player_Hand player;
	public Player_Hand ai;
	public float winTime;
	public Text winTimeText;
	public Camera gameCamera;

	public GameObject arrow1;
	public GameObject arrow2;
	public GameObject arrow3;
	public GameObject arrow4;

	public List<combat_spot> playerCombatSpots;
	public List<combat_spot> aiCombatSpots;

	void Awake(){
		/*if(instance)
			if(instance != this){
				GameObject.DestroyImmediate(instance.gameObject);
				instance = this;
			}
		else
			instance = this;*/
	}

	public void Update(){

		// handle the win/loss screens
		if(SceneManager.GetActiveScene().name == "Main_Board")
		{
			if(ai.health_value <= 0)
			{
				winTimeText.text = winTime.ToString("F2") + " seconds?\nTook you long enough...";
				winScreen.gameObject.SetActive(true);
				loseScreen.gameObject.SetActive(false);
				Time.timeScale = 0.0f;
			}
			else if(player.health_value <= 0)
			{
				winScreen.gameObject.SetActive(false);
				loseScreen.gameObject.SetActive(true);
				Time.timeScale = 0.0f;
			}
			else
			{
				winTime = Time.timeSinceLevelLoad;
				Time.timeScale = 1.0f;
				winScreen.gameObject.SetActive(false);
				loseScreen.gameObject.SetActive(false);
			}
		}

		// Tutorial
		else if(SceneManager.GetActiveScene().name == "Tutorial_Board")
		{
			if(ai.health_value <= 0)
			{
				SceneManager.LoadScene("Main_Menu");
			}
			bool aiHasCard = false;
			bool playerHasCard = false;
			for(int i = 0; i < aiCombatSpots.Count; i++){
				if(aiCombatSpots[i].currently_holding){
					aiHasCard = true;
				}
			}
			for(int i = 0; i < playerCombatSpots.Count; i++){
				if(playerCombatSpots[i].currently_holding){
					playerHasCard = true;
				}
			}
			if(!playerHasCard){
				if(player.current_hand.Count == 0){
					// arrow1
					arrow1.SetActive(true);
					arrow2.SetActive(false);
					arrow3.SetActive(false);
					arrow4.SetActive(false);
				}
				else{
					// arrow2
					arrow1.SetActive(false);
					arrow2.SetActive(true);
					arrow3.SetActive(false);
					arrow4.SetActive(false);
				}
			}else{
				if(aiHasCard){
					// arrow3
					arrow1.SetActive(false);
					arrow2.SetActive(false);
					arrow3.SetActive(true);
					arrow4.SetActive(false);
				}
				else{
					// arrow4
					arrow1.SetActive(false);
					arrow2.SetActive(false);
					arrow3.SetActive(false);
					arrow4.SetActive(true);
				}
			}
			
		}
	}

	// exit to the main menu
	public void onReturnToMenu(){
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("Main_Menu");
	}
}
