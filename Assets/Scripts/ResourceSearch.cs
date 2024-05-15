using System;
using UnityEngine;

public class ResourceSearch : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private Collider[] _resourceCandidates;
    private float _radius = 100f;

    public event Action<Resource> SearchedResources;
    
    public void MapInspection()
    {
        _resourceCandidates = Physics.OverlapSphere(transform.position, _radius, _layerMask);

        foreach (var sphere in _resourceCandidates)
        {
            if (sphere.TryGetComponent(out Resource resource))
            {
                SearchedResources?.Invoke(resource);
            }
        }
    }
}