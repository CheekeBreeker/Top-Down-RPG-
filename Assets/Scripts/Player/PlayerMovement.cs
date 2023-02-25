using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Animator _anim;
    private Transform _playerModel;

    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private CharacterStatus _characterStatus;

    [SerializeField] private float _speed = 1;

    private float vertical;
    private float horizontal;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _playerModel = gameObject.transform.Find("PlayerModel").transform;
    }

    public void MoveUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        _anim.SetFloat("vertical", vertical, 0.15f, Time.deltaTime);
        _anim.SetFloat("horizontal", horizontal, 0.15f, Time.deltaTime);

        transform.position += new Vector3(-vertical, 0f, horizontal) * _speed * Time.deltaTime;



        RotationNormal();
    }

    public void RotationNormal()
    {

    }
}
