using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractable : AbstractPlayerInteractable
{
    public override void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}