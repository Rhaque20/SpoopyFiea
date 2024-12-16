using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DetectionNamespace;

[RequireComponent(typeof(Collider))]
public class VisualSignal : AbstractSignal
{
    public float innerRadius = 20f;
    public float outerRadius = 35f;
    public float perceptionAngleInDegree = 90;
    private void Start()
    {
        detectionType = DetectionType.Visual;
        controller = DetectionController.Instance;
        controller.RegisterSignal(this);
    }
}
