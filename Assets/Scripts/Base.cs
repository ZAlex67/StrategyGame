using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Base : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ResourceSearch _resourceSearch;
    [SerializeField] private WaitingArea _waitingArea;

    private List<Resource> _resources;
    private List<Unit> _units;
    private float _duration = 5f;
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
            _resourceSearch.MapInspection();
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

    public void Work()
    {
        if (_units.Count < _maxUnit)
        {
            InitUnit();
        }
    }

    private void OnEnable()
    {
        _resourceSearch.Searching += OnAddResources;
    }

    private void OnDisable()
    {
        _resourceSearch.Searching -= OnAddResources;
    }

    private void MoveToResource(Unit unit)
    {
        Resource resource = _resources[Random.Range(0, _resources.Count)];
        _resources.Remove(resource);

        //unit.transform.DOLookAt(resource.transform.position, 10f);
        unit.transform.DOMove(resource.transform.position, _duration);
        unit.SetStatus(false);
        unit.GetResources(resource);
        unit.GetBasePosition(this);
        unit.GetWaitingAreaPosition(_waitingArea);
        Debug.Log(_resources.Count);
    }

    private void InitUnit()
    {
        Unit unit = Instantiate(_unit, _spawnPoint);
        _units.Add(unit);
    }

    private void OnAddResources(Resource resource)
    {
        _resources.Add(resource);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<Resource>(out Resource resource))
        {
            TookResource?.Invoke();
        }
    }
}