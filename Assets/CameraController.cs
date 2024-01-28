using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _target;

    void Update()
    {
        float delta = _speed * Time.deltaTime;
        if (Keyboard.current.wKey.isPressed) _target.position += Vector3.forward * delta;
        if (Keyboard.current.aKey.isPressed) _target.position += Vector3.left * delta;
        if (Keyboard.current.sKey.isPressed) _target.position += Vector3.back * delta;
        if (Keyboard.current.dKey.isPressed) _target.position += Vector3.right * delta;
    }
}
