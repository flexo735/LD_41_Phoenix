﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radial_Countdown_Timer : MonoBehaviour {

	public bool show_countdown = false;
	public float current_time;
	public float max_time; //Used to decide how quickly or slowly the bar should close.

	private Image radial_image;

	public Color start_colour;
	public Color end_colour;

	// Use this for initialization
	void Start () 
	{
		radial_image = gameObject.GetComponent<Image>();
		radial_image.enabled = false;
		current_time = 0.0f;
		radial_image.fillMethod = Image.FillMethod.Radial360;
		radial_image.fillClockwise = true;
		radial_image.fillOrigin = 2;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (show_countdown)
		{
			current_time -= Time.deltaTime;
			if (current_time <= 0)
			{
				radial_image.enabled = false;
			}
			else
			{
				radial_image.fillAmount = current_time/max_time;
				Color lerped_colour = Color.Lerp(end_colour,start_colour,current_time/max_time);
				radial_image.color = lerped_colour;
			}
		}

	}

	public void start_countdown(float countdown_time)
	{
		current_time = countdown_time;
		max_time = countdown_time;
		show_countdown = true;

		radial_image.enabled = true;
		radial_image.fillAmount = current_time/max_time;
	}
}
