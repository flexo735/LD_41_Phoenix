using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deck_holder : MonoBehaviour {

	public card_library the_deck = null;

	void Awake ()
	{
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
