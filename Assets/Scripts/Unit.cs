using DG.Tweening;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private bool _status = true;
    private Resource _currentResource;
    private Base _base;
    private WaitingArea _waitingArea;
    private float _duration = 5f;

    public bool Status => _status;

    private void Update()
    {
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 20000, sequencesCapacity: 200);

        if (_currentResource.InUnit)
        {
            _currentResource.SetResource(false);
            transform.DOMove(_base.transform.position, _duration);
        }

        if (_currentResource.transform.parent != null)
        {
            transform.DOMove(_base.transform.position, _duration);
        }

        if (_base.ResourcesCount == 0 && _currentResource.InUnit == true)
        {
            _status = true;
        }

        if (_status == true)
        {
            transform.DOMove(_waitingArea.transform.position, _duration);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<Resource>(out Resource resource) && resource == _currentResource)
        {
            TakeResource(_currentResource);
            _currentResource.SetResource(true);
        }

        if (collision.TryGetComponent<Base>(out Base basePoint) && _currentResource.InUnit == false)
        {
            _currentResource.transform.parent = null;
            _currentResource.gameObject.SetActive(false);
            SetStatus(true);
        }
    }

    public void TakeResource(Resource resource)
    {
        resource.transform.parent = transform;
        resource.transform.position = transform.position;
    }

    public void GetBasePosition(Base basePosition)
    {
        _base = basePosition;
    }

    public void GetWaitingAreaPosition(WaitingArea areaPosition)
    {
        _waitingArea = areaPosition;
    }

    public void GetResources(Resource resource)
    {
        _currentResource = resource;
    }

    public void SetStatus(bool status)
    {
        _status = status;
    }
}