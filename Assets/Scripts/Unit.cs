using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;

    private bool _statusPlaceResource = true;
    private Resource _currentResource;
    private Base _base;

    public bool StatusPlaceResource => _statusPlaceResource;

    private void Update()
    {
        if (_statusPlaceResource && _currentResource != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentResource.transform.position, _speed * Time.deltaTime);
        }
        else if (_statusPlaceResource == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _base.transform.position, _speed * Time.deltaTime);
        }     
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<Resource>(out Resource resource) && resource == _currentResource)
        {
            TakeResource(_currentResource);
            SetStatus(false);
        }

        if (collision.TryGetComponent<Base>(out Base basePoint) && _statusPlaceResource == false)
        {
            _currentResource.transform.parent = null;
            SetStatus(true);
            basePoint.AddFreeBot(this);
            gameObject.SetActive(false);
        }
    }

    public void TakeResource(Resource resource)
    {
        resource.transform.parent = transform;
        resource.transform.position = transform.position;
    }

    public void SetBasePosition(Base basePosition)
    {
        _base = basePosition;
    }

    public void SetResources(Resource resource)
    {
        _currentResource = resource;
    }

    public void SetStatus(bool status)
    {
        _statusPlaceResource = status;
    }
}