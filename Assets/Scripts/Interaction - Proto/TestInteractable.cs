using System;
using UnityEngine;

public class TestInteractable : Interactable 
{
    protected override void Interaction()
    {
        Debug.Log("interaction effectu�e !");
        base.Interaction();
    }
}
