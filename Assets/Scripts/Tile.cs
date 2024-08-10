using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    private Item _item;
    internal Image Icon;
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

    public Tile Left => x > 0 ? Board.Instance.Tiles[x - 1] [y] : null;
    public Tile Right => x < Board.Instance.Tiles[x].Length ? Board.Instance.Tiles[x + 1] [y] : null;
    public Tile Top => y > 0 ? Board.Instance.Tiles[x] [y - 1] : null;
    public Tile Botton => y < Board.Instance.Tiles.Length ? Board.Instance.Tiles[x] [y + 1] : null;

    public Tile[] Neighbours => new[]
    {
        Left,
        Right,
        Top,
        Botton
    };

    private void Awake()
    {
        Icon = gameObject.transform.GetChild(0).GetComponent<Image>();
        ButtonTile = gameObject.transform.GetChild(0).GetComponent<Button>();
    }

    private void Start()
    {
         ButtonTile.onClick.AddListener(() => Board.Instance.Select(this));
    }

    public List<Tile> GetConnectedTiles(List<Tile> exluse = null)
    {
        var result = new List<Tile> { this };

        if (exluse == null)
        {
            exluse = new List<Tile> { this };
        }
        else
        {
            exluse.Add(this);
        }

        foreach (var neighbours in Neighbours)
        {
            if (neighbours == null || exluse.Contains(neighbours) || neighbours.Item != Item) continue;
            
            result.AddRange(neighbours.GetConnectedTiles(exluse));
        }

        return result;
    }
}
