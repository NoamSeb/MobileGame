using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class CodeblockMovement : MonoBehaviour
{
    [SerializeField, Range(0, 5000)] private int _instructionDelayMilliseconds;

    public float InstructionDelayMS { get { return  _instructionDelayMilliseconds; } }
    public float InstructionDelayS { get { return _instructionDelayMilliseconds/1000f; } }

    //Ce qu'on va envoyer pour donner la direction du joueur
    public enum MoveInstruction
    {
        None,
        Walk,
        Turn,
        Wait
    }

    //La file d'instructions
    private List<MoveInstruction> _instructions = new();

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
        _instructions.Add(MoveInstruction.Walk);
    }

    void Turn()
    {
        _instructions.Add(MoveInstruction.Turn);
    }

    void Wait()
    {
        _instructions.Add(MoveInstruction.Wait);
    }

    void Launch()
    {
        PlayInstructions();
    }

    async void PlayInstructions()
    {
        for (int i = 0; i < _instructions.Count; i++)
        {
            await(StartInstruction(i));
        }

        _instructions.Clear();
    }

    async Task StartInstruction(int i)
    {
        OnMoveInstructed?.Invoke(_instructions[i]);

        await Task.Delay(_instructionDelayMilliseconds);
    }
}