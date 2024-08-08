using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }
    public Tile[][] Tiles { get; private set; }

    private readonly List<Tile> _selection = new List<Tile>();

    private void Awake() => Instance = this;

    private void Start()
    {
        Tiles = new Tile[gameObject.transform.childCount][];

        for (int y = 0; y < transform.childCount; y++)
        {
            Tiles[y] = new Tile[transform.GetChild(y).GetComponent<Row>().Tiles.Count]; 
                
            for (int x = 0; x < Tiles[y].Length; x++)
            {
                var title = transform.GetChild(y).GetComponent<Row>().Tiles[x];
                
                title.Item = ItemDataBase.Items[Random.Range(0, ItemDataBase.Items.Length)];
                
                title.x = x;
                title.y = y;

                Tiles[y][x] = title;
                Debug.Log($"{y}, {x}");

            }
        }
    }

    public void Select(Tile tile)
    {
        if (!_selection.Contains(tile))
        {
            _selection.Add(tile);
        }
        
        if (_selection.Count < 2) return;
        
        Debug.Log($"Выбрана плитка ({_selection[0].x}, {_selection[0].y}) и ({_selection[1].x}, {_selection[1].y})");
        
        _selection.Clear();
    }
}
    