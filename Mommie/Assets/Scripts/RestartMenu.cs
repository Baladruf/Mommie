using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartMenu : MonoBehaviour {

	void Start()
	{
		transform.GetChild (2).GetChild (0).GetComponent<Text> ().text = "Score: "+ transform.parent.GetComponent<MenuInGame> ().score;
		//Destroy (transform.parent.GetChild (0));
		//Destroy (transform.parent.GetChild (1));
	}

	// Use this for initialization
	public void Load (string scene) {
		SceneManager.LoadScene (scene);
	}

}
