using System.Collections.Generic;
using UnityEngine;

public class ResourcesDistributor : MonoBehaviour
{
    [SerializeField] private ResourceSearch _resourceSearcher;

    private List<Resource> _resources;

    private void Awake()
    {
        _resources = new List<Resource>();
    }

    private void Update()
    {
        if (_resources.Count == 0)
        {
            _resourceSearcher.MapInspection();
        }
    }

    private void OnEnable()
    {
        _resourceSearcher.SearchedResources += OnSearchedResources;
    }

    private void OnDisable()
    {
        _resourceSearcher.SearchedResources -= OnSearchedResources;
    }

    public Resource GiveResourse()
    {
        Resource resource = _resources[Random.Range(0, _resources.Count)];
        _resources.Remove(resource);

        return resource;
    }

    private void OnSearchedResources(List<Resource> resources)
    {
        foreach (Resource resource in resources)
        {
            if (_resources.Contains(resource) == false)
            {
                _resources.Add(resource);
            }
        }
    }
}