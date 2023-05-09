using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Player/Status")]
public class CharacterStatus : ScriptableObject
{
    public bool isNormal;
    public bool isSprint;
    public bool isBlock;
    public bool isDodge;
    public bool isAiming;
    public bool isAttack;
    public bool isAttackDamaging;
    public bool isUsing;
    public bool isTalk;
    public bool isTrade;
    public bool isScream;
}