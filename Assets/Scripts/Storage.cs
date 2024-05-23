using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private Base _base;

    private int _resourceNumber;
    private int _unitPrice = 3;
    private int _basePrice = 5;

    public int ResourceNumber => _resourceNumber;

    public event Action<int> NumberChanged;

    private void OnEnable()
    {
        _base.ObjectBought += OnObjectBought;
    }

    private void OnDisable()
    {
        _base.ObjectBought -= OnObjectBought;
    }

    public void PutResource()
    {
        _resourceNumber++;
        NumberChanged?.Invoke(_resourceNumber);
    }

    private void OnObjectBought(int resourceNumber)
    {
        if (resourceNumber == _basePrice || resourceNumber == _unitPrice)
        {
            _resourceNumber -= resourceNumber;
            NumberChanged?.Invoke(_resourceNumber);
        }
    }
}