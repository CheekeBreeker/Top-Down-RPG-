using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string nameItem;
    public string typeItem;
    public int price;
    public int mass;
    public string pathSprite;
    public string pathPrefab;

    private MeshRenderer _renderer;

    [SerializeField] private Material notActiveMaterial;
    [SerializeField] private Material activeMaterial;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        ActiveMaterial();
    }

    private void ActiveMaterial()
    {
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            _renderer.sharedMaterial = activeMaterial;
        }
        else _renderer.sharedMaterial = notActiveMaterial;
    }
}
