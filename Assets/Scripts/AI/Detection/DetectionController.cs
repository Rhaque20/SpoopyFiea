using DetectionNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionController : MonoBehaviour
{
    public static Action<AbstractStimulus> ProcessStimulus;
    // Start is called before the first frame update
    private static DetectionController _instance ;
    public static DetectionController Instance 
    {
        get 
        {
            if (_instance == null)
            {
                Debug.LogWarning("Detection Controller instance is not available");
            }
            return _instance;
        }
    }

    [SerializeField]
    public List<AbstractDetection> detectionSystems;
    public Action RegisterDetectionSystems;
    public Action InvokeDetectionLogic;
    public Action<AbstractDetector> AddDetectorToDetectionSystems;
    public Action<AbstractSignal> AddSignalToDetectionSystems;
    public Action<AbstractSignal> RemoveSignalFromDetectionSystems;

    public void RegisterDetector(AbstractDetector detector)
    {
        AddDetectorToDetectionSystems(detector);   
    }
    public void RegisterSignal(AbstractSignal signal)
    {
        AddSignalToDetectionSystems(signal);  
    }

    public void UnregisterSignal(AbstractSignal signal)
    {
        RemoveSignalFromDetectionSystems(signal);
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
        VisualDetectionSystem visualDetection = new VisualDetectionSystem();
        SoundDetectionSystem soundDetection = new SoundDetectionSystem();
        detectionSystems = new List<AbstractDetection>();
        detectionSystems.Add(visualDetection);
        detectionSystems.Add(soundDetection);
    }
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        foreach (var detectionSystem in detectionSystems)
        {
            detectionSystem.Detect();
        }
    }
    void OnDestroy()
    {
        // Clean up any resources or references here if necessary
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
