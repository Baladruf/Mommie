using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JetDeFlammeCercle : MonoBehaviour {

    public float temps = 5;
    public Vector3 angle;
    private Vector3 angleInverse;
    public bool sens = true;

    // Use this for initialization
    void Start()
    {
        angleInverse = new Vector3(0, 0, -1 * angle.z);
        transform.DORotate(sens ? angle : angleInverse, temps / 2).SetEase(Ease.Linear).OnComplete(() =>
        {
            sens = !sens;
            StartCoroutine(Loop());
        });
    }

    private IEnumerator Loop()
    {
        yield return null;
        transform.DORotate(sens ? angle : angleInverse, temps).SetEase(Ease.Linear).OnComplete(() =>
        {
            sens = !sens;
            StartCoroutine(Loop());
        });
    }

}
