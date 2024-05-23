using System;
using UnityEngine;

public class SearchTerrain : MonoBehaviour
{
    public Action<RaycastHit> ClickedTerrain;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            ClickedTerrain?.Invoke(hit);
        }
    }
}