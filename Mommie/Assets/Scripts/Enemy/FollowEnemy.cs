using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour {

    public float vitesse = 50;
    private Transform player;
    private Vector3 direction;
    public float distanceMax = 20;
    //private Rigidbody2D rigi;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //rigi = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position, player.position) <= distanceMax) {
            direction = Vector3.Normalize(player.position - transform.position);
            transform.Translate(direction * vitesse * Time.deltaTime);
        }
	}
}
