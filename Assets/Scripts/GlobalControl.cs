using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public TurnController turnController;

    public bool NewGame;

    [Header("Player options")]
    public List<Color> playerColours;
    [SerializeField]
    private List<string> playerNames;

    void Awake ()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;

            this.turnController = new TurnController();

            CreateStandardPlayers();
        }

        Instance.GetComponentInChildren<LevelLoader>().EndTransition();

        if (Instance != this)
        {
            Destroy (gameObject);
        }
    }

    private void CreateStandardPlayers()
    {
        var players = new List<Player>();
        turnController.State = new GameState() { Players = players };

        for (var i = 0; i < 2; i++)
            players.Add(CreatePlayer(string.Empty));
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
        var availableNames = playerNames.Where(n => !turnController.State.Players.Any(p => p.Name == n));
        return availableNames.ElementAt(Random.Range(0, availableNames.Count()));
    }

    private Color GetColor()
    {
        var availableColours = playerColours.Where(c => !turnController.State.Players.Any(p => p.Colour == c));
        return availableColours.ElementAt(Random.Range(0, availableColours.Count()));
    }

    public static void LoadScene(Scenes scene)
    {
        var levelLoader = Instance.GetComponentInChildren<LevelLoader>();
        levelLoader.StartTransition();

        Instance.StartCoroutine(LoadSceneAfter(scene, levelLoader.transitionDuration));
    }

    private static IEnumerator LoadSceneAfter(Scenes scene, float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        SceneManager.LoadScene((int)scene);
    }
}
