using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSense : MonoBehaviour
{
    SphereCollider _detectionCollider;
    // Eventually replace this with a delegate
    MonsterNav _monsterNav;
    
    // Start is called before the first frame update
    void Start()
    {
        _detectionCollider = GetComponent<SphereCollider>();
        _monsterNav = GetComponent<MonsterNav>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Updating position to go for " + col.name);
            _monsterNav.UpdateTarget(col.transform);
        }
        else
        {
            Debug.Log(col.name+" entered the radius");
        }
    }
}
