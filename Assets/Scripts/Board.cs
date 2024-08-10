using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }
    public Tile[][] Tiles { get; private set; }

    private readonly List<Tile> _selection = new List<Tile>();
    private const float _tweenDuration = 0.25f;
    private void Awake() => Instance = this;

    private void Start()
    {
        Tiles = new Tile[gameObject.transform.childCount][];

        for (int x = 0; x < transform.childCount; x++)
        {
            Tiles[x] = new Tile[transform.GetChild(x).GetComponent<Row>().Tiles.Count]; 
                
            for (int y = 0; y < Tiles[x].Length; y++)
            {
                var title = transform.GetChild(x).GetComponent<Row>().Tiles[y];
                
                title.Item = ItemDataBase.Items[Random.Range(0, ItemDataBase.Items.Length)];
                
                title.x = x;
                title.y = y;

                Tiles[x][y] = title;
            }
        }
    }

    public async void Select(Tile tile)
    {
        if (!_selection.Contains(tile))
        {
            _selection.Add(tile);
        }
        
        if (_selection.Count < 2) return;

        await Swap(_selection[0], _selection[1]);
        
        _selection.Clear();
    }

    public async Task Swap(Tile tile1, Tile tile2)
    {
        var icon1 = tile1.Icon;
        var icon2 = tile2.Icon;

        var icon1Transform = icon1.transform;
        var icon2Transform = icon2.transform;

        var sequence = DOTween.Sequence();
        sequence.Join(icon1Transform.DOMove(icon2Transform.position, _tweenDuration))
                .Join(icon2Transform.DOMove(icon1Transform.position, _tweenDuration));

        await sequence.Play().AsyncWaitForCompletion();
        
        icon1Transform.SetParent(tile2.transform);
        icon2Transform.SetParent(tile1.transform);

        tile1.Icon = icon2;
        tile2.Icon = icon1;

        var tempItem = tile1.Item;
        tile1.Item = tile2.Item;
        tile2.Item = tempItem;
    }

    // private void CanPop()
    // {
    //     
    // }
    //
    // private void Pop()
    // {
    //     
    // }
}
    