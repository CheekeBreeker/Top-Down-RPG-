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
    public Transform _playerModel;

    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private CharacterStatus _characterStatus;

    [SerializeField] private float _walkSpeed = 1;
    [SerializeField] private float _sprintSpeed = 2;

    public float vertical;
    public float horizontal;
    private Vector3 moveInput;
    private Vector3 moveVelocity;

    private Vector3 mousePosition;
    public float lookHight = 10f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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
        else if (_characterStatus.isDodge)
        {
            DodgeMove();
        }
        else
        {
            NormalMove();
        }
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + moveVelocity * Time.fixedDeltaTime);
    }
    public void NormalMove()
    {
        LookToMousePosition();
        MoveToMousePosition();
    }

    public void SprintMove()
    {
        LookForward();
        MoveForward();
    }

    public void DodgeMove()
    {
        MoveForward();
        LookForward();
    }

    public void MoveToMousePosition()
    {
        moveInput = new Vector3(-vertical, 0f, horizontal);
        moveVelocity.z = Vector3.Dot(moveInput, transform.forward);
        moveVelocity.x = Vector3.Dot(moveInput, transform.right);
        moveVelocity = moveInput.normalized * _walkSpeed;
    }

    public void MoveForward()
    {
        transform.position += new Vector3(-vertical, 0f, horizontal) * _sprintSpeed * Time.deltaTime;
    }

    public void LookToMousePosition()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 difference = mousePosition - transform.position;
        difference.Normalize();
        float rotationY = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg + 90f;
        _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0f, rotationY, 0f), Time.deltaTime * 5f);
    }

    public void LookForward()
    {
        if (Input.GetKey(KeyCode.W))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 5f);
        if (Input.GetKey(KeyCode.S))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 180f, 0.0f), Time.deltaTime * 5f);
        if (Input.GetKey(KeyCode.D))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 90f, 0.0f), Time.deltaTime * 5f);
        if (Input.GetKey(KeyCode.A))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 270f, 0.0f), Time.deltaTime * 5f);
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 3150f, 0.0f), Time.deltaTime * 5f);
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 45f, 0.0f), Time.deltaTime * 5f);
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 225f, 0.0f), Time.deltaTime * 5f);
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 135f, 0.0f), Time.deltaTime * 5f);
    }
}
