using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceAnims : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private InterfaceManager _interfaceManager;

    private void Start()
    {
        _interfaceManager = GetComponentInParent<InterfaceManager>();
    }

    public void ActiveAnim()
    {
        _animator.SetTrigger("Trigger");
    }

    void TimeScaleZero_AnimEvent()
    {
        _animator.speed = 10000;
        Time.timeScale = 0.0001f;
        _interfaceManager._countTimeScaleZero += 1;
    }

    void TimeScaleOne_AnimEvent()
    {
        if (_interfaceManager._countTimeScaleZero == 1)
        {
            _animator.speed = 1;
            Time.timeScale = 1;
        }
        _interfaceManager._countTimeScaleZero -= 1;
    }
    void OffDialog_AnimEvent()
    {
        gameObject.SetActive(false);
    }
    void OffTrading_AnimEvent()
    {
        Dialog dialog = GetComponentInParent<Dialog>();
        dialog._barterBut.SetActive(true);
        if (dialog._questGiver == null)
            dialog._questBut.SetActive(false);
        else dialog._questBut.SetActive(true);
        dialog._exitBut.SetActive(true);
        dialog._greetingsText.gameObject.SetActive(true);
    }
}
