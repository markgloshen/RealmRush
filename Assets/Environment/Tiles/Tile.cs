using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Tower _towerPrefab;
    [SerializeField] private bool _isPlaceable;

    private GridManager _gridManager;
    private Pathfinder _pathfinder;
    private Vector2Int _coordinates = new Vector2Int();

    public bool IsPlaceable { get { return _isPlaceable; } }

    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _pathfinder = FindObjectOfType<Pathfinder>();

        if (_gridManager != null)
        {
            _coordinates = _gridManager.GetCoordinatesFromPosition(transform.position);

            if (!_isPlaceable)
                _gridManager.BlockNode(_coordinates);
        }
    }

    private void Start()
    {

    }

    private void OnMouseDown()
    {
        if (_gridManager.GetNode(_coordinates).IsWalkable && !_pathfinder.WillBockPath(_coordinates))
        {
            bool isSuccessfull =_towerPrefab.CreateTower(_towerPrefab, transform.position);
            
            if (isSuccessfull)
            {
                _gridManager.BlockNode(_coordinates);
                _pathfinder.NotifyReceivers();
            }
        }
        else
        {
            Debug.Log("Cannot place a tower here.");
        }
    }
}
