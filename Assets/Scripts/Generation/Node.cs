using UnityEngine;

public class Node
{
    public Vector2Int position;  // The position of the node
    public string type;  // The type of the node ("Empty", "Structure")
    public GameObject obj;

    public Node(Vector2Int _position, string _type)
    {
        position = _position;
        type = _type;
    }
    
    public Node(Vector2Int _position, string _type, GameObject _prefab)
    {
        position = _position;
        type = _type;
        obj = _prefab;
    }
}
