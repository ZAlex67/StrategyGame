using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _bagPoint;

    private bool _statusPlaceResource = true;
    private Resource _currentResource;
    private Base _base;

    public bool StatusPlaceResource => _statusPlaceResource;

    private void Update()
    {
        if (_statusPlaceResource && _currentResource != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentResource.transform.position, _speed * Time.deltaTime);
            transform.LookAt(_currentResource.transform);
        }
        else if (_statusPlaceResource == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _base.transform.position, _speed * Time.deltaTime);
            transform.LookAt(_base.transform);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Resource resource) && resource == _currentResource)
        {
            TakeResource(_currentResource);
            SetStatusPlaceResourceFalse();
        }

        if (collision.TryGetComponent(out Base basePoint) && _statusPlaceResource == false)
        {
            _currentResource.transform.parent = null;
            SetStatusPlaceResourceTrue();
            basePoint.AddFreeBot(this);
            gameObject.SetActive(false);
        }
    }

    private void SetStatusPlaceResourceTrue()
    {
        _statusPlaceResource = true;
    }

    private void SetStatusPlaceResourceFalse()
    {
        _statusPlaceResource = false;
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
}