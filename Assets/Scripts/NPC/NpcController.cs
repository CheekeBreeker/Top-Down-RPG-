using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    private NpcAnimation _npcAnimation;
    private NpcMovenment _npcMovenment;
    private NpcStatus _npcStatus;

    private void Start()
    {
        _npcAnimation = GetComponent<NpcAnimation>();
        _npcMovenment = GetComponent<NpcMovenment>();
        _npcStatus = GetComponent<NpcStatus>();
    }

    private void Update()
    {
        _npcAnimation.AnimationUpdate();
        _npcMovenment.MoveUpdate();
        _npcStatus.StatusUpdate();
    }
}
