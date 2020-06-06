using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;
    public List<Player> Players;
    public bool NewGame;
    public int CurrentPlayerIndex;
    public List<Color> playerColours;
    public List<string> playerNames;

    void Awake ()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;

            CreateStandardPlayers();
        }
        else if (Instance != this)
        {
            Destroy (gameObject);
        }
    }

    private void CreateStandardPlayers()
    {
        Players = new List<Player>();

        for (var i = 0; i < 2; i++)
            Players.Add(CreatePlayer(string.Empty));
    }

    public static Player CreatePlayer(string name)
    {
        var player = (Player)ScriptableObject.CreateInstance("Player");

        player.Name = string.IsNullOrWhiteSpace(name) ? Instance.GetName() : name;
        player.Colour = Instance.GetColor();

        return player;
    }

    private string GetName()
    {
        var availableNames = playerNames.Where(n => !Players.Any(p => p.Name == n));
        return availableNames.ElementAt(Random.Range(0, availableNames.Count()));
    }

    private Color GetColor()
    {
        var availableColours = playerColours.Where(c => !Players.Any(p => p.Colour == c));
        return availableColours.ElementAt(Random.Range(0, availableColours.Count()));
    }

    public static void LoadScene(Scenes scene)
    {
        SceneManager.LoadScene((int)scene);
    }
}
