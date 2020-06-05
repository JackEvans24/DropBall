using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Bucket : MonoBehaviour
{
    [SerializeField]
    private int points;
    [SerializeField]
    private Color color;
    [SerializeField]
    private AudioClip hitSound;

    private ParticleSystem particles;

    private GameManager gameManager;
    private AudioSource audioSource;

    void Start()
    {
        var scoreText = GetComponentInChildren<Text>();
        scoreText.text = points > 0 ? $"+{points}" : points.ToString();

        var background = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(s => s.tag == "Background");
        if (background != null && color != null)
            background.color = color;

        particles = GetComponentInChildren<ParticleSystem>(true);
        var main = particles.main;
        main.startColor = color;

        gameManager = FindObjectOfType<GameManager>();

        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);

        StartCoroutine("ShowParticles");
        GameObject.Destroy(other.gameObject);

        gameManager.UpdateScore(points);
    }

    IEnumerator ShowParticles()
    {
        particles.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);

        particles.gameObject.SetActive(false);
    }
}
