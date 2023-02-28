using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public CharacterStatus _characterStatus;

    private bool isSprint;
    private bool isBlock;
    private bool isDodge;
    private bool isAiming;

    private bool debagSprint;
    private bool debagBlock;
    private bool debagDodge;
    private bool debagAiming;


    public void InputUpdate()
    {
        if (!debagSprint)
        {
            _characterStatus.isSprint = Input.GetKey(KeyCode.LeftShift);
        }
        else _characterStatus.isSprint = isSprint;
        if (!debagBlock) _characterStatus.isBlock = Input.GetMouseButtonDown(1);
        else _characterStatus.isBlock = isBlock;
        if (!debagDodge) _characterStatus.isDodge = Input.GetKeyDown(KeyCode.Space);
        else _characterStatus.isDodge = isDodge;
        if (!debagAiming) _characterStatus.isAiming = Input.GetMouseButtonDown(2);
        else _characterStatus.isAiming = isAiming;
    }
}
