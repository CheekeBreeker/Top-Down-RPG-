using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilation : MonoBehaviour
{
    [SerializeField] private Transform _firstDot;
    [SerializeField] private Transform _secondDot;
    [SerializeField] private GameObject _rim;
    [SerializeField] private Transform _player;
    public int _dotNumber;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
            _rim.SetActive(true);
        else _rim.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            _player = other.GetComponent<Transform>();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && other.transform.CompareTag("Player"))
        {
            if (_dotNumber == 1)
            {
                _player.transform.position = _firstDot.position;
                _dotNumber = 2;
            }
            else
            {
                _player.transform.position = _secondDot.position;
                _dotNumber = 1;
            }
        }
    }
}
