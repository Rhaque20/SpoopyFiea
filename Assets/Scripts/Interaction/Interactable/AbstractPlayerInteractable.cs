using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayerInteractable : MonoBehaviour, IInteractable
{
    public string promptMessage;
    public virtual void Interact()
    { }
}