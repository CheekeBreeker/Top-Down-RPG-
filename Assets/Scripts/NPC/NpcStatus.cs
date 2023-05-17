using UnityEngine;
public class NpcStatus : MonoBehaviour
{
    public int dollID;
    public NpcSaver _npcSaver;
    [Space]
    public bool isNormalDoll;
    public bool isBrokenDoll;
    public bool isBiomass;
    public bool isStartFreandlyDoll;
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
    public bool isPanic;
    [Space]
    [SerializeField] private float _panicTime;
    private float _actualPanicTime;
    private bool _panicTrigger;

    private void Start()
    {
        isCanLook = true;
        isCanMove = true;

        _panicTrigger = true;
        _actualPanicTime = _panicTime;
    }

    private void Update()
    {
        if (!isDead)
            PanicController();
    }

    private void PanicController()
    {
        if (isWounded && isWalk && _panicTime > 0)
        {
            _panicTime -= Time.deltaTime;
            if (_panicTrigger == true) isPanic = true;
            else isPanic = false;
        }
        else if (isWounded && isWalk && _panicTime < 0)
        {
            PanicTrigger();
            _panicTime = _actualPanicTime;
        }
        else if (!isWalk && _panicTime < 0)
        {
            _panicTime = _actualPanicTime;
        }
    }

    private void PanicTrigger()
    {
        if (_panicTrigger == true)
            _panicTrigger = false;
        else _panicTrigger = true;
    }
}
