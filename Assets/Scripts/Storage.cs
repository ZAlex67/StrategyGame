using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private Base _base;

    private int _resourceNumber;

    public event Action<int> NumberChanged;

    private void OnEnable()
    {
        _base.TookResource += OnTookResource;
    }

    private void OnDisable()
    {
        _base.TookResource -= OnTookResource;
    }

    private void OnTookResource()
    {
        _resourceNumber++;
        NumberChanged?.Invoke(_resourceNumber);
    }
}