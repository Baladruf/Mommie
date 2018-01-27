using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInGame : MonoBehaviour {

	public int score =0;
	// Update is called once per frame
	void Update () {
		if ((int)GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().timeLeft >= 0)
			transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = "Time left: " + (int)GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().timeLeft;
		else
			transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = "T Bézé!";
		score = (int)GameObject.FindGameObjectWithTag ("Player").transform.position.y;
		transform.GetChild (1).GetChild (0).GetComponent<Text> ().text = "Score: " + score;
	}
}
