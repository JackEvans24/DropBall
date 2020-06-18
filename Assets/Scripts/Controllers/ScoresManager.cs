using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoresManager : MonoBehaviour
{
    [Header("Scores")]
    public ColumnPositioning scoresPositioning;
    [SerializeField]
    private GameObject scoreTag;
    [SerializeField]
    private float scoreTagSize;
    [SerializeField]
    private float scoreTagAnimationDuration;
    [SerializeField]
    private float showTagInterval;

    [SerializeField]
    private float endOfScoresInterval;

    private GameState state;
    private Stack<GameObject> tags;
    private Guid currentPlayer;

    void Start()
    {
        this.state = GlobalControl.Instance.turnController.State;
        this.tags = new Stack<GameObject>();

        CreateTags();

        StartCoroutine(ShowScores());
    }

    protected void CreateTags()
    {
        var players = state.Players.OrderByDescending(p => p.Score).ToArray();

        for (var i = 0; i < players.Length; i++)
        {
            var player = players[i];

            if (player.MyTurn)
            {
                player.MyTurn = false;
                currentPlayer = player.Id;
            }

            var tag = CreateTag(i);

            tag.GetComponent<ScoreTag>().player = player;
            tag.SetActive(false);

            tags.Push(tag);
        }
    }

    protected GameObject CreateTag(int playerIndex) =>
        GameObject.Instantiate(scoreTag, scoresPositioning.CalculatePosition(playerIndex), Quaternion.identity);

    protected IEnumerator ShowScores()
    {
        while (tags.Any())
        {
            yield return new WaitForSeconds(showTagInterval);

            ShowNextScore();

            yield return new WaitForSeconds(scoreTagAnimationDuration);
        }

        yield return new WaitForSeconds(endOfScoresInterval);

        HideScores();
        yield return new WaitForSeconds(scoreTagAnimationDuration);

        state.Players.First(p => p.Id == currentPlayer).MyTurn = true;

        GlobalControl.Instance.roundController.NextRound();
    }

    protected void ShowNextScore()
    {
            var tag = tags.Pop();

            tag.SetActive(true);

            tag.transform.LeanScale(Vector3.one * scoreTagSize, scoreTagAnimationDuration).setEaseInCubic();
    }

    protected void HideScores()
    {
        var scoreTags = FindObjectsOfType<ScoreTag>();

        foreach (var tag in scoreTags)
            LeanTween.alphaCanvas(tag.GetComponentInChildren<CanvasGroup>(), 0, scoreTagAnimationDuration);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(new Vector3(scoresPositioning.InitialXPos, scoresPositioning.InitialYPos, 0), 0.2f);

        for (int i = 1; i < 6; i++)
        {
            Gizmos.DrawWireCube(
                new Vector3(scoresPositioning.InitialXPos, scoresPositioning.InitialYPos - (scoresPositioning.yOffset * i), 0),
                Vector3.one * 0.2f
            );
        }
    }
}
