using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    public void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public void Update()
    {
        _playerMovement.MoveUpdate();
    }
}
