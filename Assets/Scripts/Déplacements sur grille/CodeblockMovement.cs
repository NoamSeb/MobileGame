using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CodeblockMovement : MonoBehaviour
{
    //Ce qu'on va envoyer pour donner la direction du joueur
    public enum MoveInstruction
    {
        None,
        Walk,
        Turn,
        Wait,
        Launch
    }

    public static event Action<MoveInstruction> OnMoveInstructed;

    public void Action(string actionType)
    {
        switch(actionType)
        {
            case "Walk":
                Walk(); break;
            case "Turn":
                Turn(); break;
            case "Wait":
                Wait(); break;
            case "Launch":
                Launch(); break;
            default:
                throw new Exception("Please assign a correct action type");
        }
    }

    void Walk()
    {
        OnMoveInstructed?.Invoke(MoveInstruction.Walk);
    }

    void Turn()
    {
        OnMoveInstructed?.Invoke(MoveInstruction.Turn);
    }

    void Wait()
    {
        OnMoveInstructed?.Invoke(MoveInstruction.Wait);
    }

    void Launch()
    {
        OnMoveInstructed?.Invoke(MoveInstruction.Launch);
    }
}