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

    private Vector2Int direction;
    private List<Node> roads;
    private string currentRoadType;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = new Vector2Int(0, 0);
        direction = new Vector2Int(0, 1);
        currentRoadType = "N";
        roads = new List<Node>();

        GenerateRoads();
        InstantiateRoads();
    }

    #region Roads

    public void GenerateRoads() 
    {
        int currentStraight = 0;
        roads.Add(new Node(startPosition, currentRoadType));

        for (int i = 0; i < levelDistance; i++)
        {
            currentStraight++;
            float random = Random.Range(0f, 1f);
            if (random > Mathf.Pow(.5f, currentStraight / averageStraight) + Random.Range(-.1f, .1f))
            {
                ChangeDirection();
                currentStraight = 0;
            }
            roads.Add(new Node(roads[roads.Count - 1].position + direction, currentRoadType));
        }
    }

    public void ChangeDirection() 
    {
        direction = new Vector2Int(direction.y, direction.x);
    }

    public void InstantiateRoads() 
    {
        foreach (Node node in roads)
        {
            Instantiate(roadPrefab, new Vector3(node.position.x, .5f, node.position.y), Quaternion.identity);
        }
    }

    #endregion



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateRoads();
            InstantiateRoads();
        }
    }
}
