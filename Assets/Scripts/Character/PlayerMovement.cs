using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    public InterfaceManager _interfaceManager;
    public PlayerInventory _playerInventory;
    public Transform _playerModel;

    [SerializeField] public CharacterStatus _characterStatus;
    [SerializeField] private Dialog _dialog;
    [SerializeField] public float _walkSpeed = 1f;
    [SerializeField] public float _sprintSpeed = 2f;
    [SerializeField] public float _dodgeActiveTime = 1.15f;
    [SerializeField] private float _zoomSpeed = 1f;

    public float vertical;
    public float horizontal;
    private Vector3 moveInput;
    private Vector3 moveVelocity;

    private Vector3 mousePosition;

    private float distance;
    public bool _isCanLook;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInventory = GetComponent<PlayerInventory>();
        _dialog = GetComponent<Dialog>();
        _playerModel = gameObject.transform.Find("PlayerModel").transform;
    }

    public void MoveUpdate()
    {
        if (GetComponent<LevelUpgrade>()._curTimeMetallistSkill <= 0)
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");


            MovementDepending();

            _rb.MovePosition(_rb.position + moveVelocity * Time.deltaTime);
        }
        else
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 difference = mousePosition - transform.position;
            difference.Normalize();
            float rotationY = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg + 90f;
            _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0f, rotationY, 0f), Time.deltaTime * 5f);

        }
        CameraPosition();
    }

    private void MovementDepending()
    {
        if (_characterStatus.isSprint && !_characterStatus.isAttack)
        {
            SprintMove(_sprintSpeed);
        }
        if (_characterStatus.isDodge)
        {
            DodgeMove();
        }
        if (_characterStatus.isAttack)
        {
            ClickUpdate();
            if (!_characterStatus.isSprint)
                NormalMove(_walkSpeed / 5);
            else SprintMove(_sprintSpeed / 2);
        }
        if (_characterStatus.isUsing)
        { }
        if (_characterStatus.isNormal)
        {
            NormalMove(_walkSpeed);
            ClickUpdate();
        }
    }

    private void CameraPosition()
    {
        if (_characterStatus.isNormal)
        {
            if (_characterStatus.isAiming)
            {
                if (Camera.main.orthographicSize > 7f)
                    Camera.main.orthographicSize -= _zoomSpeed * 10f * Time.deltaTime;
                if (Camera.main.orthographicSize < 7f)
                    Camera.main.orthographicSize += _zoomSpeed * 10f * Time.deltaTime;
            }

            else
            {
                Camera.main.orthographicSize += -Input.mouseScrollDelta.y * _zoomSpeed;

                if (Camera.main.orthographicSize < 1f)
                    Camera.main.orthographicSize = 1f;
                if (Camera.main.orthographicSize > 5f)
                    Camera.main.orthographicSize -= _zoomSpeed * 10f * Time.deltaTime;
                if (Camera.main.orthographicSize > 7f)
                    Camera.main.orthographicSize = 7f;
            }
        }

        if (_characterStatus.isSprint || _characterStatus.isAttack)
        {
            if (Camera.main.orthographicSize > 5f)
                Camera.main.orthographicSize -= _zoomSpeed * 10f * Time.deltaTime;
            if (Camera.main.orthographicSize < 5f)
                Camera.main.orthographicSize += _zoomSpeed * 10f * Time.deltaTime;
        }
    }

    private void ClickUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.tag == "Item")
                {
                    TakeItem(hit);
                }
                else if (hit.transform.tag == "Trader" || hit.transform.tag == "Freandly Npc")
                {
                    NpcStatus npcStatus = hit.transform.GetComponent<NpcStatus>();
                    NpcInventory npcInventory = hit.transform.GetComponent<NpcInventory>();
                    QuestGiver questGiver = hit.transform.GetComponent<QuestGiver>();
                    NpcDialogs npcDialogs = hit.transform.GetComponent<NpcDialogs>();

                    if (npcStatus.isFreandly)
                    {
                        _dialog._npcInventory = npcInventory;
                        _dialog._questGiver = questGiver;
                        _dialog._npcDialogs = npcDialogs;

                        distance = Vector3.Distance(transform.position, hit.transform.position);

                        StartDialog(distance);
                    }
                    else return;
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && _playerInventory._weaponInHand != null)
        {
            _characterStatus.isAttack = true;
        }
        else if (Input.GetMouseButtonUp(0))
            _characterStatus.isAttack = false;
    }

    //private void Attacking()
    //{
    //    if (_characterStatus.isAttack && Input.GetMouseButtonDown(0))
    //    {
    //        _stopAttackTimer = 0.8f;
    //        _attackNumber += 1;
    //        if (_attackNumber > 3)
    //        {
    //            _attackNumber = 0;
    //        }
    //        if (_stopAttackTimer < 0)
    //            _attackNumber = 0;
    //    }
    //    if(Input.GetKeyDown(KeyCode.R))
    //    {
    //        _stopAttackTimer = 0.8f;
    //        _attackNumber = 0;
    //    }
    //}

    private void NormalMove(float walkSpeed)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 difference = mousePosition - transform.position;
        difference.Normalize();
        float rotationY = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg + 90f;
        _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0f, rotationY, 0f), Time.deltaTime * 5f);

        moveInput = new Vector3(-vertical, 0f, horizontal);
        moveVelocity.z = Vector3.Dot(moveInput, transform.forward);
        moveVelocity.x = Vector3.Dot(moveInput, transform.right);
        moveVelocity = moveInput.normalized * walkSpeed;
    }

    private void SprintMove(float speed)
    {
        _isCanLook = true;
        LookForward(5f);

        transform.position += new Vector3(-vertical, 0f, horizontal) * speed * Time.deltaTime;
    }

    private void DodgeMove()
    {
        float dodgeVertical = 0f;
        float dodgeHorizontal = 0f; 
        LookForward(15f);
        transform.position += new Vector3(dodgeHorizontal, 0f, dodgeVertical) * _sprintSpeed * Time.deltaTime; 
    }

    private void LookForward(float rotationSpeed)
    {
        if (_isCanLook)
        {
            if (Input.GetKey(KeyCode.W))
                _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * rotationSpeed);
            if (Input.GetKey(KeyCode.S))
                _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 180f, 0.0f), Time.deltaTime * rotationSpeed);
            if (Input.GetKey(KeyCode.D))
                _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 90f, 0.0f), Time.deltaTime * rotationSpeed);
            if (Input.GetKey(KeyCode.A))
                _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 270f, 0.0f), Time.deltaTime * rotationSpeed);
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
                _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 3150f, 0.0f), Time.deltaTime * rotationSpeed);
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
                _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 45f, 0.0f), Time.deltaTime * rotationSpeed);
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
                _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 225f, 0.0f), Time.deltaTime * rotationSpeed);
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
                _playerModel.transform.rotation = Quaternion.Lerp(_playerModel.transform.rotation, Quaternion.Euler(0.0f, 135f, 0.0f), Time.deltaTime * rotationSpeed);
        }
    }

    private void TakeItem(RaycastHit hit)
    {
        if (Time.timeScale != 0)
        {
            distance = Vector3.Distance(transform.position + transform.up, hit.transform.position);
            Item it = hit.transform.GetComponent<Item>();

            if (distance < 2)
            {
                if (it._owner != "Npc")
                {
                    if (it.typeItem == "Consumables")
                    {
                        _playerInventory.consumables.Add(it);
                        //Destroy(hit.transform.gameObject);
                        _playerInventory._consItemsIDs.Add(it._itemID);
                        hit.transform.gameObject.SetActive(false);
                        _characterStatus.isUsing = true;
                    }
                    else if (it.typeItem == "Weapon")
                    {
                        _playerInventory.weapon.Add(it);
                        //Destroy(hit.transform.gameObject);
                        _playerInventory._weapItemsIDs.Add(it._itemID);
                        hit.transform.gameObject.SetActive(false);
                        _characterStatus.isUsing = true;
                    }
                    else if (it.typeItem == "ExpItems")
                    {
                        _playerInventory.expItems.Add(it);
                        //Destroy(hit.transform.gameObject);
                        _playerInventory._expItemsIDs.Add(it._itemID);
                        hit.transform.gameObject.SetActive(false);
                        _characterStatus.isUsing = true;
                    }

                    _playerInventory._weight += it.mass;
                    _interfaceManager.WeightInterface();
                }
                else Debug.Log("npc weapon");
            }
            else
            {
                Debug.Log("Daleko");
            }
        }
    }
    private void StartDialog(float distance)
    {
        if (distance < 2)
            _dialog.DialogManager();
        else
            Debug.Log("Daleko");
    }
}
