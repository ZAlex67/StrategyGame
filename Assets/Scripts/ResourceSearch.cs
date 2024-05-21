using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSearch : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private Collider[] _resourceCandidates;
    private float _radius = 100f;
    private List<Resource> _resources = new List<Resource>();

    public event Action<Resource> SearchedResources;
    
    public void MapInspection()
    {
        _resourceCandidates = Physics.OverlapSphere(transform.position, _radius, _layerMask);

        foreach (var sphere in _resourceCandidates)
        {
            if (sphere.TryGetComponent(out Resource resource) && (_resources.Contains(resource) == false))
            {
                _resources.Add(resource);
                SearchedResources?.Invoke(resource);
            }
        }
    }
}