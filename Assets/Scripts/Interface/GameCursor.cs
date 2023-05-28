using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GameCursor : MonoBehaviour
{
    private Vector2 _offset;
    public float _rotAngle;
    private Texture2D _cursor;
    public Texture2D _normalCursor;
    public Texture2D _itemCursor;
    public Texture2D _npcCursor;
    public Texture2D _enemyCursor;
    public int _size = 15;
    public Animator _anim;

    void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        _offset = new Vector2(-_size / 2, -_size / 2);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.transform.tag == "Enemy")
            {
                _cursor = _enemyCursor;
                _size = 25;
            }
            else if (hit.transform.tag == "Trader" || hit.transform.tag == "Freandly Npc")
            {
                _cursor = _npcCursor;
                _size = 25;
            }
            else if (hit.transform.tag == "Item")
            {
                _cursor = _itemCursor;
                _size = 25;
            }
            else
            {
                _cursor = _normalCursor;
                _size = 20;
            }
        }
    }

    void OnGUI()
    {
        Vector2 mousePos = Event.current.mousePosition;
        GUI.depth = 999;
        GUI.Label(new Rect(mousePos.x + _offset.x, mousePos.y + _offset.y, _size, _size), _cursor);
    }
}
