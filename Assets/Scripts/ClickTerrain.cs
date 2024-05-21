using System;
using UnityEngine;

public class ClickTerrain : MonoBehaviour
{
    public Action<RaycastHit> ClickedTerrain;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        RaycastHit hit;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            ClickedTerrain?.Invoke(hit);
        }
    }
}