using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _bagPoint;

    private bool _hasResource = true;
    private Resource _currentResource;
    private Base _base;
    private Flag _flag;
    private bool _createNewBase = false;

    private void Update()
    {
        if (_hasResource && _currentResource != null && _createNewBase == false && _currentResource.transform.parent == null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentResource.transform.position, _speed * Time.deltaTime);
            transform.LookAt(_currentResource.transform);
        }
        else if (_hasResource == false && _createNewBase == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _base.transform.position, _speed * Time.deltaTime);
            transform.LookAt(_base.transform);
        }
        else if (_flag != null && _createNewBase)
        {
            transform.position = Vector3.MoveTowards(transform.position, _flag.transform.position, _speed * Time.deltaTime);
            transform.LookAt(_flag.transform);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Resource resource) && resource == _currentResource)
        {
            TakeResource(_currentResource);
            SetPlaceResourceFalse();
        }

        if (collision.TryGetComponent(out Base basePoint) && _hasResource == false)
        {
            _currentResource.transform.parent = null;
            SetPlaceResourceTrue();
            basePoint.AddFreeBot(this);
            gameObject.SetActive(false);
        }

        if (collision.TryGetComponent(out Flag flag) && _createNewBase)
        {
            Base newBase = Instantiate(_base, _flag.transform.position, Quaternion.identity);
            Destroy(flag.gameObject);
            newBase.AddFreeBot(this);
            _createNewBase = false;
        }
    }

    public void TakeResource(Resource resource)
    {
        resource.transform.parent = _bagPoint;
        resource.transform.position = _bagPoint.transform.position;
    }

    public void SetBasePosition(Base basePosition)
    {
        _base = basePosition;
    }

    public void SetResources(Resource resource)
    {
        _currentResource = resource;
    }

    public void SetFlag(Flag flag)
    {
        _flag = flag;
    }

    public void SetCreateNewBaseTrue()
    {
        _createNewBase = true;
    }

    private void SetPlaceResourceTrue()
    {
        _hasResource = true;
    }

    private void SetPlaceResourceFalse()
    {
        _hasResource = false;
    }
}