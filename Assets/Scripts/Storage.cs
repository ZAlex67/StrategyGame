using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private Base _base;

    private int _resourceNumber;

    public event Action<int> NumberChanged;

    private void OnEnable()
    {
        _base.TookResource += OnTake;
    }

    private void OnDisable()
    {
        _base.TookResource -= OnTake;
    }

    private void OnTake()
    {
        _resourceNumber++;
        NumberChanged?.Invoke(_resourceNumber);
    }
}