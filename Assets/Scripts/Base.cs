using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ResourceSearch _resourceSearcher;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private ClickTerrain _clickTerrain;
    [SerializeField] private SpawnerResources _spawner;
    [SerializeField] private Storage _storage;

    private List<Resource> _resources = new List<Resource>();
    private List<Unit> _units = new List<Unit>();
    private int _maxUnit = 3;
    private Camera _camera;
    private Flag _flag;
    private bool _isClick;
    private int _unitsCount = 6;
    private int _basePrice = 5;
    private int _unitPrice = 3;

    public int ResourcesCount => _resources.Count;

    private void Awake()
    {
        _camera = Camera.main;
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
            _storage.BuyNewBase();
        }
        else if (_storage.ResourceNumber >= _unitPrice && _maxUnit < _unitsCount && _flag == null)
        {
            CreateNewUnit();
            _storage.BuyNewUnit();
            _maxUnit++;
        }
        else
        {
            SendUnitToResource();
        }
    }

    private void OnEnable()
    {
        _resourceSearcher.SearchedResources += OnSearchedResources;
        _clickTerrain.ClickedTerrain += OnClickedTarrain;
    }

    private void OnDisable()
    {
        _resourceSearcher.SearchedResources -= OnSearchedResources;
        _clickTerrain.ClickedTerrain -= OnClickedTarrain;
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
        RaycastHit hit;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent(out Base basePoint))
            {
                if (_isClick)
                {
                    _isClick = false;
                }
                else
                {
                    _isClick = true;
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
        if (raycastHit.collider.TryGetComponent(out ClickTerrain terrain) && _isClick && Input.GetMouseButtonUp(0))
        {
            if (_flag == null)
            {
                _flag = Instantiate(_flagPrefab, raycastHit.point, Quaternion.identity);
            }
            else if (_flag != null)
            {
                Destroy(_flag.gameObject);
                _flag = Instantiate(_flagPrefab, raycastHit.point, Quaternion.identity);
            }

            _isClick = false;
        }
    }

    private void SendUnitToCreateBase()
    {
        if (_units.Count > 0)
        {
            Unit unit = _units[Random.Range(0, _units.Count)];
            _units.Remove(unit);
            
            unit.SetCreateNewBaseTrue();
            unit.gameObject.SetActive(true);
            unit.SetFlag(_flag);
        }
    }

    private void SendUnitToResource()
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
        else if (_resources.Count == 0)
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

    private void CreateNewUnit()
    {
        Unit unit = Instantiate(_unit, _spawnPoint);
        _units.Add(unit);
        unit.SetBasePosition(this);
    }

    private void OnSearchedResources(Resource resource)
    {
        _resources.Add(resource);
    }
}