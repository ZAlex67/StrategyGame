using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private Storage _resources;
    [SerializeField] private TMP_Text _score;

    private void OnEnable()
    {
        _resources.NumberChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        _resources.NumberChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int score)
    {
        _score.text = score.ToString();
    }
}