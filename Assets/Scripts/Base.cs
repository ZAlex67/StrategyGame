using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ResourceSearch _resourceSearcher;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private TerrainSearcher _clickTerrain;
    [SerializeField] private SpawnerResources _spawner;
    [SerializeField] private Storage _storage;
    [SerializeField] private ResourcesDistributor _resourcesDistributor;

    private List<Unit> _units;
    private int _maxCount = 3;
    private Camera _camera;
    private Flag _flag;
    private bool _isSelected;
    private int _unitsMax = 6;
    private int _basePrice = 5;
    private int _unitPrice = 3;
    private bool _canCreateNewBase = false;

    private void Awake()
    {
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
        if (_storage.ResourceNumber >= _basePrice && _flag != null && _canCreateNewBase)
        {
            SendUnitToCreateBase();
        }
        else if (_storage.ResourceNumber >= _unitPrice && _maxCount < _unitsMax && _flag == null)
        {
            CreateNewUnit();
            _storage.WithdrawResource(_unitPrice);
            _maxCount++;
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
            _resourcesDistributor.RemoveResource(resource);
        }
    }

    private void OnMouseDown()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Base basePoint) && basePoint == this)
            {
                _isSelected = !_isSelected;
            }
        }
    }

    public void AddFreeBot(Unit unit)
    {
        _units.Add(unit);
    }

    public void CreateNewBase()
    {
        _canCreateNewBase = false;
    }

    private void OnClickedTarrain(RaycastHit raycastHit)
    {
        if (raycastHit.collider.TryGetComponent(out TerrainSearcher terrain) && _isSelected && Input.GetMouseButtonUp(0))
        {
            if (_flag == null)
            {
                _flag = Instantiate(_flagPrefab, raycastHit.point, Quaternion.identity);
            }
            else if (_flag != null)
            {
                _flag.transform.position = raycastHit.point;
            }

            _canCreateNewBase = true;
            _isSelected = false;
        }
    }

    private void SendUnitToCreateBase()
    {
        if (_units.Count > 0)
        {
            Unit unit = _units[Random.Range(0, _units.Count)];
            _units.Remove(unit);

            _canCreateNewBase = false;
            _storage.WithdrawResource(_basePrice);
            unit.gameObject.SetActive(true);
            unit.MoveToFlag(_flag);
        }
    }

    private void SendUnitToResource()
    {
        if (_units.Count > 0)
        {
            Resource resource = _resourcesDistributor.GiveResourse();

            if (resource != null)
            {
                Unit unit = _units[Random.Range(0, _units.Count)];
                _units.Remove(unit);

                unit.gameObject.SetActive(true);
                unit.MoveToResource(resource);
            }
        }
    }

    private void InitUnit()
    {
        for (int i = 0; i < _maxCount; i++)
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
}