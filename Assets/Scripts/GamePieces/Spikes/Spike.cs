using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField]
    private float initialDelay;

    [Header("Spike Movement")]
    [SerializeField]
    private float jabDistance = 1f;
    [SerializeField]
    private float jabDuration = 0.2f;
    [SerializeField]
    private float receedDuration = 1f;
    [SerializeField]
    private float jabDelay = 2f;

    private float startPos;

    private void Start()
    {
        if (jabDistance <= 0 || jabDuration <= 0)
            return;

        startPos = transform.position.y;
        StartCoroutine(DelayedJab());
    }

    private IEnumerator DelayedJab()
    {
        yield return new WaitForSeconds(initialDelay);
        Jab();
    }

    private void Jab()
    {
        LeanTween
            .moveY(gameObject, transform.position.y + jabDistance, jabDuration)
            .setDelay(jabDelay)
            .setOnComplete(() => Receed());
    }

    private void Receed()
    {
        LeanTween
            .moveY(gameObject, startPos, receedDuration)
            .setOnComplete(() => Jab());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject.Destroy(other.gameObject);
        SpikeManager.FinishTurn();
    }
}
