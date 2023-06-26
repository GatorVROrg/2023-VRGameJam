using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator3 : MonoBehaviour
{
    public GameObject roadPrefab;
    public Transform player;
    public Vector2Int startPosition;
    public int levelDistance;
    public int maxStraight;
    public int averageStraight;
    public GameObject[] buildingPrefabs;
    public int lookAhead;


    private Vector2Int direction;
    private List<Node> roads;
    private string currentRoadType;
    private int currentNodeIndex;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = new Vector2Int(0, 0);
        direction = new Vector2Int(0, 1);
        currentRoadType = "N";
        roads = new List<Node>();
        currentNodeIndex = 0;

        GenerateRoads();
        InstantiateRoads();
    }

    void Update()
    {
        UpdateRoads();
        Debug.Log(roads[currentNodeIndex].position);
    }

    #region Roads

    public void GenerateRoads() 
    {
        int currentStraight = 0;
        List<Node> prependedRoads = new List<Node>(); // List of roads to be added at the beginning

        // Generate prepended roads
        Vector2Int prependPosition = startPosition - (direction * lookAhead); // Adjust the start position
        for (int i = 0; i < lookAhead; i++)
        {
            Node prependNode = new Node(prependPosition, currentRoadType);
            prependNode.road = Instantiate(roadPrefab, new Vector3(prependPosition.x, .5f, prependPosition.y), Quaternion.identity);
            prependNode.road.SetActive(false);
            prependedRoads.Add(prependNode);
            prependPosition += direction; // Move position one step in the same direction
        }

        // Generate normal roads
        prependPosition = startPosition;
        Node startNode = new Node(startPosition, currentRoadType);
        startNode.road = Instantiate(roadPrefab, new Vector3(startPosition.x, .5f, startPosition.y), Quaternion.identity);
        startNode.road.SetActive(false);
        roads.Add(startNode);

        for (int i = 0; i < levelDistance; i++)
        {
            currentStraight++;
            float random = Random.Range(0f, 1f);
            if (random > Mathf.Pow(.5f, currentStraight / averageStraight) + Random.Range(-.1f, .1f))
            {
                ChangeDirection();
                currentStraight = 0;
            }
            Vector2Int nextPosition = roads[roads.Count - 1].position + direction;
            Node nextNode = new Node(nextPosition, currentRoadType);
            nextNode.road = Instantiate(roadPrefab, new Vector3(nextPosition.x, .5f, nextPosition.y), Quaternion.identity);
            nextNode.road.SetActive(false);
            roads.Add(nextNode);
        }

        // Add the prepended roads at the beginning of the roads list
        roads.InsertRange(0, prependedRoads);
        currentNodeIndex += prependedRoads.Count; // Update the current node index
    }




    public void ChangeDirection() 
    {
        direction = new Vector2Int(direction.y, direction.x);
    }

    public void InstantiateRoads() 
    {
        for (int i = 0; i < lookAhead * 2; i++)
        {
            if (i > roads.Count - 1) 
            {
                Debug.Log("Errror initalizing roads, look ahead > level length");
            }
            Node node = roads[i];
            SpawnRoad(node);
        }
    }

    private void SpawnRoad(Node road)
    {
        road.road.SetActive(true);
        
        //TODO: Spawn buildings
    }

    private void DespawnRoad(Node road)
    {
        road.road.SetActive(false);

        //TODO: Despawn buildings
    }

    #endregion


    private void UpdateRoads()
    {
        // The position of the player in terms of the roads array
        Vector2Int playerRoadPosition = new Vector2Int(Mathf.FloorToInt(player.position.x), Mathf.FloorToInt(player.position.z));

        // Check if the player has moved to a new road
        if (roads[currentNodeIndex].position != playerRoadPosition)
        {
            // Check if the player has moved forward or backward
            if (roads[currentNodeIndex + 1].position == playerRoadPosition)
            {
                // The player has moved forward
                currentNodeIndex++;

                // Deactivate the road at the back of the range
                DespawnRoad(roads[currentNodeIndex - lookAhead]);

                // Activate the road at the front of the range, if it exists
                if (currentNodeIndex + lookAhead < roads.Count)
                {
                    SpawnRoad(roads[currentNodeIndex + lookAhead]);
                }
            }
            else if (roads[currentNodeIndex - 1].position == playerRoadPosition)
            {
                // The player has moved backward
                currentNodeIndex--;

                // Deactivate the road at the front of the range
                if (currentNodeIndex + lookAhead - 1 < roads.Count)
                {
                    DespawnRoad(roads[currentNodeIndex + lookAhead - 1]);
                }

                // Activate the road at the back of the range
                SpawnRoad(roads[currentNodeIndex - lookAhead]);
            }
        }
    }

}
