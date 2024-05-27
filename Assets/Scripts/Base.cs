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
    private int _maxUnit = 3;
    private Camera _camera;
    private Flag _flag;
    private bool _isSelected;
    private int _unitsCount = 6;
    private int _basePrice = 5;
    private int _unitPrice = 3;
    private bool _isNewBase = false;

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
        _storage.PutResource(-_storage.ResourceNumber);
        InitUnit();
    }

    private void Update()
    {
        if (_storage.ResourceNumber >= _basePrice && _flag != null && _isNewBase)
        {
            SendUnitToCreateBase();
        }
        else if (_storage.ResourceNumber >= _unitPrice && _maxUnit < _unitsCount && _flag == null)
        {
            CreateNewUnit();
            _storage.PutResource(-_unitPrice);
            _maxUnit++;
        }
        else
        {
            SendUnitToResource();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        int _resourceNumber = 1;

        if (collision.TryGetComponent(out Resource resource))
        {
            _spawner.ReleaseResourse(resource);
            _storage.PutResource(_resourceNumber);
            _resourcesDistributor.RemoveResource(resource);
        }
    }

    private void OnMouseDown()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Base basePoint) && _isSelected == false)
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

    public void CreateNewBase()
    {
        _isNewBase = false;
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

            _isNewBase = true;
            _isSelected = false;
        }
    }

    private void SendUnitToCreateBase()
    {
        if (_units.Count > 0)
        {
            Unit unit = _units[Random.Range(0, _units.Count)];
            _units.Remove(unit);

            _isNewBase = false;
            _storage.PutResource(-_basePrice);
            unit.SendCreateNewBase();
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
}