using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Controller : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private CharacterAnimation _characterAnimation;
    private PlayerInput _playerInput;

    [SerializeField] private float _screamTime = 1f;

    public void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _characterAnimation = GetComponent<CharacterAnimation>();
        _playerInput = GetComponent<PlayerInput>();
        Cursor.visible = false;
    }

    public void Update()
    {
        _characterAnimation.AnimationUpdate();
        _playerInput.InputUpdate();

        if (_playerMovement._characterStatus.isScream)
        {
            if (_screamTime > 0) _screamTime -= Time.deltaTime;
            else
            {
                _playerMovement._characterStatus.isScream = false;
                _screamTime = 1;
            }
        }
    }

    private void FixedUpdate()
    {
        _playerMovement.MoveUpdate();
    }
}
