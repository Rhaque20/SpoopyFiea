using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastKnownPosition : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 _position;

    private void UpdateLKP(AbstractStimulus stim)
    {
        _position = stim.position;
    }
    void Start()
    {
        DetectionController.ProcessStimulus += UpdateLKP;
        _position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
