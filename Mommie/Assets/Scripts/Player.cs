using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	public float speedMouvement;
	public float maxSpeedMouvement;
	public float cadenceDeTir = 1;
	bool isShooting = false;
	public GameObject burst;
    public GameObject machineGunBurst;
    public bool isMoving =false;
    public float recul;
    public int score = 0;
    public bool usingMachineGun = false;
    bool canMove = true;
    bool canSwitch = true;
    ParticleSystem part;
    AudioSource audioS;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Animator anim;

	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		part = transform.GetChild (0).GetComponent<ParticleSystem> ();
        audioS = GetComponent<AudioSource>();
	}

	void Update () {
		CheckMaxVelocity();
		if ((Input.GetAxisRaw ("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") != 0) && canMove) {
			isMoving = true;
			anim.SetBool ("moving", true);

            //Deplace le joueur
			rb.AddForce (new Vector2 (Input.GetAxisRaw ("Horizontal") * speedMouvement, Input.GetAxisRaw ("Vertical") * speedMouvement));

            //Quand le joueur bouge, on emet de la poussiere a ses pieds
			var emission = part.emission;
			emission.rateOverTime = 20;

            //Oriente le joueur en fonction des inputs

            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                    sprite.flipX = true;
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                    sprite.flipX = false;
            }
		} else {
			isMoving = false;
			anim.SetBool ("moving", false);

            //Si il ne bouge pas, pas de trainé de poussiere
			var emission = part.emission;
			if (!isShooting)
                emission.rateOverTime = 0;
		}

		if (Input.GetAxisRaw ("Fire1") != 0 && !isShooting) {
			StartCoroutine (Shoot ());
            if (!usingMachineGun)
                anim.SetBool("shoot", true);
            audioS.pitch = Random.Range(0.9f, 1.1f);
            audioS.Play();
            if (!usingMachineGun)
                GameObject.Find("MainCamera").GetComponent<CameraShake>().shakeDuration = 0.7f;
            else
                GameObject.Find("MainCamera").GetComponent<CameraShake>().shakeDuration = 0.2f;
            if (!usingMachineGun)
                StartCoroutine(Recul());
        }

        if (usingMachineGun)
            cadenceDeTir = 0.05f;
        else
            cadenceDeTir = 1;
        if (Input.GetAxisRaw("Fire2") != 0 && canSwitch)
            StartCoroutine(SwitchWeapons());
        //Retour au menu
        if(Input.GetAxisRaw("Cancel") != 0)
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
	}

    //Cette methode limite la vitesse maximal du joueur
	void CheckMaxVelocity()
	{
		//à optimiser
		if (rb.velocity.x > maxSpeedMouvement)
			rb.velocity = new Vector2(maxSpeedMouvement, rb.velocity.y);
		if (rb.velocity.x < -maxSpeedMouvement)
			rb.velocity = new Vector2(-maxSpeedMouvement, rb.velocity.y);
		if (rb.velocity.y > maxSpeedMouvement)
			rb.velocity = new Vector2(rb.velocity.x, maxSpeedMouvement);
		if (rb.velocity.y < -maxSpeedMouvement)
			rb.velocity = new Vector2(rb.velocity.x, -maxSpeedMouvement);
	}

    IEnumerator SwitchWeapons()
    {
        canSwitch = false;
        usingMachineGun = !usingMachineGun;
        yield return new WaitForSeconds(1);
        canSwitch = true;
    }

    IEnumerator Recul()
    {
        if (!sprite.flipX)
            rb.AddForce(Vector2.left * recul);
        else
            rb.AddForce(Vector2.right * recul);

        //Emet quelques particules de poussière à nos pieds
        var emission = part.emission;
        emission.rateOverTime = 20;
        yield return new WaitForSeconds(0.3f);
        emission.rateOverTime = 0;

    }

	IEnumerator Shoot()
	{
        if (!usingMachineGun)
            StartCoroutine(CanMoveAgain());
		isShooting = true;
		Quaternion rot;

        //Oriente le tir en fonction de l'orientation du joueur
		if (sprite.flipX == false) {
			rot = Quaternion.identity * Quaternion.Euler (0, 0, -22.5f);
		} else {
			rot = Quaternion.identity * Quaternion.Euler (0, 0, -22.5f) * Quaternion.Euler(0,0,170);
		}
        GameObject shoot = new GameObject();
        if (!usingMachineGun)
            shoot = Instantiate (burst, transform.position - new Vector3(0,0,1), rot);
        else
            shoot = Instantiate(machineGunBurst, transform.position - new Vector3(0, 0, 1), rot);
        shoot.transform.parent = transform;
		Destroy (shoot, 2);


		yield return new WaitForSeconds (cadenceDeTir);
		isShooting = false;
	}

    //Micro délaie de stun après un tir
    IEnumerator CanMoveAgain()
    {
        canMove = false;
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("shoot", false);
        canMove = true;
    }
}
