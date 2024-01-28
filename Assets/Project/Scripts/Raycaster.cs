using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Raycaster : MonoBehaviour
{
    [SerializeField] private int _maxRaycastHits;
    private RaycastHit[] _hits;
    private Camera _camera;

    void Update()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
            if (_camera == null) return;
        }

        if (_hits.Length != _maxRaycastHits) _hits = new RaycastHit[_maxRaycastHits];

        if (Pointer.current == null) return;
        Vector2Control pos = Pointer.current.position;
        if (pos == null) return;

        Ray ray = _camera.ScreenPointToRay(pos.value);
        int numHits = Physics.RaycastNonAlloc(ray, _hits);
        for (int i = 0; i < numHits; i++)
        {
            RaycastHit hit = _hits[i];
        }
    }
}