using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditPlayer : MonoBehaviour
{
    public Player player;
    public Camera mainCamera;
    [SerializeField]
    private GameObject playerOptionsMenu;

    [SerializeField]
    private InputField playerNameInput;
    [SerializeField]
    private GameObject playerOptionsButton;
    [SerializeField]
    private Button removePlayerButton;

    private MenuManager menuManager;

    private void Start()
    {
        SetUiValues();

        menuManager = FindObjectOfType<MenuManager>();

        playerNameInput.onValueChanged.AddListener(value => ChangePlayerName(value));
        playerOptionsButton.GetComponent<Button>().onClick.AddListener(() => OpenPlayerOptions());
        removePlayerButton.onClick.AddListener(() => menuManager.RemovePlayer(player));
    }

    private void SetUiValues()
    {
        playerNameInput.text = player.Name;
        playerOptionsButton.GetComponent<Image>().color = player.Colour;
    }

    public void ChangePlayerName(string newName)
    {
        player.Name = newName;
        SetUiValues();
    }

    public void OpenPlayerOptions()
    {
        var menu = GameObject.Instantiate(playerOptionsMenu);

        var canvas = menu.GetComponent<Canvas>();
        canvas.worldCamera = mainCamera;
        menu.GetComponent<PlayerOptions>().editPlayer = this;
    }

    public void ChangePlayerColour(Color colour)
    {
        player.Colour = colour;
        SetUiValues();
    }

    public void ChangePlayerSound(SoundOption option)
    {
        player.bounceSound = option;
    }
}
