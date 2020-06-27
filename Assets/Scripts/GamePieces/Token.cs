using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    public Color tokenColor;
    public AudioClip bounceSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        var sprite = GetComponent<SpriteRenderer>();
        sprite.color = tokenColor;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (bounceSound != null)
        {
            audioSource.Stop();
            audioSource.clip = bounceSound;
            audioSource.Play();
        }
    }
}
