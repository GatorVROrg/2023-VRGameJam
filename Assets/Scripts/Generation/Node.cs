using UnityEngine;

public class Node
{
    public Vector2Int position;  // The position of the node
    public string type;  // The type of the node ("Empty", "Structure")
    public GameObject road;
    public GameObject leftBuilding;
    public GameObject rightBuilding;
    public Vector2Int direction;
    public GameObject cornerBuilding;

    public Node(Vector2Int _position, string _type)
    {
        position = _position;
        type = _type;
    }
    
    public Node(Vector2Int _position, string _type, GameObject _roadPrefab)
    {
        position = _position;
        type = _type;
        road = _roadPrefab;
    }
}
