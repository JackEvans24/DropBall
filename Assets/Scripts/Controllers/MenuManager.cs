using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Canvas parentCanvas;
    [SerializeField]
    private RectTransform editPlayerTransform;
    [SerializeField]
    private RectTransform addPlayerButton;
    [SerializeField]
    private float addPlayerButtonXOffset;
    [SerializeField]
    private ColumnPositioning labelPositioning;
    [SerializeField]
    private int maxPlayers;

    private GameState state;


    void Start()
    {
        this.state = GlobalControl.Instance.turnController.State;

        CreatePlayerLabels();
    }

    private void CreatePlayerLabels()
    {
        var i = 0;

        foreach (var player in this.state.Players)
        {
            var editPlayer = editPlayerTransform.GetComponent<EditPlayer>();
            editPlayer.player = player;
            editPlayer.mainCamera = mainCamera;

            InstantiateTransform(
                editPlayerTransform.gameObject,
                parentCanvas.gameObject,
                labelPositioning.CalculatePosition(i)
            );

            i++;
        }

        if (i < maxPlayers) {
            var buttonPosition = labelPositioning.CalculatePosition(i);
            buttonPosition.x += addPlayerButtonXOffset;

            var p = InstantiateTransform(
                addPlayerButton.gameObject,
                parentCanvas.gameObject,
                buttonPosition
            );

            p.GetComponent<Button>().onClick.AddListener(() => AddPlayer());
        }
    }

    private GameObject InstantiateTransform(GameObject obj, GameObject parent, Vector3 position)
    {
        var t = GameObject.Instantiate(obj).transform;

        t.SetParent(parent.transform);
        t.localScale = Vector3.one;
        t.SetPositionAndRotation(position, Quaternion.identity);

        return t.gameObject;
    }

    private void RefreshPlayerLabels()
    {
        var labels = GameObject.FindGameObjectsWithTag("PlayerSettingLabel");
        foreach (var label in labels)
            Object.Destroy(label.gameObject);

        CreatePlayerLabels();
    }

    private void AddPlayer()
    {
        var newPlayer = GlobalControl.CreatePlayer(null);
        this.state.Players.Add(newPlayer);

        RefreshPlayerLabels();
    }

    public void RemovePlayer(Player player)
    {
        this.state.Players = this.state.Players.Where(p => p.Id != player.Id).ToList();

        RefreshPlayerLabels();
    }

    public void StartGame()
    {
        GlobalControl.Instance.turnController.State = this.state;
        GlobalControl.Instance.NewGame = true;
        GlobalControl.LoadScene(SceneHelper.GetStandardScene());
    }

    public void StartSuddenDeath()
    {
        GlobalControl.Instance.turnController.State = this.state;
        GlobalControl.Instance.NewGame = true;
        GlobalControl.LoadScene(SceneHelper.GetSuddenDeathScene());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
