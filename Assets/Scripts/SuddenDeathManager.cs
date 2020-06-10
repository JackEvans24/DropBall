using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SuddenDeathManager : GameManager
{
    private Dictionary<Guid, bool> answers;

    [SerializeField]
    private int maxRounds;
    private int rounds;
    [SerializeField]
    private Text roundsText;

    private bool gameOver;
    private bool backToMenu;
    [SerializeField]
    private GameObject winnerCanvas;
    [SerializeField]
    private Text winnerText;

    new protected void Start()
    {
        base.Start();

        answers = new Dictionary<Guid, bool>();

        UpdateRoundsText();
    }

    private void Update()
    {
        if (gameOver && Input.GetButton("Jump"))
            BackToMenu();
    }

    new public void Play()
    {
        var player = players[currentPlayerIndex];
        answers[player.Id] = true;

        base.Play();
    }

    public override void NextPlayerTurn()
    {
        base.NextPlayerTurn();

        CheckAnswers();
    }

    new public void Incorrect()
    {
        var player = players[currentPlayerIndex];
        answers[player.Id] = false;

        NextPlayerTurn();
    }

    private void CheckAnswers()
    {
        if (currentPlayerIndex != 0 || answers.Count != players.Count)
            return;

        rounds++;
        UpdateRoundsText();

        if (GameComplete())
        {
            ShowWinner();
            return;
        }

        if (answers.All(a => a.Value) || answers.All(a => !a.Value))
            return;

        RemoveTags();

        RemoveIncorrectPlayers();

        if (players.Count == 1)
        {
            ShowWinner();
            return;
        }

        CreateTags();

        UpdateTurnAndIndex();
    }

    private void UpdateRoundsText()
    {
        roundsText.text = $"Round {rounds + 1}/{maxRounds}";
    }

    private void RemoveTags()
    {
        foreach (var tag in FindObjectsOfType<ScoreTag>())
            GameObject.Destroy(tag.gameObject);
    }

    private bool GameComplete()
    {
        if (rounds < maxRounds)
            return false;

        var topScore = players.OrderByDescending(p => p.Score).First().Score;

        return players.Count(p => p.Score == topScore) == 1;
    }

    private void RemoveIncorrectPlayers()
    {
        for (var i = 0; i < players.Count; i++)
        {
            var player = players[i];

            if (answers[player.Id])
                continue;

            this.players.RemoveAt(i);

            if (currentPlayerIndex > i)
                currentPlayerIndex--;

            i--;
        }

        answers.Clear();
    }

    private void ShowWinner()
    {
        overlay.SetActive(false);

        gameOver = true;
        var winner = players.OrderByDescending(p => p.Score).First();

        winnerText.text = winner.Name;
        winnerText.color = winner.Colour;

        winnerCanvas.SetActive(true);
    }

    private void BackToMenu()
    {
        if (backToMenu)
            return;

        backToMenu = true;

        FindObjectOfType<LevelLoader>().LoadScene(Scenes.MainMenu);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(new Vector3(scoreX, scoreY, 0), 1f);
    }
}
