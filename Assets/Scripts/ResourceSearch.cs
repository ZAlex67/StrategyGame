using System;
using UnityEngine;

public class ResourceSearch : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private Collider[] _spheresColliders;
    private float _radius = 100f;

    public event Action<Resource> SearchedResources;
    
    public void MapInspection()
    {
        _spheresColliders = Physics.OverlapSphere(transform.position, _radius, _layerMask);

        foreach (var sphere in _spheresColliders)
        {
            if (sphere.TryGetComponent(out Resource resource))
            {
                SearchedResources?.Invoke(resource);
            }
        }
    }
}