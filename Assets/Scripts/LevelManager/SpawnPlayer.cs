using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private CurrentLevelPosition _levelPosition;
    [SerializeField] private List<Transform> _spawnPointsTransform;
    [SerializeField] private GameObject _player;
    [Space]
    [SerializeField] private PlayerSaver _playerSaver;
    [SerializeField] public List<NpcSaver> _npcSavers;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _playerSaver = _player.GetComponent<PlayerSaver>();

        _player.transform.position = _spawnPointsTransform[_levelPosition.numberOfCurrentPointPos].transform.position;
        _playerSaver.Loader();
        foreach (var savers in _npcSavers)
        {
            savers.Loader();
        }
    }
}
