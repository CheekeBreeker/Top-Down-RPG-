using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class NpcMovenment : MonoBehaviour
{
    [SerializeField] public FieldOfView _fieldOfView;
    [SerializeField] public FieldOfView _fieldOfHear;
    [SerializeField] private List<Transform> _wayPoints = new List<Transform>();
    [SerializeField] private Transform _lookPoint;
    [SerializeField] private Transform _curWayPointPos;
    [SerializeField] private Transform _panicWayPointPos;

    private NpcStatus _npcStatus;
    private NpcAnimation _npcAnimation;
    private NavMeshAgent _meshAgent;
    private NpcAudioManager _audioManager;
    private Vector3 _ActualcurWayPointPos;
    private int _curWayPoints;
    public bool _isWaiting;

    public float _walkSpeed;
    public float _fastAttackWalkSpeed;
    public float _acceleration;
    public float _actualAcceleration;
    public float _actualSpeed;
    public float _maxSpeed;
    public float _vertical;
    public float _horizontal;
    public float _rotateSpeed;
    private float _actualViewRadius;
    private float _actualViewAngle;

    public float _waitTime;
    public float _actualWaitTime;

    public float _distance;

    private void Start()
    {
        _meshAgent = GetComponent<NavMeshAgent>();
        _npcStatus = GetComponent<NpcStatus>();
        _npcAnimation = GetComponent<NpcAnimation>();
        _audioManager = GetComponent<NpcAudioManager>();

        _meshAgent.updateRotation = false;
        _walkSpeed = _actualSpeed;
        _maxSpeed = _actualSpeed * 2;
        _actualAcceleration = _acceleration;

        _actualViewRadius = _fieldOfView.viewRadius;
        _actualViewAngle = _fieldOfView.viewAngle;

        _actualWaitTime = _waitTime;
    }

    public void MoveUpdate()
    {

        if (!_npcAnimation.isAnimationHurtPlaying("Base Layer.Hurt") && _npcStatus.isCanMove)
        {
            if (!_npcStatus.isCanLook) 
                _meshAgent.speed = _fastAttackWalkSpeed;
            else _meshAgent.speed = _walkSpeed;
        }
        else _meshAgent.speed = 0;

        _horizontal = Mathf.Clamp(transform.localPosition.x, -1f, 1f); ;
        _vertical = Mathf.Clamp(transform.localPosition.z, -1f, 1f);

        if (_fieldOfView.visibleTargets.Count == 0 && !_curWayPointPos.gameObject.activeSelf || (_npcStatus.isFreandly && !_npcStatus.isFollow))
        {
            Patrol();
        }
        else
        {
            if (_npcStatus.isNormalDoll)
                MovenmentNormalDoll();
            else if (_npcStatus.isBrokenDoll)
                MovenmentBrokenDoll();
        }

        if (_npcStatus.isWounded)
        {
            _acceleration = _actualAcceleration / 2;
            _maxSpeed = _actualSpeed;
        }
    }

    private void Patrol()
    {
        Debug.Log("Patrol");
        _npcStatus.isPatroling = true;
        _npcStatus.isWalk = false;
        _npcStatus.isAttack = false;

        _fieldOfView.viewRadius = _actualViewRadius;
        _fieldOfView.viewAngle = _actualViewAngle;

        if (_wayPoints.Count == 1)
        {
            _meshAgent.SetDestination(_wayPoints[0].position);
            _walkSpeed = _actualSpeed;
            float distance = Vector3.Distance(transform.position, _wayPoints[0].position);

            if (distance > 1f)
            {
                if (_waitTime > 0)
                {
                    _waitTime -= Time.deltaTime;
                    _isWaiting = true;
                    if (!_npcStatus.isFreandly &&(_fieldOfView.visibleTargets.Count != 0 || _fieldOfHear.visibleTargets.Count != 0))
                        _waitTime = 0;
                }
                else _isWaiting = false;

                if (!_isWaiting)
                {
                    _walkSpeed += _acceleration * Time.deltaTime;
                    Looking();
                }
                else _walkSpeed = 0;

                if (_walkSpeed > _maxSpeed) _walkSpeed = _maxSpeed;
            }
            else
            {
                _waitTime = _actualWaitTime;
                _walkSpeed = 0;
                _isWaiting = true;

                Looking();
            }
        }
        else if (_wayPoints.Count > 1)
        {
            _meshAgent.SetDestination(_wayPoints[_curWayPoints].position);
            _walkSpeed = _actualSpeed;
            float distance = Vector3.Distance(transform.position, _wayPoints[_curWayPoints].position);

            if (distance > 2.5f)
            {
                _audioManager.PlayStartWalkClip();
                if (_waitTime > 0)
                {
                    _waitTime -= Time.deltaTime;
                    _isWaiting = true;
                    if (_fieldOfView.visibleTargets.Count != 0 || _fieldOfHear.visibleTargets.Count != 0)
                        _waitTime = 0;
                }
                else _isWaiting = false;

                if (!_isWaiting)
                {
                    _walkSpeed += _acceleration * Time.deltaTime;
                    Looking();
                }

                if (_walkSpeed > _maxSpeed) _walkSpeed = _maxSpeed;
            }
            else if (distance <= 2.5f && distance > 1f)
            {
                _audioManager.PlayStartWalkClip();
                Looking();
            }
            else
            {
                _audioManager.PlayStopWalkClip();
                _walkSpeed = 0;
                Looking();
                _curWayPoints++;
                if (_curWayPoints >= _wayPoints.Count)
                    _curWayPoints = 0;
            }
        }
        else
        {
            _meshAgent.speed = 0;
            _walkSpeed = 0;
            Looking();
        }
    }

    private void MovenmentNormalDoll()
    {
        Debug.Log("Movenment");
        _npcStatus.isWalk = true;
        _npcStatus.isPatroling = false;

        _fieldOfView.viewRadius = 100f;
        _fieldOfView.viewAngle = 240f;

        

        if ((_fieldOfView.visibleTargets.Count == 0 ||
            _fieldOfHear.visibleTargets.Count == 0) &&
            _curWayPointPos.position != _ActualcurWayPointPos)
        {
            _curWayPointPos.position = _ActualcurWayPointPos;
        }
        if (!_npcStatus.isPanic)
        {
            _meshAgent.SetDestination(_curWayPointPos.position);
            _fieldOfView.viewRadius = 100f;
            _fieldOfView.viewAngle = 300f;
        }
        else
        {
            _meshAgent.SetDestination(_panicWayPointPos.position);
            _fieldOfView.viewRadius = 100f;
            _fieldOfView.viewAngle = 300f;
        }

        float distance = Vector3.Distance(transform.position, _curWayPointPos.position);
        _distance = distance;

        if (distance > 1.75f)
        {
            _audioManager.PlayStartWalkClip();
            Looking();

            if (_fieldOfView.visibleTargets.Count == 0)
                foreach (Transform target in _fieldOfView.visibleTargets)
                {
                    _ActualcurWayPointPos = target.position;
                }

            _walkSpeed += _acceleration * Time.deltaTime;
            if (_walkSpeed > _maxSpeed) _walkSpeed = _maxSpeed;

            _waitTime = _actualWaitTime;
        }
        else
        {
            _audioManager.PlayStopWalkClip();
            if (_waitTime > 0)
            {
                _waitTime -= Time.deltaTime;
                _isWaiting = true;
                if (_fieldOfView.visibleTargets.Count != 0 || _fieldOfHear.visibleTargets.Count != 0)
                    _waitTime = 0;
            }
            else _isWaiting = false;

            if (!_isWaiting || _npcStatus.isPanic)
            {
                Looking();
            }
            if (!_isWaiting)
                _walkSpeed = 0;

            _curWayPointPos.gameObject.SetActive(false);
            if (!_npcStatus.isPanic)
                _panicWayPointPos.gameObject.SetActive(false);
        }

        if (distance > 2)
            _npcStatus.isAttack = false;
        else if (distance <= 2 && _fieldOfView.visibleTargets.Count != 0 
                && !_npcStatus.isFreandly && !_npcStatus.isPanic)
                _npcStatus.isAttack = true;
    }

    private void MovenmentBrokenDoll()
    {
        Debug.Log("Movenment");
        _npcStatus.isWalk = true;
        _npcStatus.isPatroling = false;

        _fieldOfView.viewRadius = 100f;
        _fieldOfView.viewAngle = 240f;

        if ((_fieldOfView.visibleTargets.Count == 0 ||
            _fieldOfHear.visibleTargets.Count == 0) &&
            _curWayPointPos.position != _ActualcurWayPointPos)
            _curWayPointPos.position = _ActualcurWayPointPos;

        _meshAgent.SetDestination(_curWayPointPos.position);
        float distance = Vector3.Distance(transform.position, _curWayPointPos.position);
        _distance = distance;

        if (distance > 1.5f)
        {
            _audioManager.PlayStartWalkClip();
            if (_npcStatus.isCanLook)
                Looking();

            if (_fieldOfView.visibleTargets.Count == 0)
                foreach (Transform target in _fieldOfView.visibleTargets)
                {
                    _ActualcurWayPointPos = target.position;
                }

            _walkSpeed += _acceleration * Time.deltaTime;
            if (_walkSpeed > _maxSpeed) _walkSpeed = _maxSpeed;

            _waitTime = _actualWaitTime;

            _npcStatus.isAttack = false;
        }
        else
        {
            _audioManager.PlayStopWalkClip();
            if (_waitTime > 0)
            {
                _waitTime -= Time.deltaTime;
                _isWaiting = true;
                if (_fieldOfView.visibleTargets.Count != 0 || _fieldOfHear.visibleTargets.Count != 0)
                    _waitTime = 0;
            }
            else _isWaiting = false;

            if (!_isWaiting)
            {
                Looking();
            }
            if (!_isWaiting)
                _walkSpeed = 0;

            _curWayPointPos.gameObject.SetActive(false);

            if (_fieldOfView.visibleTargets.Count != 0 && !_npcStatus.isFreandly && distance != 0)
                _npcStatus.isAttack = true;
        }
    }

    private void Looking()
    {
        // patrol
        if (_npcStatus.isPatroling && (_fieldOfView.visibleTargets.Count == 0 || _fieldOfHear.visibleTargets.Count == 0) && !_isWaiting && _wayPoints.Count != 0)
        {
            Vector3 direction = (_wayPoints[_curWayPoints].position - transform.position).normalized;
            float rotationY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, rotationY, 0f), Time.deltaTime * _rotateSpeed);
        }
        //move
        else if (_fieldOfView.visibleTargets.Count != 0 && (!_npcStatus.isFreandly || _npcStatus.isFollow))
        {
            _curWayPointPos.gameObject.SetActive(true);
            if (_npcStatus.isPanic)
                _panicWayPointPos.gameObject.SetActive(true);

            foreach (Transform target in _fieldOfView.visibleTargets)
            {
                _curWayPointPos.position = target.position;
                _panicWayPointPos.position = new Vector3(-target.position.x,
                    target.position.y, -target.position.z);
                target.GetComponent<PlayerMovement>()._characterStatus.isScream = true;
                _ActualcurWayPointPos = target.position;

                Vector3 direction = (target.position - transform.position).normalized;
                float rotationY;
                if (_npcStatus.isPanic && !_npcStatus.isFollow)
                    rotationY = Mathf.Atan2(-direction.x, -direction.z) * Mathf.Rad2Deg + 90f;
                else rotationY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90f;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, rotationY, 0f), Time.deltaTime * _rotateSpeed);
            }
        }
        //move
        else if (_fieldOfHear.visibleTargets.Count != 0 && (!_npcStatus.isFreandly || _npcStatus.isFollow))
        {
            foreach (Transform target in _fieldOfHear.visibleTargets)
            {
                if (target.GetComponent<PlayerMovement>()._characterStatus.isAttack
                    || target.GetComponent<PlayerMovement>()._characterStatus.isSprint
                    || target.GetComponent<PlayerMovement>()._characterStatus.isDodge
                    || target.GetComponent<PlayerMovement>()._characterStatus.isScream)
                {
                    _curWayPointPos.gameObject.SetActive(true);
                    if (_npcStatus.isPanic)
                        _panicWayPointPos.gameObject.SetActive(true);
                    _curWayPointPos.position = target.position;
                    _ActualcurWayPointPos = target.position;
                }
                if (_curWayPointPos.gameObject.activeSelf)
                {
                    Vector3 direction = (target.position - transform.position).normalized;
                    float rotationY;
                    if (_npcStatus.isPanic && !_npcStatus.isFollow)
                        rotationY = Mathf.Atan2(-direction.x, -direction.z) * Mathf.Rad2Deg + 90f;
                    else rotationY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90f;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, rotationY, 0f), Time.deltaTime * _rotateSpeed);
                }
            }
        }
        //patrol
        else if(_lookPoint != null && !_curWayPointPos.gameObject.activeSelf && _isWaiting)
        {
            Vector3 direction = (_lookPoint.position - transform.position).normalized;
            float rotationY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, rotationY, 0f), Time.deltaTime * _rotateSpeed);
        }
    }

    IEnumerator Retention()
    {
        _isWaiting = true;
        yield return new WaitForSeconds(_waitTime);
        _isWaiting = false;
    }
}
