using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnController
{
    public GameState CurrentState;
    protected Stack<GameState> undoStack;
    protected Stack<GameState> redoStack;

    public TurnController()
    {
        this.Clear();
    }

    public void NextTurn(GameState newState)
    {
        this.undoStack.Push(this.CurrentState);

        newState.CurrentPlayerIndex++;
        this.CurrentState = newState;

        this.UpdateTurnAndIndex();
    }

    protected void UpdateTurnAndIndex()
    {
        var state = CurrentState;
        if (state.CurrentPlayerIndex >= state.Players.Count)
            state.CurrentPlayerIndex = 0;

        foreach (var player in state.Players)
        {
            player.MyTurn = false;
        }

        state.CurrentPlayer.MyTurn = true;
    }

    public bool CanUndo() => this.undoStack.Any();

    public GameState Undo()
    {
        this.redoStack.Push(this.CurrentState);
        return this.undoStack.Pop();
    }

    public bool CanRedo() => this.redoStack.Any();

    public GameState Redo()
    {
        this.undoStack.Push(this.CurrentState);
        return this.redoStack.Pop();
    }

    public void Clear()
    {
        this.undoStack = new Stack<GameState>();
        this.redoStack = new Stack<GameState>();

        this.CurrentState = null;
    }
}
