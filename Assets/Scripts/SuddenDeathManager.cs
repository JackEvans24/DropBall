using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SuddenDeathManager : GameManager
{
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

        state.Answers = new Dictionary<Guid, bool>();

        UpdateRoundsText();
    }

    private void Update()
    {
        if (gameOver && Input.GetButton("Jump"))
            BackToMenu();
    }

    public override void Play()
    {
        state.Answers[state.CurrentPlayer.Id] = true;

        base.Play();
    }

    public override void FinishTurn(int pointsAdded)
    {
        base.FinishTurn(pointsAdded);

        CheckAnswers();
    }

    new public void Incorrect()
    {
        state.Answers[state.CurrentPlayer.Id] = false;

        FinishTurn(0);
    }

    public override void Undo()
    {
        if (this.turnController.CanUndo() && state.CurrentPlayerIndex == 0)
        {
            rounds--;
            UpdateRoundsText();
        }

        base.Undo();

        RemoveTags();
        CreateTags();
    }

    public override void Redo()
    {
        if (this.turnController.CanRedo() && state.CurrentPlayerIndex == state.Players.Count - 1)
        {
            rounds++;
            UpdateRoundsText();
        }

        base.Redo();

        RemoveTags();
        CreateTags();
    }

    private void CheckAnswers()
    {
        if (state.CurrentPlayerIndex != 0 || state.Answers.Count != state.Players.Count)
            return;

        rounds++;
        UpdateRoundsText();

        if (IsGameComplete())
        {
            ShowWinner();
            return;
        }

        if (state.Answers.All(a => a.Value) || state.Answers.All(a => !a.Value))
            return;

        RemoveTags();

        RemoveIncorrectPlayers();

        if (state.Players.Count == 1)
        {
            ShowWinner();
            return;
        }

        CreateTags();
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

    private bool IsGameComplete()
    {
        if (rounds < maxRounds)
            return false;

        var topScore = state.Players.OrderByDescending(p => p.Score).First().Score;

        return state.Players.Count(p => p.Score == topScore) == 1;
    }

    private void RemoveIncorrectPlayers()
    {
        var players = state.Players;
        for (var i = 0; i < players.Count; i++)
        {
            var player = players[i];

            if (state.Answers[player.Id])
                continue;

            players.RemoveAt(i);

            turnController.UpdateTurnAndIndex();

            i--;
        }

        state.Answers.Clear();
    }

    protected void ShowWinner()
    {
        overlay.SetActive(false);

        gameOver = true;
        var winner = state.Players.OrderByDescending(p => p.Score).First();

        winnerText.text = winner.Name;
        winnerText.color = winner.Colour;

        winnerCanvas.SetActive(true);
    }

    private void BackToMenu()
    {
        if (backToMenu)
            return;

        backToMenu = true;

        this.Quit();
    }
}
