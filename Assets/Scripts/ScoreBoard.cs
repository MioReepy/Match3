using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _scoreText;
    private int _score;

    public int Score
    {
        get => _score;
        set
        {
            if(_score == value) return;
            
            _score = value;
            _scoreText.SetText($"{_score}");
        }
    }

    private void Awake() => Instance = this;
}
