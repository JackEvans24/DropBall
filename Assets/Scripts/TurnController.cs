using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnController
{
    public struct TurnData
    {
        public Guid[] PlayerIds;
        public Guid CurrentPlayerId;
        public Dictionary<Guid, int> PlayerPoints;
        public Dictionary<Guid, bool> Answers;
    }

    public GameState State;
    public List<Player> OriginalPlayers;
    protected Stack<TurnData> undoStack;
    protected Stack<TurnData> redoStack;

    public TurnController()
    {
        this.Clear();
    }

    public void NewGame(GameState state)
    {
        this.State = state;
        this.OriginalPlayers = new List<Player>(state.Players);

        this.Clear();
    }

    private TurnData GetData()
    {
        return new TurnData()
        {
            PlayerIds = new List<Player>(State.Players).Select(p => p.Id).ToArray(),
            CurrentPlayerId = State.CurrentPlayer.Id,
            PlayerPoints = State.Players.ToDictionary(p => p.Id, p => p.Score),
            Answers = State.Answers == null ? null : new Dictionary<Guid, bool>(State.Answers)
        };
    }

    public void NextTurn(int pointsAdded)
    {
        this.redoStack.Clear();

        this.undoStack.Push(GetData());

        State.CurrentPlayer.AddPoints(pointsAdded);

        State.CurrentPlayerIndex++;

        this.UpdateTurnAndIndex();
    }

    public void UpdateTurnAndIndex()
    {
        var state = State;

        if (state.CurrentPlayerIndex >= state.Players.Count)
            state.CurrentPlayerIndex = 0;
        else if (state.CurrentPlayerIndex < 0)
            state.CurrentPlayerIndex = state.Players.Count - 1;

        foreach (var player in state.Players)
        {
            player.MyTurn = false;
        }

        state.CurrentPlayer.MyTurn = true;
    }

    public void UpdateUndoRedoButtons()
    {
        var undo = GameObject.FindGameObjectWithTag("Undo");
        var redo = GameObject.FindGameObjectWithTag("Redo");

        undo.GetComponent<Button>().interactable = CanUndo();
        redo.GetComponent<Button>().interactable = CanRedo();
    }

    public bool CanUndo() => this.undoStack.Any();

    public void Undo()
    {
        redoStack.Push(GetData());

        var data = undoStack.Pop();

        State.Players = new List<Player>(OriginalPlayers.Where(p => data.PlayerIds.Contains(p.Id)).ToList());
        State.Answers = data.Answers == null ? null : new Dictionary<Guid, bool>(data.Answers);

        foreach (var kvp in data.PlayerPoints)
        {
            var player = State.Players.First(p => p.Id == kvp.Key).Score = kvp.Value;
        }

        State.CurrentPlayerIndex--;

        UpdateTurnAndIndex();
        UpdateUndoRedoButtons();
    }

    public bool CanRedo() => this.redoStack.Any();

    public void Redo()
    {
        undoStack.Push(GetData());

        var data = redoStack.Pop();
        State.Players = new List<Player>(OriginalPlayers.Where(p => data.PlayerIds.Contains(p.Id)));

        State.Answers = data.Answers == null ? null : new Dictionary<Guid, bool>(data.Answers);

        foreach (var kvp in data.PlayerPoints)
        {
            var player = State.Players.First(p => p.Id == kvp.Key).Score = kvp.Value;
        }

        State.CurrentPlayerIndex++;

        UpdateTurnAndIndex();
        UpdateUndoRedoButtons();
    }

    public void EndGame()
    {
        this.State.Players = OriginalPlayers;
        this.State.ResetGame();

        this.Clear();
    }

    public void Clear()
    {
        this.undoStack = new Stack<TurnData>();
        this.redoStack = new Stack<TurnData>();
    }
}
