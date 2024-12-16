using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionPlacement : MonoBehaviour
{
    [SerializeField] GameObject _distractionPrefab;
    GameObject _distractionInstance;
    Coroutine _objectDuration = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator ObjectDuration()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(_distractionInstance);
        _objectDuration = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            if (_objectDuration == null)
            {
                _distractionInstance = Instantiate(_distractionPrefab,transform.position + new Vector3(0.1f,1f, 0.1f),Quaternion.identity);
                _objectDuration = StartCoroutine(ObjectDuration());
                Debug.Log("Placed down bean");
            }
            else
                Debug.Log("Couldn't place down bean");
        }
    }
}
