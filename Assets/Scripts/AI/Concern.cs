using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Concern : MonoBehaviour
{
    public LKP position;
    public SuspicionState suspicion;
    public float decayFactor = 2f;

    private Concerns concerns; // Concern object should be able to unregister itself from the Concerns component
    // Start is called before the first frame update
    void Start()
    {
        concerns = GetComponent<Concerns>();
    }

    // Update is called once per frame
    void Update()
    {
        suspicion -= decayFactor * Time.deltaTime;

        if (suspicion <= 0)
        {
            concerns.RemoveConcern(this);
        }
    }
}
