using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class NpcMovenment : MonoBehaviour
{
    [SerializeField] private FieldOfView _fieldOfView;
    [SerializeField] private FieldOfView _fieldOfHear;

    private NpcStatus _npcStatus;

    [SerializeField] private NavMeshAgent _meshAgent;
    [SerializeField] private List<Transform> _wayPoints = new List<Transform>();
    [SerializeField] private Transform _lookPoint;
    [SerializeField] private int _curWayPoint;

    public float _walkSpeed;
    public float _acceleration;
    public float _actualSpeed;
    public float _maxSpeed;
    public float _rotateSpeed;
    private float _actualViewRadius;
    private float _actualViewAngle;

    private void Start()
    {
        _meshAgent = GetComponent<NavMeshAgent>();
        _npcStatus = GetComponent<NpcStatus>();

        _meshAgent.updateRotation = false;
        _walkSpeed = _actualSpeed;
        _maxSpeed = _walkSpeed * 2;

        _actualViewRadius = _fieldOfView.viewRadius;
        _actualViewAngle = _fieldOfView.viewAngle;
    }

    public void MoveUpdate()
    {
        _meshAgent.speed = _walkSpeed;
        if (_wayPoints.Count != 0)
        {
            if (_fieldOfView.visibleTargets.Count == 0)
                Patrol();
            else
                Movenment();
        }
        else Stand();

    }

    private void Patrol()
    {
        _npcStatus.isPatroling = true;
        _npcStatus.isWalk = false;

        _fieldOfView.viewRadius = _actualViewRadius;
        _fieldOfView.viewAngle = _actualViewAngle;

        if (_wayPoints.Count == 1)
        {
            _meshAgent.SetDestination(_wayPoints[0].position);
            float distance = Vector3.Distance(transform.position, _wayPoints[0].position);

            if (distance > 1f)
            {
                Looking(_wayPoints[0].position);
                _walkSpeed += _acceleration * Time.deltaTime;
                if (_walkSpeed > _maxSpeed) _walkSpeed = _maxSpeed;
            }
            else
            {
                _walkSpeed = 0;
                Looking(_lookPoint.position);
            }
        }
        else if (_wayPoints.Count > 1)
        {
            _meshAgent.SetDestination(_wayPoints[_curWayPoint].position);
            float distance = Vector3.Distance(transform.position, _wayPoints[_curWayPoint].position);

            if (distance > 2.5f)
            {
                Looking(_wayPoints[_curWayPoint].position);
                _walkSpeed += _acceleration * Time.deltaTime;
                if (_walkSpeed > _maxSpeed) _walkSpeed = _maxSpeed;
            }
            else if (distance <= 2.5f && distance > 1f)
            {
                Looking(_wayPoints[_curWayPoint].position);
            }
            else
            {
                Looking(_wayPoints[_curWayPoint].position);
                _curWayPoint++;
                if (_curWayPoint >= _wayPoints.Count)
                    _curWayPoint = 0;
            }
        }
    }

    private void Stand()
    {
        _walkSpeed = 0f;
        _fieldOfView.viewRadius = _actualViewRadius;
        _fieldOfView.viewAngle = _actualViewAngle;

        Looking(default);
    }

    private void Movenment()
    {
        _npcStatus.isWalk = true;
        _npcStatus.isPatroling = false;

        _fieldOfView.viewRadius = 100f;
        _fieldOfView.viewAngle = 240f;

        foreach (Transform target in _fieldOfView.visibleTargets)
        {
            _meshAgent.SetDestination(target.position);
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance > 2f)
            {
                Looking(default);
                _walkSpeed += _acceleration * Time.deltaTime;
                if (_walkSpeed > _maxSpeed) _walkSpeed = _maxSpeed;
            }
            else
            {
                _walkSpeed = 0;
                Looking(default);
            }
        }
    }

    private void Looking(Vector3 lookTarget)
    {
        if (_npcStatus.isPatroling && _fieldOfView.visibleTargets.Count == 0 && _fieldOfHear.visibleTargets.Count == 0)
        {
            Vector3 direction = (lookTarget - transform.position).normalized;
            float rotationY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, rotationY, 0f), Time.deltaTime * _rotateSpeed);
        }
        if (_fieldOfView.visibleTargets.Count != 0)
        {
            foreach (Transform target in _fieldOfView.visibleTargets)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                float rotationY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90f;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, rotationY, 0f), Time.deltaTime * _rotateSpeed);
            }
        }
        if (_fieldOfHear.visibleTargets.Count != 0)
        {
            foreach (Transform target in _fieldOfHear.visibleTargets)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                float rotationY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90f;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, rotationY, 0f), Time.deltaTime * _rotateSpeed);
            }
        }
    }
}
