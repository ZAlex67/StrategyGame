using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public int ResourceNumber { get; private set; }

    public event Action<int> NumberChanged;

    public void PutResource(int resourceNumber)
    {
        int resource = 1;

        if (ResourceNumber >= resourceNumber || resourceNumber == resource)
        {
            ResourceNumber += resourceNumber;
            NumberChanged?.Invoke(ResourceNumber);
        }
    }
}