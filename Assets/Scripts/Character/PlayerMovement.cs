using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerInventory _playerInventory;
    public Transform _playerModel;

    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private CharacterStatus _characterStatus;

    [SerializeField] private float _walkSpeed = 1f;
    [SerializeField] private float _sprintSpeed = 2f;
    [SerializeField] public float _dodgeActiveTime = 1.15f;
    
    public float vertical;
    public float horizontal;
    private Vector3 moveInput;
    private Vector3 moveVelocity;

    private Vector3 mousePosition;

    public float _stopAttackTimer = 0.8f;
    public float _attackNumber;

    private float distance;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInventory = GetComponent<PlayerInventory>();
        _playerModel = gameObject.transform.Find("PlayerModel").transform;
    }

    public void MoveUpdate()
    {
        
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        if (_characterStatus.isSprint)
        {
            SprintMove();
        }
        if (_characterStatus.isDodge)
        {
            DodgeMove();
        }
        if (_characterStatus.isAttack)
        {
            ClickUpdate();
            NormalMove(_walkSpeed / 5);
        }
        if (_characterStatus.isUsing)
        { }
        if (_characterStatus.isNormal)
        {
            NormalMove(_walkSpeed);
            ClickUpdate();
        }

        _rb.MovePosition(_rb.position + moveVelocity * Time.deltaTime);

        _stopAttackTimer -= Time.deltaTime;

        if (_stopAttackTimer < 0)
        {
            _attackNumber = 0;
        }
    }

    public void ClickUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.tag == "Item")
                {
                    TakeItem(hit);
                }
                else
                {
                    if (_characterStatus.isAttack)
                    {
                        _stopAttackTimer = 0.8f;
                        _attackNumber += 1;
                        if (_attackNumber > 3)
                        {
                            _attackNumber = 0;
                        }
                        if (_stopAttackTimer < 0)
                            _attackNumber = 0;
                    }
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        _stopAttackTimer = 0.8f;
                        _attackNumber = 0;
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //_rb.MovePosition(_rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    //public void Attacking()
    //{
    //    if (_characterStatus.isAttack && Input.GetMouseButtonDown(0))
    //    {
    //        _stopAttackTimer = 0.8f;
    //        _attackNumber += 1;
    //        if (_attackNumber > 3)
    //        {
    //            _attackNumber = 0;
    //        }
    //        if (_stopAttackTimer < 0)
    //            _attackNumber = 0;
    //    }
    //    if(Input.GetKeyDown(KeyCode.R))
    //    {
    //        _stopAttackTimer = 0.8f;
    //        _attackNumber = 0;
    //    }
    //}

    IEnumerator StopAttackCor()
    {
        yield return new WaitForSeconds(_stopAttackTimer);
    }

    public void NormalMove(float walkSpeed)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 difference = mousePosition - transform.position;
        difference.Normalize();
        float rotationY = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg + 90f;
        _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0f, rotationY, 0f), Time.deltaTime * 5f);

        moveInput = new Vector3(-vertical, 0f, horizontal);
        moveVelocity.z = Vector3.Dot(moveInput, transform.forward);
        moveVelocity.x = Vector3.Dot(moveInput, transform.right);
        moveVelocity = moveInput.normalized * walkSpeed;
    }

    public void SprintMove()
    {
        LookForward(5f);

        transform.position += new Vector3(-vertical, 0f, horizontal) * _sprintSpeed * Time.deltaTime;
    }

    public void DodgeMove()
    {
        float dodgeVertical = 0f;
        float dodgeHorizontal = 0f;
        StartCoroutine(DodgeCor(dodgeVertical, dodgeHorizontal));
        transform.position += new Vector3(dodgeHorizontal, 0f, dodgeVertical) * _sprintSpeed * Time.deltaTime;
        StopCoroutine(DodgeCor(dodgeVertical, dodgeHorizontal));
    }

    IEnumerator DodgeCor(float dodgeVertical, float dodgeHorizontal)
    {
        //LookForward(15f);

        //dodgeVertical = -vertical;
        //dodgeHorizontal = horizontal;
        yield return new WaitForSeconds(_dodgeActiveTime);
    }

    public void MoveForward()
    {
        transform.position += new Vector3(-vertical, 0f, horizontal) * _sprintSpeed * Time.deltaTime;
    }

    public void LookForward(float rotationSpeed)
    {
        if (Input.GetKey(KeyCode.W))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * rotationSpeed);
        if (Input.GetKey(KeyCode.S))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 180f, 0.0f), Time.deltaTime * rotationSpeed);
        if (Input.GetKey(KeyCode.D))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 90f, 0.0f), Time.deltaTime * rotationSpeed);
        if (Input.GetKey(KeyCode.A))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 270f, 0.0f), Time.deltaTime * rotationSpeed);
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 3150f, 0.0f), Time.deltaTime * rotationSpeed);
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 45f, 0.0f), Time.deltaTime * rotationSpeed);
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 225f, 0.0f), Time.deltaTime * rotationSpeed);
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 135f, 0.0f), Time.deltaTime * rotationSpeed);
    }

    void TakeItem(RaycastHit hit)
    {
        distance = Vector3.Distance(transform.position + transform.up, hit.transform.position);
        Item it = hit.transform.GetComponent<Item>();

        if (distance < 2)
        {
            if (it.typeItem == "Consumables")
            {
                _playerInventory.consumables.Add(hit.transform.GetComponent<Item>());
                Destroy(hit.transform.gameObject);
            }
            else if (it.typeItem == "Weapon")
            {
                _playerInventory.weapon.Add(hit.transform.GetComponent<Item>());
                Destroy(hit.transform.gameObject);
            }
        }
        else
        {
            Debug.Log("Daleko");
        }
    }
}
