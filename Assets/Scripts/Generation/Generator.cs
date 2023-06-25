using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    public List<GameObject> buildingPrefabs;  // List of building prefabs
    public int citySizeX;  // The X size of the city
    public int citySizeY;  // The Y size of the city
    public int buildingCount;  // The number of buildings to generate
    private List<GameObject> displayCubes; //used in debugging

    private Node[,] nodes;  // 2D array of nodes representing the city

    // Start is called before the first frame update
    void Start()
    {
        nodes = new Node[citySizeX, citySizeY];
        displayCubes = new List<GameObject>();
        GenerateCity();
        DisplayCity();
    }

    void GenerateCity()
    {
        for (int i = 0; i < buildingCount; i++)
        {
            // Choose a random building prefab
            GameObject buildingPrefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Count)];
            Building building = buildingPrefab.GetComponent<Building>();

            // Choose a random position within the city size
            Vector2Int firstBlockPosition = new Vector2Int(Random.Range(0, citySizeX - building.width), Random.Range(0, citySizeY - building.length));

            // Generate nodes from the building's blocks
            bool buildingPlacementSuccessful = GenerateNodes(buildingPrefab, firstBlockPosition);

            if (buildingPlacementSuccessful)
            {
               //Instantiate(buildingPrefab, new Vector3(firstBlockPosition.x, 0, firstBlockPosition.y), Quaternion.identity);
            }
        }
    }

    bool GenerateNodes(GameObject buildingPrefab, Vector2Int startingPosition)
    {
        Building building = buildingPrefab.GetComponent<Building>();

        // Make sure the building does not overlap with any other building
        for (int dx = 0; dx < building.width; dx++)
        {
            for (int dy = 0; dy < building.length; dy++)
            {
                if (!CheckValidPlacement(startingPosition.x + dx, startingPosition.y + dy))
                {
                    return false;  // If any part of the building overlaps, the building placement is unsuccessful
                }
            }
        }

        // If the building does not overlap, add it to the city
        for (int dx = 0; dx < building.width; dx++)
        {
            for (int dy = 0; dy < building.length; dy++)
            {
                Vector2Int blockPosition = new Vector2Int(startingPosition.x + dx, startingPosition.y + dy);
                Node blockNode = new Node(blockPosition, "Structure");
                nodes[blockPosition.x, blockPosition.y] = blockNode;
            }
        }

        return true;
    }

    bool CheckValidPlacement(int x, int y)
    {
        if (x < 0 || x >= citySizeX || y < 0 || y >= citySizeY || nodes[x, y] != null)
        {
            return false;
        }
        return true;
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
        for (int x = 0; x < citySizeX; x++)
        {
            for (int y = 0; y < citySizeY; y++)
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
