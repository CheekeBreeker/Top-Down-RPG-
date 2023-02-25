using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private CharacterStatus _characterStatus;

    private bool isSprint;
    private bool isBlock;
    private bool isDodge;
    private bool isAiming;

    private void Awake()
    {
        _characterStatus = GetComponent<CharacterStatus>();
    }

    void Update()
    {
        _characterStatus.isSprint = Input.GetKeyDown(KeyCode.LeftShift);
        _characterStatus.isBlock = Input.GetMouseButtonDown(1);
        _characterStatus.isAiming = Input.GetMouseButtonDown(2);
        _characterStatus.isDodge = Input.GetKeyDown(KeyCode.Space);
    }
}
