using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coeur : MonoBehaviour {

    private Transform playerT;
    private Player playerS;
    private Collider2D coll;
    public float DelayDeath = 5;
    private Coroutine corou;

    private void Awake()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
        playerS = playerT.GetComponent<Player>();
        coll = GetComponent<Collider2D>();
        corou = StartCoroutine(Death());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemyMommie")
        {
            Debug.Log("test");
            Vector3 pos = collision.transform.position;
            Destroy(collision.gameObject);
            playerT.position = pos;
            playerS.ReInitSendHeart();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothCamera2D>().target = playerT;
            StopCoroutine(corou);
            Destroy(this.gameObject);
        }else if(collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.collider, coll);
        }
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(DelayDeath);
        //animation mort
        Destroy(gameObject);
    }
}
