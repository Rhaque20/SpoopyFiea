using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollection : MonoBehaviour, IInteractable
{
    private int _collectableCount = 0;
    public int CollectableCount { get => _collectableCount; }
    public void Interact()
    {
        GameManager.CollectableObtained(transform.GetSiblingIndex());
        gameObject.SetActive(false);
    }
    private void OnCollected()
    {
        _collectableCount++;
    }
    // Start is called before the first frame update
    void Start()
    {
        Collectable.OnCollected += OnCollected;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Collectable.OnCollected -= OnCollected; ;
    }
}
