
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private GameObject _leftDoor;
    [SerializeField] private GameObject _rightDoor;
    [SerializeField] private Vector3 _leftDoorClosePos;
    [SerializeField] private Vector3 _leftDoorOpenPos;
    [SerializeField] private Vector3 _rightDoorClosePos;
    [SerializeField] private Vector3 _rightDoorOpenPos;
    [Space, SerializeField] private float _speed;
    [SerializeField] private bool _isOpening;
    [SerializeField] private bool _isOpenable;

    private void Update()
    {
        if (_isOpening)
        {
            _leftDoor.transform.localPosition = Vector3.Lerp(_leftDoor.transform.localPosition, _leftDoorOpenPos, _speed * Time.deltaTime);
            _rightDoor.transform.localPosition = Vector3.Lerp(_rightDoor.transform.localPosition, _rightDoorOpenPos, _speed * Time.deltaTime);
        }
        else
        {
            _leftDoor.transform.localPosition = Vector3.Lerp(_leftDoor.transform.localPosition, _leftDoorClosePos, _speed * Time.deltaTime);
            _rightDoor.transform.localPosition = Vector3.Lerp(_rightDoor.transform.localPosition, _rightDoorClosePos, _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Untagged") && _isOpenable)
        {
            _isOpening = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Untagged"))
        {
            _isOpening = false;
        }
    }
}
