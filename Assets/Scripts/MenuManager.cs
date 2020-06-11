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


    void Start()
    {
        CreatePlayerLabels();
    }

    private void CreatePlayerLabels()
    {
        var i = 0;

        foreach (var player in GlobalControl.Instance.Players)
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
        GlobalControl.Instance.Players.Add(newPlayer);

        RefreshPlayerLabels();
    }

    public void RemovePlayer(Player player)
    {
        GlobalControl.Instance.Players =
            GlobalControl.Instance.Players.Where(p => p.Id != player.Id).ToList();

        RefreshPlayerLabels();
    }

    public void StartGame()
    {
        GlobalControl.Instance.NewGame = true;
        GlobalControl.LoadScene(Scenes.BoardOne);
    }

    public void StartSuddenDeath()
    {
        GlobalControl.Instance.NewGame = true;
        GlobalControl.LoadScene(Scenes.BoardOne_SuddenDeath);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
