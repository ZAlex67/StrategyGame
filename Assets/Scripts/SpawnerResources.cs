using UnityEngine;
using UnityEngine.Pool;

public class SpawnerResources : MonoBehaviour
{
    [SerializeField] private Resource[] _resourse;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] Base _base;
    [SerializeField] private float _minPositionX = 5f;
    [SerializeField] private float _maxPositionX = 45f;
    [SerializeField] private float _positionY = 0;
    [SerializeField] private float _minPositionZ = 2f;
    [SerializeField] private float _maxPositionZ = 20f;


    private ObjectPool<Resource> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Resource>(
            createFunc: () => Instantiate(_resourse[Random.Range(0, _resourse.Length)]),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetResourse), 0f, _repeatRate);
    }

    private void OnEnable()
    {
        _base.TookResource += OnReleaseResourse;
    }

    private void OnDisable()
    {
        _base.TookResource -= OnReleaseResourse;
    }

    private void ActionOnGet(Resource resourse)
    {
        Vector3 newPosition = new Vector3(Random.Range(_minPositionX, _maxPositionX), _positionY, Random.Range(_minPositionZ, _maxPositionZ));

        resourse.transform.position = newPosition;
        resourse.gameObject.SetActive(true);
    }

    private void GetResourse()
    {
        _pool.Get();
    }

    private void OnReleaseResourse(Resource resource)
    {
        resource.transform.parent = null;   
        _pool.Release(resource);
    }
}