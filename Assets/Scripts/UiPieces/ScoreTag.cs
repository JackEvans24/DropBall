using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTag : MonoBehaviour
{
    public Player player;

    [SerializeField]
    private Image background;
    [SerializeField]
    private Image turnMarker;
    [SerializeField]
    private Text nameLabel;
    [SerializeField]
    private Text scoreLabel;

    private bool isMyTurn;

    void Start()
    {
        background.color = player.Colour;
        nameLabel.text = player.Name;

        isMyTurn = player.MyTurn;
        turnMarker.color = player.Colour;
        turnMarker.enabled = isMyTurn;
    }

    void Update()
    {
        scoreLabel.text = player.Score.ToString();

        if (player.MyTurn != isMyTurn)
        {
            isMyTurn = turnMarker.enabled = player.MyTurn;
        }
    }
}
