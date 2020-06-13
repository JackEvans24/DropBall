using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnController
{
    public struct UndoRedoData
    {
        public Guid[] PlayerIds;
        public Guid CurrentPlayerId;
        public int CurrentPlayerPoints;
    }

    public GameState State;
    public List<Player> OriginalPlayers;
    protected Stack<UndoRedoData> undoStack;
    protected Stack<UndoRedoData> redoStack;

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

    private UndoRedoData GetData(Player player, List<Player> currentPlayers)
    {
        return new UndoRedoData()
        {
            PlayerIds = currentPlayers.Select(p => p.Id).ToArray(),
            CurrentPlayerId = player.Id,
            CurrentPlayerPoints = player.Score
        };
    }

    public void NextTurn(int pointsAdded)
    {
        this.redoStack.Clear();

        var data = GetData(State.CurrentPlayer, State.Players);

        this.undoStack.Push(data);

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
        var data = undoStack.Pop();
        var turnPlayers = new List<Player>(State.Players);

        State.Players = new List<Player>(OriginalPlayers.Where(p => data.PlayerIds.Contains(p.Id)).ToList());

        var player = State.Players.First(p => p.Id == data.CurrentPlayerId);
        redoStack.Push(GetData(player, turnPlayers));

        player.Score = data.CurrentPlayerPoints;
        State.CurrentPlayerIndex--;

        UpdateTurnAndIndex();
        UpdateUndoRedoButtons();
    }

    public bool CanRedo() => this.redoStack.Any();

    public void Redo()
    {
        var data = redoStack.Pop();
        var turnPlayers = new List<Player>(State.Players);

        State.Players = new List<Player>(OriginalPlayers.Where(p => data.PlayerIds.Contains(p.Id)));

        var player = State.Players.First(p => p.Id == data.CurrentPlayerId);
        undoStack.Push(GetData(player, turnPlayers));

        player.Score = data.CurrentPlayerPoints;
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
        this.undoStack = new Stack<UndoRedoData>();
        this.redoStack = new Stack<UndoRedoData>();
    }
}
