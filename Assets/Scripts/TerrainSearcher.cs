using System;
using UnityEngine;

public class TerrainSearcher : MonoBehaviour
{
    private Camera _camera;

    public event Action<RaycastHit> ClickedTerrain;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnMouseUp()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            ClickedTerrain?.Invoke(hit);
        }
    }
}