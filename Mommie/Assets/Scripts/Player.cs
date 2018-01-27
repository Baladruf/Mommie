using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	Rigidbody2D rb;
	public float speedMouvement=100;
	public float maxSpeedMouvement=50;
	public float drag=10;

	public float timeLeft = 3;
	public float delayBeforeDeath = 1;
	bool canMove = true;
	float initalspeed;
	private bool isMoving = false;
	private Vector2 direction;
	public GameObject coeur;
	public float LancerDeCoeur = 15;
	private bool canSend = true;
    public SmoothCamera2D camera;

	SpriteRenderer sprite;
	Animator anim;

	void Awake(){
		direction = new Vector2 (1, 1);
	}

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();
		sprite = GetComponent<SpriteRenderer> ();
		//Setting
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		rb.drag = drag;
		rb.gravityScale = 0;
	}

	void Update()
	{
		timeLeft -= Time.deltaTime;
		//Debug.Log ("x = " + Input.GetAxisRaw ("Vertical") + ", y = " + Input.GetAxisRaw ("Horizontal"));
		if( Input.GetAxisRaw ("Fire1")>0 && canSend){
			Rigidbody2D tempCoeur = Instantiate (coeur, transform.position + new Vector3(direction.x, direction.y), Quaternion.identity).GetComponent<Rigidbody2D>();
			tempCoeur.AddForce (direction * LancerDeCoeur, ForceMode2D.Impulse);
			canSend = false;
            canMove = false;
            camera.target = tempCoeur.transform;
		}
		if ( timeLeft < 0 )
		{
			canMove = false;
			StartCoroutine (Die ());
		}
	}

	void FixedUpdate() {
		CheckMaxVelocity ();
		if (canMove) {
			float tempX = Input.GetAxisRaw ("Horizontal"), tempY = Input.GetAxisRaw ("Vertical");
			if (tempX != 0 || tempY != 0) {
				direction.Set (tempX, tempY);
				rb.AddForce((direction * speedMouvement * Time.deltaTime), ForceMode2D.Force);
				//rb.AddForce (new Vector2 (Input.GetAxisRaw ("Horizontal") * speedMouvement, Input.GetAxisRaw ("Vertical") * speedMouvement));
				isMoving = true;
			} else {
				isMoving = false;
			}

			//Animation


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
		yield return new WaitForSeconds (delayBeforeDeath);	
		SceneManager.LoadScene ("Scene");
	}

    public void ReInitSendHeart()
    {
        canSend = true;
        canMove = true;
    }
}