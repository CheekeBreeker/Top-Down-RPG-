using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Player/Status")]
public class CharacterStatus : ScriptableObject
{
    public bool isSprint;
    public bool isBlock;
    public bool isDodge;
    public bool isAiming;
    public bool isGround;
}
