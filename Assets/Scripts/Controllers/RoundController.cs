using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RoundController
{
    private int maxRounds;
    private int currentRoundIndex;
    private Scenes[] gameScenes;

    public void NewGame(int rounds)
    {
        this.maxRounds = rounds;
        this.currentRoundIndex = 0;

        ShuffleScenes();

        LoadScene();
    }

    private void StartSuddenDeath()
    {
        GlobalControl.Instance.NewGame = true;

        GlobalControl.LoadScene(SceneHelper.GetSuddenDeathScene());
    }

    private void ShuffleScenes()
    {
        var rnd = new Random();
        gameScenes = SceneHelper.StandardScenes.OrderBy(s => rnd.Next()).ToArray();
    }

    public void ShowScores()
    {
        this.currentRoundIndex++;
        GlobalControl.LoadScene(Scenes.Scores);
    }

    public void NextRound()
    {
        if (gameComplete)
        {
            StartSuddenDeath();
            return;
        }

        LoadScene();
    }

    private bool gameComplete { get => this.currentRoundIndex >= this.maxRounds; }

    private void LoadScene() => GlobalControl.LoadScene(gameScenes[currentRoundIndex % gameScenes.Length]);

    public string GetRoundsText() => $"Round {currentRoundIndex + 1}/{maxRounds}";
}
