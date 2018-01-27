﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class Player : MonoBehaviour {

	Rigidbody2D rb;
	public float speedMouvement=100;
	public float maxSpeedMouvement=50;
	public float drag=10;

	public float timeLeft = 3;
	float timeLeftInitial;
	public float delayBeforeDeath = 1;
	bool canMove = true;
	float initalspeed;
	private bool isMoving = false;
	private Vector2 direction;
	public GameObject coeur;
	public float LancerDeCoeur = 15;
	private bool canSend = true;
    public SmoothCamera2D camera;
    public Transform viseur;
    public float delayLancer = 1;
	ParticleSystem particle; 
	public PostProcessingProfile initial;
	public PostProcessingProfile shoot;
	public float cdShoot =1f;
	public GameObject particuleRespawn;
	public GameObject RespawnButton;
	public GameObject DeadBody;
	SpriteRenderer sprite;
	Animator anim;
	bool canSpawnButton = true;
	Vector3 positionDeadBody;
	public float timeZoom =0.3f;

	void Awake(){
		direction = new Vector2 (1, 1);
	}

	void Start () {

		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();
		sprite = GetComponent<SpriteRenderer> ();
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		rb.drag = drag;
		rb.gravityScale = 0;
		particle = transform.GetChild (4).GetComponent<ParticleSystem> ();
		timeLeftInitial = timeLeft;
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Animator> ().SetBool ("Dezoom", false);
	}

	void Update()
	{
		timeLeft -= Time.deltaTime;
		if( Input.GetAxisRaw ("Fire1")>0 && canSend){
            StartCoroutine(PrepareLancer());
            canSend = false;
            canMove = false;
		}
		if ( timeLeft <= 1 )
		{
			canMove = false;
			if (canSend) {
				StartCoroutine (PrepareLancer());
				canSend = false;
			}
			StartCoroutine (Die());
		}
	}

	void FixedUpdate() {
		CheckMaxVelocity ();
		//Deplacement
        if (canMove) {
			float tempX = Input.GetAxisRaw ("Horizontal"), tempY = Input.GetAxisRaw ("Vertical");
			if (tempX != 0 || tempY != 0) {
				direction.Set (tempX, tempY);
				rb.AddForce((direction * speedMouvement * Time.deltaTime), ForceMode2D.Force);
				viseur.localPosition = direction * 40;
                isMoving = true;
			} else {
				isMoving = false;
			}

			if (tempY > 0) {
				anim.SetBool ("Up", true);
				if (tempX < 0) {
					sprite.flipX = false;
				} else if (tempX > 0) {
					sprite.flipX = true;
				}
			} else if (tempY < 0){
				anim.SetBool ("Up", false);
				if (tempX < 0) {
					sprite.flipX = true;
				} else if (tempX > 0) {
					sprite.flipX = false;
				}
			}

			anim.SetBool ("Running", isMoving);
			if (isMoving) {
				var emission = particle.emission;
				emission.rateOverTime = 60;
			} else {

				var emission = particle.emission;
				emission.rateOverTime = 0;

			}
		}

        else
        {
            float tempX = Input.GetAxisRaw("Horizontal"), tempY = Input.GetAxisRaw("Vertical");
            if (tempX != 0 || tempY != 0)
            {
                direction.Set(tempX, tempY);
                viseur.localPosition = direction * 40;
            }
        }
	}
	void CheckMaxVelocity()
	{
		if (rb.velocity.x > maxSpeedMouvement)
			rb.velocity = new Vector2(maxSpeedMouvement, rb.velocity.y);
		if (rb.velocity.x < -maxSpeedMouvement)
			rb.velocity = new Vector2(-maxSpeedMouvement, rb.velocity.y);
		if (rb.velocity.y > maxSpeedMouvement)
			rb.velocity = new Vector2(rb.velocity.x, maxSpeedMouvement);
		if (rb.velocity.y < -maxSpeedMouvement)
			rb.velocity = new Vector2(rb.velocity.x, -maxSpeedMouvement);
	}

	IEnumerator Die()
	{
		yield return new WaitForSeconds (delayBeforeDeath/2);	
		anim.SetBool ("Dying", true);
	}

	public void Menu()
	{
		if (canSpawnButton) {
			GameObject button = Instantiate (RespawnButton, transform.position, Quaternion.identity);
			button.transform.parent = GameObject.Find ("Canvas").transform;
			button.transform.localPosition = Vector3.zero;
			button.transform.localScale = new Vector3 (1, 1, 1);
			canSpawnButton = false;
		}
	}

    private IEnumerator PrepareLancer()
    {
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<PostProcessingBehaviour> ().profile = shoot;
		positionDeadBody = transform.position;

		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Animator> ().SetBool ("Dezoom", true);

		anim.SetBool ("Shooting", true);
		var emission = particle.emission;
		emission.rateOverTime = 0;

        yield return new WaitForSeconds(delayLancer);
        Rigidbody2D tempCoeur = Instantiate(coeur, transform.position + new Vector3(direction.x, direction.y), Quaternion.identity).GetComponent<Rigidbody2D>();
        tempCoeur.AddForce(direction * LancerDeCoeur, ForceMode2D.Impulse);
        camera.target = tempCoeur.transform;

    }

    public void ReInitSendHeart()
    {
		GameObject deadBro = Instantiate (DeadBody, positionDeadBody, Quaternion.identity);
		Destroy (deadBro, 3);


		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Animator> ().SetBool ("Dezoom",false);

		StartCoroutine (WaitBeforeCanShootAgain ());
        canMove = true;
		timeLeft = timeLeftInitial;
		anim.SetBool ("Restart", true);
		StartCoroutine (RestartBackToFalse ());
		GameObject camembert= Instantiate (particuleRespawn, transform.position+new Vector3(0,0,10), Quaternion.identity);
		Destroy (camembert, 3);
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<PostProcessingBehaviour> ().profile = initial;
    }

	IEnumerator WaitBeforeCanShootAgain()
	{
		yield return new WaitForSeconds (cdShoot);
		canSend = true;
	}

	IEnumerator RestartBackToFalse()
	{

		yield return new WaitForSeconds(0.1f);
		anim.SetBool ("Restart", false);
		anim.SetBool ("Shooting", false);
	}
}