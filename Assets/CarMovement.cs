using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 1.0f;
    public float rotationSpeed = 10.0f;
    public int waypointIndex = 0;
    public List<Vector3> waypoints;

    void Start() 
    {
    }

    void Update()
    {
        if (waypoints.Count != 0) {
            // If the car has reached the current waypoint...
            if (Vector3.Distance(transform.position, waypoints[waypointIndex]) < 0.1f)
            {
                // Move on to the next waypoint
                waypointIndex++;
                // If we've reached the last waypoint, go back to the first one
                if (waypointIndex >= waypoints.Count)
                {
                    waypointIndex = 0;
                }
            }

            // Move towards the current waypoint
            transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex], speed * Time.deltaTime);

            // Calculate direction to the next waypoint
            Vector3 targetDirection = (waypoints[waypointIndex] - transform.position).normalized;

            // Calculate a rotation a step closer to the target and applies rotation to this object
            float singleStep = rotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            if (waypointIndex == waypoints.Count - 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
