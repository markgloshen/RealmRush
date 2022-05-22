using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2Int Coordinates;
    public Node ConnectedTo;
    public bool IsWalkable;
    public bool IsExplored;
    public bool IsPath;

    public Node(Vector2Int coordinates, bool isWalkable)
    {
        Coordinates = coordinates;
        IsWalkable = isWalkable;
    }
}