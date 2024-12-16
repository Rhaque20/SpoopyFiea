using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsHeardStimulus : AbstractStimulus
{
    public IsHeardStimulus(Vector3 position, float value = 8f) : base()
    {
        base.value = value;
        base.position = position;
    }
}
