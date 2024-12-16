using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSignal : SoundSignal
{
    // Start is called before the first frame update
    public float sprintRadius = 50f;
    public float crouchRadius = 0f;
    public float walkRadius = 10f;
    protected override void Start()
    {
        base.Start();
        PlayerMovement.sprintingEvent += UpdateRadiusToSprintRadius;
        PlayerMovement.crouchingEvent += UpdateRadiusToCrouchRadius;
    }

    void UpdateRadiusToSprintRadius(bool flag)
    {
        if (flag)
        {
            base.radius = sprintRadius;
            Debug.Log("Updating radius to sprint radius");
        }
        else
        {
            base.radius = walkRadius;
            Debug.Log("Reset from Sprint to walk radius");
        }
    }

    void UpdateRadiusToCrouchRadius(bool flag)
    {
        if (flag)
            base.radius = crouchRadius;
        else
            base.radius = walkRadius;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
