using UnityEngine;

[CreateAssetMenu(menuName = "Match3/Item")]
public sealed class Item : ScriptableObject
{
    public Sprite IconSprite;
    public int Score;
}