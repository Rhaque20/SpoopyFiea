using DetectionNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VisualDetector : AbstractDetector
{
    // Start is called before the first frame update
    private Camera cam;
    void Start()
    {
        detectionType = DetectionType.Visual;
        if (CompareTag("AI"))
        {
            detectorPosition = transform.position;
            Collider collider = GetComponent<Collider>();
            if (collider is CapsuleCollider capsule)
            {
                detectorPosition = transform.position + (Vector3.up * capsule.height / 2);
            }
        }
        else
        {
            PlayerLook look = GetComponent<PlayerLook>();
            cam = look.cam;
            //storing global position of camera of the detector
            detectorPosition = cam.transform.position;
        }
        controller = DetectionController.Instance;
        controller.RegisterDetector(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
