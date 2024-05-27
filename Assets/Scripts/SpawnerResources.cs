using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnerResources : MonoBehaviour
{
    [SerializeField] private Resource[] _resourse;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private Vector3 _minPosition;
    [SerializeField] private Vector3 _maxPosition;

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
        StartCoroutine(SpawnResource());
    }

    public void ReleaseResourse(Resource resource)
    {
        resource.transform.parent = null;   
        _pool.Release(resource);
    }

    private void ActionOnGet(Resource resourse)
    {
        Vector3 newPosition = new Vector3(GetRandomPositionX(), _minPosition.y, GetRandomPositionZ());

        resourse.transform.position = newPosition;
        resourse.gameObject.SetActive(true);
    }

    private void GetResourse()
    {
        _pool.Get();
    }

    private float GetRandomPositionX()
    {
        return Random.Range(_minPosition.x, _maxPosition.x);
    }

    private float GetRandomPositionZ()
    {
        return Random.Range(_minPosition.z, _maxPosition.z);
    }

    private IEnumerator SpawnResource()
    {
        WaitForSeconds wait = new WaitForSeconds(_repeatRate);

        while (true)
        {
            GetResourse();
            yield return wait;
        }
    }
}