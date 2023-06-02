using UnityEngine;
using UnityEngine.UI;

public class LevelUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject _skillsView;
    [SerializeField] private PlayerStats _playerStats;

    public bool _isHaveSelfDefenceSkill;
    public bool _isHaveProrabSkill;
    public bool _isHaveProletarianSkill;
    public bool _isHaveHumanExplSkill;
    public bool _isHaveWelderSkill;
    public bool _isHaveMetallistSkill;

    public bool _isTakedSelfDefenceSkill;
    public bool _isTakedProrabSkill;
    public bool _isTakedProletarianSkill;
    public bool _isTakedHumanExpSkill;
    public bool _isTakedWelderSkill;
    public bool _isTakedMetallistSkill;

    public string _pathSprite;

    public Button _selfDefenceSkillBut;
    public Button _prorabSkillBut;
    public Button _proletarianSkillBut;
    public Button _HumanExplSkillBut;
    public Button _welderSkillBut;
    public Button _metallistSkillBut;

    public bool _isSelectedBadSkill;
    public bool _isSelectedGoodSkill;

    private void Start()
    {
        _playerStats = GetComponent<PlayerStats>();

        _selfDefenceSkillBut.gameObject.SetActive(false);
        _prorabSkillBut.gameObject.SetActive(false);
        _proletarianSkillBut.gameObject.SetActive(false);
        _HumanExplSkillBut.gameObject.SetActive(false);
        _welderSkillBut.gameObject.SetActive(false);
        _metallistSkillBut.gameObject.SetActive(false);

        if (!_isHaveSelfDefenceSkill)
            _selfDefenceSkillBut.GetComponent<Graphic>().color = new Color(200f / 255, 200f / 255, 200f / 255, 255f / 255);
        else _selfDefenceSkillBut.GetComponent<Graphic>().color = new Color(255, 255, 255, 255);
        if (!_isHaveProrabSkill)
            _prorabSkillBut.GetComponent<Graphic>().color = new Color(200f / 255, 200f / 255, 200f / 255, 255f / 255);
        else _selfDefenceSkillBut.GetComponent<Graphic>().color = new Color(255, 255, 255, 255);
        if (!_isHaveProletarianSkill)
            _proletarianSkillBut.GetComponent<Graphic>().color = new Color(200f / 255, 200f / 255, 200f / 255, 255f / 255);
        else _selfDefenceSkillBut.GetComponent<Graphic>().color = new Color(255, 255, 255, 255);
        if (!_isHaveHumanExplSkill)
            _HumanExplSkillBut.GetComponent<Graphic>().color = new Color(200f / 255, 200f / 255, 200f / 255, 255f / 255);
        else _selfDefenceSkillBut.GetComponent<Graphic>().color = new Color(255, 255, 255, 255);
        if (!_isHaveWelderSkill)
            _welderSkillBut.GetComponent<Graphic>().color = new Color(200f / 255, 200f / 255, 200f / 255, 255f / 255);
        else _selfDefenceSkillBut.GetComponent<Graphic>().color = new Color(255, 255, 255, 255);
        if (!_isHaveMetallistSkill)
            _metallistSkillBut.GetComponent<Graphic>().color = new Color(200f / 255, 200f / 255, 200f / 255, 255f / 255);
        else _selfDefenceSkillBut.GetComponent<Graphic>().color = new Color(255, 255, 255, 255);
    }

    private void Update()
    {
        LookSkills();
    }

    private void LookSkills()
    {
        if (_playerStats._level == 2)
        {
            _selfDefenceSkillBut.gameObject.SetActive(true);
            _prorabSkillBut.gameObject.SetActive(true);
            _proletarianSkillBut.gameObject.SetActive(true);
        }
        if (_playerStats._level == 3 && _isHaveSelfDefenceSkill)
        {
            _HumanExplSkillBut.gameObject.SetActive(true);
            _welderSkillBut.gameObject.SetActive(true);
            _metallistSkillBut.gameObject.SetActive(true);
        }
    }

    public void AcceptSkills()
    {
        if (_isSelectedBadSkill && _isSelectedGoodSkill)
        {
            if (_isTakedSelfDefenceSkill)
            {
                _isHaveSelfDefenceSkill = true;
            }
            if (_isTakedProrabSkill)
            {
                _isHaveProrabSkill = true;
            }
            if (_isTakedProletarianSkill)
            {
                _isHaveProletarianSkill = true;
            }
            if (_isTakedHumanExpSkill)
            {
                _playerStats._maxblockHP = 50;
                _isHaveHumanExplSkill = true;
            }
            if (_isTakedWelderSkill)
            {
                _isHaveWelderSkill = true;
            }
            if (_isTakedMetallistSkill)
            {
                _isHaveMetallistSkill = true;
            }

            _isSelectedBadSkill = false;
            _isSelectedGoodSkill = false;

            if (_playerStats._level == 2)
            {
                _selfDefenceSkillBut.interactable = false;
                _prorabSkillBut.interactable = false;
                _proletarianSkillBut.interactable = false;
            }

            if (_playerStats._level == 3)
            {
                _HumanExplSkillBut.interactable = false;
                _welderSkillBut.interactable = false;
                _metallistSkillBut.interactable = false;
            }
        }
    }

    private void SelfDefenceSkill()
    {
        _isTakedSelfDefenceSkill = true;
        _isSelectedBadSkill = true;
        _selfDefenceSkillBut.GetComponent<Graphic>().color = new Color(255f / 255, 255f / 255, 255f / 255, 255f / 255); ;
    }

    private void ProrabSkill()
    {
        _isTakedProrabSkill = true;
        _isTakedProletarianSkill = false;
        _isSelectedGoodSkill = true;
        _proletarianSkillBut.GetComponent<Graphic>().color = new Color(200f / 255, 200f / 255, 200f / 255, 255f / 255);
        _prorabSkillBut.GetComponent<Graphic>().color = new Color(255f / 255, 255f / 255, 255f / 255, 255f / 255);
    }

    public void ProletarianSkill()
    {
        _isTakedProletarianSkill = true;
        _isTakedProrabSkill = false;
        _isSelectedGoodSkill = true; 
        _prorabSkillBut.GetComponent<Graphic>().color = new Color(200f / 255, 200f / 255, 200f / 255, 255f / 255);
        _proletarianSkillBut.GetComponent<Graphic>().color = new Color(255f / 255, 255f / 255, 255f / 255, 255f / 255);
    }

    public void HumanExplSkill()
    {
        _isTakedHumanExpSkill = true; 
        _isSelectedBadSkill = true;
        _HumanExplSkillBut.GetComponent<Graphic>().color = new Color(255f / 255, 255f / 255, 255f / 255, 255f / 255);
    }
    private void WelderSkill()
    {
        _isTakedWelderSkill = true;
        _isTakedMetallistSkill = false;
        _isSelectedGoodSkill = true;
        _metallistSkillBut.GetComponent<Graphic>().color = new Color(200f / 255, 200f / 255, 200f / 255, 255f / 255);
        _welderSkillBut.GetComponent<Graphic>().color = new Color(255f / 255, 255f / 255, 255f / 255, 255f / 255);
    }

    private void MetallistSkill()
    {
        _isTakedMetallistSkill = true;
        _isTakedWelderSkill = false;
        _isSelectedGoodSkill = true;
        _welderSkillBut.GetComponent<Graphic>().color = new Color(200f / 255, 200f / 255, 200f / 255, 255f / 255);
        _metallistSkillBut.GetComponent<Graphic>().color = new Color(255f / 255, 255f / 255, 255f / 255, 255f / 255);

        //_curTimeMetallistSkill = _timeMetallistSkill;
        //if (_curTimeMetallistSkill > 0)
        //{
        //    _curTimeMetallistSkill -= Time.deltaTime;
        //    GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        //    gameObject.layer = 0;
        //}
        //else
        //{
        //    GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        //    gameObject.layer = 7;
        //}
    }
}
