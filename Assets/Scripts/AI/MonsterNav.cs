using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterNav : MonoBehaviour
{
    [SerializeField]private Transform _playerPosition;
    NavMeshAgent _agent;
    bool invokedGameover = false;
    public void UpdateTarget(Transform playerTransform)
    {
        _playerPosition = playerTransform;
    }
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !invokedGameover)
            KillPlayer();
    }

    // To be moved to something Shaurya does
    void KillPlayer()
    {
        UIManager.gameOver?.Invoke();
        invokedGameover = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerPosition != null)
        {
            _agent.SetDestination( _playerPosition.position );
        }
    }
}
