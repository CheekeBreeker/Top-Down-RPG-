using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAnimation : MonoBehaviour
{
    private NpcStatus _npcStatus;
    private NpcMovenment _npcMovenment;
    private Animator _anim;

    private void Start()
    {
        _npcStatus = GetComponent<NpcStatus>();
        _npcMovenment = GetComponent<NpcMovenment>();
        _anim = GetComponent<Animator>();
    }

    public void AnimationUpdate()
    {
        _anim.SetBool("hurt", _npcStatus.isHurt);
        _anim.SetFloat("speed", _npcMovenment._walkSpeed);

        if (_npcStatus.isHurt) AnimationHurt();
        if (!_npcStatus.isWounded) AnimationHurt();
    }

    private void AnimationHurt()
    {
        _npcStatus.isHurt = false;
        _anim.speed = 0.5f;
    }

    private void AnimationHealthy()
    {
        _anim.speed = 1f;
    }
}
