using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    protected GameObject token;
    [SerializeField]
    protected Transform tokenSpawn;
    [SerializeField]
    protected GameObject scoreTag;
    [SerializeField]
    protected GameObject overlay;

    [SerializeField]
    protected float scoreX;
    [SerializeField]
    protected float scoreY;
    [SerializeField]
    protected float scoreYGap;

    protected List<Player> players;
    protected int currentPlayerIndex = 0;

    protected bool tokenInPlay;

    protected void Start()
    {
        players = GlobalControl.Instance.Players.ToList();

        if (GlobalControl.Instance.NewGame) {
            ShufflePlayers();
            GlobalControl.Instance.CurrentPlayerIndex = 0;
        }
        currentPlayerIndex = GlobalControl.Instance.CurrentPlayerIndex;

        CreateTags();

        players[currentPlayerIndex].MyTurn = true;
        GlobalControl.Instance.NewGame = false;
    }

    private void ShufflePlayers()
    {
        var shuffledPlayers = new List<Player>();

        while (players.Any())
        {
            var randomPlayerIndex = Random.Range(0, players.Count);

            shuffledPlayers.Add(players[randomPlayerIndex]);
            players.RemoveAt(randomPlayerIndex);
        }

        GlobalControl.Instance.Players = shuffledPlayers;
        players = shuffledPlayers.ToList();
    }

    protected void CreateTags()
    {
        var newGame = GlobalControl.Instance.NewGame;

        for (var i = 0; i < players.Count; i++)
        {
            var player = players[i];

            if (newGame)
            {
                player.Score = 0;
                player.MyTurn = false;
            }

            var tag = CreateTag(i);
            tag.GetComponent<ScoreTag>().player = player;
        }
    }

    protected GameObject CreateTag(int playerIndex)
    {
        var instPos = new Vector3(scoreX, scoreY - (playerIndex * scoreYGap), 0);
        return GameObject.Instantiate(scoreTag, instPos, Quaternion.identity);
    }

    public void SpawnToken(Vector3 spawnPos)
    {
        if (tokenInPlay)
            return;

        var currentPlayer = players[currentPlayerIndex];
        var tokenScript = token.GetComponent<Token>();
        tokenScript.tokenColor = currentPlayer.Colour;
        tokenScript.bounceSound = currentPlayer.bounceSound?.Sound;

        tokenInPlay = true;

        GameObject.Instantiate(token, spawnPos, Quaternion.identity);
    }

    public void Play()
    {
        overlay.SetActive(false);
        SpawnToken(tokenSpawn.position);
    }

    public void UpdateScore(int points)
    {
        tokenInPlay = false;

        var currentPlayer = players[currentPlayerIndex];
        currentPlayer.AddPoints(points);

        NextPlayerTurn();
    }

    public virtual void NextPlayerTurn()
    {
        currentPlayerIndex++;
        UpdateTurnAndIndex();

        ShowOverlay();
    }

    protected void UpdateTurnAndIndex()
    {
        if (currentPlayerIndex >= players.Count)
            currentPlayerIndex = 0;

        GlobalControl.Instance.CurrentPlayerIndex = currentPlayerIndex;

        foreach (var player in players)
        {
            player.MyTurn = false;
        }
        players[currentPlayerIndex].MyTurn = true;
    }

    protected void ShowOverlay()
    {
        overlay.SetActive(true);
    }

    public void Incorrect()
    {
        this.UpdateScore(-2);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(new Vector3(scoreX, scoreY, 0), 1f);
    }

    public void StartSuddenDeath()
    {
        GlobalControl.Instance.NewGame = true;
        GlobalControl.LoadScene(Scenes.BoardOne_SuddenDeath);
    }

    public void Quit()
    {
        for (var i = 0; i < players.Count; i++)
        {
            var player = players[i];

            player.Score = 0;
            player.MyTurn = false;
        }

        GlobalControl.LoadScene(Scenes.MainMenu);
    }
}
