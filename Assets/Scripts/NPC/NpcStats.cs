using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStats : MonoBehaviour
{
    private Animator _anim;
    private Collider _collider;
    [SerializeField] private GameObject _spine;


    public float _health;

    public Transform[] RagdollElem;
    public Transform weapon;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
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
        _collider.enabled = false;
        _spine.SetActive(false);

        foreach (Transform body in RagdollElem)
            body.GetComponent<Rigidbody>().isKinematic = false;
        
    }
}
