using UnityEngine;

public class PingPongRotate : MonoBehaviour
{
    [SerializeField]
    private float ZRotation;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float delay;
    [SerializeField]
    private LeanTweenType easeType;

    void Start()
    {
        LeanTween.rotateZ(gameObject, ZRotation, rotationSpeed).setDelay(delay).setEase(easeType).setLoopPingPong();
    }
}
