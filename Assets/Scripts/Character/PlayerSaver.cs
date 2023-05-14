using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class PlayerSaver : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private PlayerJournal _playerJournal;
    [SerializeField] private LevelUpgrade _levelUpgrade;
    [SerializeField] private CurrentLevelPosition _currentLevelPosition;
    [Space]
    public float _healthToSave;
    public float _hpBlockToSave;
    public int _levelToSave;
    public float _curExpToSave;
    public string _weaponInHandIDToSave;
    public List<string> _consIDInInvToSave;
    public List<string> _weapIDInInvToSave;
    public List<string> _expIDInInvToSave;
    public List<string> _itemsIDInJourToSave;
    public float _weightToSave;
    public bool _isHaveSelfDefenceSkillToSave;
    public bool _isHaveProrabSkillToSave;
    public bool _isHaveProletarianSkillToSave;
    public bool _isHaveWelderSkillToSave;
    public bool _isHaveMetallistSkillToSave;
    [Space]
    public string _currentSceneNameToSave;
    public int _currentLevelPositionToSave;

    private void Update()
    {

        //{
        //    BinaryFormatter bf = new BinaryFormatter();
        //    FileStream file = File.Open(Application.persistentDataPath + "/PlayerSaveData.dat", FileMode.Open);
        //    PlayerSaveData data = (PlayerSaveData)bf.Deserialize(file);
        //    file.Close();
        //    SceneManager.LoadScene(data._savedCurrentSceneName);
        //    Loader();
        //}
    }

    public void Saver()
    {
        _healthToSave = _playerStats._health;
        _hpBlockToSave = _playerStats._blockHP;
        _levelToSave = _playerStats._level;
        _curExpToSave = _playerStats._curExp;
        if (_playerInventory._weaponInHand != default)
            _weaponInHandIDToSave = _playerInventory._weaponInHand.GetComponent<Item>()._itemID;
        _consIDInInvToSave = _playerInventory._consItemsIDs;
        _weapIDInInvToSave = _playerInventory._weapItemsIDs;
        _expIDInInvToSave = _playerInventory._expItemsIDs;
        _itemsIDInJourToSave = _playerJournal._itemsIDs;
        _weightToSave = _playerInventory._weight;
        _isHaveSelfDefenceSkillToSave = _levelUpgrade._isHaveSelfDefenceSkill;
        _isHaveProrabSkillToSave = _levelUpgrade._isHaveProrabSkill;
        _isHaveProletarianSkillToSave = _levelUpgrade._isHaveProletarianSkill;
        _isHaveWelderSkillToSave = _levelUpgrade._isHaveWelderSkill;
        _isHaveMetallistSkillToSave = _levelUpgrade._isHaveMetallistSkill;
        _currentSceneNameToSave = SceneManager.GetActiveScene().name;
        _currentLevelPositionToSave = _currentLevelPosition.numberOfCurrentPointPos; 

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PlayerSaveData.dat");
        PlayerSaveData data = new PlayerSaveData();
        data._savedHealth = _healthToSave;
        data._savedHpBlock = _hpBlockToSave;
        data._savedLevel = _levelToSave;
        data._savedCurExp = _curExpToSave;
        if (_playerInventory._weaponInHand != default)
            data._savedWeaponInHandID = _weaponInHandIDToSave;
        data._savedConsIDInInv = _consIDInInvToSave;
        data._savedWeapIDInInv = _weapIDInInvToSave;
        data._savedExpIDInInv = _expIDInInvToSave;
        data._savedItemsIDInJour = _itemsIDInJourToSave;
        data._savedWeight = _weightToSave;
        data._savedIsHaveSelfDefenceSkill = _isHaveSelfDefenceSkillToSave;
        data._savedIsHaveProrabSkill = _isHaveProrabSkillToSave;
        data._savedIsHaveProletarianSkill = _isHaveProletarianSkillToSave;
        data._savedIsHaveWelderSkill = _isHaveWelderSkillToSave;
        data._savedIsHaveMetallistSkill = _isHaveMetallistSkillToSave;
        data._savedCurrentSceneName = _currentSceneNameToSave;
        data._savedCurrentLevelPosition = _currentLevelPositionToSave;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Player new data saved");
    }

    public void Loader()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerSaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PlayerSaveData.dat", FileMode.Open);
            PlayerSaveData data = (PlayerSaveData)bf.Deserialize(file);
            file.Close();
            _healthToSave = data._savedHealth;
            _hpBlockToSave = data._savedHpBlock;
            _levelToSave = data._savedLevel;
            _curExpToSave = data._savedCurExp;
            _weaponInHandIDToSave = data._savedWeaponInHandID;
            _consIDInInvToSave = data._savedConsIDInInv;
            _weapIDInInvToSave = data._savedWeapIDInInv;
            _expIDInInvToSave = data._savedExpIDInInv;
            _itemsIDInJourToSave = data._savedItemsIDInJour;
            _weightToSave = data._savedWeight;
            _isHaveSelfDefenceSkillToSave = data._savedIsHaveSelfDefenceSkill;
            _isHaveProrabSkillToSave = data._savedIsHaveProrabSkill;
            _isHaveProletarianSkillToSave = data._savedIsHaveProletarianSkill;
            _isHaveWelderSkillToSave = data._savedIsHaveWelderSkill;
            _isHaveMetallistSkillToSave = data._savedIsHaveMetallistSkill;
            _currentSceneNameToSave = data._savedCurrentSceneName;
            _currentLevelPositionToSave = data._savedCurrentLevelPosition;

            _playerStats._health = _healthToSave;
            _playerStats._blockHP = _hpBlockToSave;
            _playerStats._level = _levelToSave;
            _playerStats._curExp = _curExpToSave;

            _playerInventory._weaponInHandID = _weaponInHandIDToSave;
            if (_weaponInHandIDToSave != null)
            {
                if (_weaponInHandIDToSave == "1")
                {
                    GameObject itemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/Iron pipe"));
                    Item it = itemObj.GetComponent<Item>();
                    it._partsDescr = it._description.Split('$');
                    itemObj.SetActive(false);
                    _playerInventory.TakeWeapon(it);
                }
                if (_weaponInHandIDToSave == "2")
                {
                    GameObject itemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/Steel pipe"));
                    Item it = itemObj.GetComponent<Item>();
                    it._partsDescr = it._description.Split('$');
                    itemObj.SetActive(false);
                    _playerInventory.TakeWeapon(it);
                }
                if (_weaponInHandIDToSave == "3")
                {
                    GameObject itemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/Board"));
                    Item it = itemObj.GetComponent<Item>();
                    it._partsDescr = it._description.Split('$');
                    itemObj.SetActive(false);
                    _playerInventory.TakeWeapon(it);
                }
                if (_weaponInHandIDToSave == "4")
                {
                    GameObject itemObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/Axe"));
                    Item it = itemObj.GetComponent<Item>();
                    it._partsDescr = it._description.Split('$');
                    itemObj.SetActive(false);
                    _playerInventory.TakeWeapon(it);
                }
            }

            _playerInventory._consItemsIDs = _consIDInInvToSave;
            _playerInventory._weapItemsIDs = _weapIDInInvToSave;
            _playerInventory._expItemsIDs = _expIDInInvToSave;
            _playerInventory.LoadItems();
            _playerJournal._itemsIDs = _itemsIDInJourToSave;
            _playerJournal.LoadItems();
            _playerInventory._weight = _weightToSave;

            _levelUpgrade._isTakedSelfDefenceSkill = _isHaveSelfDefenceSkillToSave;
            _levelUpgrade._isTakedProrabSkill = _isHaveProrabSkillToSave;
            _levelUpgrade._isTakedProletarianSkill = _isHaveProletarianSkillToSave;
            _levelUpgrade._isTakedWelderSkill = _isHaveWelderSkillToSave;
            _levelUpgrade._isTakedMetallistSkill = _isHaveMetallistSkillToSave;

            _currentLevelPosition.numberOfCurrentPointPos = _currentLevelPositionToSave;

            Debug.Log("Player data loaded");
        }
        else Debug.LogError("There is no save data");
    }

    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerSaveData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/PlayerSaveData.dat");
            _healthToSave = default;
            _hpBlockToSave = default;
            _levelToSave = 1;
            _curExpToSave = 0;
            _weaponInHandIDToSave = default;
            _consIDInInvToSave = default;
            _weapIDInInvToSave = default;
            _expIDInInvToSave = default;
            _itemsIDInJourToSave = default;
            _weightToSave = default;
            _isHaveSelfDefenceSkillToSave = default;
            _isHaveProrabSkillToSave = default;
            _isHaveProletarianSkillToSave = default;
            _isHaveWelderSkillToSave = default;
            _currentSceneNameToSave = "Test Level";
            _currentLevelPositionToSave = 0;

            Debug.Log("Data reset complete");
        }
        else Debug.LogError("No save data to delete.");
    }
}

[Serializable]
class PlayerSaveData
{
    public float _savedHealth;
    public float _savedHpBlock;
    public int _savedLevel;
    public float _savedCurExp;
    public string _savedWeaponInHandID;
    public List<string> _savedConsIDInInv;
    public List<string> _savedWeapIDInInv;
    public List<string> _savedExpIDInInv;
    public List<string> _savedItemsIDInJour;
    public float _savedWeight;
    public bool _savedIsHaveSelfDefenceSkill;
    public bool _savedIsHaveProrabSkill;
    public bool _savedIsHaveProletarianSkill;
    public bool _savedIsHaveWelderSkill;
    public bool _savedIsHaveMetallistSkill;
    public string _savedCurrentSceneName;
    public int _savedCurrentLevelPosition;
}