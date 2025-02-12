using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ActionQueue : MonoBehaviour
{
    private PlayerGridMovement _player;
    public TMP_Text actionListText; // référence à l'affichage des actions
    readonly private List<ActionEntry> actions = new(); // liste des actions avec compteurs

    private void Start()
    {
        _player = GameManager.Instance.PlayerScript;
    }

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

    public void AddWait()
    {
        AddAction(PlayerGridMovement.ActionType.Wait);
        UpdateUI();
    }

    private void AddAction(PlayerGridMovement.ActionType newAction)
    {
        // si la liste est vide ou si la dernière action est différente, on ajoute une nouvelle entrée
        if (actions.Count == 0 || actions[^1].actionType != newAction)
        {
            actions.Add(new ActionEntry(newAction));
        }
        else
        {
            // sinon, on incrémente le compteur de la dernière action
            actions[^1].count++;
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
                _player.AddAction(actionEntry.actionType);
            }
        }
        _player.ExecuteActions();
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
                case PlayerGridMovement.ActionType.Wait:
                    actionName = "Attendre";
                    break;
            }

            actionListText.text += $"{actionName} x{actionEntry.count}\n";
        }
    }
}

