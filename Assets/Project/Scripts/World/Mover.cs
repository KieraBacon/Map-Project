using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private List<Transform> _waypoints;
    private int _waypointIndex;
    [SerializeField] private float _moveSpeed;

    private void Update()
    {
        float moveSpeed = _moveSpeed * Time.deltaTime;
        int numWaypoints = _waypoints.Count;
        int nextIndex() => (_waypointIndex + 1) % numWaypoints;
        
        Transform myTransform = transform;
        Vector3 position = myTransform.position;
        Quaternion rotation = myTransform.rotation;

        Transform nextTransform = _waypoints[nextIndex()];
        Vector3 nextPosition = nextTransform.position;
        Quaternion nextRotation = nextTransform.rotation;
        if (Vector3.Distance(position, nextPosition) < 0.01f)
        {
            _waypointIndex = nextIndex();
            nextTransform = _waypoints[nextIndex()];
            nextPosition = nextTransform.position;
            nextRotation = nextTransform.rotation;
        }
        Vector3 lastPosition = _waypoints[_waypointIndex].position;
        Quaternion lastRotation = _waypoints[_waypointIndex].rotation;

        myTransform.position = Vector3.MoveTowards(position, nextPosition, moveSpeed);
        position = myTransform.position;
        float t = 1 - (Vector3.Distance(position, nextPosition) / Vector3.Distance(lastPosition, nextPosition));
        myTransform.rotation = Quaternion.Lerp(lastRotation, nextRotation, t);
    }
}
