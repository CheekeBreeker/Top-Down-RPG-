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

    private Vector3 mousePosition;
    public float lookSpeed = 2f;

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

        LookTo();

        transform.position += new Vector3(-vertical, 0f, horizontal) * _speed * Time.deltaTime;
    }

    public void LookTo()
    {
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Vector3 difference = mousePosition - transform.position;
        //difference.Normalize();
        //float rotationY = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0f, 0f, rotationY);

        //Plane playerPlane = new Plane(Vector3.up, transform.position); 		
        //Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 		
        //float hitdist = 0;  		
        //if (playerPlane.Raycast (ray, out hitdist)) 
        //{ 			
        //    Vector3 targetPoint = ray.GetPoint (hitdist);  			
        //    Quaternion targetRotation = Quaternion.LookRotation (targetPoint - transform.position);  			
        //    transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, lookSpeed * Time.deltaTime); 		
        //}
    }
}
