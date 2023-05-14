using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField] public NpcInventory _npcInventory;
    [SerializeField] public PlayerInventory _playerInventory;
    [SerializeField] public PlayerJournal _playerJournal;
    [SerializeField] public QuestGiver _questGiver;
    [SerializeField] public NpcDialogs _npcDialogs;
    [SerializeField] private CharacterStatus _characterStatus;
    [SerializeField] private InterfaceManager _interfaceManager;

    public List<Drag> _drags = new List<Drag>();
    public GameObject _inventory;
    public GameObject _dialog;
    public GameObject _cell;
    public Image _avatarImage;
    public Transform _cellParentNpc;
    public Text _wasteOfReputation;
    public Text _questText;

    public GameObject _barterBut;
    public Text _barterButtonText;
    public GameObject _questBut;
    public Text _questButtonText;
    public GameObject _exitBut;
    public Text _exitButtonText;
    public GameObject _talkingAboutQuest;
    public Text _DoneQuestText;
    public GameObject _DoneQuestBut;
    public Text _greetingsText;
    public Text _attentionText;

    private void Start()
    {
        _playerInventory = GetComponent<PlayerInventory>();
        _playerJournal = GetComponent<PlayerJournal>();
        _interfaceManager = GetComponentInChildren<InterfaceManager>();
    }

    private void Update()
    {
        
    }

    public void DialogManager()
    {
        //_npcInventory = hit.transform.GetComponent<NpcInventory>();
        //_questGiver = hit.transform.GetComponent<QuestGiver>();
        //_npcDialogs = hit.transform.GetComponent<NpcDialogs>();

        _characterStatus.isTalk = true;
        _dialog.SetActive(true);
        _interfaceManager.DialogTrigger();
        _talkingAboutQuest.SetActive(false);
        _exitBut.SetActive(true);
        _greetingsText.gameObject.SetActive(true);

        if (_npcDialogs._isTrader)
        {
            _barterBut.SetActive(true);
        }
        else _barterBut.SetActive(false);

        if (_questGiver != null)
            _questBut.SetActive(true);
        else _questBut.SetActive(false);

        if (_questGiver != null && _questGiver._isActive)
            _npcDialogs._questDescription = _npcDialogs._questHelpDescription;

        else _DoneQuestBut.SetActive(false);

        _avatarImage.sprite = _npcDialogs._avatar;

        _barterButtonText.text = _npcDialogs._barterText;
        _questButtonText.text = _npcDialogs._questText;
        _exitButtonText.text = _npcDialogs._exitText;
        _DoneQuestText.text = _npcDialogs._doneText;
        _greetingsText.text = _npcDialogs._greetings;
    }

    public void TradeEnabled()
    {
        _characterStatus.isTrade = true;
        _barterBut.SetActive(false);
        _questBut.SetActive(false);
        _exitBut.SetActive(false);
        _greetingsText.gameObject.SetActive(false);
        _interfaceManager.TradeTrigger();
        InventoryUpdate();
    }

    public void InventoryUpdate()
    {
        _inventory.SetActive(true);
        _playerInventory.InventoryEnabled();
        _wasteOfReputation.text = _npcInventory._reputation.ToString();

        foreach (Drag drag in _drags)
            Destroy(drag.gameObject);
        _drags.Clear();

        for (int i = 0; i < _npcInventory._items.Count; i++)
        {
            GameObject newCell = Instantiate(_cell);
            newCell.transform.SetParent(_cellParentNpc, false);
            _drags.Add(newCell.GetComponent<Drag>());
        }

        for (int i = 0; i < _npcInventory._items.Count; i++)
        {
            Item it = _npcInventory._items[i];

            for (int j = 0; j < _drags.Count; j++)
            {
                if (_drags[j]._ownerItem != "")
                {
                    if (_npcInventory._items[i].isStackable)
                    {
                        if (_drags[j]._item.nameItem == it.nameItem)
                        {
                            _drags[j]._countItem++;
                            _drags[j]._count.text = _drags[j]._countItem.ToString();
                            break;
                        }
                    }
                    else continue;

                }
                else
                {
                    _drags[j]._item = it;
                    _drags[j]._image.sprite = Resources.Load<Sprite>(_npcInventory._items[i].pathSprite);
                    _drags[j]._ownerItem = "TraderItem";
                    _drags[j]._countItem++;
                    _drags[j]._count.text = "" + _drags[j]._countItem;
                    _drags[j]._trading = this;
                    _drags[j]._descriptionObj = _playerInventory._descriptionObj;
                    _drags[j]._descriptionItem = _playerInventory._descriptionItem;
                    break;
                }
            }
        }

        for (int i = _drags.Count - 1; i >= 0; i--)
        {
            if (_drags[i]._ownerItem == "")
            {
                Destroy(_drags[i].gameObject);
                _drags.RemoveAt(i);
            }
        }
    }

    public void TradeDisable()
    {
        foreach (Drag drag in _drags)
            Destroy(drag.gameObject);
        _drags.Clear();

        _playerInventory.InventoryDisable();
        _characterStatus.isTrade = false;
        _interfaceManager.TradeTrigger();
    }

    public void TradeDisableButton()
    {
        TradeDisable();
    }
    public void StartTradeButton()
    {
        TradeEnabled();
    }

    public void StartQuestButton()
    {
        _talkingAboutQuest.SetActive(true);
        _barterBut.SetActive(false);
        _questBut.SetActive(false);
        _exitBut.SetActive(false);
        _greetingsText.gameObject.SetActive(false);

        _playerJournal._quests.Add(_questGiver);
        _questText.text = _npcDialogs._questDescription;
        _questGiver.QuestAccept(_playerInventory, transform);

        if (GetComponent<LevelUpgrade>()._isHaveSelfDefenceSkill && _npcDialogs._isLiar)
            _attentionText.text = _npcDialogs._attention;
        else _attentionText.text = "";

        if (_questGiver._isDone && !_questGiver._questCompetitor._isDone)
        {
            _DoneQuestBut.SetActive(true);
            _DoneQuestText.text = _npcDialogs._DoneQuestDescription;
        }
        else if (_questGiver._questCompetitor._isDone)
        {
            _DoneQuestBut.SetActive(true);
            _DoneQuestText.text = _npcDialogs._DoneQuestDescription;
        }
    }

    public void DoneQuestButton()
    {
        if (_questGiver._isDone && !_questGiver._questCompetitor._isDone)
        {
            if (_questGiver._itemReward.typeItem == "Consumables")
                _playerInventory.consumables.Add(_questGiver._itemReward);
            else if (_questGiver._itemReward.typeItem == "Weapon")
                _playerInventory.weapon.Add(_questGiver._itemReward);
            else if (_questGiver._itemReward.typeItem == "ExpItems")
                _playerInventory.expItems.Add(_questGiver._itemReward);
            _npcInventory._reputation += _questGiver._plusRep;

            if (_questGiver._questType == 1)
            {
                DeletingItems(_playerInventory.consumables);
                DeletingItems(_playerInventory.weapon);
                DeletingItems(_playerInventory.expItems);
            }

            _questText.text = _npcDialogs._questDone;

            _npcInventory.DeleteQuestGiver();
            _questGiver = null;
            _DoneQuestBut.SetActive(false);
        }
        else if (_questGiver._questCompetitor._isDone)
        {
            _npcInventory._reputation -= _questGiver._plusRep * 2;

            if (_questGiver._isQuestGiverWillBeEnemy)
            {
                _questGiver.gameObject.GetComponent<NpcStats>().BecomeAnEnemy();
            }

            _questText.text = _npcDialogs._questCompete;

            _npcInventory.DeleteQuestGiver();
            _questGiver = null;
            _DoneQuestBut.SetActive(false);
        }
    }

    public void DeletingItems(List<Item> type)
    {
        for(int i = 0; i < type.Count; i++)
        {
            if (type[i].nameItem == _questGiver._itemTarget.nameItem)
            {
                Destroy(type[i]);
                type.RemoveAt(i);
                break;
            }
        }
    }
    public void ExitDialogButton()
    {
        _characterStatus.isTalk = false;
        _npcInventory = null;
        _questGiver = null;
        _npcDialogs = null;

        _interfaceManager.DialogTrigger();
    }
    public void NpcToBarter(Drag drag)
    {
        Item it = drag._item;

        if (_npcInventory._reputation - it.price >= 0)
        {
            if(it.typeItem == "Consumables")
            {
                _playerInventory.consumables.Add(it);
                _npcInventory._items.Remove(it);
                _npcInventory._reputation -= it.price;
                _playerInventory.InventoryEnabled();
                InventoryUpdate();
            }
            if (it.typeItem == "Weapon")
            {
                _playerInventory.weapon.Add(it);
                _npcInventory._items.Remove(it);
                _npcInventory._reputation -= it.price;
                _playerInventory.InventoryEnabled();
                InventoryUpdate();
            }
            if (it.typeItem == "ExpItems")
            {
                _playerInventory.expItems.Add(it);
                _npcInventory._items.Remove(it);
                _npcInventory._reputation -= it.price;
                _playerInventory.InventoryEnabled();
                InventoryUpdate();
            }
        }
    }
}