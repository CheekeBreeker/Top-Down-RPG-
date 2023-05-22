using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHelps : MonoBehaviour
{
    [SerializeField] private Text _helpText;
    [Space]
    [SerializeField] private FieldOfView _fieldOfView;
    [SerializeField, Multiline(10)] private string _interactionDescr;
    [SerializeField, Multiline(10)] private string _fightDescr;

    private void Start()
    {
        _helpText.text = "";
    }

    private void Update()
    {
        if (_fieldOfView.visibleTargets.Count != 0)
            _helpText.text = _fightDescr;
        else _helpText.text = _interactionDescr;
    }
}
