using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class NpcSaver : MonoBehaviour
{
    [SerializeField] public NpcStats _npcStats;
    [SerializeField] public NpcStatus _npcStatus;
    [SerializeField] public NpcInventory _npcInventory;
    [SerializeField] public QuestGiver _questGiver;
    [Space]
    public bool _isDeadToSave;
    public bool _isFreandlyToSave;
    public bool _isFollowToSave;
    [Space]
    public int _reputationForSave;
    [Space]
    public bool _isQuestActiveToSave;
    public bool _isQuestDoneToSave;
    public bool _isWasOnScoutTargetToSave;
    public bool _isKilledTargetToSave;


    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) Saver();
        if (Input.GetKeyDown(KeyCode.F6)) Loader();
        if (Input.GetKeyDown(KeyCode.F7)) ResetData();
    }

    public void Saver()
    {
        _isDeadToSave = _npcStatus.isDead;
        _isFreandlyToSave = _npcStatus.isFreandly;
        _isFollowToSave = _npcStatus.isFollow;
        _reputationForSave = _npcInventory._reputation;

        if (_questGiver != null)
        {
            _isQuestActiveToSave = _questGiver._isActive;
            _isQuestDoneToSave = _questGiver._isDone;
            _isWasOnScoutTargetToSave = _questGiver._isWasOnScoutTarget;
            if (_questGiver._killTargets.Count == 0)
                _isKilledTargetToSave = true;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + gameObject.name + _npcStatus.dollID + "NpcSaveData.dat");
        SaveData saveData = new SaveData();
        saveData._savedIsDead = _isDeadToSave;
        saveData._savedIsFreandly = _isFreandlyToSave;
        saveData._savedIsFollow = _isFollowToSave;
        saveData._savedReputation = _reputationForSave;

        if (_questGiver != null)
        {
            saveData._savedIsQuestActive = _isQuestActiveToSave;
            saveData._savedIsQuestDone = _isQuestDoneToSave;
            saveData._savedIsWasOnScout = _isWasOnScoutTargetToSave;
            saveData._savedIsKilledTargets = _isKilledTargetToSave;
        }

        bf.Serialize(file, saveData);
        file.Close();
        Debug.Log(gameObject.name + "Npc new data saved");
    }

    public void Loader()
    {
        if (File.Exists(Application.persistentDataPath + "/" + gameObject.name + _npcStatus.dollID + "NpcSaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + gameObject.name + _npcStatus.dollID + "NpcSaveData.dat", FileMode.Open);
            SaveData saveData = (SaveData)bf.Deserialize(file);
            file.Close();
            _isDeadToSave = saveData._savedIsDead;
            _isFreandlyToSave = saveData._savedIsFreandly;
            _isFollowToSave = saveData._savedIsFollow;
            _reputationForSave = saveData._savedReputation;

            if (_questGiver != null)
            {
                _isQuestActiveToSave = saveData._savedIsQuestActive;
                _isQuestDoneToSave = saveData._savedIsQuestDone;
                _isWasOnScoutTargetToSave = saveData._savedIsWasOnScout;
                _isKilledTargetToSave = saveData._savedIsKilledTargets;
            }

            _npcStatus.isDead = _isDeadToSave;
            _npcStatus.isFreandly = _isFreandlyToSave;
            _npcStatus.isFollow = _isFollowToSave;
            _npcInventory._reputation = _reputationForSave;

            if (_questGiver != null)
            {
                _questGiver._isActive = _isQuestActiveToSave;
                _questGiver._isDone = _isQuestDoneToSave;
                _questGiver._isWasOnScoutTarget = _isWasOnScoutTargetToSave;
                if (_isKilledTargetToSave)
                    _questGiver._killTargets = null;
            }

            if (!_isFreandlyToSave) _npcStats.BecomeAnEnemy();
            if (_isDeadToSave) gameObject.SetActive(false);

            Debug.Log(gameObject.name + "Npc data loaded");
        }
        else Debug.LogError("There is no save data");
    }
    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + "/" + gameObject.name + _npcStatus.dollID + "NpcSaveData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/" + gameObject.name + _npcStatus.dollID + "NpcSaveData.dat");
            _isDeadToSave = default;
            _isFreandlyToSave = default;
            _reputationForSave = default;

            if (_questGiver != null)
            {
                _isQuestActiveToSave = default;
                _isQuestDoneToSave = default;
                _isWasOnScoutTargetToSave = default;
                _isKilledTargetToSave = default;
            }

            Debug.Log(gameObject.name + "Data reset complete");
        }
        else Debug.LogError("No save data to delete.");
    }
}

[Serializable]
class SaveData
{
    public bool _savedIsDead;
    public bool _savedIsFreandly;
    public bool _savedIsFollow;
    public int _savedReputation;
    public bool _savedIsQuestActive;
    public bool _savedIsQuestDone;
    public bool _savedIsWasOnScout;
    public bool _savedIsKilledTargets;
}