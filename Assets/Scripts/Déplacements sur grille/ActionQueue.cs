using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ActionQueue : MonoBehaviour
{
    public PlayerGridMovement player;
    public TMP_Text actionListText; // référence à l'affichage des actions
    private List<PlayerGridMovement.ActionType> actions = new List<PlayerGridMovement.ActionType>();

    public void AddMove()
    {
        actions.Add(PlayerGridMovement.ActionType.Move);
        UpdateUI();
    }

    public void AddTurnRight()
    {
        actions.Add(PlayerGridMovement.ActionType.TurnRight);
        UpdateUI();
    }

    public void ClearActions()
    {
        actions.Clear();
        UpdateUI();
    }

    public void ExecuteActions()
    {
        foreach (var action in actions)
        {
            player.AddAction(action);
        }
        player.ExecuteActions();
        ClearActions();
    }

    void UpdateUI()
    {
        actionListText.text = "";
        int moveCount = 0, turnCount = 0;

        foreach (var action in actions)
        {
            if (action == PlayerGridMovement.ActionType.Move) moveCount++;
            if (action == PlayerGridMovement.ActionType.TurnRight) turnCount++;
        }

        if (moveCount > 0) actionListText.text += $"Avancer x{moveCount}\n";
        if (turnCount > 0) actionListText.text += $"Tourner x{turnCount}\n";
    }
}
