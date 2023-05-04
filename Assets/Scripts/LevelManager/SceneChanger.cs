using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    private void Start()
    {
        _currentTimeToNext = _timeToNext;
        _playerSaver = GameObject.Find("Player").GetComponent<PlayerSaver>();
        _npcLoader = GameObject.Find("Environments").GetComponentInChildren<NpcLoader>();
    }

    public void ChangeScene()
    {
        _levelPosition.numberOfCurrentPointPos = _nextSceneSpawnNumber;
        _playerSaver.Saver();
        _npcLoader.SaveData();
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
