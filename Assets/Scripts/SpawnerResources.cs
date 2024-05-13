using UnityEngine;
using UnityEngine.Pool;

public class SpawnerResources : MonoBehaviour
{
    [SerializeField] private Resource[] _resourse;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

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
        GetResourse();
    }

    private void ActionOnGet(Resource resourse)
    {
        float minPositionX = 5f;
        float maxPositionX = 45f;
        float positionY = 0;
        float minPositionZ = 2f;
        float maxPositionZ = 20f;

        Vector3 newPosition = new Vector3(Random.Range(minPositionX, maxPositionX), positionY, Random.Range(minPositionZ, maxPositionZ));

        resourse.transform.position = newPosition;
        resourse.gameObject.SetActive(true);
    }

    private void GetResourse()
    {
        for (int i = 0; i < _poolMaxSize; i++)
            _pool.Get();
    }
}