using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public int ResourceNumber { get; private set; }

    public event Action<int> NumberChanged;

    private void Start()
    {
        ResourceNumber = 0;
        NumberChanged?.Invoke(ResourceNumber);
    }

    public void PutResource()
    {
        ResourceNumber++;
        NumberChanged?.Invoke(ResourceNumber);
    }

    public void WithdrawResource(int resourceNumber)
    {
        if (ResourceNumber >= resourceNumber)
        {
            ResourceNumber -= resourceNumber;
            NumberChanged?.Invoke(ResourceNumber);
        }
    }
}