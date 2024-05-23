using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _bagPoint;

    private bool _hasResource = false;
    private Resource _currentResource;
    private Base _base;
    private Flag _flag;
    private bool _createNewBase = false;
    private Coroutine _coroutine;

    private void OnTriggerEnter(Collider collision)
    {
        TakeResource(collision);
        PutInBase(collision);
        TakeFlag(collision);
    }

    public void SetBasePosition(Base basePosition)
    {
        _base = basePosition;
    }

    public void SetResources(Resource resource)
    {
        _currentResource = resource;

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(GoToTarget(_currentResource.transform));
    }

    public void SetFlag(Flag flag)
    {
        _flag = flag;

        if (_flag != null)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(GoToTarget(_flag.transform));
        }
    }

    public void SendCreateNewBase()
    {
        _createNewBase = true;
    }

    private void PutInUnit(Resource resource)
    {
        resource.transform.parent = _bagPoint;
        resource.transform.position = _bagPoint.transform.position;
    }

    private IEnumerator GoToTarget(Transform target)
    {
        while (transform.position != target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, _speed * Time.deltaTime);
            transform.LookAt(target.transform);
            yield return null;
        }
    }

    private void TakeResource(Collider collision)
    {
        if (collision.TryGetComponent(out Resource resource) && resource == _currentResource)
        {
            PutInUnit(_currentResource);
            _hasResource = true;

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(GoToTarget(_base.transform));
        }
    }

    private void PutInBase(Collider collision)
    {
        if (collision.TryGetComponent(out Base basePoint) && _hasResource)
        {
            _currentResource.transform.parent = null;
            _hasResource = false;
            basePoint.AddFreeBot(this);
            gameObject.SetActive(false);
        }
    }

    private void TakeFlag(Collider collision)
    {
        if (collision.TryGetComponent(out Flag flag) && _createNewBase && _flag)
        {
            Base newBase = Instantiate(_base, _flag.transform.position, _base.transform.rotation);
            flag.gameObject.SetActive(false);
            newBase.AddFreeBot(this);
            _createNewBase = false;
        }
    }
}