using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DetectionNamespace;

public class SoundDetector : AbstractDetector
{
    // Start is called before the first frame update
    void Start()
    {
        detectionType = DetectionType.Sound;
        detectorPosition = transform.position;
        controller = DetectionController.Instance;
        controller.RegisterDetector(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
