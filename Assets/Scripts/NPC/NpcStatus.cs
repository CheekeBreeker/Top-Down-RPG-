using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStatus : MonoBehaviour
{
    public bool isNormalDoll;
    public bool isBrokenDoll;
    [Space]
    public bool isWalk;
    public bool isAttack;
    public bool isAttackDamage;
    public bool isCanLook;
    public bool isCanMove;
    public bool isHurt;
    public bool isWounded;
    public bool isPatroling;
    public bool isDead;
    public bool isFreandly;
    public bool isFollow;

    private void Start()
    {
        isCanLook = true;
        isCanMove = true;
    }
}
