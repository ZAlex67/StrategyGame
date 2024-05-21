using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private int _resourceNumber;

    public int ResourceNumber => _resourceNumber;

    public event Action<int> NumberChanged;

    public void PutResource()
    {
        _resourceNumber++;
        NumberChanged?.Invoke(_resourceNumber);
    }

    public void BuyNewUnit()
    {
        _resourceNumber -= 3;
        NumberChanged?.Invoke(_resourceNumber);
    }

    public void BuyNewBase()
    {
        _resourceNumber -= 5;
        NumberChanged?.Invoke(_resourceNumber);
    }
}