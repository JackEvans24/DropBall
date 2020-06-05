using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player", order = 1)]
public class Player : ScriptableObject
{
    public Guid Id;
    public string Name;
    public int Score;
    public Color Colour;
    public SoundOption bounceSound;
    public bool MyTurn;

    public Player()
    {
        this.Id = Guid.NewGuid();
    }

    public void AddPoints(int pointsToAdd)
    {
        var newScore = this.Score + pointsToAdd;
        this.Score = Math.Max(0, newScore);

        // var text = this.Label.GetComponent<Text>();
        // text.text = this.Score.ToString();
    }
}
