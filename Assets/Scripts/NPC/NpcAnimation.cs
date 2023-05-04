using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

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
        _anim.SetBool("attack", _npcStatus.isAttack);
        _anim.SetFloat("speed", _npcMovenment._walkSpeed);
        if (_npcStatus.isBrokenDoll) _anim.SetFloat("distance", _npcMovenment._distance);

        if (_npcStatus.isHurt) AnimationHurt();

        Debug.Log(_anim.speed);
    }

    public void WalkControllTrue_AnimEvent()
    {
        _npcStatus.isAttackDamage = true;
    }
    public void WalkControllFalse_AnimEvent()
    {
        _npcStatus.isAttackDamage = true;
    }
    public void AttackControllTrue_AnimEvent()
    {
        _npcStatus.isAttackDamage = true;
    }

    public void AttackControllFalls_AnimEvent()
    {
        _npcStatus.isAttackDamage = false;
    }
    public void LookControllTrue_AnimEvent()
    {
        _npcStatus.isCanLook = true;
    }

    public void LookControllFalls_AnimEvent()
    {
        _npcStatus.isCanLook = false;
    }
    public void MoveControllTrue_AnimEvent()
    {
        _npcStatus.isCanMove = true;
    }

    public void MoveControllFalls_AnimEvent()
    {
        _npcStatus.isCanMove = false;
    }

    public bool isAnimationHurtPlaying(string animation)
    {
        var animStateInfo = _anim.GetCurrentAnimatorStateInfo(0);
        if (animStateInfo.IsName("Hurt"))
            return true;
        else return false;
    }

    private void AnimationHurt()
    {
        _anim.speed = 0.5f;
    }

    private void AnimationHealthy()
    {
        _anim.speed = 1f;
    }
}
