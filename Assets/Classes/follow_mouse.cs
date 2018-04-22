using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow_mouse : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
