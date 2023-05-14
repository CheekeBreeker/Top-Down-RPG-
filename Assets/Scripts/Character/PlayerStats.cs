using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerInventory _playerInventory;
    private LevelUpgrade _levelUpgrade;

    public CharacterStatus _characterStatus;
    public float _health;
    public float _maxHealth;
    public float _blockHP;
    public float _maxblockHP;
    public Image _imgHealth;
    public Image _imgBlock;
    public float _regenHP = 0.1f;


    public int _level;
    public float _exp;
    public float _curExp;
    public Image _imgExp;

    public bool _isOverload;
    public bool _isHurt;

    public float _plusDamage;
    public float _wendingFluidUseTime;

    [SerializeField] private Animator _animator;
    [SerializeField] private List<GameObject> _skillsList;
    [SerializeField] private GameObject _takeSkillButton;
    [SerializeField] private Sprite _skillImg;


    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInventory = GetComponent<PlayerInventory>();
        _levelUpgrade = GetComponent<LevelUpgrade>();
        _animator = GetComponent<Animator>();
        _characterStatus = _playerInventory._characterStatus;

        _exp = 100 * _level;
    }

    private void Update()
    {
        hpRegeneration();
        HPControl();
        SpeedControl();
        InterfaceUpdate();
        AttackSpeed();
        Block();
    }
    public void Block()
    {
        if (Input.GetMouseButtonDown(1)) AddBlockHP(10);
        else if (Input.GetMouseButtonUp(1)) AddBlockHP(-10);
    }
    public void AddHealth(float add)
    {
        _health += add;

        _health = Mathf.Clamp(_health, 0f, _maxHealth);
    }
    public void AddBlockHP(float add)
    {
        _blockHP += add;

        _blockHP = Mathf.Clamp(_blockHP, 0f, _maxblockHP);
    }

    public void AddMaxHealth(float add, float time)
    {
        StopCoroutine(TimeToStopAddMaxHP(add, time));
        StartCoroutine(TimeToStopAddMaxHP(add, time));
        AddHealth(add);
    }
    public void AddDamage(float add, float time)
    {
        StopCoroutine(TimeToStopAddDamage(add, time));
        StartCoroutine(TimeToStopAddDamage(add, time));
    }

    public void hpRegeneration()
    {
        if (GetComponent<LevelUpgrade>()._isHaveProletarianSkill)
        {
            _maxHealth = 125;

            if (_health < _maxHealth)
                _health += _regenHP * Time.deltaTime;
        }
    }

    public void WendingSkillhpRegeneration(float add, float time)
    {
        StopCoroutine(HpRegen(add, time));
        StartCoroutine(HpRegen(add, time));
    }

    public void BoostSpeed(float addMoveSpeed, float addAnimSpeed, float time)
    {
        StopCoroutine(TimeToBoostSpeed(addMoveSpeed, addAnimSpeed, time));
        StartCoroutine(TimeToBoostSpeed(addMoveSpeed, addAnimSpeed, time));
    }

    public void TakeAwayHealth(float damage)
    {
        if (_blockHP < 0)
        {
            _health -= damage;
            _animator.SetTrigger("impact");
            GetComponent<CharacterAnimation>()._interfaceAnim.SetTrigger("Damaged");

            if (_health < _maxHealth / 2)
                GetComponent<AudioManager>().PlayDamagedClip();

            if (_health < 0)
                Die();
        }
        else
        {
            _blockHP -= damage;
            GetComponent<AudioManager>().PlayDamagedClip();
        }
    }

    public void Die()
    {
        FadeAnimation fade = GetComponentInChildren<FadeAnimation>();
        fade._anim.SetBool("isDead", true);
        Time.timeScale = 0.0001f;
        fade._anim.speed = 10000f;
        GetComponentInChildren<AudioListener>().enabled = false;
        GetComponent<AudioManager>().enabled = false;
        SceneManager.LoadScene("Start");
    }

    public void HPControl()
    {
        if (_health > _maxHealth)
            _health = _maxHealth;
    }

    public void SpeedControl()
    {
        if (_playerInventory._weight > _playerInventory._maxWeight && !_isOverload)
        {
            _isOverload = true;
            _playerMovement._walkSpeed = _playerMovement._walkSpeed / 2;
            _playerMovement._sprintSpeed = _playerMovement._sprintSpeed / 2;
        }
        if (_playerInventory._weight <= _playerInventory._maxWeight && _isOverload)
        {
            _isOverload = false;
            _playerMovement._walkSpeed = _playerMovement._walkSpeed * 2;
            _playerMovement._sprintSpeed = _playerMovement._walkSpeed * 2;
        }

        if (_health <= _maxHealth * 0.35f && !_isHurt)
        {
            _isHurt = true;
            _playerMovement._walkSpeed = _playerMovement._walkSpeed / 2;
            _playerMovement._sprintSpeed = _playerMovement._walkSpeed / 2;
        }
        if (_health > _maxHealth * 0.35f && _isHurt)
        {
            _isHurt = false;
            _playerMovement._walkSpeed = _playerMovement._walkSpeed * 2;
            _playerMovement._sprintSpeed = _playerMovement._walkSpeed * 2;
        }
    }

    public void AttackSpeed()
    {
        if (_characterStatus.isAttackDamaging)
            _animator.speed = _playerInventory._weaponInHand.GetComponent<Item>()._attackSpeed;
        else _animator.speed = 1;
    }

    public void AddExp(float add)
    {
        _curExp += add;

        if (_curExp == _exp)
        {
            _level += 1;
            _exp = 100 * _level;
            _curExp = 0;
        }

        else if (_curExp > _exp)
        {
            _curExp -= _exp;
            _level += 1;
            _exp = 100 * _level;
        }
    }

    public void InterfaceUpdate()
    {
        _imgHealth.fillAmount = _health / _maxHealth;
        _imgBlock.fillAmount = _blockHP / 100;
        _imgExp.fillAmount = _curExp / _exp;
    }

    public IEnumerator HpRegen(float plusHP, float time)
    {
        while (plusHP > 0)
        {
            AddHealth(1);
            plusHP -= 1;
            yield return new WaitForSeconds(time);
            Debug.Log(plusHP);
        }
    }

    public IEnumerator TimeToStopAddMaxHP(float plusMaxHP, float time)
    {
        float mh = _maxHealth;
        while (time > 0)
        {
            time -= Time.deltaTime;

            _maxHealth = mh + plusMaxHP;
            yield return null;
            Debug.Log("MaxHPtime: " + time);
        }
        _maxHealth = mh;
        yield return null;
    }

    public IEnumerator TimeToStopAddDamage(float dmg, float time)
    {
        float pd = _plusDamage;
        while (time > 0)
        {
            time -= Time.deltaTime;

            _plusDamage = pd + dmg;
            yield return null;
            Debug.Log("PDtime: " + time);
        }
        _plusDamage = pd;
        yield return null;
    }

    public IEnumerator TimeToBoostSpeed(float plusMoveSpeed, float plusAnimSpeed, float time)
    {
        float moveSpeed = _playerMovement._walkSpeed;
        float animSpeed = _animator.speed;
        while (time > 0)
        {
            time -= Time.deltaTime;

            _playerMovement._walkSpeed = moveSpeed + plusMoveSpeed;
            _animator.speed = animSpeed + plusAnimSpeed;
            yield return null;
            Debug.Log("PDtime: " + time);
        }
        _playerMovement._walkSpeed = moveSpeed;
        _animator.speed = animSpeed;
        yield return null;
    }
}