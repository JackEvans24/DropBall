using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpikeManager : MonoBehaviour
{
    private int points;
    private static SpikeManager Instance;
    private GameManager gameManager;

    public Canvas pointsCanvas;
    protected Text pointsText;
    protected ParticleSystem pointParticles;
    public float textGrowMultiplier;
    public float textGrowTime;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        gameManager = GetComponent<GameManager>();

        pointsText = pointsCanvas.GetComponentInChildren<Text>();
        pointParticles = pointsCanvas.GetComponentInChildren<ParticleSystem>();
    }

    public static void StartTurn()
    {
        if (Instance != null)
            Instance.StartTurnInternal();
    }

    private void StartTurnInternal()
    {
        pointsText.text = "+0";
        pointsCanvas.gameObject.SetActive(true);
    }

    public static void IncrementPoints()
    {
        Instance.IncrementPointsInternal();
    }

    private void IncrementPointsInternal()
    {
        points++;
        pointsText.text = $"+{points}";

        pointParticles.Play();

        if (textGrowMultiplier > 0 && textGrowTime > 0)
        {
            LeanTween
                .scale(pointsText.gameObject, pointsText.transform.lossyScale * textGrowMultiplier, textGrowTime)
                .setEaseInOutBounce();
        }
    }

    public static void FinishTurn()
    {
        Instance.FinishTurnInternal();
    }

    private void FinishTurnInternal()
    {
        LeanTween.scale(pointsText.gameObject, Vector3.one, 0.01f).setEaseInOutBounce();
        pointsCanvas.gameObject.SetActive(false);

        gameManager.FinishTurn(points);

        points = 0;
    }
}
