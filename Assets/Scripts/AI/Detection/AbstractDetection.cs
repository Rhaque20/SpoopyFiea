using DetectionNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AbstractDetection
{
    // Start is called before the first frame update
    public  List<AbstractDetector> detectors;
    public  List<AbstractSignal> signals;
    public DetectionType detectionType;
    public DetectionController controller;
    public AbstractDetection()
    {
        controller = DetectionController.Instance;
        detectors = new List<AbstractDetector>();
        signals = new List<AbstractSignal>();
    }
    public abstract void Detect();
    public abstract void AddDetector(AbstractDetector detector);
    public abstract void AddSignal(AbstractSignal signal);
}
