using DetectionNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDetectionSystem : AbstractDetection
{
    // Start is called before the first frame update
    public float radius = 30f;

    public SoundDetectionSystem()
    {
        detectionType = DetectionNamespace.DetectionType.Sound;
        controller.AddDetectorToDetectionSystems += AddDetector;
        controller.AddSignalToDetectionSystems += AddSignal;
        controller.RemoveSignalFromDetectionSystems += RemoveSignal;
    }
    public override void AddDetector(AbstractDetector detector)
    {
        if (detector == null || detectors.Contains(detector))
        {
            return;
        }
        if (detector.detectionType == DetectionType.Sound)
        {
            if (detectionType == DetectionType.Sound)
            {
                detectors.Add(detector);
            }
        }
    }

    public override void AddSignal(AbstractSignal signal)
    {
        if (signal == null || signals.Contains(signal))
        { 
            return; 
        }
        if (signal.detectionType == DetectionType.Sound)
        {
            if (detectionType == DetectionType.Sound)
            {
                signals.Add(signal);
            }
        }
    }

    public void RemoveSignal(AbstractSignal signal)
    {
        if (signal.detectionType == DetectionType.Sound)
        {
            if (detectionType == DetectionType.Sound)
            {
                signals.Remove(signal);
            }
        }
    }

    public override void Detect()
    {
        for (int i = 0; i < detectors.Count; i++)
        {
            for (int j = 0; j < signals.Count; j++)
            {
                SoundDetector d = detectors[i] as SoundDetector;
                if (d != null)
                {
                    SoundSignal s = signals[j] as SoundSignal;
                    if (s != null)
                    {
                        //distance check
                        Transform detectorTransform = d.transform;
                        Transform signalTransform = s.transform;

                        //Assuming transforms can not be null. To-do : Enforce transform on the game objects
                        //Can they be? how do we handle that

                        float distance = Vector3.Distance(detectorTransform.position, signalTransform.position);
                        //Debug.Log("Distance between transforms : " + distance);

                        if (distance <= s.radius)
                        {
                            //succesfull detection
                            //Debug.Log("Successful Sound detection ");
                            //Send stimulus event to which the alertstatesystem has subscribed
                            IsHeardStimulus stim = new IsHeardStimulus(s.transform.position);
                            Distraction distraction;
                            s.gameObject.TryGetComponent<Distraction>(out distraction);
                            if (distraction != null)
                            {
                                stim.value = distraction.value;
                            }
                            DetectionController.ProcessStimulus(stim);
                        }
                    }
                }
            }
        }
    }
}
