using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIInstantiator : MonoBehaviour
{
    [SerializeField] GameObject _UIPrefab;

    private void Awake()
    {
        GameObject g = Instantiate(_UIPrefab);
        g.transform.SetParent(transform);
        g.transform.localPosition = transform.GetChild(1).transform.position;
    }
}
