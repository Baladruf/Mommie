using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {
	public GameObject[] Blocs;
	public int Indexblocs;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(0) == true)
		{
			Indexblocs = Random.Range (0, Blocs.Length);
			print (Blocs [Indexblocs]);
		}
	}
}
