using UnityEngine;

public class PingPongRotate : MonoBehaviour
{
    [SerializeField]
    private float ZRotation;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private LeanTweenType easeType;

    void Start()
    {
        LeanTween.rotateZ(gameObject, ZRotation, rotationSpeed).setEase(easeType).setLoopPingPong();
    }
}
