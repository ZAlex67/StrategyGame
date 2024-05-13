using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Base : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ResourceSearch _resourceSearcher;
    [SerializeField] private WaitingArea _waitingArea;

    private List<Resource> _resources;
    private List<Unit> _units;
    private int _maxUnit = 3;

    public int ResourcesCount => _resources.Count;

    public event Action TookResource;

    private void Awake()
    {
        _resources = new List<Resource>();
        _units = new List<Unit>();
    }

    private void Update()
    {
        if (_resources.Count == 0)
        {
            _resourceSearcher.MapInspection();
        }

        if (_resources.Count > 0 && _units.Count > 0)
        {
            foreach (var unit in _units)
            {
                if (unit.Status)
                {
                    MoveToResource(unit);
                }
            }
        }

        if (_resources.Count == 0)
        {
            _units.Clear();
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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<Resource>(out Resource resource))
        {
            TookResource?.Invoke();
        }
    }

    public void Work()
    {
        if (_units.Count < _maxUnit)
        {
            InitUnit();
        }
    }

    private void MoveToResource(Unit unit)
    {
        Resource resource = _resources[Random.Range(0, _resources.Count)];
        _resources.Remove(resource);

        unit.SetStatus(false);
        unit.GetResources(resource);
        unit.GetBasePosition(this);
        unit.GetWaitingAreaPosition(_waitingArea);
    }

    private void InitUnit()
    {
        Unit unit = Instantiate(_unit, _spawnPoint);
        _units.Add(unit);
    }

    private void OnSearchedResources(Resource resource)
    {
        _resources.Add(resource);
    }
}