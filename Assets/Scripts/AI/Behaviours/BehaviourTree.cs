using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviourTree : MonoBehaviour
{
    // Start is called before the first frame update
    public float attackSuspicionValue = 50f;
    public float investigateSuspicionValue = 20f;
    public enum BehaviourTreeState
    {
        Patrol,
        Investigate,
        Attack,
        None
    }

    public BehaviourTreeState currentBehaviour = BehaviourTreeState.None;
    private NavMeshAgent agent;
    private int currentPatrolPointIndex = 0;
    private bool isWaiting = false;
    private bool isAttacking = false;
    private bool isScanning = false;
    private bool scanComplete = false;
    private Coroutine scanCoroutine = null;
    private bool _canListen = false;
    private Concerns concerns;
    private Transform player;
    private bool _invokedGameOver = false;
    private Transform[] _patrolPoints;
    public float minWaitBetweenPlays = 1f;
    public float maxWaitBetweenPlays = 5f;
    public float waitTimeCountdown = -1f;
    public Transform patrolPointObject;
    public float waitTime = 2f;
    public float chaseSpeed = 2.5f;
    public float patrolSpeed = 1f;
    public float investigateSpeed = 1.5f;
    public float changePointRange = 1f;
    public float attackDuration = 10f;
    public List<AudioClip> audioClips;
    public AudioClip currentClip;
    public AudioSource source;
    public AudioClip alert;
    public AudioClip aggro;
    public AudioClip death;
    public AudioSource siren;
    [SerializeField] private float _listenRange = 5f;
    [SerializeField] private float _soundSusRate = 0.4f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        concerns = GetComponent<Concerns>();
        //suspicionState = SuspicionStateSystem.Instance;
        player = GameObject.FindWithTag("Player").transform;
        _patrolPoints = new Transform[patrolPointObject.childCount];

        for (int i = 0; i < patrolPointObject.childCount; i++)
        {
            _patrolPoints[i] = patrolPointObject.GetChild(i).transform;
        }
        source = GetComponent<AudioSource>();
    }
    void KillPlayer()
    {
        if (!_invokedGameOver)
            UIManager.gameOver?.Invoke();
        _invokedGameOver = true;
        siren.clip = death;
        siren.Play();
        Destroy (gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            Attack();
            return;
        }
        Concern highestConcern = concerns.GetHighestConcern();

        if (highestConcern != null)
        {
            if (highestConcern.suspicion > attackSuspicionValue)
            {
                Debug.Log("In Attack Behaviour sus :" + (float)highestConcern.suspicion);
                isAttacking = true;
                StartCoroutine(ResetAttackingStatus(attackDuration));
                Attack();
                return;
            }
            else if (highestConcern.suspicion > investigateSuspicionValue)
            {
                Debug.Log("In Investigate Behaviour sus :" + (float)highestConcern.suspicion);
                Investigate(highestConcern);
                return;
            }
        }
        //Patrol
        if (highestConcern == null)
        {
            Debug.Log("In Patrol Behaviour sus :" + 0);
        }
        else
        {
            Debug.Log("In Patrol Behaviour sus :" + (float)highestConcern.suspicion);
        }

        Patrol();

        
    }
    private void MoveToPoint(Vector3 position, float speed)
    {
        if (agent.SetDestination(position))
        {
            agent.speed = speed;
        }

        if (currentBehaviour == BehaviourTreeState.Patrol)
        {
            float waypointDistance = Vector3.Distance(transform.position, position);
            if (waypointDistance < changePointRange)
            {
                currentPatrolPointIndex = (currentPatrolPointIndex + 1) % _patrolPoints.Length;
            }
            else
            {
               // Debug.Log("Waypoint distance is " + waypointDistance);
            }
        }
    }
    void Attack()
    {
        if (currentBehaviour != BehaviourTreeState.Attack)
        {
            currentBehaviour = BehaviourTreeState.Attack;
            source.clip = aggro;
            source.Play();
        }
        MoveToPoint(player.position, chaseSpeed);
        if (Vector3.Distance(transform.position, player.position) < 2f)
        {
            Debug.Log("KILL PLAYER");
            agent.isStopped = true;
            KillPlayer();
        }

    }
    void Investigate(Concern concern)
    {
        if (currentBehaviour != BehaviourTreeState.Investigate)
        {
            currentBehaviour = BehaviourTreeState.Investigate;
            source.clip = alert;
            source.Play();
        }
       
        MoveToPoint(concern.position, investigateSpeed);
       // Debug.Log("The AI position in investigate is : " + transform.position);
        //Debug.Log("Last known Position in Investigate is : " + concern.position);

        if (!agent.hasPath && Vector3.Distance(player.position, transform.position) > 2f )
        {
            if (isScanning)
            {
                if (scanComplete)
                {
                    //scan complete then what??
                    //Remove concern
                    concerns.RemoveConcern(concern);
                    //Reset variables
                    scanComplete = false;
                    isScanning = false;
                }
            }
            else 
            {
                isScanning = true;
                scanCoroutine = StartCoroutine(Scan(10f));
            }
        }
        if (Vector3.Distance(transform.position, player.position) < 2f)
        {
            Debug.Log("KILL PLAYER");
            agent.isStopped = true;
            KillPlayer();
        }
    }
    void Patrol()
    {
        if (currentBehaviour != BehaviourTreeState.Patrol)
        {
            currentBehaviour = BehaviourTreeState.Patrol;
        }
        //Patrol
        MoveToPoint(_patrolPoints[currentPatrolPointIndex].position, patrolSpeed);
        if (!source.isPlaying)
        {
            if (waitTimeCountdown < 0f)
            {
                currentClip = audioClips[Random.Range(0, audioClips.Count)];
                source.clip = currentClip;
                source.Play();
                waitTimeCountdown = Random.Range(minWaitBetweenPlays, maxWaitBetweenPlays);
            }
            else
            {
                waitTimeCountdown -= Time.deltaTime;
            }
        }
    }
    void Cry()
    {
        source.Play();
    }

    IEnumerator ResetAttackingStatus(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    IEnumerator Scan(float duration)
    {
        float timeElapsed = 0f;
        float totalRotation = 0f;

        float rotationSpeed = 360f / duration;

        while (totalRotation < 360f && !agent.hasPath)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationStep, 0);
            totalRotation += rotationStep;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        scanComplete = true;
    }
}
