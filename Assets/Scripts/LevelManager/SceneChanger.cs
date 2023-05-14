using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private CurrentLevelPosition _levelPosition;
    [Space]
    [SerializeField] private string _nextSceneName;
    [SerializeField] private int _nextSceneSpawnNumber;
    [SerializeField] private Animator _fading;
    [Space]
    [SerializeField] private float _timeToNext = 5f;
    [SerializeField] private float _currentTimeToNext;
    [Space]
    [SerializeField] private PlayerSaver _playerSaver;
    [SerializeField] private NpcLoader _npcLoader;
    [SerializeField] private SpawnPlayer _spawnPlayer;
    private void Start()
    {
        _currentTimeToNext = _timeToNext;
        _playerSaver = GameObject.Find("Player").GetComponent<PlayerSaver>();
        _npcLoader = GameObject.Find("Environments").GetComponentInChildren<NpcLoader>();
        _spawnPlayer = GetComponentInParent<SpawnPlayer>();
    }

    public void ChangeScene()
    {
        _levelPosition.numberOfCurrentPointPos = _nextSceneSpawnNumber;
        _playerSaver.Saver();
        foreach (var savers in _spawnPlayer._npcSavers)
        {
            savers.Saver();
        }
        SceneManager.LoadScene(_nextSceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _fading.SetTrigger("Fading");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _currentTimeToNext -= Time.deltaTime;
            if (_currentTimeToNext < 0)
                ChangeScene();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _fading.SetTrigger("Fading");
            _currentTimeToNext = _timeToNext;
        }
    }
}
