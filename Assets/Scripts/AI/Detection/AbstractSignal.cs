using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DetectionNamespace;
public abstract class AbstractSignal: MonoBehaviour
{
    protected DetectionController controller;
    public Vector3 signalPosition;
    public DetectionType detectionType;
}
