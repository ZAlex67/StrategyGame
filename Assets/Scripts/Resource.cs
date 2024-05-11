using UnityEngine;

public class Resource : MonoBehaviour
{
    private bool _inUnit = false;

    public bool InUnit => _inUnit;

    public void SetResource(bool inUnit)
    {
        _inUnit = inUnit;
    }
}