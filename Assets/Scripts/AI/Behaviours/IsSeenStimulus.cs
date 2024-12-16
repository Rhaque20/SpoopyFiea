using UnityEngine;

public class IsSeenStimulus : AbstractStimulus
{
    public IsSeenStimulus(Vector3 position, float value = 10f) : base()
    {
        base.value = value;
        base.position = position;
    }
}
