using System.Collections.Generic;
using System.Linq;

public class GameState
{
    public List<Player> Players;
    public int CurrentPlayerIndex;

    public GameState()
    {
        this.Players = new List<Player>();
    }

    public Player CurrentPlayer
    {
        get => this.Players[this.CurrentPlayerIndex];
    }

    public void ResetGame()
    {
        CurrentPlayerIndex = 0;
        foreach (var player in Players)
        {
            player.Score = 0;
            player.MyTurn = false;
        }
    }
}
