using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
	public int id;
	public static Hashtable list;
	void Awake(){
		if(list == null)
			list = new Hashtable();
		if(list.Contains(id))
			{GameObject.Destroy(gameObject); return;}

		list.Add(id, gameObject);
		DontDestroyOnLoad(gameObject);
	}
}
