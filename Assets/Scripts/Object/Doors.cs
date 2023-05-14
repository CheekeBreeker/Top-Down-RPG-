using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private GameObject _leftDoor;
    [SerializeField] private GameObject _rightDoor;
    [SerializeField] private Vector3 _leftDoorClosePos;
    [SerializeField] private Vector3 _leftDoorOpenPos;
    [SerializeField] private Vector3 _rightDoorClosePos;
    [SerializeField] private Vector3 _rightDoorOpenPos;
    [Space, SerializeField] private float _speed;
    [SerializeField] private bool _isOpening;
    [SerializeField] public bool _isOpenable;
    [SerializeField] private bool _isKeyDoor;
    [SerializeField] private Item _key;
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _openClip;
    [SerializeField] private AudioClip _closeClip;
    

    private void Update()
    {
        if (_isOpening)
        {
            _leftDoor.transform.localPosition = Vector3.Lerp(_leftDoor.transform.localPosition, _leftDoorOpenPos, _speed * Time.deltaTime);
            _rightDoor.transform.localPosition = Vector3.Lerp(_rightDoor.transform.localPosition, _rightDoorOpenPos, _speed * Time.deltaTime);
        }
        else
        {
            _leftDoor.transform.localPosition = Vector3.Lerp(_leftDoor.transform.localPosition, _leftDoorClosePos, _speed * Time.deltaTime);
            _rightDoor.transform.localPosition = Vector3.Lerp(_rightDoor.transform.localPosition, _rightDoorClosePos, _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Untagged") && _isOpenable)
        {
            if (!_isKeyDoor)
            {
                if (!_isOpening)
                {
                    _audioSource.clip = _openClip;
                    _audioSource.Play();
                }
                _isOpening = true;
            }
            else if (other.CompareTag("Player") && _isKeyDoor)
            {
                _playerInventory = other.gameObject.GetComponent<PlayerInventory>();
                bool isHaveKey = false;
                foreach (Item key in _playerInventory.expItems)
                {
                    if (_key._itemID == key._itemID)
                    {
                        isHaveKey = true;
                        _playerInventory.expItems.Remove(key);
                        break;
                    }
                }
                if (isHaveKey)
                {
                    if (!_isOpening)
                    {
                        _isKeyDoor = false;
                        _audioSource.clip = _openClip;
                        _audioSource.Play();
                    }
                    _isOpening = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Untagged"))
        {
            if (_isOpening)
            {
                _audioSource.clip = _closeClip;
                _audioSource.Play();
            }
            _isOpening = false;
        }
    }
}
