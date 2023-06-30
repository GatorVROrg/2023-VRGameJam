using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public Generator3 generator; // Assign waypoints in Unity inspector
    public float speed = 1.0f;
    public int waypointIndex = 0;
    public List<Transform> waypoints;

    void Start() 
    {
        waypoints = new List<Transform>();
    }

    void Update()
    {
        if (waypoints.Count == 0)
        {
            if (generator != null)
            {
                for (int i = 0; i  < generator.roads.Count; i++)
                {
                    waypoints.Add(generator.roads[i].road.transform);
                }
            }
        }
        else {
            // If the ghost has reached the current waypoint...
            if (Vector3.Distance(transform.position, waypoints[waypointIndex].position) < 0.1f)
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
            transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].position, speed * Time.deltaTime);

            // Face towards the next waypoint
            Vector3 direction = waypoints[waypointIndex].position - transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);
            }
        }
    }
}
