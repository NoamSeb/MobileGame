using System;
using UnityEngine;

public class TestItem : IInteractable 
{
    protected override void Interaction()
    {
        Debug.Log("interaction effectuée !");
        base.Interaction();
    }
}
