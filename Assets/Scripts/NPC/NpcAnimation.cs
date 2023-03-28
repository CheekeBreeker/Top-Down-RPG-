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
        _anim.SetBool("attack", _npcStatus.isAttack);
        _anim.SetFloat("speed", _npcMovenment._walkSpeed);

        _anim.SetFloat("vertical", Vector3.Dot(new Vector3(_npcMovenment._horizontal, 0f, _npcMovenment._vertical),
            _npcMovenment.transform.forward), 0.15f, Time.deltaTime);
        _anim.SetFloat("horizontal", Vector3.Dot(new Vector3(_npcMovenment._horizontal, 0f, _npcMovenment._vertical),
            _npcMovenment.transform.right), 0.15f, Time.deltaTime);

        if (_npcStatus.isHurt) AnimationHurt();
        if (_npcStatus.isWounded) AnimationHurt();

        Debug.Log(_anim.speed);
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
        _npcStatus.isHurt = false;
        _anim.speed = 0.5f;
    }

    private void AnimationHealthy()
    {
        _anim.speed = 1f;
    }
}
