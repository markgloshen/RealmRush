using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] private Vector2Int _startCoordinates;
    [SerializeField] private Vector2Int _destinationCoordinates;

    private Node _startNode;
    private Node _destinationNode;
    private Node _currentSearchNode;
    private Queue<Node> _frontier = new Queue<Node>();
    private Dictionary<Vector2Int, Node> _reached = new Dictionary<Vector2Int, Node>();
    private Dictionary<Vector2Int, Node> _grid = new Dictionary<Vector2Int, Node>();
    private Vector2Int[] _directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    private GridManager _gridManager;

    public Vector2Int StartCoordinates { get { return _startCoordinates; } }
    public Vector2Int DestinationCoordinates { get { return _destinationCoordinates; } }

    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();

        if (_gridManager != null)
        {
            _grid = _gridManager.Grid;
            _startNode = _grid[_startCoordinates];
            _destinationNode = _grid[_destinationCoordinates];
        }
    }

    private void Start()
    {
         GetNewPath();
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(_startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        _gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }

    private void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();

        foreach (Vector2Int direction in _directions)
        {
            Vector2Int neighborCoordinates = _currentSearchNode.Coordinates + direction;

            if (_grid.ContainsKey(neighborCoordinates))
            {
                neighbors.Add(_grid[neighborCoordinates]);
            }
        }

        foreach (Node neighbor in neighbors)
        {
            if (!_reached.ContainsKey(neighbor.Coordinates) && neighbor.IsWalkable)
            {
                neighbor.ConnectedTo = _currentSearchNode;
                _reached.Add(neighbor.Coordinates, neighbor);
                _frontier.Enqueue(neighbor);
            }
        }
    }

    private void BreadthFirstSearch(Vector2Int coordinates)
    {
        bool isRunning = true;

        _startNode.IsWalkable = true;
        _destinationNode.IsWalkable = true;

        _frontier.Clear();
        _reached.Clear();

        _frontier.Enqueue(_grid[coordinates]);
        _reached.Add(coordinates, _grid[coordinates]);

        while (_frontier.Count > 0 && isRunning)
        {
            _currentSearchNode = _frontier.Dequeue();
            _currentSearchNode.IsExplored = true;
            ExploreNeighbors();

            if (_currentSearchNode.Coordinates == _destinationCoordinates)
            {
                isRunning = false;
            }
        }
    }

    private List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = _destinationNode;

        path.Add(currentNode);
        currentNode.IsPath = true;

        while (currentNode.ConnectedTo != null)
        {
            currentNode = currentNode.ConnectedTo;
            path.Add(currentNode);
            currentNode.IsPath = true;
        }

        path.Reverse();
        return path;
    }

    public bool WillBockPath(Vector2Int coordinates)
    {
        if (_grid.ContainsKey(coordinates))
        {
            bool previousState = _grid[coordinates].IsWalkable;

            _grid[coordinates].IsWalkable = false;
            List<Node> newPath = GetNewPath();
            _grid[coordinates].IsWalkable = previousState;

            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}

