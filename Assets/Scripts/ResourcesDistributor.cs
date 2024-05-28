using System.Collections.Generic;
using UnityEngine;

public class ResourcesDistributor : MonoBehaviour
{
    [SerializeField] private ResourceSearch _resourceSearcher;

    private List<Resource> _resources;
    private List<Resource> _resourcesAlreadyUsed;

    private void Awake()
    {
        _resources = new List<Resource>();
        _resourcesAlreadyUsed = new List<Resource>();
    }

    private void OnEnable()
    {
        _resourceSearcher.SearchedResources += OnSearchedResources;
    }

    private void OnDisable()
    {
        _resourceSearcher.SearchedResources -= OnSearchedResources;
    }

    private void Update()
    {
        if (_resources.Count == 0)
        {
            _resourceSearcher.MapInspection();
        }
    }

    public Resource GiveResourse()
    {
        if (_resources.Count > 0)
        {
            Resource resource = _resources[Random.Range(0, _resources.Count)];
            _resources.Remove(resource);
            _resourcesAlreadyUsed.Add(resource);

            return resource;
        }
        else
        {
            return null;
        }
    }

    public void RemoveResource(Resource resource)
    {
        _resourcesAlreadyUsed.Remove(resource);
    }

    private void OnSearchedResources(List<Resource> resources)
    {
        foreach (Resource resource in resources)
        {
            if (_resourcesAlreadyUsed.Contains(resource) == false && _resources.Contains(resource) == false)
            {
                _resources.Add(resource);
            }
        }
    }
}