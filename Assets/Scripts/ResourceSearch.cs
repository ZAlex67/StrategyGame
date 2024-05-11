using System;
using UnityEngine;

public class ResourceSearch : MonoBehaviour
{
    private Collider[] _spheres;
    private float _radius = 100f;

    public event Action<Resource> Searching;
    
    public void MapInspection()
    {
        _spheres = Physics.OverlapSphere(transform.position, _radius);

        foreach (var sphere in _spheres)
        {
            if (sphere.TryGetComponent(out Resource resource))
            {
                Searching?.Invoke(resource);
            }
        }
    }
}