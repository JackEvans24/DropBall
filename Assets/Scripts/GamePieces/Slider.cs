using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    [SerializeField]
    private float xDistance;
    [SerializeField]
    private float yDistance;
    [SerializeField]
    private float slideDuration;

    void Start()
    {
        if ((xDistance == 0 && yDistance == 0) || slideDuration == 0)
            return;

        var endPosition = new Vector3(transform.position.x + xDistance, transform.position.y + yDistance, transform.position.z);
        LeanTween.move(gameObject, endPosition, slideDuration).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
    }

    private void OnDrawGizmosSelected()
    {
        var endPos = new Vector3(transform.position.x + xDistance, transform.position.y + yDistance, transform.position.z);
        Gizmos.DrawLine(transform.position, endPos);
    }
}
