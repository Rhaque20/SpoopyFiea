using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distraction : MonoBehaviour
{
    Collider _collider;
    Rigidbody _rb;
    public float value;
    /*private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("AI"))
        {
            Debug.Log("Lured enemy to go after " + gameObject.name);
            other.GetComponent<MonsterNav>().UpdateTarget(transform);
            // Shaurya put your monster AI thingy in herey
        }
    }*/

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with "+collision.gameObject.name);
        _rb.isKinematic = true;
    }
}
