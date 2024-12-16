using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPredictor : MonoBehaviour
{
    #region Members
    private LineRenderer _trajectoryLine;
    [SerializeField, Tooltip("The marker will show where the projectile will hit")]
    private Transform _hitMarker;
    [SerializeField, Range(10, 100), Tooltip("The maximum number of points the LineRenderer can have")]
    private int _maxPoints = 50;
    [SerializeField, Range(0.01f, 0.5f), Tooltip("The time _increment used to calculate the trajectory")]
    private float _increment = 0.025f;
    [SerializeField, Range(1.05f, 2f), Tooltip("The raycast overlap between points in the trajectory, this is a multiplier of the length between points. 2 = twice as long")]
    private float _rayOverlap = 1.1f;
    #endregion

    private void Start()
    {
        if (_trajectoryLine == null)
            _trajectoryLine = GetComponent<LineRenderer>();

        SetTrajectoryVisible(false);
        _hitMarker.gameObject.SetActive(false);
    }

    public void PredictTrajectory(ProjectileProperties projectile)
    {
        Vector3 velocity = projectile.direction * (projectile.initialSpeed / projectile.mass);
        Vector3 position = projectile.initialPosition;
        Vector3 nextPosition;
        float overlap;

        UpdateLineRender(_maxPoints, (0, position));

        for (int i = 1; i < _maxPoints; i++)
        {
            velocity = CalculateNewVelocity(velocity, projectile.drag);
            nextPosition = position + velocity * _increment;

            overlap = Vector3.Distance(position, nextPosition) * _rayOverlap;

            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, overlap))
            {
                UpdateLineRender(i, (i - 1, hit.point));
                MoveHitMarker(hit);
                break;
            }

            position = nextPosition;
            UpdateLineRender(_maxPoints, (i, position));
        }
    }

    private void UpdateLineRender(int count, (int point, Vector3 pos) pointPos)
    {
        _trajectoryLine.positionCount = count;
        _trajectoryLine.SetPosition(pointPos.point, pointPos.pos);
    }

    private Vector3 CalculateNewVelocity(Vector3 velocity, float drag)
    {
        velocity += Physics.gravity * _increment;
        velocity *= Mathf.Clamp01(1f - drag * _increment);
        return velocity;
    }

    private void MoveHitMarker(RaycastHit hit)
    {
        //_hitMarker.gameObject.SetActive(true);

        float offset = 0.025f;
        _hitMarker.position = hit.point + hit.normal * offset;
        _hitMarker.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
    }

    public void SetTrajectoryVisible(bool visible)
    {
        _trajectoryLine.enabled = visible;
        _hitMarker.gameObject.SetActive(visible);
    }
}