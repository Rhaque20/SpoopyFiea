using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DetectionNamespace;
public abstract class AbstractDetector : MonoBehaviour
{
    // Start is called before the first frame update
    protected DetectionController controller;
    public Vector3 detectorPosition;
    public DetectionType detectionType;
}
