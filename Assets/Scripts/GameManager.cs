using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject token;
    [SerializeField]
    private Transform tokenSpawn;
    [SerializeField]
    private GameObject scoreTag;
    [SerializeField]
    private GameObject overlay;

    [SerializeField]
    private float scoreX;
    [SerializeField]
    private float scoreY;
    [SerializeField]
    private float scoreYGap;

    private List<Player> players;
    private int currentPlayerIndex = 0;

    private bool tokenInPlay;

    void Start()
    {
        players = GlobalControl.Instance.Players;
        currentPlayerIndex = GlobalControl.Instance.CurrentPlayerIndex;
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

        players[currentPlayerIndex].MyTurn = true;
    }

    private GameObject CreateTag(int playerIndex)
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

    public void NextPlayerTurn()
    {
        foreach (var player in players)
        {
            player.MyTurn = false;
        }

        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Count)
            currentPlayerIndex = 0;

        GlobalControl.Instance.CurrentPlayerIndex = currentPlayerIndex;

        players[currentPlayerIndex].MyTurn = true;

        StartCoroutine("ShowOverlayAfterDelay");
    }

    private IEnumerator ShowOverlayAfterDelay()
    {
        yield return new WaitForSecondsRealtime(1);
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

    public void Quit()
    {
        for (var i = 0; i < players.Count; i++)
        {
            var player = players[i];

            player.Score = 0;
            player.MyTurn = false;
        }

        SceneManager.LoadScene((int)Scenes.MainMenu);
    }
}
