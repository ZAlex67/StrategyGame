using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSearch : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private Collider[] _resourceCandidates;
    private float _radius = 100f;

    public event Action<List<Resource>> SearchedResources;
    
    public void MapInspection()
    {
        List<Resource> resources = new List<Resource>();

        _resourceCandidates = Physics.OverlapSphere(transform.position, _radius, _layerMask);

        foreach (var sphere in _resourceCandidates)
        {
            if (sphere.TryGetComponent(out Resource resource))
            {
                resources.Add(resource);
                SearchedResources?.Invoke(resources);
            }
        }
    }
}