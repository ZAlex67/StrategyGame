using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private Storage _resources;
    [SerializeField] private TMP_Text _score;

    private void OnEnable()
    {
        _resources.NumberChanged += OnNumberChanged;
    }

    private void OnDisable()
    {
        _resources.NumberChanged -= OnNumberChanged;
    }

    private void OnNumberChanged(int score)
    {
        _score.text = score.ToString();
    }
}