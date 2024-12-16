using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Collectable : MonoBehaviour
{
    public static Action OnCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!(OnCollected is null))
            {
                OnCollected();
            }

            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
