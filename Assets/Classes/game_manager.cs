using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class game_manager : MonoBehaviour {

	// singleton stuff
	private static game_manager instance;
	public static game_manager getInstance(){
		if(instance == null)
		{
			instance = new GameObject("Game_Manager").AddComponent<game_manager>();
			instance.inGame = false;
		}
		return instance;
	}
	// done singleton stuff

	public bool inGame = true;
	public Canvas winScreen;
	public Canvas loseScreen;
	public Player_Hand player;
	public Player_Hand AI;
	public float winTime;
	public Text winTimeText;

	public void Update(){

		// handle the win/loss screens
		if(AI.health_value <= 0)
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

	// exit to the main menu
	public void onReturnToMenu(){
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("Main_Menu");
	}
}
