using UnityEngine;

public class WaitingArea : MonoBehaviour
{
    [SerializeField] private Base _base;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<Unit>(out Unit unit))
        {
            unit.gameObject.SetActive(false);
        }
    }
}