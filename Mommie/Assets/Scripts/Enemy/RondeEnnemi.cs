using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RondeEnnemi : MonoBehaviour {

    public float temps = 5;
    public float destinationEnX = 95;
    private int sens = 1;

	// Use this for initialization
	void Start () {
        StartCoroutine(Loop());
	}
	
    private IEnumerator Loop()
    {
        yield return null;
        transform.DOMoveX(destinationEnX * sens, temps).SetEase(Ease.Linear).OnComplete(() =>
        {
            sens *= -1;
            StartCoroutine(Loop());
        });
    }
}
