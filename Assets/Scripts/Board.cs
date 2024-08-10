using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }
    public Tile[,] Tiles { get; private set; }

    public Row[] Rows;

    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);

    private readonly List<Tile> _selection = new List<Tile>();
    
    private const float _tweenDuration = 0.25f;
    private void Awake() => Instance = this;

    private void Start()
    {
        Rows = new Row[gameObject.transform.childCount];
            
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Rows[i] = gameObject.transform.GetChild(i).GetComponent<Row>();
        }
        
        Tiles = new Tile[Rows.Max(row => row.Tiles.Count), Rows.Length];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                // if (gameObject.transform.GetChild(y).GetComponent<Row>()._startIndex > 0)
                // {
                //     Tiles[y][x] = null;
                // }
                
                var title = Rows[y].Tiles[x];
                
                title.Item = ItemDataBase.Items[Random.Range(0, ItemDataBase.Items.Length)];
                
                title.x = x;
                title.y = y;

                Tiles[x, y] = title;
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

        if (CanPop())
        {
            Debug.Log(CanPop());
            Pop();
        }
        else
        {
            await Swap(_selection[0], _selection[1]);
        }
        
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

    private bool CanPop()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (Tiles[x, y].GetConnectedTiles().Skip(1).Count() >= 2)
                {
                    return true;
                }
            }
        }
        
        return false;
    }
    
    private async void Pop()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var tile = Tiles[x, y];
                var connectedTiles = tile.GetConnectedTiles();

                if (connectedTiles.Skip(1).Count() < 2) continue;

                var deflateSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    deflateSequence.Join(connectedTile.Icon.transform.DOScale(Vector3.zero, _tweenDuration));
                }

                await deflateSequence.Play().AsyncWaitForCompletion();

                var inFlateSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    connectedTile.Item = ItemDataBase.Items[Random.Range(0, ItemDataBase.Items.Length)];
                    inFlateSequence.Join((connectedTile.Icon.transform.DOScale(Vector3.one, _tweenDuration)));
                }

                await inFlateSequence.Play().AsyncWaitForCompletion();

                x = 0;
                y = 0;
            }
        }
    }
}
    