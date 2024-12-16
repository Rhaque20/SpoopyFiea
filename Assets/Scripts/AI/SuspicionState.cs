using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspicionState
{
    public float value;
    public SuspicionState() { }
    public SuspicionState(float value)
    {
        this.value = value;
    }

    // Implicit conversion from SuspicionState to int
    public static implicit operator SuspicionState(float value)
    {
        return new SuspicionState(value);
    }

    // Implicit conversion from int to SuspicionState
    public static implicit operator float(SuspicionState state)
    {
        return state.value;
    }
}
