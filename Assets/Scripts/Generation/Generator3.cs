using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator3 : MonoBehaviour
{

    //TODO: Add redundency for checking out of index nodes and moving more than one  node at a time
    public GameObject roadPrefab;
    public GameObject turningRoadPrefab;
    public Transform player;
    public Vector2Int startPosition;
    public int levelDistance;
    public int maxStraight;
    public int averageStraight;
    public GameObject[] buildingPrefabs;
    public int lookAhead;
    public float spawnHeight;

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
            prependNode.direction = direction;
            prependNode.road = Instantiate(roadPrefab, new Vector3(prependPosition.x, spawnHeight, prependPosition.y), Quaternion.identity);
            prependNode.road.SetActive(false);
            prependedRoads.Add(prependNode);
            prependPosition += direction; // Move position one step in the same direction
        }

        // Generate normal roads
        prependPosition = startPosition;
        Node startNode = new Node(startPosition, currentRoadType);
        startNode.direction = direction;
        startNode.road = Instantiate(roadPrefab, new Vector3(startPosition.x, spawnHeight, startPosition.y), Quaternion.identity);
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
            nextNode.direction = direction;
            nextNode.road = Instantiate(roadPrefab, new Vector3(nextPosition.x, spawnHeight, nextPosition.y), GetRotationFromDirection(direction));
            nextNode.road.SetActive(false);
            roads.Add(nextNode);
        }

        // Add the prepended roads at the beginning of the roads list
        roads.InsertRange(0, prependedRoads);
        currentNodeIndex += prependedRoads.Count; // Update the current node index
    }


    public void ChangeDirection() 
    {
        Vector2Int oldDirection = direction;
        direction = new Vector2Int(direction.y, direction.x);

        if (oldDirection == Vector2Int.up && direction == Vector2Int.right)
        {
            roads[roads.Count - 1].road = Instantiate(turningRoadPrefab, roads[roads.Count - 1].road.transform.position, Quaternion.Euler(0, 180, 0));
            roads[roads.Count - 1].road.SetActive(false);
        }
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
        
        SpawnBuildings(road);
    }

    private void DespawnRoad(Node road)
    {
        road.road.SetActive(false);

        DespawnBuildings(road);
    }

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

                // Deactivate the road at the back of the range
                DespawnRoad(roads[currentNodeIndex - lookAhead]);

                // Activate the road at the front of the range, if it exists
                if (currentNodeIndex + lookAhead < roads.Count)
                {
                    SpawnRoad(roads[currentNodeIndex + lookAhead]);
                }

                currentNodeIndex++;
            }
            else if (roads[currentNodeIndex - 1].position == playerRoadPosition)
            {

                // Deactivate the road at the front of the range
                if (currentNodeIndex + lookAhead - 1 < roads.Count)
                {
                    DespawnRoad(roads[currentNodeIndex + lookAhead - 1]);
                }

                // Activate the road at the back of the range
                SpawnRoad(roads[currentNodeIndex - lookAhead]);

                currentNodeIndex--;
            }
        }
    } 

    private Quaternion GetRotationFromDirection(Vector2Int direction)
    {
        // Change the rotation based on the direction
        switch (direction)
        {
            case var d when d.Equals(new Vector2Int(0, 1)): // North
                return Quaternion.Euler(0, 0, 0);
            case var d when d.Equals(new Vector2Int(0, -1)): // South
                return Quaternion.Euler(0, 180, 0);
            case var d when d.Equals(new Vector2Int(1, 0)): // East
                return Quaternion.Euler(0, 90, 0);
            case var d when d.Equals(new Vector2Int(-1, 0)): // West
                return Quaternion.Euler(0, -90, 0);
            default:
                return Quaternion.identity; // Default case should never be hit with your constraints
        }
    }

    #endregion

    private void SpawnBuildings(Node road) 
    {
        if (road.leftBuilding == null) 
        {
            Vector2Int leftOffset = new Vector2Int(-road.direction.y, road.direction.x);
            Vector2Int leftBuildingPos = road.position + leftOffset;
            road.leftBuilding = Instantiate(buildingPrefabs[Random.Range(0, buildingPrefabs.Length)], new Vector3(leftBuildingPos.x, spawnHeight, leftBuildingPos.y), Quaternion.identity);
        }
        else if (road.leftBuilding.tag == "Building")
        {
            road.leftBuilding.SetActive(true);
        }

        if (road.rightBuilding == null)
        {
            Vector2Int rightOffset = new Vector2Int(road.direction.y, -road.direction.x);  
            Vector2Int rightBuildingPos = road.position + rightOffset;
            road.rightBuilding = Instantiate(buildingPrefabs[Random.Range(0, buildingPrefabs.Length)], new Vector3(rightBuildingPos.x, spawnHeight, rightBuildingPos.y), Quaternion.identity);
        }
        else if (road.rightBuilding.tag == "Building")
        {
            road.rightBuilding.SetActive(true);
        }
    }


    private void DespawnBuildings(Node road) 
    {
        if (road.leftBuilding != null && road.leftBuilding.tag == "Building") 
        {
            road.leftBuilding.SetActive(false);
        }

        if (road.rightBuilding != null && road.rightBuilding.tag == "Building")
        {
            road.rightBuilding.SetActive(false);
        }
    }

}
