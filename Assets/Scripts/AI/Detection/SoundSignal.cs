using DetectionNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class SoundSignal : AbstractSignal
{
    public float radius = 30f;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        detectionType = DetectionType.Sound;
        controller = DetectionController.Instance;
        controller.RegisterSignal(this);
    }

    private void OnDisable()
    {
        if (controller == null)
        {
            controller = DetectionController.Instance;
        }
        if(controller)
        controller.UnregisterSignal(this);
    }

    private void OnEnable()
    {
        if (controller == null)
        {
            controller = DetectionController.Instance;
        }
        if(controller)
        controller.RegisterSignal(this);
    }
}
