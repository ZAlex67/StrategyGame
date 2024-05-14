using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Storage))]
public class Base : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ResourceSearch _resourceSearcher;

    private List<Resource> _resources;
    private List<Unit> _units;
    private int _maxUnit = 3;
    private Storage _storage;

    public int ResourcesCount => _resources.Count;

    public event Action<Resource> TookResource;

    private void Awake()
    {
        _resources = new List<Resource>();
        _units = new List<Unit>();
        _storage = GetComponent<Storage>();
    }

    private void Start()
    {
        InitUnit();
    }

    private void Update()
    {
        MoveToResource();
    }

    private void OnEnable()
    {
        _resourceSearcher.SearchedResources += OnSearchedResources;
    }

    private void OnDisable()
    {
        _resourceSearcher.SearchedResources -= OnSearchedResources;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<Resource>(out Resource resource))
        {
            TookResource?.Invoke(resource);
            _storage.PutResource();
        }
    }

    private void MoveToResource()
    {
        if (_resources.Count > 0 && _units.Count > 0)
        {
            Resource resource = _resources[Random.Range(0, _resources.Count)];
            _resources.Remove(resource);

            Unit unit = _units[Random.Range(0, _units.Count)];
            _units.Remove(unit);

            unit.gameObject.SetActive(true);
            unit.SetResources(resource);
        }
        else if (_resources.Count == 0 && _units.Count == _maxUnit)
        {
            _resourceSearcher.MapInspection();
        }
    }

    private void InitUnit()
    {
        for (int i = 0; i < _maxUnit; i++)
        {
            Unit unit = Instantiate(_unit, _spawnPoint);
            _units.Add(unit);
            unit.SetBasePosition(this);
        }
    }

    private void OnSearchedResources(Resource resource)
    {
        _resources.Add(resource);
    }

    public void AddFreeBot(Unit unit)
    {
        _units.Add(unit);
    }
}