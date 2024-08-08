using System;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    private Item _item;
    private Image Icon;
    private Button ButtonTile;
    
    public Item Item
    {
        get => _item;
        set
        {
            if (_item == value) return;

            _item = value;
            Icon.sprite = _item.IconSprite;
        }
    }

    private void Awake()
    {
        Icon = gameObject.GetComponent<Image>();
        ButtonTile = gameObject.GetComponent<Button>();
    }

    private void Start()
    {
         ButtonTile.onClick.AddListener(() => Board.Instance.Select(this));
    }
}
