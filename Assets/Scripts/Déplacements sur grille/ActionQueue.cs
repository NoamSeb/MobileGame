using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ActionQueue : MonoBehaviour
{
    public PlayerGridMovement player;
    public TMP_Text actionListText; // référence à l'affichage des actions
    private List<ActionEntry> actions = new List<ActionEntry>(); // liste des actions avec compteurs

    private class ActionEntry
    {
        public PlayerGridMovement.ActionType actionType;
        public int count;

        public ActionEntry(PlayerGridMovement.ActionType actionType)
        {
            this.actionType = actionType;
            this.count = 1; // par défaut, une action est ajoutée une fois
        }
    }

    public void AddMove()
    {
        AddAction(PlayerGridMovement.ActionType.Move);
    }

    public void AddTurnRight()
    {
        AddAction(PlayerGridMovement.ActionType.TurnRight);
    }

    public void AddTurnLeft()
    {
        AddAction(PlayerGridMovement.ActionType.TurnLeft);
    }

    private void AddAction(PlayerGridMovement.ActionType newAction)
    {
        // si la liste est vide ou si la dernière action est différente, on ajoute une nouvelle entrée
        if (actions.Count == 0 || actions[actions.Count - 1].actionType != newAction)
        {
            actions.Add(new ActionEntry(newAction));
        }
        else
        {
            // sinon, on incrémente le compteur de la dernière action
            actions[actions.Count - 1].count++;
        }
        UpdateUI();
    }

    public void ClearActions()
    {
        actions.Clear();
        UpdateUI();
    }

    public void ExecuteActions()
    {
        foreach (var actionEntry in actions)
        {
            for (int i = 0; i < actionEntry.count; i++)
            {
                player.AddAction(actionEntry.actionType);
            }
        }
        player.ExecuteActions();
        ClearActions();
    }

    void UpdateUI()
    {
        actionListText.text = "";
        foreach (var actionEntry in actions)
        {
            string actionName = "";

            switch (actionEntry.actionType)
            {
                case PlayerGridMovement.ActionType.Move:
                    actionName = "Avancer";
                    break;
                case PlayerGridMovement.ActionType.TurnRight:
                    actionName = "Tourner Droite";
                    break;
                case PlayerGridMovement.ActionType.TurnLeft:
                    actionName = "Tourner Gauche";
                    break;
            }

            actionListText.text += $"{actionName} x{actionEntry.count}\n";
        }
    }
}

