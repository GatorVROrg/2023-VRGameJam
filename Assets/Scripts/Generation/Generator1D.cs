using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator1D : MonoBehaviour
{
    public List<GameObject> buildingPrefabs;
    public int levelDistance;
    public int maxStraight;
    public int averageStraight;
    public int padding;
    

    private Vector2 direction;
    private Vector2Int startPosition;
    private Node[,] nodes;
    private List<GameObject> displayCubes;
    private int size;


    // Start is called before the first frame update
    void Start()
    {
        size = levelDistance + padding;
        nodes = new Node[size, size];
        displayCubes = new List<GameObject>();
        GenerateStartingPoint();
        GenerateRoads();
        DisplayCity();
    }

    public void GenerateStartingPoint() {
        // Generate a random angle in degrees and convert it to radians
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        // Convert the angle to a direction vector
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        // Choose a starting point opposite to the direction
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // More horizontal direction
            int x = direction.x > 0 ? padding : nodes.GetLength(0) - padding - 1;
            int y = Random.Range(padding, nodes.GetLength(1) - padding);
            startPosition = new Vector2Int(x, y);
        }
        else
        {
            // More vertical direction
            int x = Random.Range(padding, nodes.GetLength(0) - padding);
            int y = direction.y > 0 ? padding : nodes.GetLength(1) - padding - 1;
            startPosition = new Vector2Int(x, y);
        }

        Node startNode = new Node(startPosition, "Start");
        nodes[startPosition.x, startPosition.y] = startNode;
    }

    public void GenerateRoads() {
    }

    void DisplayCity()
    {
        // Destroy existing cubes
        foreach (GameObject cube in displayCubes)
        {
            Destroy(cube);
        }
        displayCubes.Clear();

        // Iterate over the 2D nodes array
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Node node = nodes[x, y];

                // Create a new cube at this position
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(x, 0, y);
                displayCubes.Add(cube);

                // Set the cube's color based on the node type
                if (node == null)
                {
                    cube.GetComponent<Renderer>().material.color = Color.white;  // Empty
                }
                else
                {
                    switch (node.type)
                    {
                        case "Road":
                            cube.GetComponent<Renderer>().material.color = Color.grey;  // Road
                            break;
                        case "Building":
                            cube.GetComponent<Renderer>().material.color = Color.black;  // Building
                            break;
                        case "Start":
                            cube.GetComponent<Renderer>().material.color = Color.green;  // Start
                            break;
                        case "End":
                            cube.GetComponent<Renderer>().material.color = Color.red;  // End
                            break;
                    }
                }
            }
        }
    }

}