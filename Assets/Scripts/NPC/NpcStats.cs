using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStats : MonoBehaviour
{
    private Animator _anim;

    public float _health;

    public Transform[] RagdollElem;
    public Transform weapon;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void TakeAwayHealth(float takeAway)
    {
        _health -= takeAway;

        if (_health <= 0)
            Die();
    }

    public void Die()
    {
        _anim.enabled = false;

        foreach (Transform body in RagdollElem)
            body.GetComponent<Rigidbody>().isKinematic = false;
    }
}
