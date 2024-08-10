using System.Collections.Generic;
using UnityEngine;

public sealed class Row : MonoBehaviour
{
    public List<Tile> Tiles = new List<Tile>();
    [SerializeField] internal int _startIndex = 0;
    [SerializeField] internal int _endIndex = 0;
    
    private void Awake()
    {
        CountTiles();
    }

    private void CountTiles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Tiles.Add(transform.GetChild(i).GetComponent<Tile>());
        }
    }
}
