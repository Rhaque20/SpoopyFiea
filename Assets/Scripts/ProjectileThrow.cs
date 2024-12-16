using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TrajectoryPredictor))]
public class ProjectileThrow : MonoBehaviour
{
    private TrajectoryPredictor _trajectoryPredictor;

    [SerializeField]
    private Rigidbody _objectToThrow;

    [SerializeField, Range(0.0f, 50.0f)]
    private float _force;

    [SerializeField]
    private Transform _startPosition;

    private Rigidbody _thrownObject;

    private Coroutine _thrownCoroutine;

    private PlayerInventory _playerInventory;

    private void Start()
    {
        _thrownObject = Instantiate(_objectToThrow, _startPosition.position, Quaternion.identity);
        _thrownObject.gameObject.SetActive(false);
        _playerInventory = GetComponent<PlayerInventory>();
    }

    void OnEnable()
    {
        _trajectoryPredictor = GetComponent<TrajectoryPredictor>();

        if (_startPosition == null)
            _startPosition = transform;
    }

    void Update()
    {
        Predict();
    }

    void Predict()
    {
        _trajectoryPredictor.PredictTrajectory(ProjectileData());
    }

    public void StartAiming()
    {
        if(_thrownCoroutine == null && _playerInventory.hasDistraction)
            _trajectoryPredictor.SetTrajectoryVisible(true);
    }

    private IEnumerator ProjectileTimer()
    {
        yield return new WaitForSeconds(3f);
        _thrownObject.gameObject.SetActive(false);
        _thrownCoroutine = null;
    }

    ProjectileProperties ProjectileData()
    {
        ProjectileProperties properties = new ProjectileProperties();
        Rigidbody r = _objectToThrow.GetComponent<Rigidbody>();

        properties.direction = _startPosition.forward;
        properties.initialPosition = _startPosition.position;
        properties.initialSpeed = _force;
        properties.mass = r.mass;
        properties.drag = r.drag;

        return properties;
    }

    public void ThrowObject(InputAction.CallbackContext ctx)
    {
        _trajectoryPredictor.SetTrajectoryVisible(false);

        if (_thrownCoroutine != null || !_playerInventory.hasDistraction)
            return;

        _thrownObject.transform.position = _startPosition.position;
        _thrownObject.gameObject.SetActive(true);
        _thrownObject.isKinematic = false;
        _thrownObject.AddForce(_startPosition.forward * _force, ForceMode.Impulse);
        _thrownCoroutine = StartCoroutine(ProjectileTimer());
        _playerInventory.DropItem();
    }
}