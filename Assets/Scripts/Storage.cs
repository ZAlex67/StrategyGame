using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private int _resourceNumber;

    public event Action<int> NumberChanged;

    public void PutResource()
    {
        _resourceNumber++;
        NumberChanged?.Invoke(_resourceNumber);
    }
}