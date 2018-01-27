using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JetDeFlammeTournante : MonoBehaviour {

    public float temps = 5;
    private Vector3 angle;

	// Use this for initialization
	void Start () {
        angle = new Vector3(0, 0, 180);
        StartCoroutine(Loop());
	}

    private IEnumerator Loop()
    {
        yield return null;
        transform.DORotate(angle, temps).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.eulerAngles = Vector3.zero;
            StartCoroutine(Loop());
        });
    }

}
