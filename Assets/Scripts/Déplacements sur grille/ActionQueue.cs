using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using UnityEditor.Experimental.GraphView;

public class ActionQueue : MonoBehaviour
{
    private PlayerGridMovement _player;
    public TMP_Text actionListText; // r�f�rence � l'affichage des actions
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
            this.count = 1; // par d�faut, une action est ajout�e une fois
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
        // si la liste est vide ou si la derni�re action est diff�rente, on ajoute une nouvelle entr�e
        if (actions.Count == 0 || actions[^1].actionType != newAction)
        {
            actions.Add(new ActionEntry(newAction));
        }
        else
        {
            // sinon, on incr�mente le compteur de la derni�re action
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

            actionListText.text += $">>> {actionName} x{actionEntry.count}\n";
        }
    }

    readonly private List<GameObject> _previsItems = new();
    [SerializeField] private GameObject _previsDot;
    [SerializeField, Layer] int _playzoneLayer;
    [SerializeField, Range(1, 10)] int _maxPrevisAmount;

    void DrawPrevisualisation()
    {
        ClearPrevisualisation();

        Vector3 currentPos = GameManager.Instance.PlayerScript.transform.position;
        int currentRot = GameManager.Instance.PlayerScript.CurrentRotation;
        int currentPrevisAmount = 0;

        foreach (var action in _actions)
        {
            if (action.actionType == PlayerGridMovement.ActionType.Move)
            {
                var nextPos = currentRot switch
                {
                    0 => Vector3.up,
                    90 => Vector3.right,
                    180 => Vector3.down,
                    270 => Vector3.left,
                    _ => throw new Exception("The player's rotation isn't correct"),
                };

                for (int i = 0; i < action.count; i++)
                {
                    bool hasHitTilemap = false, hasHitTeleporter = false, hasHitPusher = false, hasHitLocker = false;

                    Vector3 possibleNextPos = Vector3.zero;
                    Vector3 possibleAdditionalMove = Vector3.zero;
                    int possibleNextRot = 0;

                    currentPos += nextPos;
                    Collider2D[] colliders = Physics2D.OverlapPointAll(currentPos);
                    foreach (var collider in colliders)
                    {
                        if (collider.gameObject.layer == _playzoneLayer)
                        {
                            hasHitTilemap = true;
                        }
                        if (collider.gameObject.TryGetComponent(out GridTeleporter teleport))
                        {
                            possibleNextPos = teleport.OtherTeleporter.transform.position;
                            hasHitTeleporter = true;
                        }
                        if (collider.gameObject.TryGetComponent(out GridPusher push))
                        {
                            possibleAdditionalMove = push.PushVector;
                            hasHitPusher = true;
                        }
                        if (collider.gameObject.TryGetComponent(out GridRotationLocker rotate))
                        {
                            possibleNextRot = rotate.Rotation;
                            hasHitLocker = true;
                        }
                    }

                    if (hasHitTilemap && currentPrevisAmount < _maxPrevisAmount)
                    {
                        if (hasHitTeleporter && !hasHitPusher && !hasHitLocker)
                        {
                            _previsItems.Add(Instantiate(_previsDot, currentPos, Quaternion.identity));
                            _previsItems.Add(Instantiate(_previsDot, possibleNextPos, Quaternion.identity));
                            currentPos = possibleNextPos;
                            currentPrevisAmount++;
                        }
                        if (!hasHitTeleporter && hasHitPusher && !hasHitLocker)
                        {
                            _previsItems.Add(Instantiate(_previsDot, currentPos, Quaternion.identity));
                            currentPos += possibleAdditionalMove;
                            _previsItems.Add(Instantiate(_previsDot, currentPos, Quaternion.identity));
                            currentPrevisAmount++;
                        }
                        if (!hasHitTeleporter && !hasHitPusher && hasHitLocker)
                        {
                            _previsItems.Add(Instantiate(_previsDot, currentPos, Quaternion.identity));
                            currentRot = possibleNextRot;
                            currentPrevisAmount++;
                        }
                        else
                        {
                            _previsItems.Add(Instantiate(_previsDot, currentPos, Quaternion.identity));
                            currentPrevisAmount++;
                        }
                    }
                }
            }
            if (action.actionType == PlayerGridMovement.ActionType.TurnRight)
            {
                currentRot = (currentRot + 90) % 360;
            }
            if (action.actionType == PlayerGridMovement.ActionType.TurnLeft)
            {
                currentRot = (currentRot - 90 + 360) % 360;
            }
        }
    }

    void ClearPrevisualisation()
    {
        foreach (GameObject obj in _previsItems)
        {
            Destroy(obj);
        }
        _previsItems.Clear();
    }
}

