using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Base : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ResourceSearch _resourceSearcher;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private SearchTerrain _clickTerrain;
    [SerializeField] private SpawnerResources _spawner;
    [SerializeField] private Storage _storage;
    [SerializeField] private ResourcesDistributor _resourcesDistributor;

    private List<Resource> _resources;
    private List<Resource> _resourcesForUnit;
    private List<Unit> _units;
    private int _maxUnit = 3;
    private Camera _camera;
    private Flag _flag;
    private bool _isSelected;
    private int _unitsCount = 6;
    private int _basePrice = 5;
    private int _unitPrice = 3;

    public event Action<int> ObjectBought;

    public int ResourcesCount => _resources.Count;

    private void Awake()
    {
        _resources = new List<Resource>();
        _resourcesForUnit = new List<Resource>();
        _units = new List<Unit>();
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _clickTerrain.ClickedTerrain += OnClickedTarrain;
    }

    private void OnDisable()
    {
        _clickTerrain.ClickedTerrain -= OnClickedTarrain;
    }

    private void Start()
    {
        InitUnit();
    }

    private void Update()
    {
        if (_storage.ResourceNumber >= _basePrice && _flag != null)
        {
            SendUnitToCreateBase();
            ObjectBought?.Invoke(_basePrice);
            _flag = null;
        }
        else if (_storage.ResourceNumber >= _unitPrice && _maxUnit < _unitsCount && _flag == null)
        {
            CreateNewUnit();
            ObjectBought?.Invoke(_unitPrice);
            _maxUnit++;
        }
        else
        {
            SendUnitToResource();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Resource resource))
        {
            _spawner.ReleaseResourse(resource);
            _storage.PutResource();
        }
    }

    private void OnMouseDown()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Base basePoint))
            {
                if (_isSelected)
                {
                    _isSelected = false;
                }
                else
                {
                    _isSelected = true;
                }
            }
        }
    }

    public void AddFreeBot(Unit unit)
    {
        _units.Add(unit);
    }

    private void OnClickedTarrain(RaycastHit raycastHit)
    {
        if (raycastHit.collider.TryGetComponent(out SearchTerrain terrain) && _isSelected && Input.GetMouseButtonUp(0))
        {
            if (_flag == null)
            {
                _flag = Instantiate(_flagPrefab, raycastHit.point, Quaternion.identity);
            }
            else if (_flag != null)
            {
                _flag.transform.position = raycastHit.point;
            }

            _isSelected = false;
        }
    }

    private void SendUnitToCreateBase()
    {
        if (_units.Count > 0)
        {
            Unit unit = _units[Random.Range(0, _units.Count)];
            _units.Remove(unit);

            unit.SendCreateNewBase();
            unit.gameObject.SetActive(true);
            unit.SetFlag(_flag);
        }
    }

    private void SendUnitToResource()
    {
        if (_units.Count > 0 && _resources.Count > 0)
        {
            Resource resource = _resources[Random.Range(0, _resources.Count)];
            _resources.Remove(resource);

            _resourcesForUnit.Add(resource);

            Unit unit = _units[Random.Range(0, _units.Count)];
            _units.Remove(unit);

            unit.gameObject.SetActive(true);
            unit.SetResources(resource);
        }
        else if (_resources.Count == 0)
        {
            GetNewResourse();
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

    private void CreateNewUnit()
    {
        Unit unit = Instantiate(_unit, _spawnPoint);
        _units.Add(unit);
        unit.SetBasePosition(this);
    }

    private void GetNewResourse()
    {
        Resource resource = _resourcesDistributor.GiveResourse();

        if (_resourcesForUnit.Contains(resource) == false)
        {
            _resources.Add(resource);
        }
    }
}