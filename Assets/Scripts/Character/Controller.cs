using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private CharacterAnimation _characterAnimation;
    private PlayerInput _playerInput;

    public void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _characterAnimation = GetComponent<CharacterAnimation>();
        _playerInput = GetComponent<PlayerInput>();
    }

    public void Update()
    {
        _characterAnimation.AnimationUpdate();
        _playerInput.InputUpdate();
    }

    private void FixedUpdate()
    {
        _playerMovement.MoveUpdate();
    }
}
